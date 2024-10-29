using System;
using System.Collections.Generic;
using System.Linq;

namespace oledid.SyntaxImprovement.Algorithms
{
	public static class ParentChildSorter
	{
		/// <summary>
		/// Sorts a list of items based on their parent-child relationships using breadth-first traversal.
		///
		/// This method ensures that:
		/// - Parents come before their immediate children.
		/// - Siblings (items with the same parent) are ordered based on a specified key (typically the item's ID).
		/// - The traversal proceeds level by level, processing all nodes at the current depth before moving to the next level.
		///
		/// This is particularly useful when inserting items into a database with foreign key constraints,
		/// as it ensures that parent items are inserted before their children to avoid constraint violations.
		///
		/// Algorithm:
		/// 1. Create a lookup to map parent IDs to their immediate children.
		/// 2. Initialize a queue for breadth-first traversal.
		/// 3. Start with root nodes (items with null ParentId), ordered by the key selector.
		/// 4. While the queue is not empty:
		///    a. Dequeue an item and add it to the sorted list.
		///    b. Enqueue the item's children (ordered by the key selector) if they haven't been visited.
		/// 5. Return the sorted list.
		///
		/// Parameters:
		/// <param name="groups">The list of items to sort.</param>
		/// <param name="getParentId">Function to retrieve the ParentId of an item.</param>
		/// <param name="getId">Function to retrieve the Id of an item.</param>
		///
		/// Returns:
		/// <returns>A list of items sorted based on their parent-child relationships.</returns>
		/// </summary>
		public static List<T> SortGroupsByParentChildRelationship<T, TKey>(
			List<T> groups,
			Func<T, TKey?> getParentId,
			Func<T, TKey> getId) where TKey : notnull
		{
			// Create a lookup to map ParentId to its immediate children
			var lookup = groups.ToLookup(getParentId);

			var sortedList = new List<T>();
			var visited = new HashSet<TKey?>();

			var queue = new Queue<T>();

			// Start with root nodes (ParentId == null), ordered by ID
			var rootNodes = lookup[default].OrderBy(getId);
			foreach (var rootNode in rootNodes)
			{
				queue.Enqueue(rootNode);
				visited.Add(getId(rootNode));
			}

			// Perform breadth-first traversal
			while (queue.Count > 0)
			{
				var currentNode = queue.Dequeue();
				sortedList.Add(currentNode);
				var currentId = getId(currentNode);

				// Get children of the current node, ordered by ID
				var children = lookup[currentId]
					.Where(child => visited.Add(getId(child))) // Add to visited if not already present
					.OrderBy(getId);

				foreach (var child in children)
				{
					queue.Enqueue(child);
				}
			}

			return sortedList;
		}
	}
}
