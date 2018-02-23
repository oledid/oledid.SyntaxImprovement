using System;
using System.Linq.Expressions;

namespace oledid.SyntaxImprovement.Generators.Sql.Internal
{
	internal class SetExpression<TableType> where TableType : DatabaseTable, new()
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
