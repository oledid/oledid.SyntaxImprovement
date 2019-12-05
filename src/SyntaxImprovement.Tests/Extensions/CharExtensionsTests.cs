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

		public class IsLetterAtoZ
		{
			[Fact]
			public void It_returns_true_if_the_char_is_a_letter_between_a_and_Z()
			{
				Assert.True('A'.IsLetterAtoZ());
				Assert.True('b'.IsLetterAtoZ());
				Assert.True('C'.IsLetterAtoZ());
				Assert.True('d'.IsLetterAtoZ());
				Assert.True('E'.IsLetterAtoZ());
				Assert.True('f'.IsLetterAtoZ());
				Assert.True('G'.IsLetterAtoZ());
				Assert.True('h'.IsLetterAtoZ());
				Assert.True('I'.IsLetterAtoZ());
				Assert.True('j'.IsLetterAtoZ());
				Assert.True('K'.IsLetterAtoZ());
				Assert.True('l'.IsLetterAtoZ());
				Assert.True('M'.IsLetterAtoZ());
				Assert.True('n'.IsLetterAtoZ());
				Assert.True('O'.IsLetterAtoZ());
				Assert.True('p'.IsLetterAtoZ());
				Assert.True('Q'.IsLetterAtoZ());
				Assert.True('r'.IsLetterAtoZ());
				Assert.True('S'.IsLetterAtoZ());
				Assert.True('t'.IsLetterAtoZ());
				Assert.True('U'.IsLetterAtoZ());
				Assert.True('v'.IsLetterAtoZ());
				Assert.True('W'.IsLetterAtoZ());
				Assert.True('x'.IsLetterAtoZ());
				Assert.True('Y'.IsLetterAtoZ());
				Assert.True('z'.IsLetterAtoZ());
			}

			[Fact]
			public void It_returns_false_if_the_char_is_not_a_letter_between_A_and_z()
			{
				Assert.False('1'.IsLetterAtoZ());
				Assert.False('5'.IsLetterAtoZ());
				Assert.False('-'.IsLetterAtoZ());
				Assert.False('æ'.IsLetterAtoZ());
				Assert.False('ø'.IsLetterAtoZ());
				Assert.False('å'.IsLetterAtoZ());
				Assert.False('.'.IsLetterAtoZ());
				Assert.False(','.IsLetterAtoZ());
				Assert.False('"'.IsLetterAtoZ());
				Assert.False('@'.IsLetterAtoZ());
			}
		}

		public class IsUppercaseAtoZ
		{
			[Fact]
			public void It_validates_correctly()
			{
				Assert.True('A'.IsUppercaseLetterAtoZ());
				Assert.True('Z'.IsUppercaseLetterAtoZ());
				Assert.False('a'.IsUppercaseLetterAtoZ());
				Assert.False('0'.IsUppercaseLetterAtoZ());
				Assert.False('Æ'.IsUppercaseLetterAtoZ());
			}
		}

		public class IsLowercaseAtoZ
		{
			[Fact]
			public void It_validates_correctly()
			{
				Assert.True('a'.IsLowercaseLetterAtoZ());
				Assert.True('z'.IsLowercaseLetterAtoZ());
				Assert.False('A'.IsLowercaseLetterAtoZ());
				Assert.False('0'.IsLowercaseLetterAtoZ());
				Assert.False('æ'.IsLowercaseLetterAtoZ());
			}
		}
	}
}
