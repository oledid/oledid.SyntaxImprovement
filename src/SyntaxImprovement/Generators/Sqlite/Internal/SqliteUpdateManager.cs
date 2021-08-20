using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace oledid.SyntaxImprovement.Generators.Sqlite.Internal
{
	internal class SqliteUpdateManager<TableType> where TableType : SqliteDatabaseTable, new()
	{
		private readonly SqliteTableInformation<TableType> sqliteTableInformation;
		private readonly List<SqliteSetExpression<TableType>> setExpressions;
		private Expression<Func<TableType, bool>> whereStatement;

		public SqliteUpdateManager()
		{
			sqliteTableInformation = new SqliteTableInformation<TableType>();
			setExpressions = new List<SqliteSetExpression<TableType>>();
		}

		public SqliteUpdate<TableType> Update()
		{
			return new SqliteUpdate<TableType>(this);
		}

		public void SetWhereExpression(Expression<Func<TableType, bool>> expression)
		{
			if (whereStatement != null)
				throw new NotSupportedException("Where expression already set.");
			whereStatement = expression;
		}

		public void AddSetExpression(SqliteSetExpression<TableType> expression)
		{
			setExpressions.Add(expression);
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
			var whereQueryPart = SqliteWhereGenerator.CreateQuery(sqliteParameterFactory, whereStatement);
			var query = "UPDATE " + tableName + " SET ";
			query += GenerateSetExpressions(sqliteParameterFactory);
			query += whereQueryPart;
			return query;
		}

		private string GenerateSetExpressions(SqliteParameterFactory sqliteParameterFactory)
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
					throw new SqliteComputedFieldExplicitlySetInUpdateException("Cannot update field with SqliteIsComputedAttribute: " + columnName);
				}
				expressions.Add(columnName + " = " + sqliteParameterFactory.Create(setExpression.Value));
			}

			return string.Join(", ", expressions);
		}
	}
}
