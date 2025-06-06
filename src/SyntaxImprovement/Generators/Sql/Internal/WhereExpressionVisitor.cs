﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace oledid.SyntaxImprovement.Generators.Sql.Internal
{
	internal class WhereExpressionVisitor<TableType> : ExpressionVisitor where TableType : DatabaseTable, new()
	{
		private readonly ParameterFactory parameterFactory;
		private readonly StringBuilder stringBuilder;
		private readonly Stack<ExpressionType> operatorStack;
		private readonly DatabaseType databaseType = new TableType().GetDatabaseType();

		private bool IsSingleExpression = true;

		internal WhereExpressionVisitor(ParameterFactory parameterFactory)
		{
			this.parameterFactory = parameterFactory;
			stringBuilder = new StringBuilder();
			operatorStack = new Stack<ExpressionType>();
		}

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (node.Method.Name == "Contains")
			{
				var argumentVisitor = new WhereExpressionVisitor<TableType>(new ParameterFactory(databaseType));
				argumentVisitor.IsSingleExpression = false;
				argumentVisitor.Visit(node.Arguments);

				var callVisitor = new WhereExpressionVisitor<TableType>(new ParameterFactory(databaseType));
				callVisitor.IsSingleExpression = false;
				callVisitor.Visit(node.Object);

				if (node.Method.DeclaringType == typeof(string))
				{
					stringBuilder.Append(callVisitor.stringBuilder);
					stringBuilder.Append(" LIKE ");
					stringBuilder.Append(parameterFactory.Create("%" + argumentVisitor.parameterFactory.parameters.Single().Value + "%"));
					return node;
				}
				else if (node.Method.DeclaringType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(node.Method.DeclaringType))
				{
					stringBuilder.Append(argumentVisitor.stringBuilder);
					stringBuilder.Append(" IN ");
					stringBuilder.Append('(');
					for (var i = 0; i < callVisitor.parameterFactory.parameters.Count; i++)
					{
						var parameter = callVisitor.parameterFactory.parameters[i];
						var value = parameter.Value;
						var valueExpression = parameterFactory.Create(value);
						stringBuilder.Append(valueExpression);
						if (i != callVisitor.parameterFactory.parameters.Count - 1)
						{
							stringBuilder.Append(',');
							stringBuilder.Append(' ');
						}
					}
					stringBuilder.Append(')');
					return node;
				}
			}

			// Handle LINQ methods: First() and Single() for constants
			if (node.Method.DeclaringType == typeof(Enumerable))
			{
				if (node.Method.Name.In("First", "Single"))
				{
					// Try to evaluate the method call
					var lambda = Expression.Lambda(node);
					var compiled = lambda.Compile();
					var value = compiled.DynamicInvoke();

					var parameterizedValue = parameterFactory.Create(value);
					stringBuilder.Append(parameterizedValue);

					return node;
				}
				else
				{
					throw new NotSupportedException("The method " + node.Method.Name + " is not supported in this context.");
				}
			}

			return base.VisitMethodCall(node);
		}

		protected override Expression VisitMember(MemberExpression node)
		{
			if (node.Expression.Type == typeof(TableType) && node.Expression.NodeType == ExpressionType.Parameter)
			{
				var columnExpression = databaseType.GetColumnName(node.Member.Name);
				stringBuilder.Append(columnExpression);

				if (IsSingleExpression == false)
				{
					return node;
				}

				stringBuilder.Append(databaseType.GetEqualsOperator());
				stringBuilder.Append(parameterFactory.Create(true));
				return node;
			}

			var objectMember = Expression.Convert(node, typeof(object));
			var getterLambda = Expression.Lambda<Func<object>>(objectMember);
			var getter = getterLambda.Compile();
			var value = getter.Invoke();
			var parameterizedValue = parameterFactory.Create(value);
			stringBuilder.Append(parameterizedValue);

			return node;
		}

		protected override Expression VisitConstant(ConstantExpression node)
		{
			if (node.Value == null)
			{
				stringBuilder.Append("NULL");
				return node;
			}
			else if (node.Value.GetType().IsClass == false || node.Value is string)
			{
				var value = node.Value;
				var parameterizedValue = parameterFactory.Create(value);
				stringBuilder.Append(parameterizedValue);
				return node;
			}
			else
			{
				var fields = node.Value.GetType().GetFields();
				if (fields.Length != 1)
				{
					throw new NotSupportedException();
				}
				var field = fields.Single();
				var value = field.GetValue(node.Value);
				var parameterizedValue = parameterFactory.Create(value);
				stringBuilder.Append(parameterizedValue);
				return node;
			}
		}

		protected override Expression VisitBinary(BinaryExpression binaryExpression)
		{
			IsSingleExpression = false;

			var addParantheses = binaryExpression.NodeType.In(ExpressionType.AndAlso, ExpressionType.OrElse);

			operatorStack.Push(binaryExpression.NodeType);

			if (addParantheses)
			{
				stringBuilder.Append('(');
			}
			Visit(binaryExpression.Left);
			if (addParantheses)
			{
				stringBuilder.Append(')');
			}

			var operatorIndex = stringBuilder.Length;
			var operatorPlaceholder = "{{" + Guid.NewGuid() + "}}";
			stringBuilder.Append(operatorPlaceholder);

			if (addParantheses)
			{
				stringBuilder.Append('(');
			}
			Visit(binaryExpression.Right);
			if (addParantheses)
			{
				stringBuilder.Append(')');
			}

			var operatorExpression = GetOperatorExpression(binaryExpression.NodeType, binaryExpression.Left, binaryExpression.Right);
			stringBuilder.Replace(operatorPlaceholder, operatorExpression, operatorIndex, Guid.Empty.ToString().Length + "{{}}".Length);

			operatorStack.Pop();

			IsSingleExpression = true;
			return binaryExpression;
		}

		protected override Expression VisitLambda<T>(Expression<T> node)
		{
			var callVisitor = new WhereExpressionVisitor<TableType>(new ParameterFactory(databaseType));
			callVisitor.Visit(node.Body);

			return base.VisitLambda(node);
		}

		protected override Expression VisitUnary(UnaryExpression node)
		{
			if (node.NodeType == ExpressionType.Not)
			{
				var operandVisitor = new WhereExpressionVisitor<TableType>(new ParameterFactory(databaseType));
				operandVisitor.IsSingleExpression = false;
				operandVisitor.Visit(node.Operand);

				stringBuilder.Append(operandVisitor.stringBuilder.ToString());
				stringBuilder.Append(databaseType.GetEqualsOperator());
				stringBuilder.Append(parameterFactory.Create(false));
				return node;
			}

			return base.VisitUnary(node);
		}

		private string GetOperatorExpression(ExpressionType @operator, Expression leftExpression, Expression rightExpression)
		{
			switch (@operator)
			{
				case ExpressionType.Equal:
					{
						return databaseType.GetEqualsOperator();
					}
				case ExpressionType.NotEqual:
					{
						return databaseType.GetNotEqualsOperator();
					}
				case ExpressionType.LessThan:
					return " < ";
				case ExpressionType.LessThanOrEqual:
					return " <= ";
				case ExpressionType.GreaterThan:
					return " > ";
				case ExpressionType.GreaterThanOrEqual:
					return " >= ";
				case ExpressionType.AndAlso:
					return " AND ";
				case ExpressionType.OrElse:
					return " OR ";
				default:
					throw new NotSupportedException();
			}
		}

		public string GetQuery()
		{
			return stringBuilder.ToString();
		}
	}
}
