using oledid.SyntaxImprovement.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace oledid.SyntaxImprovement
{
	/// <summary>
	/// Represents a month
	/// </summary>
	[DebuggerDisplay("[Month] Y: {Year}, M: {MonthNumber}")]
	[Serializable]
	public class Month : IComparable<Month>, IEquatable<Month>
	{
		public int Year { get; }
		public int MonthNumber { get; }

		public Month(int year, int month)
		{
			Year = year;
			MonthNumber = month;

			ValidateYear();
			ValidateMonth();
		}

		private void ValidateYear()
		{
			if (Year < 1 || Year > 9999)
				throw new ArgumentException("Invalid year: " + Year);
		}

		private void ValidateMonth()
		{
			if (MonthNumber < 1 || MonthNumber > 12)
				throw new ArgumentException("Invalid month number: " + MonthNumber);
		}

		private Month(int internalValue)
		{
			Year = internalValue / 12;
			MonthNumber = internalValue % 12 + 1;
		}

		private int InternalValue => Year * 12 + (MonthNumber - 1);

		public static implicit operator Month(DateTime date)
		{
			return new Month(date.Year, date.Month);
		}

		public static Month Current => DateTime.Today;

		public static IEnumerable<Month> EnumerateRange(Month from, Month to)
		{
			var month = (Month)from.MemberwiseClone();
			if (from <= to)
			{
				while (month <= to)
				{
					yield return month;
					month = month.Next();
				}
			}
			else
			{
				while (month >= to)
				{
					yield return month;
					month = month.Prev();
				}
			}
		}

		public static IEnumerable<Month> EnumerateCount(Month from, int count)
		{
			var delta = Math.Sign(count);
			var month = (Month)from.MemberwiseClone();
			for (var i = 0; i != count; i += delta)
			{
				yield return month;
				month = month.Add(delta);
			}
		}

		public Month Next()
		{
			return Add(1);
		}

		public Month Prev()
		{
			return Add(-1);
		}

		public Month Add(int monthCount, int yearCount = 0)
		{
			return new Month(InternalValue + (yearCount * 12) + monthCount);
		}

		public Month Subtract(int monthCount, int yearCount = 0)
		{
			return Add(-monthCount, -yearCount);
		}

		public Month AddYears(int count)
		{
			return Add(count * 12);
		}

		public Month SubtractYears(int count)
		{
			return Add(count * -12);
		}

		public DateTime FirstDayOfMonth => new DateTime(Year, MonthNumber, 1);

		public DateTime LastDayOfMonth => FirstDayOfMonth.AddMonths(1).AddDays(-1);

		public int NumberOfDays => LastDayOfMonth.Day;

		public static int GetNumberOfDays(int monthNumber)
		{
			return new Month(1, monthNumber).NumberOfDays;
		}

		public string MonthName => FirstDayOfMonth.ToString("MMMM").Capitalize();

		public static string GetMonthName(int monthNumber)
		{
			return new Month(1, monthNumber).MonthName;
		}

		public string ThreeLetterMonthName => FirstDayOfMonth.ToString("MMM").Capitalize();

		public static string GetThreeLetterMonthName(int monthNumber)
		{
			return new Month(1, monthNumber).ThreeLetterMonthName;
		}

		public string ValueString => FirstDayOfMonth.ToString("yyyyMM");

		public string ToString(string format, bool capitalize = true)
		{
			var str = FirstDayOfMonth.ToString(format);
			if (capitalize) str = str.Capitalize();
			return str;
		}

		public override string ToString()
		{
			return ValueString;
		}

		public static Month FromValueString(string str)
		{
			return DateTime.ParseExact(str, "yyyyMM", null);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return Year * 100 + MonthNumber;
			}
		}

		public override bool Equals(object obj)
		{
			return obj?.GetType() == GetType() && Equals((Month)obj);
		}

		public bool Equals(Month other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (ReferenceEquals(null, other)) return false;
			return (Year == other.Year && MonthNumber == other.MonthNumber);
		}

		public int CompareTo(Month other)
		{
			if (ReferenceEquals(this, other)) return 0;
			if (ReferenceEquals(null, other)) return 1;
			var result = Year.CompareTo(other.Year);
			return result != 0
				? result
				: MonthNumber.CompareTo(other.MonthNumber);
		}

		public static bool operator >(Month m1, Month m2)
		{
			return m1.CompareTo(m2) == 1;
		}

		public static bool operator <(Month m1, Month m2)
		{
			return m1.CompareTo(m2) == -1;
		}

		public static bool operator >=(Month m1, Month m2)
		{
			return m1.CompareTo(m2) != -1;
		}

		public static bool operator <=(Month m1, Month m2)
		{
			return m1.CompareTo(m2) != 1;
		}

		public static bool operator ==(Month m1, Month m2)
		{
			return ReferenceEquals(m1, null)
				? ReferenceEquals(m2, null)
				: m1.Equals(m2);
		}

		public static bool operator !=(Month m1, Month m2)
		{
			return !(m1 == m2);
		}

		public static int operator -(Month m1, Month m2)
		{
			return ((m1.Year - m2.Year) * 12) + (m1.MonthNumber - m2.MonthNumber);
		}
	}
}
