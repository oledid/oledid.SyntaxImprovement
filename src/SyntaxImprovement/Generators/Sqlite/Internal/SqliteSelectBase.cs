namespace oledid.SyntaxImprovement.Generators.Sqlite.Internal
{
	public abstract class SqliteSelectBase<TableType> where TableType : SqliteDatabaseTable, new()
	{
		internal readonly SqliteSelectManager<TableType> Manager;

		internal SqliteSelectBase(SqliteSelectManager<TableType> manager)
		{
			Manager = manager;
		}

		public SqliteQuery ToQuery()
		{
			return Manager.ToQuery();
		}
	}
}
