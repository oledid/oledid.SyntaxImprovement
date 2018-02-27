using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace oledid.SyntaxImprovement.Generators.Sql.Internal
{
	internal class WhereExpressionVisitor<TableType> : ExpressionVisitor where TableType : DatabaseTable, new()
	{
		private readonly ParameterFactory parameterFactory;
		private readonly Stack<ExpressionType> operatorStack;
		private readonly Stack<MemberInfo> memberStack;
		private readonly Stack<object> valueStack;
		private readonly List<string> whereStatements;

		internal WhereExpressionVisitor(ParameterFactory parameterFactory)
		{
			this.parameterFactory = parameterFactory;
			operatorStack = new Stack<ExpressionType>();
			memberStack = new Stack<MemberInfo>();
			valueStack = new Stack<object>();
			whereStatements = new List<string>();
		}

		protected override Expression VisitBinary(BinaryExpression node)
		{
			operatorStack.Push(node.NodeType);
			TryFinishStatement();
			return base.VisitBinary(node);
		}

		protected override Expression VisitMember(MemberExpression node)
		{
			try
			{
				var value = GetValueFromConstant(node);
				if (value != null)
				{
					valueStack.Push(value);
					TryFinishStatement();
				}
			}
			catch
			{
				//
			}

			if (node.Expression != null && node.Expression.Type == typeof(TableType))
			{
				memberStack.Push(node.Member);
				CheckIfIsSingleBooleanStatement();
				TryFinishStatement();
			}
			else
			{
				var value = GetValueFromConstant(node);
				valueStack.Push(value);
				TryFinishStatement();
			}

			return base.VisitMember(node);
		}

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (node.Method.Name == "Contains")
			{
				operatorStack.Push(ExpressionType.Modulo);
			}
			else
			{
				throw new NotSupportedException("Unknown method " + node.Method.Name);
			}

			return base.VisitMethodCall(node);
		}

		protected override Expression VisitConstant(ConstantExpression node)
		{
			if (node.Value.GetType().IsClass == false || node.Value is string)
			{
				valueStack.Push(node.Value);
				TryFinishStatement();
			}
			return base.VisitConstant(node);
		}

		private static object GetValueFromConstant(MemberExpression member)
		{
			var objectMember = Expression.Convert(member, typeof(object));
			var getterLambda = Expression.Lambda<Func<object>>(objectMember);
			var getter = getterLambda.Compile();
			return getter.Invoke();
		}

		private void CheckIfIsSingleBooleanStatement()
		{
			if (memberStack.Any() == false)
				return;

			var memberPeek = memberStack.Peek();
			var memberProperty = memberPeek as PropertyInfo;
			if (memberProperty == null || memberProperty.PropertyType != typeof(bool))
				return;

			if (operatorStack.Any() && operatorStack.Peek().In(ExpressionType.Equal, ExpressionType.NotEqual))
				return;

			var member = memberStack.Pop();
			var hasMoreOperators = operatorStack.Any();
			AddStatement(member, ExpressionType.Equal, true, hasMoreOperators);
			TryAddCombinationStatement();
		}

		private void TryFinishStatement()
		{
			if (memberStack.Any() && valueStack.Any())
				FinishStatement();
		}

		private void FinishStatement()
		{
			var member = memberStack.Pop();
			var @operator = operatorStack.Pop();
			var value = valueStack.Pop();
			var hasMoreOperators = operatorStack.Any() || whereStatements.Any();
			AddStatement(member, @operator, value, hasMoreOperators);
			TryAddCombinationStatement();
		}

		private void TryAddCombinationStatement()
		{
			if (operatorStack.Any())
			{
				AddOperatorStatement(operatorStack.Pop());
			}
		}

		private void AddStatement(MemberInfo member, ExpressionType @operator, object value, bool hasMoreOperators)
		{
			var columnExpression = "[" + member.Name + "]";
			var operatorExpression = GetOperatorExpression(@operator, isCollection: value is ICollection);
			var valueExpression = operatorExpression == " LIKE "
				? parameterFactory.Create("%" + value + "%")
				: parameterFactory.Create(value);
			var whereStatement = columnExpression + operatorExpression + valueExpression;
			if (hasMoreOperators)
			{
				whereStatement = "(" + whereStatement + ")";
			}
			whereStatements.Add(whereStatement);
		}

		private void AddOperatorStatement(ExpressionType @operator)
		{
			switch (@operator)
			{
				case ExpressionType.OrElse:
				case ExpressionType.AndAlso:
					break;
				default:
					throw new NotSupportedException();
			}

			var operatorExpression = GetOperatorExpression(@operator, isCollection: false);
			whereStatements.Add(operatorExpression);
		}

		private static string GetOperatorExpression(ExpressionType @operator, bool isCollection)
		{
			switch (@operator)
			{
				case ExpressionType.Equal:
					return " = ";
				case ExpressionType.NotEqual:
					return " != ";
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
				case ExpressionType.Modulo:
					return isCollection
						? " IN "
						: " LIKE ";
				default:
					throw new NotSupportedException();
			}
		}

		public string GetQuery()
		{
			return string.Join("", whereStatements);
		}
	}
}
