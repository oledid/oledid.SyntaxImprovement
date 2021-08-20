using System;
using System.Runtime.Serialization;

namespace oledid.SyntaxImprovement.Generators.Sqlite
{
	public class SqliteComputedFieldExplicitlySetInUpdateException : Exception
	{
		public SqliteComputedFieldExplicitlySetInUpdateException()
		{
		}

		public SqliteComputedFieldExplicitlySetInUpdateException(string message) : base(message)
		{
		}

		public SqliteComputedFieldExplicitlySetInUpdateException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected SqliteComputedFieldExplicitlySetInUpdateException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
