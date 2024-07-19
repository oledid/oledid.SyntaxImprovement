namespace oledid.SyntaxImprovement.Generators.Db
{
	public static class SqlGeneratorSettings
	{
		public static DbType DefaultDbType { get; set; } = DbType.MicrosoftSql;
	}

	public enum DbType
	{
		MicrosoftSql,
		Sqlite
	}
}
