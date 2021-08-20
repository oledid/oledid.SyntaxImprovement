using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace oledid.SyntaxImprovement.Generators.Sqlite.Internal
{
	internal class SqliteSelectManager<TableType> where TableType : SqliteDatabaseTable, new()
	{
		private readonly SqliteTableInformation<TableType> sqliteTableInformation;
		private Expression<Func<TableType, bool>> whereStatement;
		private readonly List<Tuple<Expression<Func<TableType, object>>, bool>> orderByStatements;
		private readonly int? TopOrNull = null;

		public SqliteSelectManager(int? top)
		{
			TopOrNull = top;
			sqliteTableInformation = new SqliteTableInformation<TableType>();
			orderByStatements = new List<Tuple<Expression<Func<TableType, object>>, bool>>();
		}

		public SqliteSelect<TableType> Select()
		{
			return new SqliteSelect<TableType>(this);
		}

		public void SetWhereExpression(Expression<Func<TableType, bool>> expression)
		{
			if (whereStatement != null)
				throw new NotSupportedException("Where expression already set.");
			whereStatement = expression;
		}

		public SqliteQuery ToQuery()
		{
			var parameterFactory = new SqliteParameterFactory();
			var query = CreateQuery(parameterFactory);
			return new SqliteQuery(query, parameterFactory.GetParameters());
		}

		private string CreateQuery(SqliteParameterFactory sqliteParameterFactory)
		{
			var tableName = sqliteTableInformation.GetSchemaAndTableName();
			var topPart = TopOrNull == null
				? string.Empty
				: "TOP " + TopOrNull.Value + " ";
			var columns = sqliteTableInformation.GetColumnNames(excludeIgnoredFields: true);
			var whereQueryPart = SqliteWhereGenerator.CreateQuery(sqliteParameterFactory, whereStatement);
			var orderByQueryPart = SqliteOrderByGenerator.CreateQuery(orderByStatements);
			return
				  "SELECT " + topPart + string.Join(", ", columns)
				+ " FROM " + tableName
				+ whereQueryPart
				+ orderByQueryPart + ";";
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
