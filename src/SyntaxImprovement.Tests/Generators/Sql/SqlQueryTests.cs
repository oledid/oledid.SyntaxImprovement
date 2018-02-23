using System;
using System.Collections.Generic;
using System.Linq;
using oledid.SyntaxImprovement.Generators.Sql;
using oledid.SyntaxImprovement.Tests.Generators.Sql.TestModels;
using Xunit;

namespace oledid.SyntaxImprovement.Tests.Generators.Sql
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
				Guid = new Guid("67a76215-bc11-41cb-838f-c43fe81efcae")
			};
			var query = new Insert<DataTypesModel>().Add(model).ToQuery();

			var list = query.EnumerateParameters().ToList();

			Assert.Equal(new KeyValuePair<string, object>("p0", true), list[0]);
			Assert.Equal(new KeyValuePair<string, object>("p1", int.MaxValue + 1L), list[1]);
			Assert.Equal(new KeyValuePair<string, object>("p2", 0.42m), list[2]);
			Assert.Equal(new KeyValuePair<string, object>("p3", new DateTime(2018, 2, 23, 13, 37, 0)), list[3]);
			Assert.Equal(new KeyValuePair<string, object>("p4", new Guid("67a76215-bc11-41cb-838f-c43fe81efcae")), list[4]);
			Assert.Equal(5, list.Count);
		}
	}
}
