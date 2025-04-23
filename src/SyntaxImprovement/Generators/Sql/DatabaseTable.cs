namespace oledid.SyntaxImprovement.Generators.Sql
{
	public abstract class DatabaseTable
	{
		public abstract string GetTableName();

		public virtual string GetSchemaName()
		{
			return null;
		}

		/// <summary>
		/// Sets the database type to generate the query for. Default is <see cref="DatabaseType.MSSQL"/>
		/// </summary>
		public virtual DatabaseType GetDatabaseType()
		{
			return DatabaseType.MSSQL;
		}
	}
}
