using System.IO;

// https://stackoverflow.com/questions/1701457/directory-delete-doesnt-work-access-denied-error-but-under-windows-explorer-it

namespace oledid.SyntaxImprovement.IO
{
	public static class DirectoryDeleter
	{
		/// <summary>
		/// Deletes all files in the folder recursive. Also removes the target folder.
		/// </summary>
		public static void DeleteRecursive(string fullPath) => DeleteRecursive(new DirectoryInfo(fullPath));

		/// <summary>
		/// Deletes all files in the folder recursive. Also removes the target folder.
		/// </summary>
		public static void DeleteRecursive(DirectoryInfo directoryInfo)
		{
			SetAttributesNormal(directoryInfo);
			Directory.Delete(directoryInfo.FullName, recursive: true);
		}

		private static void SetAttributesNormal(DirectoryInfo directoryInfo)
		{
			foreach (var file in directoryInfo.EnumerateFiles())
			{
				file.Attributes = FileAttributes.Normal;
			}

			foreach (var dir in directoryInfo.EnumerateDirectories())
			{
				SetAttributesNormal(dir);
			}
		}
	}
}
