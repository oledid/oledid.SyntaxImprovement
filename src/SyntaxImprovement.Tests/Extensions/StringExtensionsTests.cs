using Xunit;

namespace oledid.SyntaxImprovement.Tests.Extensions
{
	public class StringExtensionsTests
	{
		public class AfterLast
		{
			[Fact]
			public void It_returns_text_after_last_str()
			{
				var text = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
				Assert.Equal(" industry.", text.AfterLast("typesetting"));
				Assert.Equal("typesetting industry.", text.AfterLast("typesetting", includeMatchStringInResult: true));
				Assert.Equal(" industry.", text.AfterLast("typesetting", includeMatchStringInResult: false));
			}

			[Fact]
			public void It_returns_text_after_last_char()
			{
				var text = "https://domain.com/page.html";
				Assert.Equal("page.html", text.AfterLast('/'));
				Assert.Equal("/page.html", text.AfterLast('/', includeMatchCharInResult: true));
				Assert.Equal("page.html", text.AfterLast('/', includeMatchCharInResult: false));
			}
		}

		public class AfterFirst
		{
			[Fact]
			public void It_returns_text_after_first_str()
			{
				var text = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
				Assert.Equal(" industry. Lorem Ipsum is simply dummy text of the printing and typesetting industry.", text.AfterFirst("typesetting"));
				Assert.Equal("typesetting industry. Lorem Ipsum is simply dummy text of the printing and typesetting industry.", text.AfterFirst("typesetting", includeMatchStringInResult: true));
				Assert.Equal(" industry. Lorem Ipsum is simply dummy text of the printing and typesetting industry.", text.AfterFirst("typesetting", includeMatchStringInResult: false));
			}

			[Fact]
			public void It_returns_text_after_first_char()
			{
				var text = "https://domain.com/page.html";
				Assert.Equal("/domain.com/page.html", text.AfterFirst('/'));
				Assert.Equal("//domain.com/page.html", text.AfterFirst('/', includeMatchCharInResult: true));
				Assert.Equal("/domain.com/page.html", text.AfterFirst('/', includeMatchCharInResult: false));
			}
		}
	}
}
