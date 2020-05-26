using oledid.SyntaxImprovement.Reflection;
using Xunit;

namespace oledid.SyntaxImprovement.Tests.Reflection
{
	public class ObjectComparerTests
	{
		public class GetParametersWithValueDifferenceTests
		{
			public void It_detects_differences()
			{
				var a = new TestModelOne
				{
					Id = 1,
					Name = "A"
				};

				var b = new TestModelOne
				{
					Id = 2,
					Name = "B"
				};

				var comparison = ObjectComparer.GetParametersWithValueDifference(a, b);
				Assert.Contains(comparison, item => item.Property.Name == nameof(TestModelOne.Id));
				Assert.Contains(comparison, item => item.Property.Name == nameof(TestModelOne.Name));
				Assert.Equal(2, comparison.Count);
			}

			public void It_ignores_equal_fields()
			{
				var a = new TestModelOne
				{
					Id = 1,
					Name = "One"
				};

				var b = new TestModelOne
				{
					Id = 2,
					Name = "One"
				};

				var comparison = ObjectComparer.GetParametersWithValueDifference(a, b);
				Assert.Contains(comparison, item => item.Property.Name == nameof(TestModelOne.Id));
				Assert.Single(comparison);
			}

			public class TestModelOne
			{
				public int Id { get; set; }
				public string Name { get; set; }
			}
		}
	}
}
