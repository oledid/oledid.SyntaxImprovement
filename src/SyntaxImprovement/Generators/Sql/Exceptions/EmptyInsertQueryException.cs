using System;

namespace oledid.SyntaxImprovement.Generators.Sql.Exceptions
{
	/// <summary>
	/// Thrown when <see cref="Insert{TableType}.ToQuery"/> is called before adding any items with <see cref="Insert{TableType}.Add(TableType, TableType[])"/>
	/// </summary>
	public class EmptyInsertQueryException : Exception
	{
		public EmptyInsertQueryException() : base("Cannot generate empty INSERT-query.")
		{
		}
	}
}
