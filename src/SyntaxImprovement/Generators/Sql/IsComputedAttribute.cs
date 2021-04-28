using System;

namespace oledid.SyntaxImprovement.Generators.Sql
{
	/// <summary>
	/// A computed field will be included in select, but cannot be updated.
	/// Will throw <see cref="ComputedFieldExplicitlySetInUpdateException"/> if explicitly updated.
	/// </summary>
	public class IsComputedAttribute : Attribute
	{
	}
}
