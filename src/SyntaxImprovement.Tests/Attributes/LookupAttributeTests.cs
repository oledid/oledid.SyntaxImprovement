using oledid.SyntaxImprovement.Attributes;
using Xunit;

namespace oledid.SyntaxImprovement.Tests.Attributes
{
	public class LookupAttributeTests
	{
		public enum TestEnum
		{
			[Lookup("A", "1")]
			ItemWithOneValue,

			[Lookup("A", "1")]
			[Lookup("B", "2")]
			ItemWithTwoValues,

			ItemWithoutValues
		}

		[Fact]
		public void Can_get_stored_values()
		{
			{
				var expected = "1";
				var actual = LookupAttribute.GetValueFromAttribute(TestEnum.ItemWithOneValue, "A");
				Assert.Equal(expected, actual);
			}

			{
				var expected = "1";
				var actual = LookupAttribute.GetValueFromAttribute(TestEnum.ItemWithTwoValues, "A");
				Assert.Equal(expected, actual);
			}

			{
				var expected = "2";
				var actual = LookupAttribute.GetValueFromAttribute(TestEnum.ItemWithTwoValues, "B");
				Assert.Equal(expected, actual);
			}
		}

		[Fact]
		public void Returns_null_if_no_values_stored()
		{
			{
				string expected = null;
				var actual = LookupAttribute.GetValueFromAttribute(TestEnum.ItemWithOneValue, "B");
				Assert.Equal(expected, actual);
			}

			{
				string expected = null;
				var actual = LookupAttribute.GetValueFromAttribute(TestEnum.ItemWithoutValues, "C");
				Assert.Equal(expected, actual);
			}
		}
	}
}
