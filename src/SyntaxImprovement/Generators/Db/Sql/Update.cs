using System;
using System.Linq.Expressions;
using oledid.SyntaxImprovement.Generators.Db.Sql.Internal;

namespace oledid.SyntaxImprovement.Generators.Db.Sql
{
	/// <summary>
	/// Used to generate a sql UPDATE-query
	/// </summary>
	/// <typeparam name="TableType">A class which inherits from <see cref="DatabaseTable"/></typeparam>
	public class Update<TableType> : UpdateBase<TableType> where TableType : DatabaseTable, new()
	{
		public Update() : base(new UpdateManager<TableType>())
		{
		}

		internal Update(UpdateManager<TableType> manager) : base(manager)
		{
		}

		/// <summary>
		/// Update single column
		/// </summary>
		public UpdateSet<TableType> Set(Expression<Func<TableType, object>> columnSelector, object value)
		{
			manager.AddSetExpression(new SetExpression<TableType>
			{
				ColumnExpression = columnSelector,
				Value = value
			});
			return new UpdateSet<TableType>(manager);
		}

		/// <summary>
		/// Update all columns (except identity, computed, ignore)
		/// </summary>
		public UpdateSet<TableType> Set(TableType entity)
		{
			var tableInformation = new TableInformation<TableType>();
			var columns = tableInformation.GetColumnNames(excludeIdentityColumns: true, excludeComputedFields: true, excludeIgnoredFields: true);
			var values = tableInformation.GetColumnValues(entity, excludeIdentityColumns: true, excludeComputedFields: true, excludeIgnoredFields: true);

			for (var i = 0; i < columns.Count; i++)
			{
				var column = columns[i];
				var value = values[i];

				manager.AddSetExpression(new SetExpression<TableType>
				{
					ColumnName = column,
					Value = value
				});
			}

			return new UpdateSet<TableType>(manager);
		}
	}
}
