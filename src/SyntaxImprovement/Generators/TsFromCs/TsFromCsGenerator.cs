using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
			var typeList = (types ?? new List<Type>()).Where(e => e.Name.StartsWith("<>") == false).ToList();
			var ignoredTypes = typeList.Where(t => t.GetCustomAttribute<TsFromCsIgnoreAttribute>() != null).ToList();
			typeList = typeList.Where(t => t.NotIn(ignoredTypes)).ToList();

			var stringBuilder = new StringBuilder();

			foreach (var kvp in typeList.GroupBy(e => GetTsName(e)).Where(e => e.Count() > 1))
			{
				stringBuilder.Append($"error error // multiple classes with the same name: \"{kvp.Key}\", please correct by renaming a class or adding a TsFromCsNameAttribute");
				return stringBuilder.ToString();
			}

			var index = 0;
			foreach (var type in typeList.OrderBy(t => GetTsName(t)))
			{
				var isLastItem = ++index == typeList.Count;
				var typeName = GetTsName(type);
				if (typeName[0] == 'I' && typeName.Length > 2 && typeName[1].IsUppercaseLetterAtoZ() && typeName[2].IsLowercaseLetterAtoZ())
				{
					typeName = typeName.Substring(1);
				}
				var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

				stringBuilder
					.Append($"export interface I{typeName} {{\r\n");

				if (type.IsInterface)
				{
					foreach (var method in type.GetMethods())
					{
						stringBuilder
							.Append("\t")
							.Append(method.Name)
							.Append("(");

						var methodParams = method.GetParameters();
						for (var i = 0; i < methodParams.Length; i++)
						{
							var mParam = methodParams[i];
							var isLast = i == methodParams.Length - 1;
							stringBuilder
								.Append(mParam.Name)
								.Append(":")
								.Append(" ")
								.Append(GetTsType(mParam.ParameterType, knownTypes: typeList, parentTypeName: typeName))
								.Append(isLast ? string.Empty :", ");
						}

						stringBuilder.Append(")")
							.Append(":")
							.Append(" ")
							.Append(GetTsType(method.ReturnType, knownTypes: typeList, parentTypeName: typeName))
							.Append(";")
							.Append(Environment.NewLine);
					}
				}

				foreach (var property in properties.Where(p => p.GetCustomAttribute<TsFromCsIgnoreAttribute>() == null))
				{
					var isNullable = Nullable.GetUnderlyingType(property.PropertyType) != null || property.PropertyType == typeof(string) || typeList.Contains(property.PropertyType) || ignoredTypes.Contains(property.PropertyType);

					stringBuilder
						.Append('\t')
						.Append(property.Name.ToLowerCamelCase())
						.Append(isNullable ? "?" : string.Empty)
						.Append(':')
						.Append(' ')
						.Append(GetTsType(property.PropertyType, knownTypes: typeList, parentTypeName: typeName))
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

		private static string GetTsName(Type type)
		{
			var overrideAttribute = type.GetCustomAttribute<TsFromCsNameAttribute>();
			return overrideAttribute?.Name ?? type.Name;
		}

		private static string GetTsType(Type type, IReadOnlyList<Type> knownTypes, string parentTypeName)
		{
			try
			{
				return GetTsTypeInner(type, knownTypes, parentTypeName);
			}
			catch (Exception ex)
			{
				throw new Exception($"Could not {nameof(GetTsType)} of type {type.Namespace}.{type.Name}. Parent type: {parentTypeName}", ex);
			}
		}

		private static string GetTsTypeInner(Type type, IReadOnlyList<Type> knownTypes, string parentTypeName)
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
				{ typeof(void), () => "void" },
				{ typeof(Task), () => "Promise<void>" }
			};

			var tsType = @switch.GetValueOrNull(type)?.Invoke();
			if (tsType != null)
			{
				return tsType;
			}

			if (type.IsArray)
			{
				var arrayType = type.GetElementType();
				return "Array<" + GetTsType(arrayType, knownTypes, parentTypeName) + ">";
			}

			if (type.GetInterfaces().Any(@interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
			{
				var argTypes = type.GetGenericArguments();
				var keyType = GetTsType(argTypes[0], knownTypes, parentTypeName);
				var valueType = GetTsType(argTypes[1], knownTypes, parentTypeName);
				return $"{{ [key: {keyType}]: {valueType} }}";
			}

			if (typeof(System.Collections.IEnumerable).IsAssignableFrom(type))
			{
				var collectionType = type.GetGenericArguments()[0];
				return "Array<" + GetTsType(collectionType, knownTypes, parentTypeName) + ">";
			}

			if (Nullable.GetUnderlyingType(type) != null)
			{
				return GetTsType(Nullable.GetUnderlyingType(type), knownTypes, parentTypeName);
			}

			if (type.IsEnum)
			{
				return "number";
			}

			if (knownTypes.Contains(type))
			{
				return "I" + type.Name;
			}

			return "any";
		}
	}
}
