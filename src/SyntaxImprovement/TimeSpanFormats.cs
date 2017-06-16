namespace oledid.SyntaxImprovement
{
	/// <summary>
	/// Contains common TimeSpan format strings
	/// </summary>
	public static class TimeSpanFormats
	{
		/// <summary>
		/// HH:mm:ss, "13:37:01"
		/// </summary>
		public const string HH_mm_ss = @"hh\:mm\:ss";

		/// <summary>
		/// HH:mm, "13:37"
		/// </summary>
		public const string HH_mm = @"hh\:mm";

		/// <summary>
		/// HHmm, "1337"
		/// </summary>
		public const string HHmm = "hhmm";
	}
}