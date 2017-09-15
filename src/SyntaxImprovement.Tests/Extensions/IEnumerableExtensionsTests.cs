using System.Collections.Generic;
using Xunit;

namespace oledid.SyntaxImprovement.Tests.Extensions
{
	public class IEnumerableExtensionsTests
	{
		[Fact]
		public void InsertAsFirstItem()
		{
			var list = new List<string> {"A", "B", "C"};
			var expected = new List<string> {"Z", "A", "B", "C"};
			var actual = list.InsertAsFirstItem("Z");
			Assert.Equal(expected[0], actual[0]);
			Assert.Equal(expected[1], actual[1]);
			Assert.Equal(expected[2], actual[2]);
			Assert.Equal(expected[3], actual[3]);
			Assert.Equal(expected.Count, actual.Count);
		}

		[Fact]
		public void InsertAsLastItem()
		{
			var list = new List<string> { "A", "B", "C" };
			var expected = new List<string> { "A", "B", "C", "Z" };
			var actual = list.InsertAsLastItem("Z");
			Assert.Equal(expected[0], actual[0]);
			Assert.Equal(expected[1], actual[1]);
			Assert.Equal(expected[2], actual[2]);
			Assert.Equal(expected[3], actual[3]);
			Assert.Equal(expected.Count, actual.Count);
		}
	}
}
