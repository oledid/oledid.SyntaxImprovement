using System.Collections.Generic;

namespace oledid.SyntaxImprovement.Extensions
{
	public static class HashSetExtensions
	{
		public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> elements)
		{
			foreach (var element in elements)
			{
				hashSet.Add(element);
			}
		}
	}
}
