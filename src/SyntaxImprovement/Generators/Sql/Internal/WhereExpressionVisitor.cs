using System;
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

		internal WhereExpressionVisitor(ParameterFactory parameterFactory)
		{
			this.parameterFactory = parameterFactory;
			stringBuilder = new StringBuilder();
			operatorStack = new Stack<ExpressionType>();
		}

		public override Expression Visit(Expression node)
		{
			if (node is MethodCallExpression methodCallExpression && methodCallExpression.Method.Name == "Contains")
			{
				var argumentVisitor = new WhereExpressionVisitor<TableType>(new ParameterFactory());
				argumentVisitor.Visit(methodCallExpression.Arguments);

				var callVisitor = new WhereExpressionVisitor<TableType>(new ParameterFactory());
				callVisitor.Visit(methodCallExpression.Object);

				if (methodCallExpression.Method.DeclaringType == typeof(string))
				{
					stringBuilder.Append(callVisitor.stringBuilder);
					stringBuilder.Append(" LIKE ");
					stringBuilder.Append(parameterFactory.Create("%" + argumentVisitor.parameterFactory.parameters.Single().Value + "%"));
					return node;
				}
				else if (methodCallExpression.Method.DeclaringType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(methodCallExpression.Method.DeclaringType))
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

			if (node is MemberExpression memberExpression && memberExpression.Expression.Type == typeof(TableType) && memberExpression.Expression.NodeType == ExpressionType.Parameter)
			{
				var columnExpression = "[" + memberExpression.Member.Name + "]";
				stringBuilder.Append(columnExpression);
				return node;
			}

			if (node is MemberExpression m)
			{
				var objectMember = Expression.Convert(m, typeof(object));
				var getterLambda = Expression.Lambda<Func<object>>(objectMember);
				var getter = getterLambda.Compile();
				var value = getter.Invoke();
				var parameterizedValue = parameterFactory.Create(value);
				stringBuilder.Append(parameterizedValue);
				return node;
			}

			if (node is ConstantExpression constantExpression)
			{
				if (constantExpression.Value == null)
				{
					stringBuilder.Append("NULL");
					return node;
				}
				else if (constantExpression.Value.GetType().IsClass == false || constantExpression.Value is string)
				{
					var value = constantExpression.Value;
					var parameterizedValue = parameterFactory.Create(value);
					stringBuilder.Append(parameterizedValue);
					return node;
				}
				else
				{
					var fields = constantExpression.Value.GetType().GetFields();
					if (fields.Length != 1)
					{
						throw new NotSupportedException();
					}
					var field = fields.Single();
					var value = field.GetValue(constantExpression.Value);
					var parameterizedValue = parameterFactory.Create(value);
					stringBuilder.Append(parameterizedValue);
					return node;
				}
			}

			if (node is BinaryExpression binaryExpression)
			{
				var addParantheses = binaryExpression.NodeType.In(ExpressionType.AndAlso, ExpressionType.OrElse);

				operatorStack.Push(node.NodeType);

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

				var operatorExpression = GetOperatorExpression(node.NodeType, binaryExpression.Left, binaryExpression.Right);
				stringBuilder.Replace(operatorPlaceholder, operatorExpression, operatorIndex, Guid.Empty.ToString().Length + "{{}}".Length);

				operatorStack.Pop();
				return node;
			}

			return base.Visit(node);
		}

		private static string GetOperatorExpression(ExpressionType @operator, Expression leftExpression, Expression rightExpression)
		{
			switch (@operator)
			{
				case ExpressionType.Equal:
					{
						return " IS NOT DISTINCT FROM ";
					}
				case ExpressionType.NotEqual:
					{
						return " IS DISTINCT FROM ";
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
