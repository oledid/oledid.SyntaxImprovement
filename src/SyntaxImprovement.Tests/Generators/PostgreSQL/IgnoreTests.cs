﻿using oledid.SyntaxImprovement.Generators.Sql;
using oledid.SyntaxImprovement.Tests.Generators.PostgreSQL.TestModels;
using System.Collections.Generic;
using Xunit;

namespace oledid.SyntaxImprovement.Tests.Generators.PostgreSQL
{
	public class IgnoreTests
	{
		[Fact]
		public void It_selects_correct_fields()
		{
			var query = new Select<ModelWithIgnoreField>()
				.ToQuery();

			Assert.Equal("SELECT \"Id\", \"Name\" FROM \"ModelWithIgnoreField\";", query.QueryText);
		}

		[Fact]
		public void It_inserts_correct_fields()
		{
			var instance = new ModelWithIgnoreField
			{
				Id = 3,
				Name = "a",
				Nope = "b"
			};

			var query = new Insert<ModelWithIgnoreField>()
				.Add(instance)
				.ToQuery();

			Assert.Equal("INSERT INTO \"ModelWithIgnoreField\" (\"Name\") SELECT @p0 RETURNING \"Id\";", query.QueryText.Trim());
			Assert.Equal("a", ((IDictionary<string, object>)((dynamic)query).Parameters)["@p0"]);
			Assert.Throws<KeyNotFoundException>(() => ((IDictionary<string, object>)((dynamic)query).Parameters)["@p1"]);
		}
	}
}
