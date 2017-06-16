using System.Collections.Concurrent;

namespace oledid.SyntaxImprovement.Cache
{
	public class ApplicationLifetimeCache<T> where T : class
	{
		private static readonly ConcurrentDictionary<string, T> cacheItems = new ConcurrentDictionary<string, T>();

		private readonly string cacheKey;

		public ApplicationLifetimeCache(string cacheKey)
		{
			this.cacheKey = cacheKey;
		}

		public T Content
		{
			get
			{
				return cacheItems.TryGetValue(cacheKey, out T value) ? value : null;
			}
			set
			{
				if (value == null)
				{
					cacheItems.TryRemove(cacheKey, out T val);
				}
				else
				{
					cacheItems.AddOrUpdate(cacheKey, value, (k, v) => value);
				}
			}
		}
	}
}