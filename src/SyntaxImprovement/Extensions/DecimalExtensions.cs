using System;

namespace oledid.SyntaxImprovement
{
	public static class DecimalExtensions
	{
		public static decimal RoundDown(this decimal value, double numberOfDecimals)
		{
			var power = Convert.ToDecimal(Math.Pow(10, numberOfDecimals));
			return Math.Floor(value * power) / power;
		}

		public static decimal RoundUp(this decimal value, double numberOfDecimals)
		{
			var power = Convert.ToDecimal(Math.Pow(10, numberOfDecimals));
			return Math.Ceiling(value * power) / power;
		}
	}
}
