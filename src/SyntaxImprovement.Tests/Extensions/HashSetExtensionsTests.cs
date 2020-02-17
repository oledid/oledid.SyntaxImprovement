using System.Collections.Generic;
using oledid.SyntaxImprovement.Extensions;
using Xunit;

namespace oledid.SyntaxImprovement.Tests.Extensions
{
	public class HashSetExtensionsTests
	{
		[Fact]
		public void It_defines_AddRange()
		{
			var hashSet = new HashSet<int>();
			hashSet.Add(3);
			hashSet.AddRange(new[] { 3, 2, 1 });
			Assert.Equal(3, hashSet.Count);
			Assert.Contains(1, hashSet);
			Assert.Contains(2, hashSet);
			Assert.Contains(3, hashSet);
		}
	}
}
