using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace oledid.SyntaxImprovement.Generators.Db.Sql.Internal
{
	internal static class OrderByGenerator
	{
		public static string CreateQuery<TableType>(List<Tuple<Expression<Func<TableType, object>>, bool>> expressions) where TableType : DatabaseTable, new()
		{
			var queryList = new List<string>();
			foreach (var expression in expressions)
			{
				var visitor = new OrderByExpressionVisitor<TableType>(expression.Item2);
				visitor.Visit(expression.Item1);
				queryList.Add(visitor.GetQuery());
			}
			return queryList.Any()
				? " ORDER BY " + string.Join(", ", queryList)
				: string.Empty;
		}
	}
}
