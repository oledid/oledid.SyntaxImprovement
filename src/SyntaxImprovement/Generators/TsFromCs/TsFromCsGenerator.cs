using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace oledid.SyntaxImprovement.Generators.TsFromCs
{
	public class TsFromCsGenerator
	{
		/// <summary>
		/// Returns TypeScript-interfaces for the specified C#-classes, ready to be written to file. Use <see cref="TsFromCsIgnoreAttribute"/> to ignore stuff.
		/// </summary>
		public static string GenerateTypescriptInterfaceFromCsharpClass(Type type, params Type[] additionalTypes)
		{
			var list = new List<Type>();
			list.Add(type);
			list.AddRange(additionalTypes);
			return GenerateTypescriptInterfaceFromCsharpClass(list.Where(t => t != null));
		}

		/// <summary>
		/// Returns TypeScript-interfaces for the specified C#-classes, ready to be written to file. Use <see cref="TsFromCsIgnoreAttribute"/> to ignore stuff.
		/// </summary>
		public static string GenerateTypescriptInterfaceFromCsharpClass(IEnumerable<Type> types)
		{
			var typeList = types?.ToList() ?? new List<Type>();
			var ignoredTypes = typeList.Where(t => t.GetCustomAttribute<TsFromCsIgnoreAttribute>() != null).ToList();
			typeList = typeList.Where(t => t.NotIn(ignoredTypes)).ToList();

			var stringBuilder = new StringBuilder();

			var index = 0;
			foreach (var type in typeList)
			{
				var isLastItem = ++index == typeList.Count;
				var typeName = type.Name;
				var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

				stringBuilder
					.Append($"interface I{typeName} {{\r\n");

				foreach (var property in properties.Where(p => p.GetCustomAttribute<TsFromCsIgnoreAttribute>() == null))
				{
					var isNullable = Nullable.GetUnderlyingType(property.PropertyType) != null || property.PropertyType == typeof(string) || typeList.Contains(property.PropertyType) || ignoredTypes.Contains(property.PropertyType);

					stringBuilder
						.Append('\t')
						.Append(property.Name.ToLowerCamelCase())
						.Append(isNullable ? "?" : string.Empty)
						.Append(':')
						.Append(' ')
						.Append(GetTsType(property.PropertyType, knownTypes: typeList))
						.Append(';')
						.Append(Environment.NewLine);
				}

				stringBuilder
					.Append('}')
					.Append(Environment.NewLine)
					.Append(isLastItem ? string.Empty : Environment.NewLine);
			}

			return stringBuilder.ToString();
		}

		private static string GetTsType(Type type, IReadOnlyList<Type> knownTypes)
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
			if (tsType != null)
			{
				return tsType;
			}

			if (type.IsArray)
			{
				var arrayType = type.GetElementType();
				return "Array<" + GetTsType(arrayType, knownTypes) + ">";
			}

			if (type.GetInterfaces().Any(@interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
			{
				var argTypes = type.GetGenericArguments();
				var keyType = GetTsType(argTypes[0], knownTypes);
				var valueType = GetTsType(argTypes[1], knownTypes);
				return $"{{ [key: {keyType}]: {valueType} }}";
			}

			if (typeof(System.Collections.IEnumerable).IsAssignableFrom(type))
			{
				var collectionType = type.GetGenericArguments()[0];
				return "Array<" + GetTsType(collectionType, knownTypes) + ">";
			}

			if (Nullable.GetUnderlyingType(type) != null)
			{
				return GetTsType(Nullable.GetUnderlyingType(type), knownTypes);
			}

			if (type.IsEnum)
			{
				return "number";
			}

			if (knownTypes.Contains(type))
			{
				return "I" + type.Name;
			}

			return tsType ?? "any";
		}
	}
}
