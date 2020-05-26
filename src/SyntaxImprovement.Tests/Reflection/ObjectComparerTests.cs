using oledid.SyntaxImprovement.Reflection;
using System;
using Xunit;

namespace oledid.SyntaxImprovement.Tests.Reflection
{
	public class ObjectComparerTests
	{
		public class GetParametersWithValueDifferenceTests
		{
			[Fact]
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

			[Fact]
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

			[Fact]
			public void It_does_not_crash_if_something_is_null()
			{
				{
					var a = new TestModelOne
					{
						Id = 1,
						Name = "One",
						Challenge = new TestModelTwo(),
					};

					var b = new TestModelOne
					{
						Id = 2,
						Name = "One"
					};

					var comparison = ObjectComparer.GetParametersWithValueDifference(a, b);
					Assert.Contains(comparison, item => item.Property.Name == nameof(TestModelOne.Id));
					Assert.Contains(comparison, item => item.Property.Name == nameof(TestModelOne.Challenge));
					Assert.Equal(2, comparison.Count);
				}

				{
					var a = new TestModelOne
					{
						Id = 1,
						Name = "One",
					};

					var b = new TestModelOne
					{
						Id = 2,
						Name = "One",
						Challenge = new TestModelTwo()
					};

					var comparison = ObjectComparer.GetParametersWithValueDifference(a, b);
					Assert.Contains(comparison, item => item.Property.Name == nameof(TestModelOne.Id));
					Assert.Contains(comparison, item => item.Property.Name == nameof(TestModelOne.Challenge));
					Assert.Equal(2, comparison.Count);
				}

				{
					var a = new TestModelOne
					{
						Id = 1,
						Name = "One",
						Challenge = new TestModelTwo()
					};

					var b = new TestModelOne
					{
						Id = 2,
						Name = "One",
						Challenge = new TestModelTwo()
					};

					var comparison = ObjectComparer.GetParametersWithValueDifference(a, b);
					Assert.Contains(comparison, item => item.Property.Name == nameof(TestModelOne.Id));
					Assert.Contains(comparison, item => item.Property.Name == nameof(TestModelOne.Challenge));
					Assert.Equal(2, comparison.Count);
				}

				{
					var challenge = new TestModelTwo();

					var a = new TestModelOne
					{
						Id = 1,
						Name = "One",
						Challenge = challenge
					};

					var b = new TestModelOne
					{
						Id = 2,
						Name = "One",
						Challenge = challenge
					};

					var comparison = ObjectComparer.GetParametersWithValueDifference(a, b);
					Assert.Contains(comparison, item => item.Property.Name == nameof(TestModelOne.Id));
					Assert.Single(comparison);
				}

				{
					var challenge = new TestModelTwo();

					var a = new TestModelOne
					{
						Id = 1,
						Name = "One",
						Challenge = challenge,
						TestValueEqualsDateTime = DateTime.Now
					};

					var b = new TestModelOne
					{
						Id = 2,
						Name = "One",
						Challenge = challenge
					};

					var comparison = ObjectComparer.GetParametersWithValueDifference(a, b);
					Assert.Contains(comparison, item => item.Property.Name == nameof(TestModelOne.Id));
					Assert.Contains(comparison, item => item.Property.Name == nameof(TestModelOne.TestValueEqualsDateTime));
					Assert.Equal(2, comparison.Count);
				}
			}

			public class TestModelOne
			{
				public int Id { get; set; }
				public string Name { get; set; }
				public TestModelTwo Challenge { get; set; }
				public Guid TestValueEqualsGuid { get; set; } = Guid.Parse("9d102d39-cc11-45f2-8c72-e68afb63d09b");
				public DateTime TestValueEqualsDateTime { get; set; } = new DateTime(2020, 5, 26, 13, 37, 00);
			}

			public class TestModelTwo
			{
			}
		}
	}
}
