using oledid.SyntaxImprovement.Algorithms.SocialSecurityNumbers;
using System;
using Xunit;

// fnr-source: http://www.fnrinfo.no/Verktoy/FinnLovlige_Dato.aspx

namespace oledid.SyntaxImprovement.Tests.Algorithms
{
	public class IdNumberParserTests
	{
		public class GetAge
		{
			[Fact]
			public void It_guesses_age_from_fnr()
			{
				Assert.Equal(31, NorwegianIdNumberParser.GuessAgeOrNull(170188_00179.ToString(), new DateTime(2019, 12, 4)));
				Assert.Equal(30, NorwegianIdNumberParser.GuessAgeOrNull("07128849933", new DateTime(2019, 12, 4)));
			}

			[Fact]
			public void It_guesses_age_from_dnr()
			{
				Assert.Equal(31, NorwegianIdNumberParser.GuessAgeOrNull(570188_00179.ToString(), new DateTime(2019, 12, 4)));
				Assert.Equal(31, NorwegianIdNumberParser.GuessAgeOrNull("57018800179", new DateTime(2019, 12, 4)));
			}

			[Fact]
			public void It_cannot_calculate_from_dufNr()
			{
				Assert.Null(NorwegianIdNumberParser.GuessAgeOrNull(2012_586975_61L.ToString(), new DateTime(2019, 12, 4)));
			}
		}

		public class GetGenderFromFnr
		{
			[Fact]
			public void It_detects_male_from_fnr()
			{
				Assert.True(NorwegianIdNumberParser.IsMale(30121299913.ToString()));
				Assert.False(NorwegianIdNumberParser.IsMale(30121299832.ToString()));
				Assert.Null(NorwegianIdNumberParser.IsMale(2012_586975_61L.ToString()));
				Assert.Null(NorwegianIdNumberParser.IsMale("ABC123"));
			}

			[Fact]
			public void It_detects_female_from_fnr()
			{
				Assert.True(NorwegianIdNumberParser.IsFemale(30121299832.ToString()));
				Assert.False(NorwegianIdNumberParser.IsFemale(30121299913.ToString()));
				Assert.Null(NorwegianIdNumberParser.IsFemale(2012_586975_61L.ToString()));
				Assert.Null(NorwegianIdNumberParser.IsFemale("ABC123"));
			}
		}

		public class GuessBirthDateFromFnr
		{
			[Fact]
			public void It_guesses_birthdate_from_fnr()
			{
				var fakeToday = new DateTime(2019, 12, 5);
				Assert.NotEqual(new DateTime(1919, 12, 31), NorwegianIdNumberParser.GuessBirthDate("31121945196", fakeToday));
				Assert.Equal(new DateTime(1920, 1, 1), NorwegianIdNumberParser.GuessBirthDate("01012047610", fakeToday)); // earliest correct guess
				Assert.Equal(new DateTime(1925, 12, 1), NorwegianIdNumberParser.GuessBirthDate("01122548003", fakeToday));
				Assert.Equal(new DateTime(1950, 12, 1), NorwegianIdNumberParser.GuessBirthDate("01125046173", fakeToday));
				Assert.Equal(new DateTime(1975, 12, 1), NorwegianIdNumberParser.GuessBirthDate("01127546548", fakeToday));
				Assert.Equal(new DateTime(1999, 12, 1), NorwegianIdNumberParser.GuessBirthDate("01129945512", fakeToday));
				Assert.Equal(new DateTime(2000, 12, 1), NorwegianIdNumberParser.GuessBirthDate("01120088870", fakeToday));
				Assert.Equal(new DateTime(2018, 12, 1), NorwegianIdNumberParser.GuessBirthDate("01121897187", fakeToday));
				Assert.Equal(new DateTime(2019, 12, 4), NorwegianIdNumberParser.GuessBirthDate("04121998241", fakeToday));
				Assert.Equal(new DateTime(2019, 12, 5), NorwegianIdNumberParser.GuessBirthDate("05121997306", fakeToday));
				Assert.Equal(new DateTime(2019, 12, 31), NorwegianIdNumberParser.GuessBirthDate("31121998923", fakeToday)); // last correct guess
				Assert.NotEqual(new DateTime(2020, 1, 1), NorwegianIdNumberParser.GuessBirthDate("01012098266", fakeToday));
			}

			[Fact]
			public void It_guesses_birthdate_from_dnr()
			{
				var fakeToday = new DateTime(2019, 12, 5);
				Assert.NotEqual(new DateTime(1919, 12, 31), NorwegianIdNumberParser.GuessBirthDate("71121945196", fakeToday));
				Assert.Equal(new DateTime(1920, 1, 1), NorwegianIdNumberParser.GuessBirthDate("41012047610", fakeToday)); // earliest correct guess
				Assert.Equal(new DateTime(1925, 12, 1), NorwegianIdNumberParser.GuessBirthDate("41122548003", fakeToday));
				Assert.Equal(new DateTime(1950, 12, 1), NorwegianIdNumberParser.GuessBirthDate("41125046173", fakeToday));
				Assert.Equal(new DateTime(1975, 12, 1), NorwegianIdNumberParser.GuessBirthDate("41127546548", fakeToday));
				Assert.Equal(new DateTime(1999, 12, 1), NorwegianIdNumberParser.GuessBirthDate("41129945512", fakeToday));
				Assert.Equal(new DateTime(2000, 12, 1), NorwegianIdNumberParser.GuessBirthDate("41120088870", fakeToday));
				Assert.Equal(new DateTime(2018, 12, 1), NorwegianIdNumberParser.GuessBirthDate("41121897187", fakeToday));
				Assert.Equal(new DateTime(2019, 12, 4), NorwegianIdNumberParser.GuessBirthDate("44121998241", fakeToday));
				Assert.Equal(new DateTime(2019, 12, 5), NorwegianIdNumberParser.GuessBirthDate("45121997306", fakeToday));
				Assert.Equal(new DateTime(2019, 12, 31), NorwegianIdNumberParser.GuessBirthDate("71121998923", fakeToday)); // last correct guess
				Assert.NotEqual(new DateTime(2020, 1, 1), NorwegianIdNumberParser.GuessBirthDate("41012098266", fakeToday));
			}

			[Fact]
			public void It_cannot_guess_birthDate_from_duf_nr()
			{
				var fakeToday = new DateTime(2019, 12, 5);
				Assert.Null(NorwegianIdNumberParser.GuessBirthDate(2012_586975_61L.ToString(), fakeToday));
			}
		}
	}
}
