using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using oledid.SyntaxImprovement.Security;

namespace oledid.SyntaxImprovement
{
	public static class CompilerInformation
	{
		public static string GetUniqueKeyFromCallerInformation([CallerMemberName]string memberName = "", [CallerFilePath]string sourceFilePath = "", [CallerLineNumber]int sourceLineNumber = 0)
		{
			return GetUniqueKeyFromCustomCallerInformation(memberName, sourceFilePath, sourceLineNumber);
		}

		public static string GetUniqueKeyFromCustomCallerInformation(string memberName, string sourceFilePath, int sourceLineNumber)
		{
			sourceFilePath = RemoveDiskInformationFromSourceFilePath(sourceFilePath);
			return FastHashKey(string.Join("_", sourceFilePath, memberName, sourceLineNumber.ToString()));
		}

		private static string RemoveDiskInformationFromSourceFilePath(string sourceFilePath)
		{
			var baseDirectoryParts = AppDomain.CurrentDomain.BaseDirectory
				.Split('\\')
				.Reverse();

			var solutionFolder = baseDirectoryParts.ElementAtOrDefault(2);
			if (solutionFolder == null)
				return sourceFilePath;

			var solutionFolderIndex = sourceFilePath.IndexOf(solutionFolder, StringComparison.InvariantCulture);
			return solutionFolderIndex == -1
				? sourceFilePath
				: sourceFilePath.Substring(solutionFolderIndex);
		}

		private static string FastHashKey(string key)
		{
			const string salt = "SR\"[fG4:%i[eHnMiW,/\\ZRz7]5[+N@5i@JjQU>-K&#{+FU}Ah}wGs-iN\\mwA=Mma";
			return HashGenerator<MD5CryptoServiceProvider>.GetHashFromString(salt + key);
		}

		public static string GetCallerSourceFilePath([CallerFilePath] string sourceFilePath = "")
		{
			return sourceFilePath;
		}

		public static string GetCallerMemberName([CallerMemberName] string memberName = "")
		{
			return memberName;
		}

		public static int GetCallerSourceLineNumber([CallerLineNumber] int sourceLineNumber = 0)
		{
			return sourceLineNumber;
		}
	}
}