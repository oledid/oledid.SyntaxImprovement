using System;
using System.Linq.Expressions;

namespace oledid.SyntaxImprovement.Generators.Sqlite.Internal
{
	internal class SqliteDeleteManager<TableType> where TableType : SqliteDatabaseTable, new()
	{
		private readonly SqliteTableInformation<TableType> sqliteTableInformation;
		private Expression<Func<TableType, bool>> whereStatement;

		public SqliteDeleteManager()
		{
			sqliteTableInformation = new SqliteTableInformation<TableType>();
		}

		public void SetWhereExpression(Expression<Func<TableType, bool>> expression)
		{
			if (whereStatement != null)
				throw new NotSupportedException("Where expression already set.");
			whereStatement = expression;
		}

		public SqliteQuery ToQuery()
		{
			var parameterFactory = new SqliteParameterFactory();
			var query = CreateQuery(parameterFactory);
			return new SqliteQuery(query, parameterFactory.GetParameters());
		}

		private string CreateQuery(SqliteParameterFactory sqliteParameterFactory)
		{
			var tableName = sqliteTableInformation.GetSchemaAndTableName();
			var whereQueryPart = SqliteWhereGenerator.CreateQuery(sqliteParameterFactory, whereStatement);
			return
				"DELETE FROM " + tableName
			  + whereQueryPart + ";";
		}
	}
}
