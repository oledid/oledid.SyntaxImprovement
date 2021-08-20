namespace oledid.SyntaxImprovement.Generators.Sqlite.Internal
{
	public class SqliteUpdateBase<TableType> where TableType : SqliteDatabaseTable, new()
	{
		internal readonly SqliteUpdateManager<TableType> manager;

		internal SqliteUpdateBase(SqliteUpdateManager<TableType> manager)
		{
			this.manager = manager;
		}
	}
}
