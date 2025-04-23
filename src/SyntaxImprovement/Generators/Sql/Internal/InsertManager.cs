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
			var parameterFactory = new ParameterFactory(tableInformation.GetDatabaseType());
			var query = "";

			var rowList = rows.ToList();

			foreach (var row in rowList)
			{
				var tableName = tableInformation.GetSchemaAndTableName();
				var columns = tableInformation.GetColumnNames(excludeIdentityColumns: true, excludeComputedFields: true, excludeIgnoredFields: true);
				var values = tableInformation.GetColumnValues(row, excludeIdentityColumns: true, excludeComputedFields: true, excludeIgnoredFields: true);
				var identityColumn = tableInformation.GetIdentityColumn();
				query += "INSERT INTO " + tableName + " (" + string.Join(", ", columns) + ") SELECT " + string.Join(", ", values.Select(parameterFactory.Create)) + "; ";
				if (identityColumn != null && rowList.Count == 1)
				{
					query += "SELECT SCOPE_IDENTITY();";
				}
			}

			return new SqlQuery(query, parameterFactory.GetParameters());
		}
	}
}
