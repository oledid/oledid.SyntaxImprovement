using System;
using System.Linq.Expressions;
using oledid.SyntaxImprovement.Generators.Sql.Internal;

namespace oledid.SyntaxImprovement.Generators.Sql
{
	/// <summary>
	/// Used to generate a sql DELETE-query
	/// </summary>
	/// <typeparam name="TableType">A class which inherits from <see cref="DatabaseTable"/></typeparam>
	public class Delete<TableType> : DeleteBase<TableType> where TableType : DatabaseTable, new()
	{
		public Delete() : base(new DeleteManager<TableType>())
		{
		}

		internal Delete(DeleteManager<TableType> manager) : base(manager)
		{
		}

		public DeleteWhere<TableType> Where(Expression<Func<TableType, bool>> expression)
		{
			manager.SetWhereExpression(expression);
			return new DeleteWhere<TableType>(manager);
		}
	}
}
