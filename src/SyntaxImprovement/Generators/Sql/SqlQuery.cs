namespace oledid.SyntaxImprovement.Generators.Sql
{
	public class SqlQuery
	{
		public readonly string QueryText;
		public readonly object Parameters;

		public SqlQuery(string queryText, object parameters)
		{
			QueryText = queryText;
			Parameters = parameters;
		}
	}
}
