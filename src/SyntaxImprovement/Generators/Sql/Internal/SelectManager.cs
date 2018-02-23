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

		public SelectManager()
		{
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
			var parameterFactory = new ParameterFactory();
			var query = CreateQuery(parameterFactory);
			return new SqlQuery(query, parameterFactory.GetParameters());
		}

		private string CreateQuery(ParameterFactory parameterFactory)
		{
			var tableName = tableInformation.GetSchemaAndTableName();
			var columns = tableInformation.GetColumnNames();
			var whereQueryPart = WhereGenerator.CreateQuery(parameterFactory, whereStatement);
			var orderByQueryPart = OrderByGenerator.CreateQuery(orderByStatements);
			return
				  "SELECT " + string.Join(", ", columns)
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
