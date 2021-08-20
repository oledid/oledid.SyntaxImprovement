using System;
using System.Linq.Expressions;
using oledid.SyntaxImprovement.Generators.Sqlite.Internal;

namespace oledid.SyntaxImprovement.Generators.Sqlite
{
	/// <summary>
	/// Used to generate a sql UPDATE-query
	/// </summary>
	/// <typeparam name="TableType">A class which inherits from <see cref="SqliteDatabaseTable"/></typeparam>
	public class SqliteUpdate<TableType> : SqliteUpdateBase<TableType> where TableType : SqliteDatabaseTable, new()
	{
		public SqliteUpdate() : base(new SqliteUpdateManager<TableType>())
		{
		}

		internal SqliteUpdate(SqliteUpdateManager<TableType> manager) : base(manager)
		{
		}

		/// <summary>
		/// SqliteUpdate single column
		/// </summary>
		public SqliteUpdateSet<TableType> Set(Expression<Func<TableType, object>> columnSelector, object value)
		{
			manager.AddSetExpression(new SqliteSetExpression<TableType>
			{
				ColumnExpression = columnSelector,
				Value = value
			});
			return new SqliteUpdateSet<TableType>(manager);
		}

		/// <summary>
		/// SqliteUpdate all columns (except identity, computed, ignore)
		/// </summary>
		public SqliteUpdateSet<TableType> Set(TableType entity)
		{
			var tableInformation = new SqliteTableInformation<TableType>();
			var columns = tableInformation.GetColumnNames(excludeIdentityColumns: true, excludeComputedFields: true, excludeIgnoredFields: true);
			var values = tableInformation.GetColumnValues(entity, excludeIdentityColumns: true, excludeComputedFields: true, excludeIgnoredFields: true);

			for (var i = 0; i < columns.Count; i++)
			{
				var column = columns[i];
				var value = values[i];

				manager.AddSetExpression(new SqliteSetExpression<TableType>
				{
					ColumnName = column,
					Value = value
				});
			}

			return new SqliteUpdateSet<TableType>(manager);
		}
	}
}
