using System;
using System.IO;

namespace oledid.SyntaxImprovement
{
	public static class DirectoryInfoExtensions
	{
		public static FileInfo GetRandomNonExistantFilename(this DirectoryInfo directory, string extensionWithDot = ".tmp")
		{
			while (true)
			{
				var fileName = Path.Combine(directory.FullName, Guid.NewGuid() + extensionWithDot);
				var fileInfo = new FileInfo(fileName);
				if (fileInfo.Exists)
					continue;

				return fileInfo;
			}
		}
	}
}