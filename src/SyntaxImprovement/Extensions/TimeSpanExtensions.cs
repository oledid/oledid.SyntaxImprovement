using System;
using System.Diagnostics.Contracts;

namespace oledid.SyntaxImprovement
{
	public static class TimeSpanExtensions
	{
		/// <summary>
		/// Sets specific time parts to zero.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="removeDays">Sets days to 0 if true</param>
		/// <param name="removeHours">Sets hours to 0 if true</param>
		/// <param name="removeMinutes">Sets minutes to 0 if true</param>
		/// <param name="removeSeconds">Sets seconds to 0 if true</param>
		/// <param name="removeMilliseconds">Sets milliseconds to 0 if true</param>
		[Pure]
		public static TimeSpan? RemoveTimeParts(this TimeSpan? value, bool removeDays = false, bool removeHours = false, bool removeMinutes = false, bool removeSeconds = false, bool removeMilliseconds = false)
		{
			return value?.RemoveTimeParts(removeDays, removeHours, removeMinutes, removeSeconds, removeMilliseconds);
		}

		/// <summary>
		/// Sets specific time parts to zero. Returns null if used on a nullable TimeSpan where value is null.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="removeDays">Sets days to 0 if true</param>
		/// <param name="removeHours">Sets hours to 0 if true</param>
		/// <param name="removeMinutes">Sets minutes to 0 if true</param>
		/// <param name="removeSeconds">Sets seconds to 0 if true</param>
		/// <param name="removeMilliseconds">Sets milliseconds to 0 if true</param>
		[Pure]
		public static TimeSpan RemoveTimeParts(this TimeSpan value, bool removeDays = false, bool removeHours = false, bool removeMinutes = false, bool removeSeconds = false, bool removeMilliseconds = false)
		{
			var days = removeDays ? 0 : value.Days;
			var hours = removeHours ? 0 : value.Hours;
			var minutes = removeMinutes ? 0 : value.Minutes;
			var seconds = removeSeconds ? 0 : value.Seconds;
			var milliseconds = removeMilliseconds ? 0 : value.Milliseconds;
			return new TimeSpan(days: days, hours: hours, minutes: minutes, seconds: seconds, milliseconds: milliseconds);
		}
	}
}
