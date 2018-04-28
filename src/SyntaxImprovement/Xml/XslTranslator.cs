using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace oledid.SyntaxImprovement.Xml
{
	public static class XslTranslator
	{
		public static string ConvertDataSetToHtml(DataSet dataSet, string xsl, Encoding encoding = null)
		{
			if (encoding == null)
				encoding = Encoding.UTF8;

			var xslTransform = new XslCompiledTransform();

			using (var xslStream = new MemoryStream(encoding.GetBytes(xsl)))
			using (var xslReader = new XmlTextReader(xslStream))
			{
				xslTransform.Load(xslReader);
			}

			using (var dataSetOutputStream = new MemoryStream())
			using (var dataSetXmlWriter = new XmlTextWriter(dataSetOutputStream, encoding))
			{
				dataSet.WriteXml(dataSetXmlWriter);
				dataSetOutputStream.Position = 0;

				var stringBuilder = new StringBuilder();

				var outputSettings = CreateOutputSettings(xslTransform.OutputSettings);

				using (var dataSetXml = new XmlTextReader(dataSetOutputStream))
				using (var resultWriter = XmlWriter.Create(stringBuilder, outputSettings))
				{
					xslTransform.Transform(dataSetXml, resultWriter);
					return stringBuilder.ToString();
				}
			}
		}

		private static XmlWriterSettings CreateOutputSettings(XmlWriterSettings settingsFromReader)
		{
			return new XmlWriterSettings
			{
				Async = settingsFromReader.Async,
				CheckCharacters = settingsFromReader.CheckCharacters,
				CloseOutput = settingsFromReader.CloseOutput,
				ConformanceLevel = settingsFromReader.ConformanceLevel,
				DoNotEscapeUriAttributes = settingsFromReader.DoNotEscapeUriAttributes,
				Encoding = settingsFromReader.Encoding,
				Indent = settingsFromReader.Indent,
				IndentChars = settingsFromReader.IndentChars,
				NamespaceHandling = settingsFromReader.NamespaceHandling,
				NewLineChars = settingsFromReader.NewLineChars,
				NewLineHandling = settingsFromReader.NewLineHandling,
				NewLineOnAttributes = settingsFromReader.NewLineOnAttributes,
				OmitXmlDeclaration = settingsFromReader.OutputMethod == XmlOutputMethod.AutoDetect || settingsFromReader.OmitXmlDeclaration,
				WriteEndDocumentOnClose = settingsFromReader.WriteEndDocumentOnClose
			};
		}
	}
}
