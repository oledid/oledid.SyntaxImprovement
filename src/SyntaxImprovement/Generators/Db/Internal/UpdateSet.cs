using System;
using System.Linq.Expressions;

namespace oledid.SyntaxImprovement.Generators.Sql.Internal
{
	public class UpdateSet<TableType> : Update<TableType> where TableType : DatabaseTable, new()
	{
		internal UpdateSet(MsSqlUpdateManager<TableType> manager)
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
