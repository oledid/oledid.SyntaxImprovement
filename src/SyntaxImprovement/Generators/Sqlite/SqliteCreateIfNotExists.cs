using oledid.SyntaxImprovement.Generators.Sqlite.Internal;

namespace oledid.SyntaxImprovement.Generators.Sqlite
{
	public class SqliteCreateIfNotExists<TableType> where TableType : SqliteDatabaseTable, new()
	{
		private readonly SqliteCreateManager<TableType> Manager;

		public SqliteCreateIfNotExists()
		{
			Manager = new SqliteCreateManager<TableType>();
		}

		public SqliteQuery ToQuery()
		{
			return Manager.ToQuery();
		}
	}
}
