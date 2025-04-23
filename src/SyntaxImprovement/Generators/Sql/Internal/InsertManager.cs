using System;
using System.Collections.Generic;
using System.Linq;

namespace oledid.SyntaxImprovement.Generators.Sql.Internal
{
	internal class InsertManager<TableType> where TableType : DatabaseTable, new()
	{
		private readonly TableInformation<TableType> tableInformation;

		public InsertManager()
		{
			tableInformation = new TableInformation<TableType>();
		}

		public SqlQuery ToQuery(IEnumerable<TableType> rows)
		{
			var databaseType = tableInformation.GetDatabaseType();
			var parameterFactory = new ParameterFactory(databaseType);
			var query = "";

			var rowList = rows.ToList();

			foreach (var row in rowList)
			{
				var tableName = tableInformation.GetSchemaAndTableName();
				var columns = tableInformation.GetColumnNames(excludeIdentityColumns: true, excludeComputedFields: true, excludeIgnoredFields: true);
				var values = tableInformation.GetColumnValues(row, excludeIdentityColumns: true, excludeComputedFields: true, excludeIgnoredFields: true);
				var identityColumn = tableInformation.GetIdentityColumn();
				query += "INSERT INTO " + tableName + " (" + string.Join(", ", columns) + ") SELECT " + string.Join(", ", values.Select(parameterFactory.Create));
				if (databaseType == DatabaseType.PostgreSQL)
				{
					if (identityColumn != null && rowList.Count == 1)
					{
						query += " RETURNING " + tableInformation.GetColumnName(identityColumn) + ";";
					}
					else
					{
						query += "; ";
					}
				}
				else if (databaseType == DatabaseType.MSSQL)
				{
					query += "; ";
					if (identityColumn != null && rowList.Count == 1)
					{
						query += "SELECT SCOPE_IDENTITY();";
					}
				}
				else if (databaseType == DatabaseType.SQLite)
				{
					query += "; ";
					if (identityColumn != null && rowList.Count == 1)
					{
						query += "SELECT last_insert_rowid();";
					}
				}
				else
				{
					throw new NotSupportedException($"{nameof(DatabaseType)} is not implemented: {(int)databaseType} {databaseType.ToString()}");
				}
			}

			return new SqlQuery(query, parameterFactory.GetParameters());
		}
	}
}
