using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace oledid.SyntaxImprovement.Generators.Sql.Internal
{
	internal class MsSqlUpdateManager<TableType> where TableType : DatabaseTable, new()
	{
		private readonly TableInformation<TableType> tableInformation;
		private readonly List<SetExpression<TableType>> setExpressions;
		private Expression<Func<TableType, bool>> whereStatement;

		public MsSqlUpdateManager()
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
			var whereQueryPart = WhereGenerator.CreateQuery(parameterFactory, whereStatement);
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
				var existingIndex = expressions.FindIndex(str => str.StartsWith(setExpression.GetColumnName()));
				if (existingIndex >= 0)
				{
					expressions.RemoveAt(existingIndex);
				}
				var columnName = setExpression.GetColumnName();
				if (setExpression.IsComputedField())
				{
					throw new ComputedFieldExplicitlySetInUpdateException("Cannot update field with IsComputedAttribute: " + columnName);
				}
				expressions.Add(columnName + " = " + parameterFactory.Create(setExpression.Value));
			}

			return string.Join(", ", expressions);
		}
	}
}
