using System;
using System.Linq.Expressions;
using System.Reflection;

namespace oledid.SyntaxImprovement.Generators.Sqlite.Internal
{
	internal class SqliteColumnExpressionVisitor<TableType> : ExpressionVisitor where TableType : SqliteDatabaseTable, new()
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
				throw new NotSupportedException("Could not find valid column from SqliteUpdate.Set-expression");
			return "[" + memberInfo.Name + "]";
		}

		public bool IsComputedField()
		{
			return memberInfo?.GetCustomAttribute<SqliteIsComputedAttribute>() != null;
		}
	}
}
