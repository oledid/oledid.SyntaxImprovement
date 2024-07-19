using oledid.SyntaxImprovement.Algorithms.Cryptography;
using oledid.SyntaxImprovement.Tests.Algorithms.Deprecated;
using System;
using System.Security.Cryptography;
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
				var encrypted = AESThenHMAC.SimpleEncryptWithPassword(secretMessage, password, HashAlgorithmName.SHA256);
				var decrypted = AESThenHMAC.SimpleDecryptWithPassword(encrypted, password, HashAlgorithmName.SHA256);
				Assert.Equal(secretMessage, decrypted);
			}

			{
				var secretMessage = "Cryptography is hard.";
				var password = Guid.NewGuid().ToString();
				var encrypted = AESThenHMAC.SimpleEncryptWithPassword(secretMessage, password, HashAlgorithmName.SHA256);
				var decrypted = AESThenHMAC.SimpleDecryptWithPassword(encrypted, "wrong password", HashAlgorithmName.SHA256);
				Assert.NotEqual(secretMessage, decrypted);
			}
		}

		[Fact]
		public static void It_still_works()
		{
			{
				var secretMessage = "Cryptography is hard.";
				var password = Guid.NewGuid().ToString();
				var encrypted1 = AESThenHMAC.SimpleEncryptWithPassword(secretMessage, password, HashAlgorithmName.SHA1);
				var decrypted1 = AESThenHMAC.SimpleDecryptWithPassword(encrypted1, password, HashAlgorithmName.SHA1);
				var decrypted2 = AESThenHMACDeprecated.SimpleDecryptWithPassword(encrypted1, password);
				Assert.Equal(secretMessage, decrypted1);
				Assert.Equal(secretMessage, decrypted2);
			}

			{
				var secretMessage = "Cryptography is hard.";
				var password = Guid.NewGuid().ToString();

				var encrypted = AESThenHMAC.SimpleEncryptWithPassword(secretMessage, password, HashAlgorithmName.SHA1);

				var decrypted1 = AESThenHMAC.SimpleDecryptWithPassword(encrypted, "wrong password", HashAlgorithmName.SHA1);
				var decrypted2 = AESThenHMACDeprecated.SimpleDecryptWithPassword(encrypted, "wrong password");
				var decrypted3 = AESThenHMAC.SimpleDecryptWithPassword(encrypted, password, HashAlgorithmName.SHA1);
				var decrypted4 = AESThenHMAC.SimpleDecryptWithPassword(encrypted, password, HashAlgorithmName.SHA1);

				Assert.NotEqual(secretMessage, decrypted1);
				Assert.NotEqual(secretMessage, decrypted2);
				Assert.Equal(secretMessage, decrypted3);
				Assert.Equal(secretMessage, decrypted4);
			}
		}
	}
}
