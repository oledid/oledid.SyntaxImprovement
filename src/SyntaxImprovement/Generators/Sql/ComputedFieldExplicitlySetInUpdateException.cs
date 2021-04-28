using System;
using System.Runtime.Serialization;

namespace oledid.SyntaxImprovement.Generators.Sql
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

		protected ComputedFieldExplicitlySetInUpdateException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
