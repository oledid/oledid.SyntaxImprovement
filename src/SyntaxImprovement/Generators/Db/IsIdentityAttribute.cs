using System;

namespace oledid.SyntaxImprovement.Generators.Db
{
	/// <summary>
	/// An identity column will be included in select, but will never be included in an update.
	/// </summary>
	public class IsIdentityAttribute : Attribute
	{
	}
}
