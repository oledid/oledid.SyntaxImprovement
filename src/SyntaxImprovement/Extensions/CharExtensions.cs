namespace oledid.SyntaxImprovement
{
	public static class CharExtensions
	{
		/// <summary>
		/// Returns c.In("0123456789");
		/// </summary>
		public static bool IsNumber(this char c)
		{
			return c.In("0123456789");
		}

		/// <summary>
		/// Returns true if char is [a-zA-Z]
		/// </summary>
		public static bool IsLetterAtoZ(this char c)
		{
			return c.ToString().ToUpperInvariant().ToCharArray()[0].In("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
		}

		/// <summary>
		/// Returns true if char is [A-Z]
		/// </summary>
		public static bool IsUppercaseLetterAtoZ(this char c)
		{
			return c.ToString().ToCharArray()[0].In("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
		}

		/// <summary>
		/// Returns true if char is [a-z]
		/// </summary>
		public static bool IsLowercaseLetterAtoZ(this char c)
		{
			return c.ToString().ToCharArray()[0].In("abcdefghijklmnopqrstuvwxyz");
		}
	}
}
