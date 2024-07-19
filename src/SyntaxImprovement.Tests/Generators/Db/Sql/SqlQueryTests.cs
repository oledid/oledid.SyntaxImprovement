using System;
using System.Collections.Generic;
using System.Linq;
using oledid.SyntaxImprovement.Generators.Sql;
using oledid.SyntaxImprovement.Tests.Generators.Db.TestModels;
using Xunit;

namespace oledid.SyntaxImprovement.Tests.Generators.Db.Sql
{
	public class SqlQueryTests
	{
		[Fact]
		public void It_enumerates_correct_values()
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

			var list = query.EnumerateParameters().ToList();

			Assert.Equal(new KeyValuePair<string, object>("p0", true), list[0]);
			Assert.Equal(new KeyValuePair<string, object>("p1", int.MaxValue + 1L), list[1]);
			Assert.Equal(new KeyValuePair<string, object>("p2", 0.42m), list[2]);
			Assert.Equal(new KeyValuePair<string, object>("p3", new DateTime(2018, 2, 23, 13, 37, 0)), list[3]);
			Assert.Equal(new KeyValuePair<string, object>("p4", new Guid("67a76215-bc11-41cb-838f-c43fe81efcae")), list[4]);
			Assert.Equal(new KeyValuePair<string, object>("p5", "abc123"), list[5]);
			Assert.Equal(new KeyValuePair<string, object>("p6", null), list[6]);
			Assert.Equal(7, list.Count);
		}

		[Fact]
		public void It_knows_if_the_query_is_empty()
		{
			var emptyInsert = new Insert<User>();
			Assert.False(emptyInsert.HasValue);

			var insertWithValue = emptyInsert.Add(new User());
			Assert.True(insertWithValue.HasValue);
		}
	}
}
