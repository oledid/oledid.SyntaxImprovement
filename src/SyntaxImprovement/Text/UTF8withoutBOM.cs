using System.Text;

namespace oledid.SyntaxImprovement.Text;

public class UTF8withoutBOM
{
	public static Encoding CreateNewInstance() => new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
}
