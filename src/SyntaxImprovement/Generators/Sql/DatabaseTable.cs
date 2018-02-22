namespace oledid.SyntaxImprovement.Generators.Sql
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
