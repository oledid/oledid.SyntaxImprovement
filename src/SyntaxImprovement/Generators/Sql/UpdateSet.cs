using System;
using System.Linq.Expressions;
using oledid.SyntaxImprovement.Generators.Sql.Internal;

namespace oledid.SyntaxImprovement.Generators.Sql
{
	public class UpdateSet<TableType> : Update<TableType> where TableType : DatabaseTable, new()
	{
		internal UpdateSet(UpdateManager<TableType> manager)
			: base(manager)
		{
		}

		public UpdateWhere<TableType> Where(Expression<Func<TableType, bool>> expression)
		{
			manager.SetWhereExpression(expression);
			return new UpdateWhere<TableType>(manager);
		}
	}
}
