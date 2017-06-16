using System;

namespace oledid.SyntaxImprovement.Cache
{
	internal class MemoryCacheItem<T> where T : class
	{
		private readonly string cacheKey;
		private readonly int numberOfSecondsToCache;

		public MemoryCacheItem(string cacheKey, int numberOfSecondsToCache)
		{
			this.cacheKey = cacheKey;
			this.numberOfSecondsToCache = numberOfSecondsToCache;
		}

		public T Content
		{
			get
			{
				var cache = MemoryCacheWrapper.Instance;
				return cache.Get(cacheKey) as T;
			}
			set
			{
				var cache = MemoryCacheWrapper.Instance;
				if (cache.Contains(cacheKey))
				{
					cache.Remove(cacheKey);
				}
				if (value == null)
				{
					return;
				}
				cache.Add(cacheKey, value, DateTime.Now.AddSeconds(numberOfSecondsToCache));
			}
		}
	}
}
