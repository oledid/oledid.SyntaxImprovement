using System;
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

			[Fact]
			public void It_can_order_by_asc()
			{
				var query = new Select<Person>()
					.OrderBy(person => person.Id)
					.ToQuery();

				Assert.Equal("SELECT [Id], [Name] FROM [Person] ORDER BY [Id];", query.QueryText);
			}

			[Fact]
			public void It_can_order_by_desc()
			{
				var query = new Select<Person>()
					.OrderBy(person => person.Id, descending: true)
					.ToQuery();

				Assert.Equal("SELECT [Id], [Name] FROM [Person] ORDER BY [Id] desc;", query.QueryText);
			}

			[Fact]
			public void It_can_order_by_multi()
			{
				var query = new Select<Person>()
					.OrderBy(person => person.Id, descending: true)
					.ThenBy(person => person.Name)
					.ToQuery();

				Assert.Equal("SELECT [Id], [Name] FROM [Person] ORDER BY [Id] desc, [Name];", query.QueryText);
			}

			[Fact]
			public void It_can_do_both_where_and_order_by()
			{
				var list = new List<int> {1, 3, 5};

				var query = new Select<Person>()
					.Where(person => list.Contains(person.Id))
					.OrderBy(person => person.Id, descending: true)
					.ThenBy(person => person.Name)
					.ToQuery();

				Assert.Equal("SELECT [Id], [Name] FROM [Person] WHERE [Id] IN (@p0, @p1, @p2) ORDER BY [Id] desc, [Name];", query.QueryText);
				Assert.Equal(1, ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
				Assert.Equal(3, ((IDictionary<string, object>)((dynamic)query).Parameters)["p1"]);
				Assert.Equal(5, ((IDictionary<string, object>)((dynamic)query).Parameters)["p2"]);
			}

			[Fact]
			public void It_can_do_like_queries()
			{
				var query = new Select<Person>()
					.Where(person => person.Name.Contains("Pete"))
					.ToQuery();

				Assert.Equal("SELECT [Id], [Name] FROM [Person] WHERE [Name] LIKE @p0;", query.QueryText);
				Assert.Equal("%Pete%", ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
			}
		}

		public class UpdateTests
		{
			[Fact]
			public void It_generates_correct_update()
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
			public void It_generates_correct_update_with_multi_params()
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

		public class InsertTests
		{
			[Fact]
			public void It_generates_correct_insert_for_single_object_with_identity_column()
			{
				var person = new Person { Id = 1, Name = "Peter" };
				var query = new Insert<Person>().Add(person).ToQuery();

				Assert.Equal("INSERT INTO [Person] ([Name]) SELECT @p0; SELECT SCOPE_IDENTITY();", query.QueryText);
				Assert.Equal("Peter", ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
			}

			[Fact]
			public void It_generates_correct_insert_for_multi_objects_with_identity_column()
			{
				var persons = new List<Person>
				{
					new Person { Name = "Peter" },
					new Person { Name = "Roger" }
				};
				var query = new Insert<Person>().Add(persons).ToQuery();

				Assert.Equal("INSERT INTO [Person] ([Name]) SELECT @p0; INSERT INTO [Person] ([Name]) SELECT @p1; ", query.QueryText);
				Assert.Equal("Peter", ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
				Assert.Equal("Roger", ((IDictionary<string, object>)((dynamic)query).Parameters)["p1"]);
			}

			[Fact]
			public void It_generates_correct_insert_for_multi_objects_with_identity_column_and_param()
			{
				var peter = new Person {Name = "Peter"};
				var roger = new Person {Name = "Roger"};
				var query = new Insert<Person>().Add(peter, roger).ToQuery();

				Assert.Equal("INSERT INTO [Person] ([Name]) SELECT @p0; INSERT INTO [Person] ([Name]) SELECT @p1; ", query.QueryText);
				Assert.Equal("Peter", ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
				Assert.Equal("Roger", ((IDictionary<string, object>)((dynamic)query).Parameters)["p1"]);
			}

			[Fact]
			public void It_generates_correct_insert_for_multi_objects_with_identity_column_and_nesting()
			{
				var peter = new Person { Name = "Peter" };
				var roger = new Person { Name = "Roger" };
				var query = new Insert<Person>()
					.Add(peter)
					.Add(roger)
					.ToQuery();

				Assert.Equal("INSERT INTO [Person] ([Name]) SELECT @p0; INSERT INTO [Person] ([Name]) SELECT @p1; ", query.QueryText);
				Assert.Equal("Peter", ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
				Assert.Equal("Roger", ((IDictionary<string, object>)((dynamic)query).Parameters)["p1"]);
			}

			[Fact]
			public void It_generates_correct_insert_for_single_object()
			{
				var model = new DataTypesModel
				{
					Boolean = true,
					Long = int.MaxValue + 1L,
					Decimal = 0.42m,
					DateTime = new DateTime(2018, 2, 23, 13, 37, 0),
					Guid = new Guid("67a76215-bc11-41cb-838f-c43fe81efcae")
				};
				var query = new Insert<DataTypesModel>().Add(model).ToQuery();

				Assert.Equal("INSERT INTO [TestSchema].[DataTypesModel] ([Boolean], [Long], [Decimal], [DateTime], [Guid]) SELECT @p0, @p1, @p2, @p3, @p4;", query.QueryText.Trim());
				Assert.Equal(true, ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
				Assert.Equal(int.MaxValue + 1L, ((IDictionary<string, object>)((dynamic)query).Parameters)["p1"]);
				Assert.Equal(0.42m, ((IDictionary<string, object>)((dynamic)query).Parameters)["p2"]);
				Assert.Equal(new DateTime(2018, 2, 23, 13, 37, 0), ((IDictionary<string, object>)((dynamic)query).Parameters)["p3"]);
				Assert.Equal(new Guid("67a76215-bc11-41cb-838f-c43fe81efcae"), ((IDictionary<string, object>)((dynamic)query).Parameters)["p4"]);
			}
		}

		public class DeleteTests
		{
			[Fact]
			public void It_generates_correct_query()
			{
				var query = new Delete<Person>().Where(person => person.Name == "Peter" || person.Id == 1).ToQuery();
				Assert.Equal("DELETE FROM [Person] WHERE ([Name] = @p0) OR ([Id] = @p1);", query.QueryText);
				Assert.Equal("Peter", ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
				Assert.Equal(1, ((IDictionary<string, object>)((dynamic)query).Parameters)["p1"]);
			}
		}
	}
}
