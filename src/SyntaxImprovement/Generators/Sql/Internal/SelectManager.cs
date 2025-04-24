using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace oledid.SyntaxImprovement.Generators.Sql.Internal
{
	internal class SelectManager<TableType> where TableType : DatabaseTable, new()
	{
		private readonly TableInformation<TableType> tableInformation;
		private Expression<Func<TableType, bool>> whereStatement;
		private readonly List<Tuple<Expression<Func<TableType, object>>, bool>> orderByStatements;
		private readonly int? TopOrNull;
		private readonly IncludeFields<TableType> FieldsToInclude;
		private readonly DatabaseType databaseType = new TableInformation<TableType>().GetDatabaseType();

		public SelectManager(int? top, IncludeFields<TableType> fieldsToInclude)
		{
			TopOrNull = top;
			FieldsToInclude = fieldsToInclude;
			tableInformation = new TableInformation<TableType>();
			orderByStatements = new List<Tuple<Expression<Func<TableType, object>>, bool>>();
		}

		public Select<TableType> Select()
		{
			return new Select<TableType>(this);
		}

		public void SetWhereExpression(Expression<Func<TableType, bool>> expression)
		{
			if (whereStatement != null)
				throw new NotSupportedException("Where expression already set.");
			whereStatement = expression;
		}

		public SqlQuery ToQuery()
		{
			var parameterFactory = new ParameterFactory(tableInformation.GetDatabaseType());
			var query = CreateQuery(parameterFactory);
			return new SqlQuery(query, parameterFactory.GetParameters());
		}

		private string CreateQuery(ParameterFactory parameterFactory)
		{
			var tableName = tableInformation.GetSchemaAndTableName();
			var columns = tableInformation.GetColumnNames(excludeIgnoredFields: true, fieldsToInclude: FieldsToInclude);
			var whereQueryPart = WhereGenerator.CreateQuery(parameterFactory, whereStatement);
			var orderByQueryPart = OrderByGenerator.CreateQuery(orderByStatements);

			if (databaseType == DatabaseType.MSSQL)
			{
				var topPart = TopOrNull == null
					? string.Empty
					: "TOP " + TopOrNull.Value + " ";
				return
					  "SELECT " + topPart + string.Join(", ", columns)
					+ " FROM " + tableName
					+ whereQueryPart
					+ orderByQueryPart + ";";
			}
			else
			{
				var topPart = TopOrNull == null
					? string.Empty
					: " LIMIT " + TopOrNull.Value;
				return
					  "SELECT " + string.Join(", ", columns)
					+ " FROM " + tableName
					+ whereQueryPart
					+ orderByQueryPart + topPart + ";";
			}
		}

		public void OrderByExpression(Expression<Func<TableType, object>> expression, bool ascending)
		{
			orderByStatements.Clear();
			ThenByExpression(expression, ascending);
		}

		public void ThenByExpression(Expression<Func<TableType, object>> expression, bool ascending)
		{
			orderByStatements.Add(Tuple.Create(expression, ascending));
		}
	}
}
