using System;

namespace oledid.SyntaxImprovement.Generators.TsFromCs
{
	[AttributeUsage(AttributeTargets.Class)]
	public class TsFromCsNameAttribute : Attribute
	{
		/// <summary>
		/// The name to use in the generated .ts, without I-prefix
		/// </summary>
		/// <param name="name"></param>
		public TsFromCsNameAttribute(string name)
		{
			Name = name;
		}

		public string Name { get; set; }
	}
}
