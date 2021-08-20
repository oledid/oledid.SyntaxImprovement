using System.Collections.Generic;

namespace oledid.SyntaxImprovement.Generators.Sqlite
{
	public class SqliteQuery
	{
		/// <summary>
		/// The query to execute
		/// </summary>
		public readonly string QueryText;

		/// <summary>
		/// A dynamic object containing the query parameters
		/// </summary>
		public readonly object Parameters;

		public SqliteQuery(string queryText, object parameters)
		{
			QueryText = queryText;
			Parameters = parameters;
		}

		/// <summary>
		/// Enumerates the values contained in <see cref="Parameters"/>
		/// </summary>
		public IEnumerable<KeyValuePair<string, object>> EnumerateParameters()
		{
			var iterator = 0;
			while (true)
			{
				var key = "p" + iterator;
				if (((IDictionary<string, object>)Parameters).ContainsKey(key) == false)
					yield break;

				yield return new KeyValuePair<string, object>(key, ((IDictionary<string, object>)Parameters)[key]);

				iterator = iterator + 1;
			}
		}
	}
}
