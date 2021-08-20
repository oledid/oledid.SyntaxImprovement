using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using oledid.SyntaxImprovement.Generators.Sqlite;
using oledid.SyntaxImprovement.Tests.Generators.Sqlite.TestModels;
using Xunit;

namespace oledid.SyntaxImprovement.Tests.Generators.Sqlite
{
	public class SqliteTests
	{
		[Fact]
		public async Task It_can_create_if_not_exists()
		{
			await using var connection = await CreateConnectionAsync();
			var create = new SqliteCreateIfNotExists<DataTypesModel>().ToQuery();
			var expected = @"
				CREATE TABLE [TestSchema].[DataTypesModel] IF NOT EXISTS (
					[Boolean] NUM,
					[Long] NUM,
					[Decimal] REAL,
					[DateTime] TEXT,
					[Guid] TEXT,
					[StringWithValue] TEXT,
					[NullString] TEXT
				);".Replace("\t", "").Replace("\r", string.Empty).Replace("\n", string.Empty);
			Assert.Equal(expected, create.QueryText);
		}

		private async Task<SqliteConnection> CreateConnectionAsync()
		{
			SQLitePCL.Batteries.Init();
			var connection = new SqliteConnection("Data Source=:memory:");
			await connection.OpenAsync();
			return connection;
		}
	}
}
