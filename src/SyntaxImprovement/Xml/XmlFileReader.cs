using System;
using System.IO;
using System.Xml.Serialization;

namespace oledid.SyntaxImprovement.Xml
{
	public static class XmlFileReader
	{
		public static T ReadAndDeserializeXml_SuppressExceptions<T>(string fileFullPath) where T : class
		{
			return ReadAndDeserializeXml_SuppressExceptions<T>(fileFullPath, out DateTime? lastWriteTime);
		}

		public static T ReadAndDeserializeXml_SuppressExceptions<T>(string fileFullPath, out DateTime? lastWriteTime) where T : class
		{
			T model = null;
			lastWriteTime = null;

			try
			{
				model = ReadAndDeserializeXml_SuppressExceptions<T>(fileFullPath, out lastWriteTime);
			}
			catch
			{
				//
			}

			return model;
		}

		public static T ReadAndDeserializeXml_ThrowExceptions<T>(string fileFullPath) where T : class
		{
			return ReadAndDeserializeXml_ThrowExceptions<T>(fileFullPath, out DateTime? lastWriteTime);
		}

		public static T ReadAndDeserializeXml_ThrowExceptions<T>(string fileFullPath, out DateTime? lastWriteTime) where T : class
		{
			Stream xmlStream = null;
			T xmlFile;

			try
			{
				xmlStream = new FileStream(fileFullPath, FileMode.Open, FileAccess.Read, FileShare.Read, 1024);

				var serializer = new XmlSerializer(typeof(T));
				xmlFile = (T)serializer.Deserialize(xmlStream);
				xmlStream.Close();

				lastWriteTime = File.GetLastWriteTime(fileFullPath);
			}
			finally
			{
				if (xmlStream != null)
				{
					xmlStream.Close();
					xmlStream.Dispose();
				}
			}

			return xmlFile;
		}

		public static T ReadAndDeserializeXml_ThrowExceptions<T>(System.Reflection.Assembly assembly, string resourceFullName)
		{
			using (var stream = assembly.GetManifestResourceStream(resourceFullName))
			{
				if (stream == null)
					throw new NotSupportedException("Could not open manifest resource stream.");

				var serializer = new XmlSerializer(typeof(T));
				return (T)serializer.Deserialize(stream);
			}
		}
	}
}
