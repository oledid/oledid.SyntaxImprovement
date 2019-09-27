using System.Security.Claims;
using oledid.SyntaxImprovement.Extensions;
using Xunit;

namespace oledid.SyntaxImprovement.Tests.Extensions
{
	public class ClaimsIdentityExtensionsTests
	{
		[Fact]
		public void It_adds_or_updates_a_string_claim()
		{
			var ci1 = new ClaimsIdentity();

			ci1.AddClaim(new Claim("a", "1"));
			Assert.Equal("1", ci1.FindFirst(c => c.Type == "a").Value);
			ci1.SetClaim("a", "2");
			Assert.Equal("2", ci1.FindFirst(c => c.Type == "a").Value);

			Assert.Null(ci1.FindFirst(c => c.Type == "b"));
			ci1.SetClaim("b", "notnull");
			Assert.NotNull(ci1.FindFirst(c => c.Type == "b"));
			Assert.Equal("notnull", ci1.FindFirst(c => c.Type == "b").Value);
		}

		[Fact]
		public void It_adds_or_updates_a_bool_claim()
		{
			var ci1 = new ClaimsIdentity();

			ci1.AddClaim(new Claim("a", false.ToString()));
			Assert.Equal(false.ToString(), ci1.FindFirst(c => c.Type == "a").Value);
			ci1.SetClaim("a", true);
			Assert.Equal(true.ToString(), ci1.FindFirst(c => c.Type == "a").Value);

			Assert.Null(ci1.FindFirst(c => c.Type == "b"));
			ci1.SetClaim("b", true);
			Assert.NotNull(ci1.FindFirst(c => c.Type == "b"));
			Assert.Equal(true.ToString(), ci1.FindFirst(c => c.Type == "b").Value);
		}
	}
}
