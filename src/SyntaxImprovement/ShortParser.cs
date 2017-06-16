namespace oledid.SyntaxImprovement
{
	public class ShortParser
	{
		public static short ParseOrZero(string value)
		{
			return ParseOrNull(value) ?? 0;
		}

		public static short? ParseOrNull(string value, bool returnNullIfZero = false)
		{
			bool success = short.TryParse(value, out short s);
			if (returnNullIfZero && s == 0)
				return null;
			return success
				? s
				: (short?)null;
		}

		public static bool TryParse(string value, bool returnFalseIfZero = false)
		{
			bool success = short.TryParse(value, out short s);
			if (returnFalseIfZero && s == 0)
				return false;
			return success;
		}
	}
}
