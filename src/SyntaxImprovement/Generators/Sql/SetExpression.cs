using System;
using System.Linq.Expressions;
using oledid.SyntaxImprovement.Generators.Sql.Internal;

namespace oledid.SyntaxImprovement.Generators.Sql
{
	public class SetExpression<TableType> where TableType : DatabaseTable, new()
	{
		internal SetExpression()
		{
		}

		public Expression<Func<TableType, object>> ColumnExpression { get; set; }
		public object Value { get; set; }

		public string GetColumnName()
		{
			var visitor = new ColumnExpressionVisitor<TableType>();
			visitor.Visit(ColumnExpression);
			return visitor.GetColumnName();
		}
	}
}
