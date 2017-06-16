using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace oledid.SyntaxImprovement.Security
{
	public static class HashGenerator<T> where T : HashAlgorithm, new()
	{
		public static string GetHashFromString(string input)
		{
			using (var algorithm = new T())
			{
				var bytesIn = Encoding.UTF8.GetBytes(input);
				var bytesOut = algorithm.ComputeHash(bytesIn);
				var output = BitConverter.ToString(bytesOut);
				return output.ToLowerInvariant();
			}
		}

		public static string GetHashFromStream(Stream input)
		{
			using (var algorithm = new T())
			{
				var bytesOut = algorithm.ComputeHash(input);
				var output = BitConverter.ToString(bytesOut);
				return output.ToLowerInvariant();
			}
		}

		public static string GetHashFromStream(Stream input, int byteCount)
		{
			using (var algorithm = new T())
			{
				var buffer = new byte[byteCount];
				input.Read(buffer, offset: 0, count: byteCount);
				var bytesOut = algorithm.ComputeHash(buffer);
				var output = BitConverter.ToString(bytesOut);
				return output.ToLowerInvariant();
			}
		}
	}
}