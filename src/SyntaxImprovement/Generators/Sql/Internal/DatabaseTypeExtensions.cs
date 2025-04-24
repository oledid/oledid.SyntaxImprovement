namespace oledid.SyntaxImprovement.Generators.Sql.Internal
{
	internal static class DatabaseTypeExtensions
	{
		public static string GetEqualsOperator(this DatabaseType databaseType)
		{
			return databaseType switch
			{
				DatabaseType.SQLite => " IS ",
				_ => " IS NOT DISTINCT FROM "
			};
		}

		public static string GetNotEqualsOperator(this DatabaseType databaseType)
		{
			return databaseType switch
			{
				DatabaseType.SQLite => " IS NOT ",
				_ => " IS DISTINCT FROM "
			};
		}

		public static string GetColumnName(this DatabaseType databaseType, string columnName)
		{
			return databaseType switch
			{
				DatabaseType.MSSQL => "[" + columnName + "]",
				_ => "\"" + columnName + "\""
			};
		}

		public static string GetInsertedIdentity(this DatabaseType databaseType, string identityColumn)
		{
			return databaseType switch
			{
				DatabaseType.PostgreSQL => " RETURNING " + databaseType.GetColumnName(identityColumn) + ";",
				DatabaseType.MSSQL => "; SELECT SCOPE_IDENTITY();",
				_ => "; SELECT last_insert_rowid();"
			};
		}
	}
}
