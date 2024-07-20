using System.Collections.Generic;
using System.Linq;

namespace oledid.SyntaxImprovement
{
	public static class QueueExtensions
	{
		/// <summary>
		/// Removes up to N items from the queue and returns them.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="queue"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public static IList<T> Dequeue<T>(this Queue<T> queue, int count)
		{
			var list = new List<T>();
			for (int i = 0; i < count; ++i)
			{
				if (queue.Any() == false)
					break;
				list.Add(queue.Dequeue());
			}
			return list;
		}
	}
}