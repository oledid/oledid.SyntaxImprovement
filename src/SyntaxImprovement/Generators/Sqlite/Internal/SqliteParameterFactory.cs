using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace oledid.SyntaxImprovement.Generators.Sqlite.Internal
{
	internal class SqliteParameterFactory
	{
		private int currentParameterIterator;
		internal readonly List<SqliteParameter> parameters;

		public SqliteParameterFactory()
		{
			currentParameterIterator = 0;
			parameters = new List<SqliteParameter>();
		}

		public string Create(object value)
		{
			if (value is ICollection values)
				return CreateCollection(values);

			var parameter = new SqliteParameter
			{
				Name = "@p" + currentParameterIterator++,
				Value = value
			};
			parameters.Add(parameter);
			return parameter.Name;
		}

		private string CreateCollection(ICollection values)
		{
			int iterator = 0;
			var output = string.Empty;
			foreach (var value in values)
			{
				output += Create(value);
				var isLast = ++iterator == values.Count;
				if (isLast == false)
					output += ", ";
			}

			return "(" + output + ")";
		}

		public dynamic GetParameters()
		{
			dynamic obj = new ExpandoObject();
			for (var i = 0; i < parameters.Count; i++)
			{
				var parameter = parameters[i];
				((IDictionary<string, object>) obj)["p" + i] = parameter.Value;
			}

			return obj;
		}

		internal SqliteParameter LastOrNull() => parameters?.LastOrDefault();
	}
}
