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
		/// Returns c.ToString().ToUpperInvariant().In("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
		/// </summary>
		public static bool IsLetterAtoZ(this char c)
		{
			return c.ToString().ToUpperInvariant().ToCharArray()[0].In("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
		}
	}
}
