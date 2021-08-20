namespace oledid.SyntaxImprovement.Generators.Sqlite
{
	public abstract class SqliteDatabaseTable
	{
		public abstract string GetTableName();

		public virtual string GetSchemaName()
		{
			return null;
		}
	}
}
