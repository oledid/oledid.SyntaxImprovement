using System;
using oledid.SyntaxImprovement.Threading;

namespace oledid.SyntaxImprovement.Cache
{
	public class ConcurrentMemoryCacheNullableItem<T> where T : struct
	{
		private readonly string cacheKey;
		private readonly int numberOfSecondsToCache;

		public ConcurrentMemoryCacheNullableItem(string cacheKey, int numberOfSecondsToCache)
		{
			this.cacheKey = cacheKey;
			this.numberOfSecondsToCache = numberOfSecondsToCache;
		}

		public void SetValue(T? value)
		{
			GetOrCreate(() => value, forceRefresh: true);
		}

		public T? GetOrCreate(Func<T?> createItemFunc, bool forceRefresh = false)
		{
			var cacheItem = new MemoryCacheNullableItem<T>(cacheKey, numberOfSecondsToCache);
			var result = cacheItem.Content;

			if (result != null && forceRefresh == false)
			{
				return result;
			}

			lock (LockObject)
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
			lock (LockObject)
			{
				// ReSharper disable once UseObjectOrCollectionInitializer
				var cacheItem = new MemoryCacheNullableItem<T>(cacheKey, numberOfSecondsToCache);
				cacheItem.Clear();
			}
		}

		private object LockObject => LockObjectProvider.GetOrCreateLockObjectFromCallerInformation(additionalKeyParts: new[] { cacheKey });
	}
}
