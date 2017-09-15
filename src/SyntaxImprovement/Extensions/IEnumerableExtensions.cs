using System;
using System.Collections.Generic;
using System.Linq;

namespace oledid.SyntaxImprovement
{
	// ReSharper disable once InconsistentNaming
	public static class IEnumerableExtensions
	{
		public static IEnumerable<TSource> Distinct<TKey, TSource>(this IEnumerable<TSource> collection, Func<TSource, TKey> selector)
		{
			var hashSet = new HashSet<TKey>();
			foreach (var element in collection)
			{
				var value = selector.Invoke(element);
				if (hashSet.Contains(value))
				{
					continue;
				}
				hashSet.Add(value);
				yield return element;
			}
		}

		public static List<TSource> InsertAsFirstItem<TSource>(this IEnumerable<TSource> collection, TSource firstItem)
		{
			var list = new List<TSource> { firstItem };
			list.AddRange(collection);
			return list;
		}

		public static List<TSource> InsertAsLastItem<TSource>(this IEnumerable<TSource> collection, TSource lastItem)
		{
			var list = collection.ToList();
			list.Add(lastItem);
			return list;
		}
	}
}