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

				using (var dataSetXml = new XmlTextReader(dataSetOutputStream))
				using (var resultOutputStream = new MemoryStream())
				using (var resultWriter = new XmlTextWriter(resultOutputStream, encoding))
				{
					xslTransform.Transform(dataSetXml, resultWriter);
					return encoding.GetString(resultOutputStream.ToArray());
				}
			}
		}
	}
}