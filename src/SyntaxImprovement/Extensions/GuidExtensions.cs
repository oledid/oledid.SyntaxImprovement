using System;

namespace oledid.SyntaxImprovement
{
	public static class GuidExtensions
	{
		public static Guid? ParseOrNull(string value)
		{
			return Guid.TryParse(value, out Guid guid)
				? guid
				: (Guid?)null;
		}

		public static bool IsNullOrEmpty(this Guid? input)
		{
			if (!input.HasValue)
				return true;

			if (input.Value == Guid.Empty)
				return true;

			return false;
		}

		public static bool HasValue(this Guid input)
		{
			return !IsEmpty(input);
		}

		public static bool IsEmpty(this Guid input)
		{
			return input == Guid.Empty;
		}
	}
}