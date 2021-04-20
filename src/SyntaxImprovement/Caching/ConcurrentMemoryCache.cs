using System;
using Microsoft.Extensions.Caching.Memory;
using oledid.SyntaxImprovement.Threading;

namespace oledid.SyntaxImprovement.Caching
{
	public class ConcurrentMemoryCache
	{
		private readonly IMemoryCache MemoryCache;
		private readonly int CacheDurationInSeconds;

		public ConcurrentMemoryCache(IMemoryCache memoryCache, int cacheDurationInSeconds)
		{
			MemoryCache = memoryCache;
			CacheDurationInSeconds = cacheDurationInSeconds;
		}

		public T GetOrCreate<T>(string cacheKey, Func<T> constructItem)
		{
			// ReSharper disable once InconsistentlySynchronizedField
			if (MemoryCache.TryGetValue(cacheKey, out var cacheItem1))
			{
				return (T)cacheItem1;
			}

			lock (LockObjectProvider.GetOrCreateLockObjectFromCallerInformation(additionalKeyParts: new object[]{ cacheKey }))
			{
				if (MemoryCache.TryGetValue(cacheKey, out var cacheItem2))
				{
					return (T)cacheItem2;
				}

				var item = constructItem.Invoke();
				_ = MemoryCache.Set(cacheKey, item, absoluteExpirationRelativeToNow: TimeSpan.FromSeconds(CacheDurationInSeconds));
				return item;
			}
		}
	}
}
