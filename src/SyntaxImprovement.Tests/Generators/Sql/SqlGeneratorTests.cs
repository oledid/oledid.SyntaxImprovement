using System;
using System.Collections.Generic;
using System.Linq;
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
			public void It_can_select_top_1()
			{
				var query = new Select<Person>(top: 1).ToQuery();
				Assert.Equal("SELECT TOP 1 [Id], [Name] FROM [Person];", query.QueryText);
			}

			[Fact]
			public void It_can_select_top_1337()
			{
				var query = new Select<Person>(top: 1337).ToQuery();
				Assert.Equal("SELECT TOP 1337 [Id], [Name] FROM [Person];", query.QueryText);
			}

			[Fact]
			public void It_generates_correct_where_with_single_argument()
			{
				{
					var query = new Select<Person>()
					.Where(person => person.Name == "Peter")
					.ToQuery();

					Assert.Equal("SELECT [Id], [Name] FROM [Person] WHERE [Name] = @p0;", query.QueryText);
					Assert.Equal("Peter", ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
				}

				{
					var query = new Select<Person>()
					.Where(person => person.Name != "Peter")
					.ToQuery();

					Assert.Equal("SELECT [Id], [Name] FROM [Person] WHERE [Name] != @p0;", query.QueryText);
					Assert.Equal("Peter", ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
				}
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
				var list = new List<int> { 1, 3, 5 };

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

			[Fact]
			public void It_does_not_have_the_bug_described_in_issue_2()
			{
				{
					var idString = "DeviceId";
					var query = new Select<BooleanTestModel>().Where(model => model.IdStr == idString && model.IsActive == true).ToQuery();
					Assert.Equal("SELECT [IdStr], [IsActive] FROM [BooleanTestModel] WHERE ([IdStr] = @p0) AND ([IsActive] = @p1);", query.QueryText);
					Assert.Equal(idString, ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
					Assert.Equal(true, ((IDictionary<string, object>)((dynamic)query).Parameters)["p1"]);
				}

				{
					long? id = 28;
					var query = new Select<LongTestEntity>().Where(model => model.Id == id && model.IsDeleted == false).ToQuery();
					Assert.Equal("SELECT [Id], [IsDeleted] FROM [LongTest] WHERE ([Id] = @p0) AND ([IsDeleted] = @p1);", query.QueryText);
					Assert.Equal(id, ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
					Assert.Equal(false, ((IDictionary<string, object>)((dynamic)query).Parameters)["p1"]);
				}

				{
					long? id = 28;
					var query = new Select<LongTestEntity>().Where(model => model.Id == id.Value && model.IsDeleted == false).ToQuery();
					Assert.Equal("SELECT [Id], [IsDeleted] FROM [LongTest] WHERE ([Id] = @p0) AND ([IsDeleted] = @p1);", query.QueryText);
					Assert.Equal(id, ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
					Assert.Equal(false, ((IDictionary<string, object>)((dynamic)query).Parameters)["p1"]);
				}

				{
					Guid? id = Guid.NewGuid();
					var query = new Select<GuidTestEntity>().Where(model => model.Fk == id && model.IsDeleted == false).ToQuery();
					Assert.Equal("SELECT [Id], [Fk], [IsDeleted] FROM [GuidTestEntity] WHERE ([Fk] = @p0) AND ([IsDeleted] = @p1);", query.QueryText);
					Assert.Equal(id, ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
					Assert.Equal(false, ((IDictionary<string, object>)((dynamic)query).Parameters)["p1"]);
				}

				{
					Guid? id = Guid.NewGuid();
					var query = new Select<GuidTestEntity>().Where(model => model.Fk == id.Value && model.IsDeleted == false).ToQuery();
					Assert.Equal("SELECT [Id], [Fk], [IsDeleted] FROM [GuidTestEntity] WHERE ([Fk] = @p0) AND ([IsDeleted] = @p1);", query.QueryText);
					Assert.Equal(id, ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
					Assert.Equal(false, ((IDictionary<string, object>)((dynamic)query).Parameters)["p1"]);
				}
			}

			[Fact]
			public void It_understands_unary_not()
			{
				{
					long id = 0;
					var query = new Select<LongTestEntity>().Where(model => model.Id == id && model.IsDeleted == !false).ToQuery();
					Assert.Equal("SELECT [Id], [IsDeleted] FROM [LongTest] WHERE ([Id] = @p0) AND ([IsDeleted] = @p1);", query.QueryText);
					Assert.Equal(id, ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
					Assert.Equal(true, ((IDictionary<string, object>)((dynamic)query).Parameters)["p1"]);
				}
			}

			[Fact]
			public void It_understands_gt_and_lt()
			{
				var query = new Select<LongTestEntity>().Where(model => model.Id > 1000 && model.IsDeleted == !true && model.Id <= 1001L).ToQuery();
				Assert.Equal("SELECT [Id], [IsDeleted] FROM [LongTest] WHERE (([Id] > @p0) AND ([IsDeleted] = @p1)) AND ([Id] <= @p2);", query.QueryText);
				Assert.Equal(1000L, ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
				Assert.Equal(false, ((IDictionary<string, object>)((dynamic)query).Parameters)["p1"]);
				Assert.Equal(1001L, ((IDictionary<string, object>)((dynamic)query).Parameters)["p2"]);
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

			class It_can_update_entire_model_Class : DatabaseTable
			{
				[IsPrimaryKey]
				public int Id { get; set; }
				[IsIdentity]
				public int Counter { get; set; }
				public string FirstName { get; set; }
				public string LastName { get; set; }
				[IsComputed]
				public string FullName { get; set; }
				[Ignore]
				public Guid InternalField { get; set; }
				public bool IsAdmin { get; set; }

				public override string GetTableName() => "Table";
			}

			[Fact]
			public void It_can_update_entire_model()
			{
				var original = new It_can_update_entire_model_Class
				{
					Id = 2,
					Counter = 1337,
					FirstName = "Ole",
					LastName = "Did",
					FullName = "Ole Did",
					InternalField = Guid.Empty,
					IsAdmin = false
				};

				var changed = new It_can_update_entire_model_Class
				{
					Id = 5,
					Counter = 1338,
					FirstName = "Ole M",
					LastName = "Did",
					FullName = "Ole M Did",
					InternalField = Guid.NewGuid(),
					IsAdmin = true
				};

				var query = new Update<It_can_update_entire_model_Class>()
					.Set(changed)
					.Set(e => e.LastName, "Diddy")
					.Where(e => e.Id == original.Id)
					.ToQuery();

				Assert.Equal("UPDATE [Table] SET [Id] = @p1, [FirstName] = @p2, [IsAdmin] = @p4, [LastName] = @p5 WHERE [Id] = @p0", query.QueryText);
				Assert.Equal(2, ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
				Assert.Equal(5, ((IDictionary<string, object>)((dynamic)query).Parameters)["p1"]);
				Assert.Equal("Ole M", ((IDictionary<string, object>)((dynamic)query).Parameters)["p2"]);
				Assert.Equal("Did", ((IDictionary<string, object>)((dynamic)query).Parameters)["p3"]);
				Assert.Equal(true, ((IDictionary<string, object>)((dynamic)query).Parameters)["p4"]);
				Assert.Equal("Diddy", ((IDictionary<string, object>)((dynamic)query).Parameters)["p5"]);
				Assert.Throws<KeyNotFoundException>(() => ((IDictionary<string, object>)((dynamic)query).Parameters)["p6"]);
			}

			[Fact]
			public void It_can_update_boolean_fields()
			{
				var userId = Guid.NewGuid();
				var personId = Guid.NewGuid();

				var user = new User
				{
					Id = userId,
					CanWrite = true,
					ExternalId = "123",
					ExternalIdType = "fds",
					ExternalName = "fds",
					IsActive = true,
					IsAdmin = false,
					LastLogin = DateTime.Now,
					PersonId = personId
				};

				var query = new Update<User>()
					.Set(u => u.PersonId, user.PersonId)
					.Set(u => u.IsActive, user.IsActive)
					.Set(u => u.IsAdmin, user.IsAdmin)
					.Set(u => u.CanWrite, user.CanWrite)
					.Where(u => u.Id == user.Id)
					.ToQuery();

				Assert.Equal("UPDATE [userschema].[User] SET [PersonId] = @p1, [IsActive] = @p2, [IsAdmin] = @p3, [CanWrite] = @p4 WHERE [Id] = @p0", query.QueryText);
				Assert.Equal(userId, ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
				Assert.Equal(personId, ((IDictionary<string, object>)((dynamic)query).Parameters)["p1"]);
				Assert.Equal(true, ((IDictionary<string, object>)((dynamic)query).Parameters)["p2"]);
				Assert.Equal(false, ((IDictionary<string, object>)((dynamic)query).Parameters)["p3"]);
				Assert.Equal(true, ((IDictionary<string, object>)((dynamic)query).Parameters)["p4"]);
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
				var peter = new Person { Name = "Peter" };
				var roger = new Person { Name = "Roger" };
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
					Guid = new Guid("67a76215-bc11-41cb-838f-c43fe81efcae"),
					StringWithValue = "abc123",
					NullString = null
				};
				var query = new Insert<DataTypesModel>().Add(model).ToQuery();

				Assert.Equal("INSERT INTO [TestSchema].[DataTypesModel] ([Boolean], [Long], [Decimal], [DateTime], [Guid], [StringWithValue], [NullString]) SELECT @p0, @p1, @p2, @p3, @p4, @p5, @p6;", query.QueryText.Trim());
				Assert.Equal(true, ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
				Assert.Equal(int.MaxValue + 1L, ((IDictionary<string, object>)((dynamic)query).Parameters)["p1"]);
				Assert.Equal(0.42m, ((IDictionary<string, object>)((dynamic)query).Parameters)["p2"]);
				Assert.Equal(new DateTime(2018, 2, 23, 13, 37, 0), ((IDictionary<string, object>)((dynamic)query).Parameters)["p3"]);
				Assert.Equal(new Guid("67a76215-bc11-41cb-838f-c43fe81efcae"), ((IDictionary<string, object>)((dynamic)query).Parameters)["p4"]);
				Assert.Equal("abc123", ((IDictionary<string, object>)((dynamic)query).Parameters)["p5"]);
				Assert.Null(((IDictionary<string, object>)((dynamic)query).Parameters)["p6"]);
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

		public class NullTests
		{
			[Fact]
			public void It_handles_NULL_in_WHERE()
			{
				var select = new Select<NullTest>()
					.Where(e =>
						e.FkId == 1
						&& (
							e.NullableDateTime == null
							|| (e.NullableBool == false && e.NullableStr == null)
							|| (e.NullableStr == "123")
						)
					);

				var query = select.ToQuery();

				var expected = $"SELECT [Id], [FkId], [NullableDateTime], [NullableBool], [NullableStr] FROM [NullTest] WHERE ([FkId] = @p0) AND ((([NullableDateTime] IS NULL) OR (([NullableBool] = @p1) AND ([NullableStr] IS NULL))) OR ([NullableStr] = @p2));";
				Assert.Equal(expected, query.QueryText);
				Assert.Equal(1, ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
				Assert.Equal(false, ((IDictionary<string, object>)((dynamic)query).Parameters)["p1"]);
				Assert.Equal("123", ((IDictionary<string, object>)((dynamic)query).Parameters)["p2"]);
				Assert.Equal(3, query.EnumerateParameters().Count());
			}

			public class NullTest : DatabaseTable
			{
				[IsPrimaryKey]
				public Guid Id { get; set; }
				public int? FkId { get; set; }
				public DateTime? NullableDateTime { get; set; }
				public bool? NullableBool { get; set; }
				public string NullableStr { get; set; }

				public override string GetTableName() => nameof(NullTest);
			}
		}
	}
}
