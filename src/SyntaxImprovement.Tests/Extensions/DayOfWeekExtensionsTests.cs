using oledid.SyntaxImprovement.Extensions;
using System;
using Xunit;

namespace oledid.SyntaxImprovement.Tests.Extensions
{
	public class DayOfWeekExtensionsTests
	{
		[Fact]
		public void IsWeekday()
		{
			{
				var expected = true;
				var actual = DayOfWeek.Monday.IsWeekday();
				Assert.Equal(expected, actual);
			}
			{
				var expected = true;
				var actual = DayOfWeek.Tuesday.IsWeekday();
				Assert.Equal(expected, actual);
			}
			{
				var expected = true;
				var actual = DayOfWeek.Wednesday.IsWeekday();
				Assert.Equal(expected, actual);
			}
			{
				var expected = true;
				var actual = DayOfWeek.Thursday.IsWeekday();
				Assert.Equal(expected, actual);
			}
			{
				var expected = true;
				var actual = DayOfWeek.Friday.IsWeekday();
				Assert.Equal(expected, actual);
			}
			{
				var expected = false;
				var actual = DayOfWeek.Saturday.IsWeekday();
				Assert.Equal(expected, actual);
			}
			{
				var expected = false;
				var actual = DayOfWeek.Sunday.IsWeekday();
				Assert.Equal(expected, actual);
			}
		}

		[Fact]
		public void IsWeekend()
		{
			{
				var expected = false;
				var actual = DayOfWeek.Monday.IsWeekend();
				Assert.Equal(expected, actual);
			}
			{
				var expected = false;
				var actual = DayOfWeek.Tuesday.IsWeekend();
				Assert.Equal(expected, actual);
			}
			{
				var expected = false;
				var actual = DayOfWeek.Wednesday.IsWeekend();
				Assert.Equal(expected, actual);
			}
			{
				var expected = false;
				var actual = DayOfWeek.Thursday.IsWeekend();
				Assert.Equal(expected, actual);
			}
			{
				var expected = false;
				var actual = DayOfWeek.Friday.IsWeekend();
				Assert.Equal(expected, actual);
			}
			{
				var expected = true;
				var actual = DayOfWeek.Saturday.IsWeekend();
				Assert.Equal(expected, actual);
			}
			{
				var expected = true;
				var actual = DayOfWeek.Sunday.IsWeekend();
				Assert.Equal(expected, actual);
			}
		}
	}
}