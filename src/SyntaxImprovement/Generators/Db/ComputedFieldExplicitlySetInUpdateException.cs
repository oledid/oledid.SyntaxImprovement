using System;

namespace oledid.SyntaxImprovement.Generators.Db
{
	public class ComputedFieldExplicitlySetInUpdateException : Exception
	{
		public ComputedFieldExplicitlySetInUpdateException()
		{
		}

		public ComputedFieldExplicitlySetInUpdateException(string message) : base(message)
		{
		}

		public ComputedFieldExplicitlySetInUpdateException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
