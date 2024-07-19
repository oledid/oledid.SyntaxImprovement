using oledid.SyntaxImprovement.Extensions;
using System;
using System.Linq;

namespace oledid.SyntaxImprovement.Algorithms.Cryptography
{
	/// <summary>
	/// Caesar cipher, ROT13
	/// </summary>
	public class CaesarCipher
	{
		/// <summary>
		/// Shifts the characters in the string right (positive) or left (negative). Returns the shifted string in the same casing as it was.
		/// <para>Example: ShiftCharacters("AB", 1) == "BC"</para>
		/// <para>Example: ShiftCharacters("bC", -1) == "aB"</para>
		/// </summary>
		/// <param name="toEncrypt">The string to encrypt</param>
		/// <param name="shift">The number of characters to shift</param>
		public static string ShiftCharacters(string toEncrypt, int shift)
		{
			if (toEncrypt.Any(c => !c.IsLetterAtoZ()))
				throw new ArgumentException("String can only contain letters from A-Z.");

			var toEncryptUppercase = toEncrypt.ToUpperInvariant();
			const int alphabetLength = 'Z' - 'A' + 1;
			var result = new string(Array.ConvertAll(toEncryptUppercase.ToCharArray(), c => (char)('A' + (c - 'A' + shift) % alphabetLength)));
			return string.Join(string.Empty, toEncrypt.Select((c, i) => c.IsUppercaseLetterAtoZ() ? result[i].ToString().ToUpperInvariant() : result[i].ToString().ToLowerInvariant()));
		}
	}
}
