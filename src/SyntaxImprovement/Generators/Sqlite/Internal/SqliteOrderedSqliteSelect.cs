using System;
using System.Linq.Expressions;

namespace oledid.SyntaxImprovement.Generators.Sqlite.Internal
{
	public class SqliteOrderedSqliteSelect<TableType> : SqliteSelectBase<TableType> where TableType : SqliteDatabaseTable, new()
	{
		internal SqliteOrderedSqliteSelect(SqliteSelectManager<TableType> manager) : base(manager)
		{
		}

		public SqliteOrderedSqliteSelect<TableType> ThenBy(Expression<Func<TableType, object>> expression, bool descending = false)
		{
			var ascending = !descending;
			Manager.ThenByExpression(expression, ascending);
			return new SqliteOrderedSqliteSelect<TableType>(Manager);
		}
	}
}
