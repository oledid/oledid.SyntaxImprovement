namespace oledid.SyntaxImprovement.Generators.Sqlite.Internal
{
	public class SqliteUpdateWhere<TableType> : SqliteUpdateBase<TableType> where TableType : SqliteDatabaseTable, new()
	{
		internal SqliteUpdateWhere(SqliteUpdateManager<TableType> manager)
			: base(manager)
		{
		}

		public SqliteQuery ToQuery()
		{
			return manager.ToQuery();
		}
	}
}
