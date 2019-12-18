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
					stringBuilder.Append(GetOperatorExpression(ExpressionType.Modulo, isCollection: false));
					stringBuilder.Append(parameterFactory.Create("%" + argumentVisitor.parameterFactory.parameters.Single().Value + "%"));
					return node;
				}
				else if (methodCallExpression.Method.DeclaringType.IsGenericType && methodCallExpression.Method.DeclaringType.GetGenericTypeDefinition() == typeof(List<>))
				{
					stringBuilder.Append(argumentVisitor.stringBuilder);
					stringBuilder.Append(GetOperatorExpression(ExpressionType.Modulo, isCollection: true));
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
				var valueExpression = operatorStack.Any() && operatorStack.Peek() == ExpressionType.Modulo
					? parameterFactory.Create("%" + value + "%")
					: parameterFactory.Create(value);
				stringBuilder.Append(valueExpression);
				return node;
			}

			if (node is ConstantExpression constantExpression)
			{
				if (constantExpression.Value.GetType().IsClass == false || constantExpression.Value is string)
				{
					var value = constantExpression.Value;
					var valueExpression = operatorStack.Any() && operatorStack.Peek() == ExpressionType.Modulo
						? parameterFactory.Create("%" + value + "%")
						: parameterFactory.Create(value);
					stringBuilder.Append(valueExpression);
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
					var valueExpression = operatorStack.Any() && operatorStack.Peek() == ExpressionType.Modulo
						? parameterFactory.Create("%" + value + "%")
						: parameterFactory.Create(value);
					stringBuilder.Append(valueExpression);
					return node;
				}
			}

			if (node is BinaryExpression binaryExpression)
			{
				var addParantheses = binaryExpression.NodeType.In(ExpressionType.AndAlso, ExpressionType.OrElse);

				operatorStack.Push(node.NodeType);

				if (addParantheses)
				{
					stringBuilder.Append("(");
				}
				Visit(binaryExpression.Left);
				if (addParantheses)
				{
					stringBuilder.Append(")");
				}

				var operatorIndex = stringBuilder.Length;
				var operatorPlaceholder = "{{" + Guid.NewGuid() + "}}";
				stringBuilder.Append(operatorPlaceholder);

				if (addParantheses)
				{
					stringBuilder.Append("(");
				}
				Visit(binaryExpression.Right);
				if (addParantheses)
				{
					stringBuilder.Append(")");
				}

				var operatorExpression = GetOperatorExpression(node.NodeType, parameterFactory.LastOrNull()?.Value is ICollection);
				stringBuilder.Replace(operatorPlaceholder, operatorExpression, operatorIndex, Guid.Empty.ToString().Length + "{{}}".Length);

				operatorStack.Pop();
				return node;
			}

			return base.Visit(node);
		}

		//protected override Expression VisitBinary(BinaryExpression node)
		//{
		//	operatorStack.Push(node.NodeType);
		//	TryFinishStatement();
		//	return base.VisitBinary(node);
		//}

		//protected override Expression VisitMember(MemberExpression node)
		//{
		//	var isNullable = IsNullableType(node.Type);
		//	if (isNullable)
		//	{
		//		var expression = Visit(node.Expression);

		//		if (expression.NodeType == ExpressionType.Constant)
		//			return base.VisitMember(node);
		//	}

		//	try
		//	{
		//		var value = GetValueFromConstant(node);
		//		if (value != null)
		//		{
		//			valueStack.Push(value);
		//			TryFinishStatement();
		//		}
		//	}
		//	catch
		//	{
		//		//
		//	}

		//	if (node.Expression != null && node.Expression.Type == typeof(TableType))
		//	{
		//		memberStack.Push(node.Member);
		//		CheckIfIsSingleBooleanStatement();
		//		TryFinishStatement();
		//	}

		//	return base.VisitMember(node);
		//}

		//private static bool IsNullableType(Type type)
		//{
		//	return type.IsClass == false && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		//}

		//protected override Expression VisitMethodCall(MethodCallExpression node)
		//{
		//	if (node.Method.Name == "Contains")
		//	{
		//		operatorStack.Push(ExpressionType.Modulo);
		//	}
		//	else
		//	{
		//		throw new NotSupportedException("Unknown method " + node.Method.Name);
		//	}

		//	return base.VisitMethodCall(node);
		//}

		//protected override Expression VisitConstant(ConstantExpression node)
		//{
		//	if (node.Value.GetType().IsClass == false || node.Value is string)
		//	{
		//		valueStack.Push(node.Value);
		//		TryFinishStatement();
		//	}
		//	return base.VisitConstant(node);
		//}

		//private static object GetValueFromConstant(MemberExpression member)
		//{
		//	var objectMember = Expression.Convert(member, typeof(object));
		//	var getterLambda = Expression.Lambda<Func<object>>(objectMember);
		//	var getter = getterLambda.Compile();
		//	return getter.Invoke();
		//}

		//private void CheckIfIsSingleBooleanStatement()
		//{
		//	if (memberStack.Any() == false)
		//		return;

		//	var memberPeek = memberStack.Peek();
		//	var memberProperty = memberPeek as PropertyInfo;
		//	if (memberProperty == null || memberProperty.PropertyType != typeof(bool))
		//		return;

		//	if (operatorStack.Any() && operatorStack.Peek().In(ExpressionType.Equal, ExpressionType.NotEqual))
		//		return;

		//	var member = memberStack.Pop();
		//	var hasMoreOperators = operatorStack.Any();
		//	AddStatement(member, ExpressionType.Equal, true, hasMoreOperators);
		//	TryAddCombinationStatement();
		//}

		//private void TryFinishStatement()
		//{
		//	if (memberStack.Any() && valueStack.Any())
		//		FinishStatement();
		//}

		//private void FinishStatement()
		//{
		//	var member = memberStack.Pop();
		//	var @operator = operatorStack.Pop();
		//	var value = valueStack.Pop();
		//	var hasMoreOperators = operatorStack.Any() || whereStatements.Any();
		//	AddStatement(member, @operator, value, hasMoreOperators);
		//	TryAddCombinationStatement();
		//}

		//private void TryAddCombinationStatement()
		//{
		//	if (operatorStack.Any())
		//	{
		//		AddOperatorStatement(operatorStack.Pop());
		//	}
		//}

		//private void AddStatement(MemberInfo member, ExpressionType @operator, object value, bool hasMoreOperators)
		//{
		//	var columnExpression = "[" + member.Name + "]";
		//	var operatorExpression = GetOperatorExpression(@operator, isCollection: value is ICollection);
		//	var valueExpression = operatorExpression == " LIKE "
		//		? parameterFactory.Create("%" + value + "%")
		//		: parameterFactory.Create(value);
		//	var whereStatement = columnExpression + operatorExpression + valueExpression;
		//	if (hasMoreOperators)
		//	{
		//		whereStatement = "(" + whereStatement + ")";
		//	}
		//	whereStatements.Add(whereStatement);
		//}

		//private void AddOperatorStatement(ExpressionType @operator)
		//{
		//	switch (@operator)
		//	{
		//		case ExpressionType.OrElse:
		//		case ExpressionType.AndAlso:
		//			break;
		//		default:
		//			throw new NotSupportedException();
		//	}

		//	var operatorExpression = GetOperatorExpression(@operator, isCollection: false);
		//	whereStatements.Add(operatorExpression);
		//}

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
			return stringBuilder.ToString();
		}
	}

	public class BinaryOperation
	{
		public object A { get; set; }
		public object B { get; set; }
		public ExpressionType Operator { get; set; }
	}
}
