using System;
using oledid.SyntaxImprovement.Algorithms.Crypto;
using Xunit;

namespace oledid.SyntaxImprovement.Tests.Algorithms
{
	public class CaesarCipherTests
	{
		[Fact]
		public void TestRot13()
		{
			var secret = "OlEdId";
			var expected = "BYRQVQ";
			var actual = CaesarCipher.ShiftCharacters(secret, 13);
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void TestRot25()
		{
			var secret = "OLEDID";
			var expected = "NKDCHC";
			var actual = CaesarCipher.ShiftCharacters(secret, 25);
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void TestNegativeRot1()
		{
			var secret = "OLEDID";
			var expected = "NKDCHC";
			var actual = CaesarCipher.ShiftCharacters(secret, -1);
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void ThrowsIfNotLetter()
		{
			var secret = "OlEdId1";
			Assert.Throws<ArgumentException>(() => CaesarCipher.ShiftCharacters(secret, 13));
		}
	}
}
