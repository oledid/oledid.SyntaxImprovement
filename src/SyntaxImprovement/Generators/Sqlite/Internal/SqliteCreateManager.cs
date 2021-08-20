namespace oledid.SyntaxImprovement.Generators.Sqlite.Internal
{
	public class SqliteCreateManager<TableType> where TableType : SqliteDatabaseTable, new()
	{
		private readonly SqliteTableInformation<TableType> sqliteTableInformation;

		public SqliteCreateManager()
		{
			sqliteTableInformation = new SqliteTableInformation<TableType>();
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
			var columns = sqliteTableInformation.GetColumnNames(excludeIgnoredFields: true);
			return "CREATE TABLE " + tableName + " IF NOT EXISTS (" + string.Join(", ", columns) + ");";
		}
	}
}
