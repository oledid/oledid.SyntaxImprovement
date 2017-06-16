using System;
using System.Globalization;

namespace oledid.SyntaxImprovement
{
	public static class DateTimeParser
	{
		public static DateTime? ParseOrNull(string value, string format)
		{
			if (value == null)
				return null;

			return DateTime.TryParseExact(value, format, null, DateTimeStyles.None, out DateTime date)
				? date
				: (DateTime?)null;
		}

		public static DateTime Parse(string value, string format)
		{
			if (value == null)
				throw new ArgumentNullException(nameof(value));

			var result = DateTime.TryParseExact(value, format, null, DateTimeStyles.None, out DateTime date)
				? date
				: (DateTime?)null;

			if (result.HasValue == false)
				throw new ArgumentException("Could not parse DateTime with value: " + value, nameof(value));

			return result.Value;
		}
	}
}
