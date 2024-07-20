using System.Collections.Generic;
using oledid.SyntaxImprovement.Generators.Db.Sql;
using oledid.SyntaxImprovement.Tests.Generators.Sql.TestModels;
using Xunit;

namespace oledid.SyntaxImprovement.Tests.Generators.Sql
{
	public class IsComputedTests
	{
		[Fact]
		public void It_selects_computed_fields()
		{
			var query = new Select<ModelWithComputedField>()
				.ToQuery();

			Assert.Equal("SELECT [Id], [DoubleOfId] FROM [ModelWithComputedField];", query.QueryText);
		}

		[Fact]
		public void It_does_not_insert_computed_fields()
		{
			{
				var instance = new ModelWithComputedField
				{
					Id = 3
				};

				var query = new Insert<ModelWithComputedField>()
					.Add(instance)
					.ToQuery();

				Assert.Equal("INSERT INTO [ModelWithComputedField] ([Id]) SELECT @p0;", query.QueryText.Trim());
				Assert.Equal(3, ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
			}
			{
				var instance = new ModelWithComputedField
				{
					Id = 3,
					DoubleOfId = 6
				};

				var query = new Insert<ModelWithComputedField>()
					.Add(instance)
					.ToQuery();

				Assert.Equal("INSERT INTO [ModelWithComputedField] ([Id]) SELECT @p0;", query.QueryText.Trim());
				Assert.Equal(3, ((IDictionary<string, object>)((dynamic)query).Parameters)["p0"]);
			}
		}
	}
}
