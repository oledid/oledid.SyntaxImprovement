using oledid.SyntaxImprovement.Generators.Sql;
using oledid.SyntaxImprovement.Generators.Sql.Internal;
using System;
using System.Linq.Expressions;

namespace oledid.SyntaxImprovement.Generators.Db.Internal.Sqlite
{
	internal class SqliteDeleteManager<TableType> where TableType : DatabaseTable, new()
	{
		private readonly TableInformation<TableType> tableInformation;
		private Expression<Func<TableType, bool>> whereStatement;

		public SqliteDeleteManager()
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
			var parameterFactory = new ParameterFactory();
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
