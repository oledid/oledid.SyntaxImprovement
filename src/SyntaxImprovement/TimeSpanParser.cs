using System;
using System.Globalization;

namespace oledid.SyntaxImprovement
{
	public static class TimeSpanParser
	{
		public static TimeSpan? ParseOrNull(string value, string format)
		{
			if (value == null)
				return null;

			return TimeSpan.TryParseExact(value, format, null, TimeSpanStyles.None, out TimeSpan time)
				? time
				: (TimeSpan?)null;
		}

		public static TimeSpan? ParseSubstringOrNull(string value, int substringMaxLength, string format)
		{
			if (value == null)
				return null;

			if (value.Length > substringMaxLength)
			{
				value = value.Substring(0, substringMaxLength);
			}

			return TimeSpan.TryParseExact(value, format, null, TimeSpanStyles.None, out TimeSpan time)
				? time
				: (TimeSpan?)null;
		}
	}
}
