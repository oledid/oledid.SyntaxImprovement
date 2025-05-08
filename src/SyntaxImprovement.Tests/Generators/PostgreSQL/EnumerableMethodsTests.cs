using oledid.SyntaxImprovement.Generators.Sql;
using oledid.SyntaxImprovement.Tests.Generators.PostgreSQL.TestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace oledid.SyntaxImprovement.Tests.Generators.PostgreSQL
{
	public class EnumerableMethodsTests
	{
		[Fact]
		public void It_understands_Enumerable_First_method()
		{
			var list = new List<long> { 100, 200 };
			var query = new Select<LongTestEntity>().Where(e => e.Id == list.First()).ToQuery();
			Assert.Equal("WHERE \"Id\" = @p0;", query.QueryText.AfterFirst("WHERE", includeMatchStringInResult: true));
			Assert.Single(query.EnumerateParameters());
			Assert.Equal(100, (long)query.EnumerateParameters().First().Value);
		}

		[Fact]
		public void It_understands_Enumerable_Single_method()
		{
			var list = new List<long> { 200 };
			var query = new Select<LongTestEntity>().Where(e => e.Id == list.Single()).ToQuery();
			Assert.Equal("WHERE \"Id\" = @p0;", query.QueryText.AfterFirst("WHERE", includeMatchStringInResult: true));
			Assert.Single(query.EnumerateParameters());
			Assert.Equal(200, (long)query.EnumerateParameters().First().Value);
		}

		[Fact]
		public void It_does_not_support_Enumerable_other_methods()
		{
			var list = new List<long> { 100, 200 };
			Assert.Throws<NotSupportedException>(() => new Select<LongTestEntity>().Where(e => e.Id == list.FirstOrDefault()).ToQuery());
		}
	}
}
