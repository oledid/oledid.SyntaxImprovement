using System.Collections.Generic;
using oledid.SyntaxImprovement.Generators.Sql;
using oledid.SyntaxImprovement.Tests.Generators.Sql.TestModels;
using Xunit;

namespace oledid.SyntaxImprovement.Tests.Generators.Sql
{
	public class SqlGeneratorTests
	{
		public class SelectTests
		{
			[Fact]
			public void It_generates_correct_select()
			{
				var query = new Select<Person>().ToQuery();
				Assert.Equal("SELECT [Id], [Name] FROM [Person];", query.QueryText);
			}

			[Fact]
			public void It_generates_correct_select_with_schema()
			{
				var query = new Select<PersonWithSchema>().ToQuery();
				Assert.Equal("SELECT [Id], [Name] FROM [MySchema].[Person];", query.QueryText);
			}

			[Fact]
			public void It_generates_correct_where_with_single_argument()
			{
				var query = new Select<Person>()
					.Where(person => person.Name == "Peter")
					.ToQuery();
				Assert.Equal("SELECT [Id], [Name] FROM [Person] WHERE [Name] = @p0;", query.QueryText);
				Assert.Equal("Peter", ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
			}

			[Fact]
			public void It_generates_correct_where_with_multiple_arguments()
			{
				var query = new Select<Person>()
					.Where(person => person.Name == "Peter" && person.Id == 1)
					.ToQuery();
				Assert.Equal("SELECT [Id], [Name] FROM [Person] WHERE ([Name] = @p0) AND ([Id] = @p1);", query.QueryText);
				Assert.Equal("Peter", ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
				Assert.Equal(1, ((IDictionary<string, object>)((dynamic)query).Parameters)["p1"]);
			}
		}

		public class UpdateTests
		{
			[Fact]
			public void It_generates_correct_insert()
			{
				var query = new Update<Person>()
					.Set(person => person.Name, "Peter")
					.Where(person => person.Id == 1)
					.ToQuery();
				Assert.Equal("UPDATE [Person] SET [Name] = @p1 WHERE [Id] = @p0", query.QueryText);
				Assert.Equal("Peter", ((IDictionary<string, object>)((dynamic)query).Parameters)["p1"]);
				Assert.Equal(1, ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
			}

			[Fact]
			public void It_generates_correct_insert_with_multi_params()
			{
				var query = new Update<Person>()
					.Set(person => person.Name, "Peter")
					.Set(person => person.Id, 2)
					.Where(person => person.Id == 1)
					.ToQuery();
				Assert.Equal("UPDATE [Person] SET [Name] = @p1, [Id] = @p2 WHERE [Id] = @p0", query.QueryText);
				Assert.Equal("Peter", ((IDictionary<string, object>)((dynamic)query).Parameters)["p1"]);
				Assert.Equal(2, ((IDictionary<string, object>)((dynamic)query).Parameters)["p2"]);
				Assert.Equal(1, ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
			}
		}
	}
}
