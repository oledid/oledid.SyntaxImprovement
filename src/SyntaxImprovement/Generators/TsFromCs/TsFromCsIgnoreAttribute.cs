using System;

namespace oledid.SyntaxImprovement.Generators.TsFromCs
{
	/// <summary>
	/// Stops generation of typescript for a class or property when using <see cref="TsFromCsGenerator"/>
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
	public class TsFromCsIgnoreAttribute : Attribute
	{
	}
}
