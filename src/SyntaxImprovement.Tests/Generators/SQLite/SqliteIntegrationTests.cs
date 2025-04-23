using Dapper;
using Microsoft.Data.Sqlite;
using oledid.SyntaxImprovement.Generators.Sql;
using oledid.SyntaxImprovement.Tests.Generators.Sqlite.TestModels;
using System.Threading.Tasks;
using Xunit;

namespace oledid.SyntaxImprovement.Tests.Generators.SQLite
{
	public class SqliteIntegrationTests
	{
		[Fact]
		public async Task It_can_update()
		{
			// todo: fails because of IS instead of =
			// should be changed to: a = b OR (a IS NULL AND b IS NULL)

			await using var connection = new SqliteConnection("Data Source=:memory:");
			await connection.OpenAsync();

			await connection.ExecuteAsync(@"CREATE TABLE IF NOT EXISTS Person (
				Id INT PRIMARY KEY,
				Name TEXT NOT NULL
			);");

			var expected = new Person
			{
				Name = "John Doe"
			};

			var insert = new Insert<Person>().Add(expected).ToQuery();
			var id = await connection.QueryFirstAsync<long>(insert.QueryText, insert.Parameters);

			var select = new Select<Person>(top: 1).Where(e => e.Id == id).ToQuery();
			var actual = await connection.QuerySingleAsync<Person>(select.QueryText, select.Parameters);

			Assert.Equivalent(expected, actual);
		}
	}
}
