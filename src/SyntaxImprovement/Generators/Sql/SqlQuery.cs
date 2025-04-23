using System.Collections.Generic;

namespace oledid.SyntaxImprovement.Generators.Sql
{
	public class SqlQuery
	{
		/// <summary>
		/// The query to execute
		/// </summary>
		public readonly string QueryText;

		/// <summary>
		/// A dynamic object containing the query parameters
		/// </summary>
		public readonly object Parameters;

		public SqlQuery(string queryText, object parameters)
		{
			QueryText = queryText;
			Parameters = parameters;
		}

		/// <summary>
		/// Enumerates the values contained in <see cref="Parameters"/>
		/// </summary>
		public IEnumerable<KeyValuePair<string, object>> EnumerateParameters()
		{
			foreach (var kvp in (IDictionary<string, object>)Parameters)
			{
				yield return kvp;
			}
		}
	}
}
