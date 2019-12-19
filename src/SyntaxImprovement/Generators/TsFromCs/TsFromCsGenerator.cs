using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace oledid.SyntaxImprovement.Generators.TsFromCs
{
	public class TsFromCsGenerator
	{
		public static string GenerateTypescriptInterfaceFromCsharpClass(params Type[] types)
		{
			var stringBuilder = new StringBuilder();

			foreach (var type in types)
			{
				var typeName = type.Name;
				var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

				stringBuilder
					.Append($"interface I{typeName} {{\r\n");

				foreach (var property in properties)
				{
					var isNullable = Nullable.GetUnderlyingType(property.PropertyType) != null || property.PropertyType == typeof(string) || types.Contains(property.PropertyType);

					stringBuilder
						.Append('\t')
						.Append(property.Name.ToLowerCamelCase())
						.Append(isNullable ? "?" : string.Empty)
						.Append(':')
						.Append(' ')
						.Append(GetTsType(property.PropertyType))
						.Append(';')
						.Append(Environment.NewLine);
				}

				stringBuilder
					.Append('}')
					.Append(Environment.NewLine);
			}

			return stringBuilder.ToString();
		}

		private static string GetTsType(Type type)
		{
			var @switch = new Dictionary<Type, Func<string>>
			{
				{ typeof(bool), () => "boolean" },
				{ typeof(char), () => "string" },
				{ typeof(string), () => "string" },
				{ typeof(Guid), () => "string" },
				{ typeof(short), () => "number" },
				{ typeof(int), () => "number" },
				{ typeof(long), () => "number" },
				{ typeof(decimal), () => "number" },
				{ typeof(DateTime), () => "Date" },
			};

			var tsType = @switch.GetValueOrNull(type)?.Invoke();

			if (type.IsArray)
			{
				var arrayType = type.GetElementType();
				return "Array<" + GetTsType(arrayType) + ">";
			}

			if (type.GetInterfaces().Any(@interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
			{
				var argTypes = type.GetGenericArguments();
				var keyType = GetTsType(argTypes[0]);
				var valueType = GetTsType(argTypes[1]);
				return $"{{ [key: {keyType}]: {valueType} }}";
			}

			if (typeof(System.Collections.IEnumerable).IsAssignableFrom(type))
			{
				var collectionType = type.GetGenericArguments()[0];
				return "Array<" + GetTsType(collectionType) + ">";
			}

			if (Nullable.GetUnderlyingType(type) != null)
			{
				return GetTsType(Nullable.GetUnderlyingType(type));
			}

			if (type.IsEnum)
			{
				return "number";
			}

			return tsType ?? "any";
		}
	}
}
