using oledid.SyntaxImprovement.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace oledid.SyntaxImprovement
{
	public static class IntParser
	{
		public static int ParseOrZero(string value)
		{
			return ParseOrNull(value) ?? 0;
		}

		public static int? ParseOrNull(string value, bool returnNullIfZero = false)
		{
			bool success = int.TryParse(value, out int i);
			if (returnNullIfZero && i == 0)
				return null;
			return success
				? i
				: (int?)null;
		}

		public static bool TryParse(string value, bool returnFalseIfZero = false)
		{
			bool success = int.TryParse(value, out int i);
			if (returnFalseIfZero && i == 0)
				return false;
			return success;
		}

		/// <summary>
		/// Returns valid integers from a list represented as a string with a separator char.
		/// </summary>
		public static List<int> ParseList(string valueList, char separator, bool returnNullIfValueListIsNull = false)
		{
			if (returnNullIfValueListIsNull && valueList == null)
				return null;

			return (valueList ?? string.Empty)
				.Trim()
				.Split(separator)
				.Where(str => str.Trim().HasValue())
				.Select(str => ParseOrNull(str))
				.Where(nullableInt => nullableInt.HasValue)
				.Select(nullableInt => nullableInt.Value)
				.ToList();
		}

		/// <summary>
		/// Ignores invalid integers: '1', '2', 'b', '4' returns [1, 2, 4]
		/// </summary>
		/// <param name="integers"></param>
		/// <returns></returns>
		public static List<int> ParseValidIntegersFromStringArray(IEnumerable<string> integers)
		{
			return integers?
				.Select(integer => ParseOrNull(integer))
				.Where(integer => integer.HasValue)
				.Select(integer => integer.Value)
				.ToList() ?? new List<int>();
		}
	}
}