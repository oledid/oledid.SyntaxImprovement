using System;

namespace oledid.SyntaxImprovement.Generators.Sqlite
{
	/// <summary>
	/// A computed field will be included in select, but cannot be updated.
	/// Will throw <see cref="SqliteComputedFieldExplicitlySetInUpdateException"/> if explicitly updated.
	/// </summary>
	public class SqliteIsComputedAttribute : Attribute
	{
	}
}
