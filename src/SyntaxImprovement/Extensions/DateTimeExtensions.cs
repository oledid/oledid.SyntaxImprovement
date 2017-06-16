using System;
using System.Diagnostics.Contracts;

namespace oledid.SyntaxImprovement
{
	public static class DateTimeExtensions
	{
		[Pure]
		public static DateTime FirstDayOfYear(this DateTime date)
		{
			return new DateTime(date.Year, 1, 1);
		}

		[Pure]
		public static DateTime LastDayOfYear(this DateTime date)
		{
			return new DateTime(date.Year, 12, 31);
		}

		[Pure]
		public static DateTime FirstDayOfMonth(this DateTime date)
		{
			return new DateTime(date.Year, date.Month, 1);
		}

		[Pure]
		public static DateTime LastDayOfMonth(this DateTime date)
		{
			return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
		}

		/// <summary>
		/// Returns the last possible fraction of a millisecond of the specified date.
		/// </summary>
		[Pure]
		public static DateTime EndOfDay(this DateTime date)
		{
			return date.Date.AddDays(1).Subtract(TimeSpan.FromTicks(1));
		}

		/// <summary>
		/// Sets specific time parts to zero.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="removeHours">Sets hours to 0 if true</param>
		/// <param name="removeMinutes">Sets minutes to 0 if true</param>
		/// <param name="removeSeconds">Sets seconds to 0 if true</param>
		/// <param name="removeMilliseconds">Sets milliseconds to 0 if true</param>
		/// <returns></returns>
		[Pure]
		public static DateTime RemoveTimeParts(this DateTime value, bool removeHours = false, bool removeMinutes = false, bool removeSeconds = false, bool removeMilliseconds = false)
		{
			var newValue = new DateTime(value.Ticks);

			if (removeHours)
			{
				newValue = newValue.AddHours(-value.Hour);
			}

			if (removeMinutes)
			{
				newValue = newValue.AddMinutes(-value.Minute);
			}

			if (removeSeconds)
			{
				newValue = newValue.AddSeconds(-value.Second);
			}

			if (removeMilliseconds)
			{
				newValue = newValue.AddMilliseconds(-value.Millisecond);
			}

			return newValue;
		}

		/// <summary>
		/// Sets specific time parts to zero. Returns null if used on a nullable DateTime where value is null.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="removeHours">Sets hours to 0 if true</param>
		/// <param name="removeMinutes">Sets minutes to 0 if true</param>
		/// <param name="removeSeconds">Sets seconds to 0 if true</param>
		/// <param name="removeMilliseconds">Sets milliseconds to 0 if true</param>
		/// <returns></returns>
		[Pure]
		public static DateTime? RemoveTimeParts(this DateTime? value, bool removeHours = false, bool removeMinutes = false, bool removeSeconds = false, bool removeMilliseconds = false)
		{
			if (value.HasValue == false)
				return null;

			return value.Value.RemoveTimeParts(
				removeHours: removeHours,
				removeMinutes: removeMinutes,
				removeSeconds: removeSeconds,
				removeMilliseconds: removeMilliseconds);
		}
	}
}