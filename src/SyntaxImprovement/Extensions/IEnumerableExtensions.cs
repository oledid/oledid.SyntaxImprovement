using System;
using System.Collections.Generic;

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
	}
}