﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace oledid.SyntaxImprovement.Generators.Sql.Internal
{
	internal class TableInformation<TableType> where TableType : DatabaseTable, new()
	{
		private string tableName;
		private string schemaName;

		public TableInformation()
		{
		}

		public string GetSchemaAndTableName()
		{
			if (tableName == null)
			{
				var instance = new TableType();
				tableName = instance.GetTableName();
				schemaName = instance.GetSchemaName();
			}

			var schemaValue = schemaName.HasValue()
				? "[" + schemaName + "]."
				: string.Empty;

			return schemaValue + "[" + tableName + "]";
		}

		public List<string> GetColumnNames(bool excludeIdentityColumns = false, bool excludeComputedFields = false)
		{
			var columns = GetColumns(excludeIdentityColumns, excludeComputedFields);

			var result = new List<string>();
			foreach (var column in columns)
			{
				var name = "[" + column.Name + "]";
				result.Add(name);
			}

			return result;
		}

		public string GetColumnName(MemberInfo memberInfo)
		{
			var column = GetColumn(memberInfo);
			return "[" + column.Name + "]";
		}

		public PropertyInfo GetColumn(MemberInfo memberInfo)
		{
			var columns = GetColumns();
			var column = columns.SingleOrDefault(propertyInfo => propertyInfo == memberInfo);
			if (column == null)
				throw new NotSupportedException("Could not find correct column.");
			return column;
		}

		public List<object> GetColumnValues(DatabaseTable instance, bool excludeIdentityColumns = false, bool excludeComputedFields = false)
		{
			var columns = GetColumns(excludeIdentityColumns, excludeComputedFields);

			var result = new List<object>();
			foreach (var column in columns)
			{
				var value = column.GetValue(instance) ?? DBNull.Value;
				result.Add(value);
			}

			return result;
		}

		public IEnumerable<PropertyInfo> GetColumns(bool excludeIdentityColumns = false, bool excludeComputedFields = false)
		{
			return typeof(TableType).GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(column =>
					(excludeIdentityColumns == false || Attribute.IsDefined(column, typeof(IsIdentityAttribute)) == false)
					&& (excludeComputedFields == false || Attribute.IsDefined(column, typeof(IsComputedAttribute)) == false))
				.ToList();
		}

		public PropertyInfo GetIdentityColumn()
		{
			return GetColumns().SingleOrDefault(column => Attribute.IsDefined(column, typeof(IsIdentityAttribute)));
		}
	}
}
