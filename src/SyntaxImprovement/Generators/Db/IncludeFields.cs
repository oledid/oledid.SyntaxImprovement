using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using oledid.SyntaxImprovement.Extensions;

namespace oledid.SyntaxImprovement.Generators.Db;

public class IncludeFields<TableType> where TableType : DatabaseTable, new()
{
	public readonly List<Expression<Func<TableType, object>>> FieldsToInclude;

	public IncludeFields(params Expression<Func<TableType, object>>[] includeFields)
	{
		FieldsToInclude = includeFields.ToList();
	}

	public HashSet<string> GetFieldNames()
	{
		var fieldNames = new HashSet<string>();
		foreach (var expression in FieldsToInclude)
		{
			var visitor = new IncludeFieldsExpressionVisitor<TableType>();
			visitor.Visit(expression);
			fieldNames.AddRange(visitor.GetFieldNames());
		}
		return fieldNames;
	}
}

internal class IncludeFieldsExpressionVisitor<TableType> : ExpressionVisitor where TableType : DatabaseTable, new()
{
	private readonly HashSet<string> FieldNames = new();

	public override Expression Visit(Expression node)
	{
		if (node is MemberExpression memberExpression && memberExpression.Expression.Type == typeof(TableType) && memberExpression.Expression.NodeType == ExpressionType.Parameter)
		{
			FieldNames.Add(memberExpression.Member.Name);
			return node;
		}

		return base.Visit(node);
	}

	public HashSet<string> GetFieldNames()
	{
		return FieldNames.ToHashSet();
	}
}
