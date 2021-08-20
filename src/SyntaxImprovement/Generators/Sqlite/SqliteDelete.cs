using System;
using System.Linq.Expressions;
using oledid.SyntaxImprovement.Generators.Sqlite.Internal;

namespace oledid.SyntaxImprovement.Generators.Sqlite
{
	/// <summary>
	/// Used to generate a sql DELETE-query
	/// </summary>
	/// <typeparam name="TableType">A class which inherits from <see cref="SqliteDatabaseTable"/></typeparam>
	public class SqliteDelete<TableType> : SqliteDeleteBase<TableType> where TableType : SqliteDatabaseTable, new()
	{
		public SqliteDelete() : base(new SqliteDeleteManager<TableType>())
		{
		}

		internal SqliteDelete(SqliteDeleteManager<TableType> manager) : base(manager)
		{
		}

		public SqliteDeleteWhere<TableType> Where(Expression<Func<TableType, bool>> expression)
		{
			manager.SetWhereExpression(expression);
			return new SqliteDeleteWhere<TableType>(manager);
		}
	}
}
