namespace oledid.SyntaxImprovement
{
	public static class LongParser
	{
		public static long ParseOrZero(string value)
		{
			return ParseOrNull(value) ?? 0;
		}

		public static long? ParseOrNull(string value, bool returnNullIfZero = false)
		{
			bool success = long.TryParse(value, out long i);
			if (returnNullIfZero && i == 0)
				return null;
			return success
				? i
				: (long?)null;
		}

		public static bool TryParse(string value, bool returnFalseIfZero = false)
		{
			bool success = long.TryParse(value, out long i);
			if (returnFalseIfZero && i == 0)
				return false;
			return success;
		}
	}
}
