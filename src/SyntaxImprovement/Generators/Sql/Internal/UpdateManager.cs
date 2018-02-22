using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace oledid.SyntaxImprovement.Generators.Sql.Internal
{
	internal class UpdateManager<TableType> where TableType : DatabaseTable, new()
	{
		private readonly TableInformation<TableType> tableInformation;
		private readonly List<SetExpression<TableType>> setExpressions;
		private Expression<Func<TableType, bool>> whereStatement;

		public UpdateManager()
		{
			tableInformation = new TableInformation<TableType>();
			setExpressions = new List<SetExpression<TableType>>();
		}

		public Update<TableType> Update()
		{
			return new Update<TableType>(this);
		}

		public void SetWhereExpression(Expression<Func<TableType, bool>> expression)
		{
			if (whereStatement != null)
				throw new NotSupportedException("Where expression already set.");
			whereStatement = expression;
		}

		public void AddSetExpression(SetExpression<TableType> expression)
		{
			setExpressions.Add(expression);
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
			var whereQueryPart = WhereQueryGenerator.CreateQuery(parameterFactory, whereStatement);
			var query = "UPDATE " + tableName + " SET ";
			query += GenerateSetExpressions(parameterFactory);
			query += whereQueryPart;
			return query;
		}

		private string GenerateSetExpressions(ParameterFactory parameterFactory)
		{
			var expressions = new List<string>();
			foreach (var setExpression in setExpressions)
			{
				expressions.Add(setExpression.GetColumnName() + " = " + parameterFactory.Create(setExpression.Value));
			}

			return string.Join(", ", expressions);
		}
	}
}
