using System;
using oledid.SyntaxImprovement.Threading;

namespace oledid.SyntaxImprovement.Cache
{
	public class ConcurrentMemoryCacheItem<T> where T : class
	{
		private readonly string cacheKey;
		private readonly int numberOfSecondsToCache;

		public ConcurrentMemoryCacheItem(string cacheKey, int numberOfSecondsToCache)
		{
			this.cacheKey = cacheKey;
			this.numberOfSecondsToCache = numberOfSecondsToCache;
		}

		public void SetValue(T value)
		{
			GetOrCreate(() => value, forceRefresh: true);
		}

		public T GetOrCreate(Func<T> createItemFunc, bool forceRefresh = false)
		{
			var cacheItem = new MemoryCacheItem<T>(cacheKey, numberOfSecondsToCache);
			var result = cacheItem.Content;

			if (result != null && forceRefresh == false)
			{
				return result;
			}

			lock(LockObject)
			{
				if (forceRefresh)
				{
					cacheItem.Content = null;
				}
				return cacheItem.Content ?? (cacheItem.Content = createItemFunc.Invoke());
			}
		}

		public void Clear()
		{
			lock(LockObject)
			{
				// ReSharper disable once UseObjectOrCollectionInitializer
				var cacheItem = new MemoryCacheItem<T>(cacheKey, numberOfSecondsToCache);
				cacheItem.Content = null;
			}
		}

		private object LockObject => LockObjectProvider.GetOrCreateLockObjectFromCallerInformation(additionalKeyParts: new[] {cacheKey});
	}
}
