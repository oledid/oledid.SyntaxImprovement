using System.Collections.Generic;
using System.Linq;

namespace oledid.SyntaxImprovement
{
	public static class ObjectExtensions
	{
		/// <summary>
		/// returns collection.Contains(obj);
		/// </summary>
		public static bool In<T>(this T obj, HashSet<T> collection)
		{
			return collection.Contains(obj);
		}

		/// <summary>
		/// returns collection.Contains(obj);
		/// </summary>
		public static bool In<T>(this T obj, IEnumerable<T> collection)
		{
			return collection.Contains(obj);
		}

		/// <summary>
		/// returns args.Contains(obj);
		/// </summary>
		public static bool In<T>(this T obj, params T[] args)
		{
			return args.Contains(obj);
		}

		/// <summary>
		/// returns collection.Contains(obj) == false;
		/// </summary>
		public static bool NotIn<T>(this T obj, IEnumerable<T> collection)
		{
			return collection.Contains(obj) == false;
		}

		/// <summary>
		/// returns args.Contains(obj) == false;
		/// </summary>
		public static bool NotIn<T>(this T obj, params T[] args)
		{
			return args.Contains(obj) == false;
		}
	}
}