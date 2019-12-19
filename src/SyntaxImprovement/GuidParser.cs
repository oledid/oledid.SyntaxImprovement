using System;

namespace oledid.SyntaxImprovement
{
	public static class GuidParser
	{
		public static Guid? ParseOrNull(string value)
		{
			return Guid.TryParse(value, out var guid)
				? guid
				: (Guid?)null;
		}
	}
}
