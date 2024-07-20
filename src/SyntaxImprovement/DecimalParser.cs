using System.Globalization;

namespace oledid.SyntaxImprovement
{
	public static class DecimalParser
	{
		public static decimal? ParseOrNull(string str, bool returnNullIfZero = false)
		{
			if (str.HasValue(isWhitespaceValue: false) == false)
			{
				return null;
			}

			var cultureInfo = CultureInfo.GetCultureInfo("nb-NO");
			str = str.Replace(".", cultureInfo.NumberFormat.CurrencyDecimalSeparator);
			str = str.Replace(cultureInfo.NumberFormat.NumberGroupSeparator, string.Empty);
			str = str.Replace(" ", string.Empty);

			var success = decimal.TryParse(str, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, cultureInfo, out var result);
			return returnNullIfZero && result == default
				? null
				: success
					? result
					: (decimal?)null;
		}
	}
}
