﻿using System.IO;
using System.Text;

namespace oledid.SyntaxImprovement.Text;

public class Utf8StringWriter : StringWriter
{
	public override Encoding Encoding => Encoding.UTF8;
}
