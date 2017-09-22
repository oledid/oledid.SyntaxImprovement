using System;
using Xunit;

namespace oledid.SyntaxImprovement.Tests.Extensions
{
	public class DateTimeExtensionTests
	{
		[Fact]
		public void FirstDayOfYear()
		{
			var expected = new DateTime(2017, 1, 1);
			var actual = new DateTime(2017, 3, 7).FirstDayOfYear();
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void LastDayOfYear()
		{
			var expected = new DateTime(2017, 12, 31);
			var actual = new DateTime(2017, 3, 7).LastDayOfYear();
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void FirstDayOfMonth()
		{
			var expected = new DateTime(2017, 1, 1);
			var actual = new DateTime(2017, 1, 20).FirstDayOfMonth();
			Assert.Equal(expected, actual);
		}


		[Fact]
		public void LastDayOfMonth()
		{
			{
				var expected = new DateTime(2016, 2, 29);
				var actual = new DateTime(2016, 2, 1).LastDayOfMonth();
				Assert.Equal(expected, actual);
			}
			{
				var expected = new DateTime(2015, 2, 28);
				var actual = new DateTime(2015, 2, 1).LastDayOfMonth();
				Assert.Equal(expected, actual);
			}
		}

		[Fact]
		public void EndOfDay()
		{
			var expected = new DateTime(2017, 1, 2).AddTicks(-1);
			var actual = new DateTime(2017, 1, 1).EndOfDay();
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void RemoveTimeParts()
		{
			{
				var expected = new DateTime(2017, 1, 1, 0, 3, 4);
				var actual = new DateTime(2017, 1, 1, 2, 3, 4).RemoveTimeParts(removeHours: true);
				Assert.Equal(expected, actual);
			}
			{
				var expected = new DateTime(2017, 1, 1, 2, 0, 4);
				var actual = new DateTime(2017, 1, 1, 2, 3, 4).RemoveTimeParts(removeMinutes: true);
				Assert.Equal(expected, actual);
			}
			{
				var expected = new DateTime(2017, 1, 1, 2, 3, 0);
				var actual = new DateTime(2017, 1, 1, 2, 3, 4).RemoveTimeParts(removeSeconds: true);
				Assert.Equal(expected, actual);
			}
			{
				var expected = new DateTime(2017, 1, 1, 2, 3, 4);
				var actual = new DateTime(2017, 1, 1, 2, 3, 4).AddMilliseconds(123).RemoveTimeParts(removeMilliseconds: true);
				Assert.Equal(expected, actual);
			}
			{
				var expected = (DateTime?)null;
				var actual = ((DateTime?)null).RemoveTimeParts(removeHours: true);
				Assert.Equal(expected, actual);
			}
		}

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