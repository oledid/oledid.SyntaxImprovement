using oledid.SyntaxImprovement.Extensions;
using System;
using System.Linq.Expressions;

namespace oledid.SyntaxImprovement.Generators.Db.Sql.Internal
{
	internal static class WhereGenerator
	{
		public static string CreateQuery<TableType>(ParameterFactory parameterFactory, Expression<Func<TableType, bool>> expression) where TableType : DatabaseTable, new()
		{
			var visitor = new WhereExpressionVisitor<TableType>(parameterFactory);
			visitor.Visit(expression);
			var query = visitor.GetQuery();
			return query.HasValue()
				? " WHERE " + query
				: string.Empty;
		}
	}
}
