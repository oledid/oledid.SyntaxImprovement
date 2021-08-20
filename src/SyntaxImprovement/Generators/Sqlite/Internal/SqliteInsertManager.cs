using System.Collections.Generic;
using System.Linq;

namespace oledid.SyntaxImprovement.Generators.Sqlite.Internal
{
	internal class SqliteInsertManager<TableType> where TableType : SqliteDatabaseTable, new()
	{
		private readonly SqliteTableInformation<TableType> sqliteTableInformation;

		public SqliteInsertManager()
		{
			sqliteTableInformation = new SqliteTableInformation<TableType>();
		}

		public SqliteQuery ToQuery(IEnumerable<TableType> rows)
		{
			var parameterFactory = new SqliteParameterFactory();
			var query = "";

			var rowList = rows.ToList();

			foreach (var row in rowList)
			{
				var tableName = sqliteTableInformation.GetSchemaAndTableName();
				var columns = sqliteTableInformation.GetColumnNames(excludeIdentityColumns: true, excludeComputedFields: true, excludeIgnoredFields: true);
				var values = sqliteTableInformation.GetColumnValues(row, excludeIdentityColumns: true, excludeComputedFields: true, excludeIgnoredFields: true);
				var identityColumn = sqliteTableInformation.GetIdentityColumn();
				query += "INSERT INTO " + tableName + " (" + string.Join(", ", columns) + ") SELECT " + string.Join(", ", values.Select(parameterFactory.Create)) + "; ";
				if (identityColumn != null && rowList.Count == 1)
				{
					query += "SELECT SCOPE_IDENTITY();";
				}
			}

			return new SqliteQuery(query, parameterFactory.GetParameters());
		}
	}
}
