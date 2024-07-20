using System;
using System.Linq;

namespace oledid.SyntaxImprovement.Algorithms.SocialSecurityNumbers
{
	public static class NorwegianIdNumberParser
	{
		public static bool IsDNumber(string number)
		{
			return number != null && number.Length == 11 && number[0].In("456789") && number.All(c => c.IsNumber());
		}

		public static bool IsDufNumber(string number)
		{
			return number != null && number.Length == 12 && number.All(c => c.IsNumber()) && (number.StartsWith("19") || number.StartsWith("20"));
		}

		public static bool IsFnr(string number)
		{
			var truth = number != null && !IsDNumber(number) && !IsDufNumber(number) && number.Length == 11 && number.All(c => c.IsNumber());

			if (!truth)
			{
				return false;
			}

			var controlChar1 = 11 - ((
				(3 * int.Parse(number[0].ToString()))
				+ (7 * int.Parse(number[1].ToString()))
				+ (6 * int.Parse(number[2].ToString()))
				+ (1 * int.Parse(number[3].ToString()))
				+ (8 * int.Parse(number[4].ToString()))
				+ (9 * int.Parse(number[5].ToString()))
				+ (4 * int.Parse(number[6].ToString()))
				+ (5 * int.Parse(number[7].ToString()))
				+ (2 * int.Parse(number[8].ToString()))) % 11);
			truth = truth && (controlChar1 == int.Parse(number[9].ToString()) || controlChar1 == 11 && number[9].ToString() == "0");

			var controlChar2 = 11 - ((
				(5 * int.Parse(number[0].ToString()))
				+ (4 * int.Parse(number[1].ToString()))
				+ (3 * int.Parse(number[2].ToString()))
				+ (2 * int.Parse(number[3].ToString()))
				+ (7 * int.Parse(number[4].ToString()))
				+ (6 * int.Parse(number[5].ToString()))
				+ (5 * int.Parse(number[6].ToString()))
				+ (4 * int.Parse(number[7].ToString()))
				+ (3 * int.Parse(number[8].ToString()))
				+ (2 * int.Parse(number[9].ToString()))) % 11);
			truth = truth && (controlChar2 == int.Parse(number[10].ToString()) || controlChar2 == 11 && number[10].ToString() == "0");

			return truth;
		}

		public static int? GuessAgeOrNull(string number, DateTime? overrideNow = null)
		{
			var today = overrideNow ?? DateTime.Today;
			var now = LongParser.ParseOrNull(today.ToString("yyyyMMdd"));
			var then = LongParser.ParseOrNull(GuessBirthDate(number, today)?.ToString("yyyyMMdd"));

			return now.HasValue && then.HasValue
				? (int)Math.Floor(((decimal)now.Value - then.Value) / 10000m)
				: (int?)null;
		}

		public static bool? IsMale(string fnr)
		{
			return IsFnr(fnr) ? IntParser.ParseOrNull(fnr.Substring(8, 1)) % 2 != 0 : (bool?)null;
		}

		public static bool? IsFemale(string fnr)
		{
			var isMale = IsMale(fnr);
			return isMale == null ? null : !isMale;
		}

		public static DateTime? GuessBirthDate(string number, DateTime? overrideNow = null)
		{
			if (IsFnr(number) == false && IsDNumber(number) == false)
			{
				return null;
			}

			var d = IntParser.ParseOrNull(number.Substring(0, 2));
			var m = IntParser.ParseOrNull(number.Substring(2, 2));
			var y = IntParser.ParseOrNull(number.Substring(4, 2));

			if (IsDNumber(number))
			{
				d -= 40;
			}

			var birthDate = DateTimeParser.ParseOrNull(d.ToString().PadLeft(2, '0') + m.ToString().PadLeft(2, '0') + "19" + y.ToString().PadLeft(2, '0'), "ddMMyyyy");
			if (birthDate.HasValue == false)
				return null;

			const int maxYearsGuess = 100;
			var today = overrideNow ?? DateTime.Today;
			if (today.Year - birthDate.Value.Year >= maxYearsGuess)
			{
				birthDate = birthDate.Value.AddYears(maxYearsGuess);
			}

			return birthDate;
		}
	}
}
