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
				var aValue = property.GetValue(a);
				var bValue = property.GetValue(b);

				if (aValue != bValue)
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
