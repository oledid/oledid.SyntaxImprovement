using Dapper;
using Microsoft.Data.Sqlite;
using oledid.SyntaxImprovement.Generators.Sql;
using oledid.SyntaxImprovement.Tests.Generators.Sqlite.TestModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

// must not be run in parallel because of the DapperTypeHandlerRegistration.Register()

namespace oledid.SyntaxImprovement.Tests.Generators.SQLite
{
	// todo: for null checks should be changed to: a = b OR (a IS NULL AND b IS NULL)

	[CollectionDefinition(nameof(SqliteNonParallelTests), DisableParallelization = true)]
	public class SqliteNonParallelTests { }

	[Collection(nameof(SqliteNonParallelTests))]
	public class SqliteIntegrationTests
	{
		[Fact]
		public async Task Integration_test_DataTypes()
		{
			DapperTypeHandlerRegistration.Register();

			await using var connection = new SqliteConnection("Data Source=:memory:");
			await connection.OpenAsync();

			var expected = new DataTypesModel
			{
				Boolean = true,
				Long = 1234567890,
				Decimal = 123.456m,
				DateTime = DateTime.UtcNow.AddHours(-3),
				Guid = Guid.NewGuid(),
				StringWithValue = "Test",
				NullString = null
			};

			await connection.ExecuteAsync(@$"
				CREATE TABLE IF NOT EXISTS ""{expected.GetSchemaName()}_{expected.GetTableName()}"" (
					Boolean INTEGER NOT NULL,
					Long INTEGER NOT NULL,
					Decimal NUMERIC NOT NULL,
					DateTime TEXT NOT NULL,
					Guid TEXT NOT NULL,
					StringWithValue TEXT NOT NULL,
					NullString TEXT
				);");

			var insert = new Insert<DataTypesModel>().Add(expected).ToQuery();
			await connection.ExecuteAsync(insert.QueryText, insert.Parameters);

			var select = new Select<DataTypesModel>(top: 1).ToQuery();
			var actual = await connection.QuerySingleAsync<DataTypesModel>(select.QueryText, select.Parameters);

			Assert.Equivalent(expected, actual);
		}

		[Fact]
		public async Task Integration_test_Person()
		{
			DapperTypeHandlerRegistration.Register();

			await using var connection = new SqliteConnection("Data Source=:memory:");
			await connection.OpenAsync();

			var def = new Person
			{
			};

			await connection.ExecuteAsync(@$"
				CREATE TABLE IF NOT EXISTS ""{def.GetTableName()}"" (
					Id INTEGER PRIMARY KEY AUTOINCREMENT,
					Name TEXT NOT NULL
				);");

			var peter = new Person
			{
				Name = "Peter"
			};

			var insertPeter = new Insert<Person>().Add(peter).ToQuery();
			var peterId = await connection.QuerySingleOrDefaultAsync<int>(insertPeter.QueryText, insertPeter.Parameters);

			var alan = new Person
			{
				Name = "Alan"
			};
			var insertAlan = new Insert<Person>().Add(alan).ToQuery();
			var alanId = await connection.QuerySingleOrDefaultAsync<int>(insertAlan.QueryText, insertAlan.Parameters);

			var robert = new Person
			{
				Name = "Robert"
			};
			var insertRobert = new Insert<Person>().Add(robert).ToQuery();
			var robertId = await connection.QuerySingleOrDefaultAsync<int>(insertRobert.QueryText, insertRobert.Parameters);

			var selectAll = new Select<Person>().ToQuery();
			var all = (await connection.QueryAsync<Person>(selectAll.QueryText, selectAll.Parameters)).AsList();
			Assert.Equal(3, all.Count);

			var selectTwo = new Select<Person>(top: 2).OrderBy(e => e.Name).ToQuery();
			var two = (await connection.QueryAsync<Person>(selectTwo.QueryText, selectTwo.Parameters)).AsList();
			Assert.Equal(2, two.Count);
			Assert.Equal("Alan", two[0].Name);
			Assert.Equal("Peter", two[1].Name);
			Assert.Equal(alanId, two[0].Id);
			Assert.Equal(peterId, two[1].Id);

			var selectRobert = new Select<Person>().Where(e => e.Name == "Robert").ToQuery();
			var robertResult = await connection.QuerySingleAsync<Person>(selectRobert.QueryText, selectRobert.Parameters);
			Assert.Equal("Robert", robertResult.Name);
			Assert.Equal(robertId, robertResult.Id);

			var selectAlanById = new Select<Person>().Where(e => e.Id == alanId).ToQuery();
			var alanResult = await connection.QuerySingleAsync<Person>(selectAlanById.QueryText, selectAlanById.Parameters);
			Assert.Equal("Alan", alanResult.Name);
			Assert.Equal(alanId, alanResult.Id);

			var ids = new[] { peterId, alanId }.ToList();
			var selectIn = new Select<Person>().Where(e => ids.Contains(e.Id)).ToQuery();
			var inResult = (await connection.QueryAsync<Person>(selectIn.QueryText, selectIn.Parameters)).AsList();
			Assert.Equal(2, inResult.Count);
			Assert.Equal("Peter", inResult[0].Name);
			Assert.Equal("Alan", inResult[1].Name);

			var deleteRobert = new Delete<Person>().Where(e => e.Id == robertId).ToQuery();
			await connection.ExecuteAsync(deleteRobert.QueryText, deleteRobert.Parameters);

			var fetchAllByNameAsc = new Select<Person>().OrderBy(e => e.Name).ToQuery();
			var refetchAll = (await connection.QueryAsync<Person>(fetchAllByNameAsc.QueryText, fetchAllByNameAsc.Parameters)).AsList();
			Assert.Equal(2, refetchAll.Count);
			Assert.Equal("Alan", refetchAll[0].Name);
			Assert.Equal("Peter", refetchAll[1].Name);

			var fetchAllByNameDesc = new Select<Person>().OrderBy(e => e.Name, descending: true).ToQuery();
			var refetchAllDesc = (await connection.QueryAsync<Person>(fetchAllByNameDesc.QueryText, fetchAllByNameDesc.Parameters)).AsList();
			Assert.Equal(2, refetchAll.Count);
			Assert.Equal("Peter", refetchAllDesc[0].Name);
			Assert.Equal("Alan", refetchAllDesc[1].Name);
		}
	}
}
