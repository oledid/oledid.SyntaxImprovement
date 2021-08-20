using System;
using System.Linq.Expressions;
using oledid.SyntaxImprovement.Generators.Sqlite.Internal;

namespace oledid.SyntaxImprovement.Generators.Sqlite
{
	/// <summary>
	/// Used to generate a sql SELECT-query
	/// </summary>
	/// <typeparam name="TableType">A class which inherits from <see cref="SqliteDatabaseTable"/></typeparam>
	public class SqliteSelect<TableType> : SqliteSelectBase<TableType> where TableType : SqliteDatabaseTable, new()
	{
		public SqliteSelect(int? top = null) : base(new SqliteSelectManager<TableType>(top))
		{
		}

		internal SqliteSelect(SqliteSelectManager<TableType> manager) : base(manager)
		{
		}

		public SqliteSelectWhere<TableType> Where(Expression<Func<TableType, bool>> expression)
		{
			Manager.SetWhereExpression(expression);
			return new SqliteSelectWhere<TableType>(Manager);
		}

		public SqliteOrderedSqliteSelect<TableType> OrderBy(Expression<Func<TableType, object>> expression, bool descending = false)
		{
			var ascending = !descending;
			Manager.OrderByExpression(expression, ascending);
			return new SqliteOrderedSqliteSelect<TableType>(Manager);
		}
	}
}
