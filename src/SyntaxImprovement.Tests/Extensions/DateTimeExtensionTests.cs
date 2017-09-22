using System;
using Xunit;

namespace oledid.SyntaxImprovement.Tests.Extensions
{
	public class DateTimeExtensionTests
	{
		[Fact]
		public void ToUnixTimestampLocal()
		{
			const int expected = 1483228800;
			var actual = new DateTime(2017, 1, 1, 0, 0, 0, DateTimeKind.Local).ToUnixTimestamp();
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void ToUnixTimestampUtc()
		{
			const int expected = 1483228800;
			var actual = new DateTime(2017, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToUnixTimestamp();
			Assert.Equal(expected, actual);
		}
	}
}