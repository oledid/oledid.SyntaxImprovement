using System;
using System.Linq.Expressions;

namespace oledid.SyntaxImprovement.Generators.Sql.Internal
{
	internal class SelectManager<TableType> where TableType : DatabaseTable, new()
	{
		private readonly TableInformation<TableType> tableInformation;
		private Expression<Func<TableType, bool>> whereStatement;

		public SelectManager()
		{
			tableInformation = new TableInformation<TableType>();
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
			var whereQueryPart = WhereQueryGenerator.CreateQuery(parameterFactory, whereStatement);
			return
				  "SELECT " + string.Join(", ", columns)
				+ " FROM " + tableName
				+ whereQueryPart + ";";
		}
	}
}
