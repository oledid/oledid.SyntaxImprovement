using System;
using System.Linq;

namespace oledid.SyntaxImprovement.Attributes
{
	/// <summary>
	/// Add keyValuePairs to enum-values, access them with LookupAttribute.GetValueFromAttribute(EnumType.Value, key)
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public class LookupAttribute : Attribute
	{
		public string Key { get; }
		public string Value { get; }

		public LookupAttribute(string key, string value)
		{
			Key = key;
			Value = value;
		}

		public static string GetValueFromAttribute<T>(T instance, string key) => GetValueFromAttribute(typeof(T), instance, key);

		public static string GetValueFromAttribute(Type type, object instance, string key)
		{
			var memberInfos = type.GetMember(instance.ToString());
			if (memberInfos.Length == 0)
			{
				return string.Empty;
			}

			var memberInfo = memberInfos[0];
			var attributeValues = memberInfo.GetCustomAttributes(typeof(LookupAttribute), false);

			return attributeValues
				.Cast<LookupAttribute>()
				.Where(attr => attr.Key == key).FirstOrDefault()?.Value;
		}
	}
}
