using System;
using System.Linq.Expressions;
using System.Reflection;

namespace oledid.SyntaxImprovement.Generators.Sql.Internal
{
	internal class ColumnExpressionVisitor<TableType> : ExpressionVisitor where TableType : DatabaseTable, new()
	{
		private MemberInfo memberInfo;

		protected override Expression VisitMember(MemberExpression node)
		{
			if (node.Expression != null && node.Expression.Type == typeof(TableType))
			{
				memberInfo = node.Member;
			}
			return base.VisitMember(node);
		}

		public string GetColumnName()
		{
			if (memberInfo == null)
				throw new NotSupportedException("Could not find valid column from Update.Set-expression");
			return "[" + memberInfo.Name + "]";
		}
	}
}
