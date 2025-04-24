using System;
using System.Data.Common;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Testcontainers.PostgreSql;
using Testcontainers.Xunit;
using JetBrains.Annotations;
using Npgsql;
using oledid.SyntaxImprovement.Generators.Sql;
using System.Linq;
using oledid.SyntaxImprovement.Tests.Generators.PostgreSQL.TestModels;
using Dapper;

namespace oledid.SyntaxImprovement.Tests.Generators.PostgreSQL
{
	public sealed class PostgreSqlContainerTest : IAsyncLifetime
	{
		private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();

		public Task InitializeAsync()
		{
			return _postgreSqlContainer.StartAsync();
		}

		public Task DisposeAsync()
		{
			return _postgreSqlContainer.DisposeAsync().AsTask();
		}

		[Fact]
		public void ConnectionStateReturnsOpen()
		{
			// Given
			using DbConnection connection = new NpgsqlConnection(_postgreSqlContainer.GetConnectionString());

			// When
			connection.Open();

			// Then
			Assert.Equal(ConnectionState.Open, connection.State);
		}

		[Fact]
		public async Task ExecScriptReturnsSuccessful()
		{
			// Given
			const string scriptContent = "SELECT 1;";

			// When
			var execResult = await _postgreSqlContainer.ExecScriptAsync(scriptContent)
				.ConfigureAwait(true);

			// Then
			Assert.True(0L.Equals(execResult.ExitCode), execResult.Stderr);
			Assert.Empty(execResult.Stderr);
		}

		[Fact]
		public async Task Integration_test_DataTypes()
		{
			await using DbConnection connection = new NpgsqlConnection(_postgreSqlContainer.GetConnectionString());
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

			expected.DateTime = expected.DateTime.RemoveTimeParts(removeMilliseconds: true);

			var createQuery = @$"
				CREATE SCHEMA IF NOT EXISTS ""{expected.GetSchemaName()}"";

				CREATE TABLE IF NOT EXISTS ""{expected.GetSchemaName()}"".""{expected.GetTableName()}"" (
					""Boolean"" BOOLEAN NOT NULL,
					""Long"" BIGINT NOT NULL,
					""Decimal"" NUMERIC NOT NULL,
					""DateTime"" TIMESTAMP NOT NULL,
					""Guid"" UUID NOT NULL,
					""StringWithValue"" TEXT NOT NULL,
					""NullString"" TEXT
				);";
			await connection.ExecuteAsync(createQuery);

			var insert = new Insert<DataTypesModel>().Add(expected).ToQuery();
			await connection.ExecuteAsync(insert.QueryText, insert.Parameters);

			var select = new Select<DataTypesModel>(top: 1).ToQuery();
			var actual = await connection.QuerySingleAsync<DataTypesModel>(select.QueryText, select.Parameters);

			Assert.Equivalent(expected, actual);
		}

		[Fact]
		public async Task Integration_test_Person()
		{
			using DbConnection connection = new NpgsqlConnection(_postgreSqlContainer.GetConnectionString());
			await connection.OpenAsync();

			var def = new Person
			{
			};

			await connection.ExecuteAsync(@$"
				CREATE TABLE IF NOT EXISTS ""{def.GetTableName()}"" (
					""Id"" INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
					""Name"" TEXT NOT NULL
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

		public sealed class ReuseContainerTest : IClassFixture<PostgreSqlFixture>, IDisposable
		{
			private readonly CancellationTokenSource _cts = new CancellationTokenSource(TimeSpan.FromMinutes(1));

			private readonly PostgreSqlFixture _fixture;

			public ReuseContainerTest(PostgreSqlFixture fixture)
			{
				_fixture = fixture;
			}

			public void Dispose()
			{
				_cts.Dispose();
			}

			[Theory]
			[InlineData(0)]
			[InlineData(1)]
			[InlineData(2)]
			public async Task StopsAndStartsContainerSuccessful(int _)
			{
				await _fixture.Container.StopAsync(_cts.Token)
					.ConfigureAwait(true);

				await _fixture.Container.StartAsync(_cts.Token)
					.ConfigureAwait(true);

				Assert.False(_cts.IsCancellationRequested);
			}
		}

		[UsedImplicitly]
		public sealed class PostgreSqlFixture : ContainerFixture<PostgreSqlBuilder, PostgreSqlContainer>
		{
			public PostgreSqlFixture(IMessageSink messageSink)
				: base(messageSink)
			{
			}
		}
	}
}
