using System;
using System.Linq.Expressions;

namespace oledid.SyntaxImprovement.Generators.Sqlite.Internal
{
	/// <summary>
	/// Define EITHER ColumnExpression or ColumnName
	/// </summary>
	internal class SqliteSetExpression<TableType> where TableType : SqliteDatabaseTable, new()
	{
		internal SqliteSetExpression()
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

			var visitor = new SqliteColumnExpressionVisitor<TableType>();
			visitor.Visit(ColumnExpression);
			return visitor.GetColumnName();
		}

		public bool IsComputedField()
		{
			var visitor = new SqliteColumnExpressionVisitor<TableType>();
			visitor.Visit(ColumnExpression);
			return visitor.IsComputedField();
		}
	}
}
