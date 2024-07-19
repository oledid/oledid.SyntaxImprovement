namespace oledid.SyntaxImprovement.Generators.Db
{
	public abstract class DatabaseTable
	{
		public abstract string GetTableName();

		public virtual string GetSchemaName()
		{
			return null;
		}
	}
}
