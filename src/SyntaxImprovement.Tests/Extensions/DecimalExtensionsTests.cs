using Xunit;

namespace oledid.SyntaxImprovement.Tests.Extensions
{
	public class DecimalExtensionsTests
	{
		[Fact]
		public void TheRoundDownMethod()
		{
			const decimal value = 0.123456789m;

			Assert.NotEqual(0.12345m, value);

			Assert.Equal(0m, value.RoundDown(0));

			Assert.Equal(0.12345m, value.RoundDown(5));

			Assert.Equal(0.123456m, value.RoundDown(6));
		}

		[Fact]
		public void TheRoundUpMethod()
		{
			const decimal value = 0.123456789m;

			Assert.NotEqual(0.12345m, value);

			Assert.Equal(1m, value.RoundUp(0));

			Assert.Equal(0.124m, value.RoundUp(3));

			Assert.Equal(0.123457m, value.RoundUp(6));
		}
	}
}
