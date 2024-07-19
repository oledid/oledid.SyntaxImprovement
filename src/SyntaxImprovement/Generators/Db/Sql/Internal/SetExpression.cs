using System;
using System.Linq.Expressions;

namespace oledid.SyntaxImprovement.Generators.Db.Sql.Internal
{
	/// <summary>
	/// Define EITHER ColumnExpression or ColumnName
	/// </summary>
	internal class SetExpression<TableType> where TableType : DatabaseTable, new()
	{
		internal SetExpression()
		{
		}

		public Expression<Func<TableType, object>> ColumnExpression { get; set; }
		public string ColumnName { get; set; }
		public object Value { get; set; }

		public string GetColumnName()
		{
			if (ColumnName != null)
			{
				return ColumnName;
			}

			var visitor = new ColumnExpressionVisitor<TableType>();
			visitor.Visit(ColumnExpression);
			return visitor.GetColumnName();
		}

		public bool IsComputedField()
		{
			var visitor = new ColumnExpressionVisitor<TableType>();
			visitor.Visit(ColumnExpression);
			return visitor.IsComputedField();
		}
	}
}
