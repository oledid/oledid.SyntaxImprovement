using System;
using System.Linq.Expressions;

namespace oledid.SyntaxImprovement.Generators.Sql.Internal
{
	public class SelectWhere<TableType> : SelectBase<TableType> where TableType : DatabaseTable, new()
	{
		internal SelectWhere(MsSqlSelectManager<TableType> manager) : base(manager)
		{
		}

		public OrderedSelect<TableType> OrderBy(Expression<Func<TableType, object>> columnSelector, bool descending = false)
		{
			Manager.OrderByExpression(columnSelector, !descending);
			return new OrderedSelect<TableType>(Manager);
		}
	}
}
