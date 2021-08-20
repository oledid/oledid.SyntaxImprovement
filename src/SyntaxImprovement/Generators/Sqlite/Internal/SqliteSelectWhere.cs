using System;
using System.Linq.Expressions;

namespace oledid.SyntaxImprovement.Generators.Sqlite.Internal
{
	public class SqliteSelectWhere<TableType> : SqliteSelectBase<TableType> where TableType : SqliteDatabaseTable, new()
	{
		internal SqliteSelectWhere(SqliteSelectManager<TableType> manager) : base(manager)
		{
		}

		public SqliteOrderedSqliteSelect<TableType> OrderBy(Expression<Func<TableType, object>> columnSelector, bool descending = false)
		{
			Manager.OrderByExpression(columnSelector, !descending);
			return new SqliteOrderedSqliteSelect<TableType>(Manager);
		}
	}
}
