using System;
using System.Linq.Expressions;

namespace oledid.SyntaxImprovement.Generators.Sqlite.Internal
{
	internal static class SqliteWhereGenerator
	{
		public static string CreateQuery<TableType>(SqliteParameterFactory sqliteParameterFactory, Expression<Func<TableType, bool>> expression) where TableType : SqliteDatabaseTable, new()
		{
			var visitor = new SqliteWhereExpressionVisitor<TableType>(sqliteParameterFactory);
			visitor.Visit(expression);
			var query = visitor.GetQuery();
			return query.HasValue()
				? " WHERE " + query
				: string.Empty;
		}
	}
}
