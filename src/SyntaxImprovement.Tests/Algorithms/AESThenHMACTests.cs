using oledid.SyntaxImprovement.Algorithms.Cryptography;
using System;
using Xunit;

namespace oledid.SyntaxImprovement.Tests.Algorithms
{
	public class AESThenHMACTests
	{
		[Fact]
		public static void It_works()
		{
			{
				var secretMessage = "Cryptography is hard.";
				var password = Guid.NewGuid().ToString();
				var encrypted = AESThenHMAC.SimpleEncryptWithPassword(secretMessage, password);
				var decrypted = AESThenHMAC.SimpleDecryptWithPassword(encrypted, password);
				Assert.Equal(secretMessage, decrypted);
			}

			{
				var secretMessage = "Cryptography is hard.";
				var password = Guid.NewGuid().ToString();
				var encrypted = AESThenHMAC.SimpleEncryptWithPassword(secretMessage, password);
				var decrypted = AESThenHMAC.SimpleDecryptWithPassword(encrypted, "wrong password");
				Assert.NotEqual(secretMessage, decrypted);
			}
		}
	}
}
