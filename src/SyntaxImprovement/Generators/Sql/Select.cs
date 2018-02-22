using System;
using System.Linq.Expressions;
using oledid.SyntaxImprovement.Generators.Sql.Internal;

namespace oledid.SyntaxImprovement.Generators.Sql
{
	public class Select<TableType> : SelectBase<TableType> where TableType : DatabaseTable, new()
	{
		public Select() : base(new SelectManager<TableType>())
		{
		}

		internal Select(SelectManager<TableType> manager) : base(manager)
		{
		}

		public SelectWhere<TableType> Where(Expression<Func<TableType, bool>> expression)
		{
			Manager.SetWhereExpression(expression);
			return new SelectWhere<TableType>(Manager);
		}
	}
}
