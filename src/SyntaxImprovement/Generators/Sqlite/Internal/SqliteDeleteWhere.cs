namespace oledid.SyntaxImprovement.Generators.Sqlite.Internal
{
	public class SqliteDeleteWhere<TableType> : SqliteDeleteBase<TableType> where TableType : SqliteDatabaseTable, new()
	{
		internal SqliteDeleteWhere(SqliteDeleteManager<TableType> manager) : base(manager)
		{
		}

		public SqliteQuery ToQuery()
		{
			return manager.ToQuery();
		}
	}
}
