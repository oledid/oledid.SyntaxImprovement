using System;
using System.Linq.Expressions;

namespace oledid.SyntaxImprovement.Generators.Sqlite.Internal
{
	public class SqliteUpdateSet<TableType> : SqliteUpdate<TableType> where TableType : SqliteDatabaseTable, new()
	{
		internal SqliteUpdateSet(SqliteUpdateManager<TableType> manager)
			: base(manager)
		{
		}

		public SqliteUpdateWhere<TableType> Where(Expression<Func<TableType, bool>> expression)
		{
			manager.SetWhereExpression(expression);
			return new SqliteUpdateWhere<TableType>(manager);
		}
	}
}
