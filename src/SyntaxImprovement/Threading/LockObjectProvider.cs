using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace oledid.SyntaxImprovement.Threading
{
	public class LockObjectProvider
	{
		private static readonly ConcurrentDictionary<string, object> lockObjectDictionary = new ConcurrentDictionary<string, object>();

		public static object GetOrCreateLockObjectFromCallerInformation([CallerMemberName]string memberName = "", [CallerFilePath]string sourceFilePath = "", [CallerLineNumber]int sourceLineNumber = 0, IEnumerable<object> additionalKeyParts = null)
		{
			return GetOrCreateLockObjectFromCustomCallerInformation(memberName, sourceFilePath, sourceLineNumber, additionalKeyParts);
		}

		public static object GetOrCreateLockObjectFromCustomCallerInformation(string memberName, string sourceFilePath, int sourceLineNumber, IEnumerable<object> additionalKeyParts = null)
		{
			var lockKey = CompilerInformation.GetUniqueKeyFromCustomCallerInformation(memberName, sourceFilePath, sourceLineNumber) + "_" + ParameterString(additionalKeyParts);
			return lockObjectDictionary.GetOrAdd(lockKey, new object());
		}

		private static string ParameterString(IEnumerable<object> parameters)
		{
			if (parameters == null)
				return string.Empty;
			var list = parameters.ToList();
			return list.Any()
				? "_" + string.Join("_", list)
				: string.Empty;
		}
	}
}
