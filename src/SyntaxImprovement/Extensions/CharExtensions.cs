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
	}
}