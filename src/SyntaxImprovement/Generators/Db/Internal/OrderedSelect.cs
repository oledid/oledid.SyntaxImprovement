using System;
using System.Linq.Expressions;

namespace oledid.SyntaxImprovement.Generators.Sql.Internal
{
	public class OrderedSelect<TableType> : SelectBase<TableType> where TableType : DatabaseTable, new()
	{
		internal OrderedSelect(MsSqlSelectManager<TableType> manager) : base(manager)
		{
		}

		public OrderedSelect<TableType> ThenBy(Expression<Func<TableType, object>> expression, bool descending = false)
		{
			var ascending = !descending;
			Manager.ThenByExpression(expression, ascending);
			return new OrderedSelect<TableType>(Manager);
		}
	}
}
