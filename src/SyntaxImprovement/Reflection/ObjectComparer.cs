using System.Collections.Generic;
using System.Reflection;

namespace oledid.SyntaxImprovement.Reflection
{
	public static class ObjectComparer
	{
		public static List<PropertyWithValueDifference<TObject>> GetParametersWithValueDifference<TObject>(TObject a, TObject b, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
		{
			var fieldsWithDiff = new List<PropertyWithValueDifference<TObject>>();
			foreach (var property in typeof(TObject).GetProperties(bindingFlags))
			{
				var hasDiff = false;

				var aValue = property.GetValue(a);
				var bValue = property.GetValue(b);

				if ((aValue == null) != (bValue == null))
				{
					hasDiff = true;
				}

				if (aValue != null && bValue != null)
				{
					if (property.GetMethod.ReturnType.IsValueType || property.GetMethod.ReturnType == typeof(string))
					{
						hasDiff = hasDiff || (aValue.Equals(bValue) == false);
					}
					else if (aValue != bValue)
					{
						hasDiff = true;
					}
				}

				if (hasDiff)
				{
					fieldsWithDiff.Add(new PropertyWithValueDifference<TObject>
					{
						Property = property,
						A = a,
						B = b,
						AValue = aValue,
						BValue = bValue
					});
				}
			}

			return fieldsWithDiff;
		}

		public class PropertyWithValueDifference<TObject>
		{
			public PropertyInfo Property { get; set; }
			public TObject A { get; set; }
			public TObject B { get; set; }
			public object AValue { get; set; }
			public object BValue { get; set; }
		}
	}
}
