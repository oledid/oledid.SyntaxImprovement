using System.Runtime.Caching;

namespace oledid.SyntaxImprovement.Cache
{
	public static class MemoryCacheWrapper
	{
		private static MemoryCache instance;
		private static readonly object locker = new object();

		public static MemoryCache Instance => InitCache(clear: false);

		public static void ClearCache()
		{
			InitCache(clear: true);
		}

		private static MemoryCache InitCache(bool clear)
		{
			if (instance != null && !clear)
				return instance;

			lock (locker)
			{
				if (instance != null && clear)
				{
					instance.Dispose();
					instance = null;
				}
				return instance ?? (instance = new MemoryCache(MemoryCacheName));
			}
		}

		private static string MemoryCacheName => CompilerInformation.GetUniqueKeyFromCallerInformation();
	}
}