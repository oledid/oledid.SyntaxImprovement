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

		public class RemoveFromEnd
		{
			[Fact]
			public void It_ignores_if_it_does_not_end_with()
			{
				var text = "C:\\Temp"; ;
				Assert.Equal(text, text.RemoveFromEnd("\\"));
				Assert.Equal(text, text.RemoveFromEnd('\\'));
				Assert.NotEqual(text, text.RemoveFromEnd((int)'\\'));
				Assert.Equal(text, text.RemoveFromEnd(0));
			}

			[Fact]
			public void It_removes_specified_if_it_ends_with()
			{
				var text = "C:\\Temp\\"; ;
				var expected = "C:\\Temp";
				Assert.Equal(expected, text.RemoveFromEnd("\\"));
				Assert.Equal(expected, text.RemoveFromEnd('\\'));
				Assert.NotEqual(expected, text.RemoveFromEnd((int)'\\'));
				Assert.Equal("C:\\", text.RemoveFromEnd("Temp\\"));
				Assert.Equal(text, text.RemoveFromEnd(0));
				Assert.Equal(expected, text.RemoveFromEnd(1));
			}
		}

		public class RemoveFromStart
		{
			[Fact]
			public void It_ignores_if_it_does_not_start_with()
			{
				var text = "C:\\Temp"; ;
				Assert.Equal(text, text.RemoveFromStart("\\"));
				Assert.Equal(text, text.RemoveFromStart('\\'));
				Assert.NotEqual(text, text.RemoveFromStart((int)'\\'));
				Assert.Equal(text, text.RemoveFromStart(0));
				Assert.Equal(string.Empty, text.RemoveFromEnd(100));
			}

			[Fact]
			public void It_removes_specified_if_it_starts_with()
			{
				var text = "C:\\Temp\\";
				Assert.Equal("\\Temp\\", text.RemoveFromStart("C:"));
				Assert.Equal(":\\Temp\\", text.RemoveFromStart('C'));
				Assert.NotEqual(":\\Temp\\", text.RemoveFromStart((int)'C'));
				Assert.Equal(text, text.RemoveFromStart(0));
				Assert.Equal(":\\Temp\\", text.RemoveFromStart(1));
				Assert.Equal(string.Empty, text.RemoveFromStart(100));
			}
		}
	}
}
