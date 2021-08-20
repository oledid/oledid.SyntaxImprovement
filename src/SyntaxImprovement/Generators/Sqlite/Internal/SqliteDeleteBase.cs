namespace oledid.SyntaxImprovement.Generators.Sqlite.Internal
{
	public class SqliteDeleteBase<TableType> where TableType : SqliteDatabaseTable, new()
	{
		internal readonly SqliteDeleteManager<TableType> manager;

		internal SqliteDeleteBase(SqliteDeleteManager<TableType> manager)
		{
			this.manager = manager;
		}
	}
}
