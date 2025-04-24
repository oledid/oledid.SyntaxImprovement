using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace oledid.SyntaxImprovement.Generators.Sql.Internal
{
	internal class OrderByExpressionVisitor<TableType> : ExpressionVisitor where TableType : DatabaseTable, new()
	{
		private readonly Stack<MemberInfo> memberStack;
		private readonly List<string> orderByStatements;
		private readonly bool ascending;
		private readonly DatabaseType databaseType;

		internal OrderByExpressionVisitor(bool ascending)
		{
			memberStack = new Stack<MemberInfo>();
			orderByStatements = new List<string>();
			this.ascending = ascending;
			databaseType = new TableInformation<TableType>().GetDatabaseType();
		}

		protected override Expression VisitBinary(BinaryExpression node)
		{
			TryFinishStatement();
			return base.VisitBinary(node);
		}

		protected override Expression VisitMember(MemberExpression node)
		{
			if (node.Expression != null && node.Expression.Type == typeof(TableType))
			{
				memberStack.Push(node.Member);
				TryFinishStatement();
			}
			return base.VisitMember(node);
		}

		private void TryFinishStatement()
		{
			if (memberStack.Any())
			{
				FinishStatement();
			}
		}

		private void FinishStatement()
		{
			var member = memberStack.Pop();
			AddStatement(member);
		}

		private void AddStatement(MemberInfo member)
		{
			var columnExpression = databaseType.GetColumnName(member.Name);
			if (ascending == false)
			{
				columnExpression += " desc";
			}
			orderByStatements.Add(columnExpression);
		}

		public string GetQuery()
		{
			return string.Join("", orderByStatements);
		}
	}
}
