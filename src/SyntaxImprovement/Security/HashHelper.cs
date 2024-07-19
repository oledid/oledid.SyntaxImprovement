using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace oledid.SyntaxImprovement.Security
{
	public static class HashHelper
	{
		/// <summary>
		/// Usage: using var algo = HASH.Create(); return HashHelper.GetHashFromString(algo, input);
		/// </summary>
		public static string GetHashFromString(HashAlgorithm algorithm, string input)
		{
			var bytesIn = Encoding.UTF8.GetBytes(input);
			var bytesOut = algorithm.ComputeHash(bytesIn);
			var output = BitConverter.ToString(bytesOut);
			return output.ToLowerInvariant();
		}

		/// <summary>
		/// Usage: using var algo = HASH.Create(); return HashHelper.GetHashFromStream(algo, input);
		/// </summary>
		public static string GetHashFromStream(HashAlgorithm algorithm, Stream input)
		{
			var bytesOut = algorithm.ComputeHash(input);
			var output = BitConverter.ToString(bytesOut);
			return output.ToLowerInvariant();
		}

		/// <summary>
		/// Usage: using var algo = HASH.Create(); return HashHelper.GetHashFromStream(algo, input, numFirstBytesToBeRead);
		/// </summary>
		public static string GetHashFromStream(HashAlgorithm algorithm, Stream input, int numFirstBytesToBeRead)
		{
			var buffer = new byte[numFirstBytesToBeRead];
			input.Read(buffer, offset: 0, count: numFirstBytesToBeRead);
			var bytesOut = algorithm.ComputeHash(buffer);
			var output = BitConverter.ToString(bytesOut);
			return output.ToLowerInvariant();
		}
	}
}
