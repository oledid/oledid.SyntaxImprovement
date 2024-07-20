using System.Collections.Generic;

namespace oledid.SyntaxImprovement
{
	public static class DictionaryExtensions
	{
		/// <summary>
		/// Returns value if found. Returns null if dictionary is null, or dictionary does not contain key.
		/// </summary>
		public static TResult GetValueOrNull<T, TResult>(this IDictionary<T, TResult> dic, T key) where TResult : class
		{
			if (dic == null)
				return null;

			return dic.ContainsKey(key) == false
				? null
				: dic[key];
		}

		/// <summary>
		/// Returns value if found. Returns (TResult?)null if dictionary is null, or dictionary does not contain key.
		/// </summary>
		public static TResult? GetValueOrNullable<T, TResult>(this IDictionary<T, TResult> dic, T key) where TResult : struct
		{
			if (dic == null)
				return null;

			return dic.ContainsKey(key) == false
				? (TResult?)null
				: dic[key];
		}

		/// <summary>
		/// Returns value if found. Returns (TResult?)null if dictionary is null, or dictionary does not contain key.
		/// </summary>
		public static TResult? GetValueOrNullable<T, TResult>(this IDictionary<T, TResult?> dic, T key) where TResult : struct
		{
			if (dic == null)
				return null;

			return dic.ContainsKey(key) == false
				? null
				: dic[key];
		}
	}
}
