using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using oledid.SyntaxImprovement.Caching;
using Xunit;

namespace oledid.SyntaxImprovement.Tests.Caching
{
	public class ConcurrentMemoryCacheTests
	{
		[Fact]
		public void It_works()
		{
			var memoryCache = new MemoryCache(new MemoryCacheOptions());

			var wrapper = new ConcurrentMemoryCache(memoryCache, 5 * 60);

			var counter = 0;

			var operations = new List<Action>();
			for (var i = 0; i < 10000; ++i)
			{
				operations.Add(() => wrapper.GetOrCreate("ConcurrentMemoryCacheTests.It_works", () => counter += 1));
			}

			Parallel.Invoke(operations.ToArray());

			Assert.Equal(1, counter);
		}
	}
}
