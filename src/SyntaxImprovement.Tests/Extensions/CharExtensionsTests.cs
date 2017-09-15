using Xunit;

namespace oledid.SyntaxImprovement.Tests.Extensions
{
	public class CharExtensionsTests
	{
		public class IsNumber
		{
			[Fact]
			public void It_returns_true_if_the_char_is_a_number()
			{
				Assert.True('0'.IsNumber());
				Assert.True('1'.IsNumber());
				Assert.True('2'.IsNumber());
				Assert.True('3'.IsNumber());
				Assert.True('4'.IsNumber());
				Assert.True('5'.IsNumber());
				Assert.True('6'.IsNumber());
				Assert.True('7'.IsNumber());
				Assert.True('8'.IsNumber());
				Assert.True('9'.IsNumber());
			}

			[Fact]
			public void It_returns_false_if_the_char_is_not_a_number()
			{
				Assert.False('a'.IsNumber());
				Assert.False('b'.IsNumber());
				Assert.False('c'.IsNumber());
				Assert.False('æ'.IsNumber());
				Assert.False('ø'.IsNumber());
				Assert.False('å'.IsNumber());
				Assert.False('.'.IsNumber());
				Assert.False(','.IsNumber());
				Assert.False('"'.IsNumber());
				Assert.False('@'.IsNumber());
			}
		}
	}
}