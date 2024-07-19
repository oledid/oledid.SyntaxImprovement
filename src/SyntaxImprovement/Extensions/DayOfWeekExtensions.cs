using System;

namespace oledid.SyntaxImprovement.Extensions
{
	public static class DayOfWeekExtensions
	{
		public static bool IsWeekday(this DayOfWeek day)
		{
			return day.IsWeekend() == false;
		}

		public static bool IsWeekend(this DayOfWeek day)
		{
			return day.In(DayOfWeek.Saturday, DayOfWeek.Sunday);
		}
	}
}