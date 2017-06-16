using System;

namespace oledid.SyntaxImprovement.Cache
{
	public class MemoryCacheNullableItem<T> where T : struct
	{
		private readonly string cacheKey;
		private readonly int numberOfMinutesToCache;

		public MemoryCacheNullableItem(string cacheKey, int numberOfMinutesToCache)
		{
			this.cacheKey = cacheKey;
			this.numberOfMinutesToCache = numberOfMinutesToCache;
		}

		public T? Content
		{
			get
			{
				var cache = MemoryCacheWrapper.Instance;
				return cache.Get(cacheKey) as T?;
			}
			set
			{
				var cache = MemoryCacheWrapper.Instance;
				if (cache.Contains(cacheKey))
				{
					cache.Remove(cacheKey);
				}
				cache.Add(cacheKey, value, DateTime.Now.AddMinutes(numberOfMinutesToCache));
			}
		}

		public void Clear()
		{
			var cache = MemoryCacheWrapper.Instance;
			if (cache.Contains(cacheKey))
			{
				cache.Remove(cacheKey);
			}
		}
	}
}
