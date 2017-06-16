using System;
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
	}
}