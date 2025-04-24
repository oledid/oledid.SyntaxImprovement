using oledid.SyntaxImprovement.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace oledid.SyntaxImprovement.Tests.Algorithms
{
	public class ParentChildSorterTests
	{
		[Fact]
		public void TestWithStringIds()
		{
			var groups = new List<StringGroup>
			{
				// Level 0
				new StringGroup { Id = "A", ParentId = null },

				// Level 1
				new StringGroup { Id = "B", ParentId = "A" },
				new StringGroup { Id = "C", ParentId = "A" },

				// Level 2
				new StringGroup { Id = "D", ParentId = "B" },
				new StringGroup { Id = "E", ParentId = "B" },
				new StringGroup { Id = "F", ParentId = "C" },
				new StringGroup { Id = "G", ParentId = "C" },

				// Level 3
				new StringGroup { Id = "H", ParentId = "D" },
				new StringGroup { Id = "I", ParentId = "E" },
				new StringGroup { Id = "J", ParentId = "F" },
				new StringGroup { Id = "K", ParentId = "G" },

				// Level 4
				new StringGroup { Id = "L", ParentId = "H" },
				new StringGroup { Id = "M", ParentId = "I" },
				new StringGroup { Id = "N", ParentId = "J" },
				new StringGroup { Id = "O", ParentId = "K" }
			};

			// Shuffle the list to simulate unordered input
			var random = new Random();
			groups = groups.OrderBy(_ => random.Next()).ToList();

			var orderedGroups = ParentChildSorter.SortGroupsByParentChildRelationship(
				groups,
				e => e.ParentId,
				e => e.Id);

			var expectedOrder = new List<string>
			{
				"A", // Level 0
				"B", "C", // Level 1
				"D", "E", "F", "G", // Level 2
				"H", "I", "J", "K", // Level 3
				"L", "M", "N", "O"  // Level 4
			};

			// Assert that the orderedGroups match the expected order
			Assert.Equal(expectedOrder.Count, orderedGroups.Count);
			for (int i = 0; i < expectedOrder.Count; i++)
			{
				Assert.Equal(expectedOrder[i], orderedGroups[i].Id);
			}
		}

		[Fact]
		public void TestWithLongIds()
		{
			// Define a list of groups with long IDs
			var groups = new List<LongGroup>
			{
				// Level 0
				new LongGroup { Id = 1, ParentId = null },

				// Level 1
				new LongGroup { Id = 2, ParentId = 1 },
				new LongGroup { Id = 3, ParentId = 1 },

				// Level 2
				new LongGroup { Id = 4, ParentId = 2 },
				new LongGroup { Id = 5, ParentId = 2 },
				new LongGroup { Id = 6, ParentId = 3 },
				new LongGroup { Id = 7, ParentId = 3 },

				// Level 3
				new LongGroup { Id = 8, ParentId = 4 },
				new LongGroup { Id = 9, ParentId = 5 },
				new LongGroup { Id = 10, ParentId = 6 },
				new LongGroup { Id = 11, ParentId = 7 },

				// Level 4
				new LongGroup { Id = 12, ParentId = 8 },
				new LongGroup { Id = 13, ParentId = 9 },
				new LongGroup { Id = 14, ParentId = 10 },
				new LongGroup { Id = 15, ParentId = 11 }
			};

			// Shuffle the list to simulate unordered input
			var random = new Random();
			groups = groups.OrderBy(_ => random.Next()).ToList();

			var orderedGroups = ParentChildSorter.SortGroupsByParentChildRelationship(
				groups,
				e => e.ParentId,
				e => e.Id);

			var expectedOrder = new List<long>
			{
				1,      // Level 0
				2, 3,   // Level 1
				4, 5, 6, 7,   // Level 2
				8, 9, 10, 11, // Level 3
				12, 13, 14, 15 // Level 4
			};

			// Assert that the orderedGroups match the expected order
			Assert.Equal(expectedOrder.Count, orderedGroups.Count);
			for (int i = 0; i < expectedOrder.Count; i++)
			{
				Assert.Equal(expectedOrder[i], orderedGroups[i].Id);
			}
		}

		#nullable enable
		public class StringGroup
		{
			public string Id { get; set; } = string.Empty;
			public string? ParentId { get; set; }
		}
		#nullable restore

		public class LongGroup
		{
			public long Id { get; set; }
			public long? ParentId { get; set; }
		}
	}
}
