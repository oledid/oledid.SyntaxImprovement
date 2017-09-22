using System;
using Xunit;

namespace oledid.SyntaxImprovement.Tests
{
	public class DateTimeParserTests
	{
		[Fact]
		public void ParseOrNull_Null()
		{
			{
				var expected = (DateTime?)null;
				var actual = DateTimeParser.ParseOrNull(null, DateTimeFormats.yyyy_MM_dd_ISO);
				Assert.Equal(expected, actual);
			}

			{
				var expected = new DateTime(2017, 1, 1, 13, 37, 0);
				var actual = DateTimeParser.ParseOrNull("2017-01-01 13:37:00", "yyyy-MM-dd HH:mm:ss");
				Assert.Equal(expected, actual);
			}
		}

		[Fact]
		public void Parse()
		{
			Assert.Throws<ArgumentNullException>(() => DateTimeParser.Parse(null, DateTimeFormats.yyyy_MM_dd_ISO));

			Assert.Throws<ArgumentException>(() => DateTimeParser.Parse("1234", DateTimeFormats.yyyy_MM_dd_ISO));

			var expected = new DateTime(2017, 1, 1);
			var actual = DateTimeParser.Parse("2017-01-01", DateTimeFormats.yyyy_MM_dd_ISO);
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void FromUnixTimeStampToLocalTime()
		{
			var expected = new DateTime(2017, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();
			var actual = DateTimeParser.FromUnixTimeStampToLocalTime(1483228800);
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void FromUnixTimeStampToUtcTime()
		{
			var expected = new DateTime(2017, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			var actual = DateTimeParser.FromUnixTimeStampToUtcTime(1483228800);
			Assert.Equal(expected, actual);
		}
	}
}