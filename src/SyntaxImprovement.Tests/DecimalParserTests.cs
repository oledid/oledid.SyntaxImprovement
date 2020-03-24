using Xunit;

namespace oledid.SyntaxImprovement.Tests
{
	public class DecimalParserTests
	{
		public class ParseOrNull
		{
			[Fact]
			public void It_returns_null_if_parsing_empty_or_whitespace_or_invalid_string()
			{
				Assert.Null(DecimalParser.ParseOrNull(string.Empty));
				Assert.Null(DecimalParser.ParseOrNull(" "));
				Assert.Null(DecimalParser.ParseOrNull("\t\n    "));
				Assert.Null(DecimalParser.ParseOrNull(null));
				Assert.Null(DecimalParser.ParseOrNull("abc"));
				Assert.Null(DecimalParser.ParseOrNull("             0", returnNullIfZero: true));
				Assert.Null(DecimalParser.ParseOrNull("0.0000000000", returnNullIfZero: true));
				Assert.Null(DecimalParser.ParseOrNull("             0x"));
			}

			[Fact]
			public void It_parses_integers()
			{
				Assert.Equal(1234m, DecimalParser.ParseOrNull("1234"));
				Assert.Equal(0m, DecimalParser.ParseOrNull("0"));
				Assert.Equal(0, DecimalParser.ParseOrNull("             0", returnNullIfZero: false));
				Assert.Equal(0, DecimalParser.ParseOrNull("             0"));
			}

			[Fact]
			public void It_parses_decimals()
			{
				Assert.Equal(37.3m, DecimalParser.ParseOrNull("37,3"));
				Assert.Equal(37.3m, DecimalParser.ParseOrNull("37.3"));
				Assert.Equal(0.12345678910m, DecimalParser.ParseOrNull("0.12345678910"));
				Assert.Equal(37.0m, DecimalParser.ParseOrNull("00000037.00000000"));
				Assert.Equal(-37.0m, DecimalParser.ParseOrNull("-00000037.00000000"));
				Assert.Equal(-37.0m, DecimalParser.ParseOrNull("-00000037,00000000"));
			}
		}
	}
}
