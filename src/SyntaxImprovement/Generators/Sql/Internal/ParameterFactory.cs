using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace oledid.SyntaxImprovement.Generators.Sql.Internal
{
	internal class ParameterFactory
	{
		private readonly DatabaseType databaseType;

		private int currentParameterIterator;
		internal readonly List<Parameter> parameters;

		public ParameterFactory(DatabaseType databaseType)
		{
			this.databaseType = databaseType;
			currentParameterIterator = 0;
			parameters = new List<Parameter>();

			if (databaseType == DatabaseType.PostgreSQL)
			{
				currentParameterIterator = 1; // PostgreSQL starts with $1, $2, etc.
			}
		}

		public string Create(object value)
		{
			if (value is ICollection values)
				return CreateCollection(values);

			var parameter = new Parameter
			{
				Name = GetParameterPrefix() + currentParameterIterator++,
				Value = value
			};
			parameters.Add(parameter);
			return parameter.Name;
		}

		private string GetParameterPrefix()
		{
			return databaseType switch
			{
				DatabaseType.PostgreSQL => "$",
				_ => "@p",
			};
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
				((IDictionary<string, object>) obj)[parameter.Name] = parameter.Value;
			}

			return obj;
		}

		internal Parameter LastOrNull() => parameters?.LastOrDefault();
	}
}
