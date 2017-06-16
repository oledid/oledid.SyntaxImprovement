using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace oledid.SyntaxImprovement
{
	/// <summary>
	/// Represents a (ISO8601) week
	/// </summary>
	[DebuggerDisplay("[Week] Y: {Year}, W: {WeekNumber}")]
	[Serializable]
	public class Week : IComparable<Week>, IEquatable<Week>
	{
		public int Year { get; }
		public int WeekNumber { get; }

		public Week(int year, int weekNumber)
		{
			Year = year;
			WeekNumber = weekNumber;
			ValidateYear();
			ValidateWeek();
		}

		private void ValidateYear()
		{
			if (Year < 1 || Year > 9999)
				throw new ArgumentException("Invalid year: " + Year);
		}

		private void ValidateWeek()
		{
			if (WeekNumber < 1 || WeekNumber > GetNumberOfWeeks(Year))
				throw new ArgumentException("Invalid week number. Year: " + Year + ", Week: " + WeekNumber);
		}

		public static implicit operator Week(DateTime date)
		{
			GetWeekAndYearForDate_ISO8601(date, out int year, out int week);
			return new Week(year, week);
		}

		private static void GetWeekAndYearForDate_ISO8601(DateTime date, out int year, out int week)
		{
			var januaryFirst = new DateTime(date.Year, 1, 1);
			var dayOfWeekForJanuaryFirst = (int)(januaryFirst.DayOfWeek + 6) % 7; // 0 = Monday, 6 = Sunday

			year = date.Year;
			week = (date.DayOfYear + dayOfWeekForJanuaryFirst - 1) / 7;

			if (dayOfWeekForJanuaryFirst <= 3) // 3 = Thursday
			{
				week++;
			}

			if (week == 0)
			{
				year--;
				week = GetNumberOfWeeks(year);
			}
			else if (week == 53 && GetNumberOfWeeks(year) == 52)
			{
				year++;
				week = 1;
			}
		}

		public static Week Current => DateTime.Today;

		public static IEnumerable<Week> EnumerateRange(Week wFrom, Week wTo)
		{
			var week = (Week)wFrom.MemberwiseClone();
			if (wFrom <= wTo)
			{
				while (week <= wTo)
				{
					yield return week;
					week = week.Next();
				}
			}
			else
			{
				while (week >= wTo)
				{
					yield return week;
					week = week.Prev();
				}
			}
		}

		public static IEnumerable<Week> EnumerateCount(Week wFrom, int count)
		{
			var forward = (count > 0);
			count = Math.Abs(count);
			var week = (Week)wFrom.MemberwiseClone();
			for (var i = 0; i != count; i += count)
			{
				yield return week;
				week = forward
					? week.Next()
					: week.Prev();
			}
		}

		public IEnumerable<DateTime> EnumerateDays()
		{
			var date = FirstDayOfWeek;
			for (var i = 0; i < 7; ++i)
			{
				yield return date;
				date = date.AddDays(1);
			}
		}

		public Week Next()
		{
			var newYear = Year;
			var newWeek = WeekNumber + 1;
			if (newWeek > GetNumberOfWeeks(newYear))
			{
				newYear++;
				newWeek = 1;
			}
			return new Week(newYear, newWeek);
		}

		public Week Prev()
		{
			var newYear = Year;
			var newWeek = WeekNumber - 1;
			if (newWeek == 0)
			{
				newYear--;
				newWeek = GetNumberOfWeeks(newYear);
			}
			return new Week(newYear, newWeek);
		}

		public Week Add(int count)
		{
			var week = new Week(Year, WeekNumber);
			if (count > 0)
			{
				while (count-- > 0)
				{
					week = week.Next();
				}
			}
			else if (count < 0)
			{
				while (count++ < 0)
				{
					week = week.Prev();
				}
			}
			return week;
		}

		public DateTime FirstDayOfWeek => GetDay(DayOfWeek.Monday);

		public DateTime LastDayOfWeek => FirstDayOfWeek.AddDays(6);

		public string ValueString => Year.ToString().PadLeft(4, '0') + WeekNumber.ToString().PadLeft(2, '0');

		public DateTime GetDay(DayOfWeek requestedDay)
		{
			var returnDate = new DateTime(Year, 1, 1);
			var week = ((Week)returnDate).WeekNumber;

			returnDate = week == 1
				? returnDate.AddDays((WeekNumber - 1) * 7)
				: returnDate.AddDays(WeekNumber * 7);

			var daysToSubtract = Convert.ToInt32(returnDate.DayOfWeek);
			daysToSubtract = (daysToSubtract == 0)
				? 7
				: daysToSubtract;

			returnDate = returnDate.AddDays(-daysToSubtract + 1);

			var requestedDayNumber = Convert.ToInt32(requestedDay);
			requestedDayNumber = requestedDayNumber == 0
				? 7
				: requestedDayNumber;
			returnDate = returnDate.AddDays(requestedDayNumber - 1);

			return returnDate;
		}

		public override string ToString()
		{
			return ValueString;
		}

		public static Week FromValueString(string str)
		{
			if (str.Length != 6) throw new ArgumentException("Invalid length of week valuestring.");
			var year = int.Parse(str.Substring(0, 4));
			var week = int.Parse(str.Substring(4, 2));
			return new Week(year, week);
		}

		public static int GetNumberOfWeeks(int year)
		{
			if (new DateTime(year, 1, 1).DayOfWeek == DayOfWeek.Thursday) return 53;
			if (new DateTime(year, 12, 31).DayOfWeek == DayOfWeek.Thursday) return 53;
			return 52;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Year * 100) + WeekNumber;
			}
		}

		public override bool Equals(object obj)
		{
			return obj?.GetType() == GetType() && Equals((Week)obj);
		}

		public bool Equals(Week other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (ReferenceEquals(null, other)) return false;
			return Year == other.Year && WeekNumber == other.WeekNumber;
		}

		public int CompareTo(Week other)
		{
			if (ReferenceEquals(this, other)) return 0;
			if (ReferenceEquals(null, other)) return 1;
			var result = Year.CompareTo(other.Year);
			return result != 0 ? result : WeekNumber.CompareTo(other.WeekNumber);
		}

		public static bool operator >(Week w1, Week w2)
		{
			return w1.CompareTo(w2) == 1;
		}

		public static bool operator <(Week w1, Week w2)
		{
			return w1.CompareTo(w2) == -1;
		}

		public static bool operator >=(Week w1, Week w2)
		{
			return w1.CompareTo(w2) != -1;
		}

		public static bool operator <=(Week w1, Week w2)
		{
			return w1.CompareTo(w2) != 1;
		}

		public static bool operator ==(Week w1, Week w2)
		{
			return ReferenceEquals(w1, null)
				? ReferenceEquals(w2, null)
				: w1.Equals(w2);
		}

		public static bool operator !=(Week w1, Week w2)
		{
			return !(w1 == w2);
		}
	}
}
