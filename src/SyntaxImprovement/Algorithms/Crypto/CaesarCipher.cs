using System;
using System.Linq;

namespace oledid.SyntaxImprovement.Algorithms.Crypto
{
	/// <summary>
	/// Caesar cipher, ROT13
	/// </summary>
	public class CaesarCipher
	{
		/// <summary>
		/// Shifts the characters in the string right (positive) or left (negative). Returns the shifted string in uppercase.
		/// <para>Example: ShiftUppercase("AB", 1) == "BC"</para>
		/// <para>Example: ShiftUppercase("bc", -1) == "AB"</para>
		/// </summary>
		/// <param name="toEncrypt">The string to encrypt</param>
		/// <param name="shift">The number of characters to shift</param>
		public static string ShiftCharacters(string toEncrypt, int shift)
		{
			toEncrypt = toEncrypt.ToUpperInvariant();
			if (toEncrypt.Any(c => !c.IsLetterAtoZ()))
				throw new ArgumentException("String can only contain letters from A-Z.");

			const int alphabetLength = 'Z' - 'A' + 1;
			return new string(Array.ConvertAll(toEncrypt.ToCharArray(),
				c => (char)('A' + (c - 'A' + shift) % alphabetLength)));
		}
	}
}
