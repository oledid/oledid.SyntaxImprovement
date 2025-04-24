using System;
using System.Linq.Expressions;

namespace oledid.SyntaxImprovement.Generators.Sql.Internal
{
	internal class DeleteManager<TableType> where TableType : DatabaseTable, new()
	{
		private readonly TableInformation<TableType> tableInformation;
		private Expression<Func<TableType, bool>> whereStatement;

		public DeleteManager()
		{
			tableInformation = new TableInformation<TableType>();
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
			var whereQueryPart = WhereGenerator.CreateQuery(parameterFactory, whereStatement);
			return
				"DELETE FROM " + tableName
			  + whereQueryPart + ";";
		}
	}
}
