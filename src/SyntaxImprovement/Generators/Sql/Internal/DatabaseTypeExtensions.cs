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
	}
}
