﻿using System;
using System.Linq.Expressions;
using oledid.SyntaxImprovement.Generators.Sql.Internal;

namespace oledid.SyntaxImprovement.Generators.Sql
{
	/// <summary>
	/// Used to generate a sql SELECT-query
	/// </summary>
	/// <typeparam name="TableType">A class which inherits from <see cref="DatabaseTable"/></typeparam>
	public class Select<TableType> : SelectBase<TableType> where TableType : DatabaseTable, new()
	{
		public Select(int? top = null, IncludeFields<TableType> includeFields = null) : base(new MsSqlSelectManager<TableType>(top, includeFields))
		{
		}

		internal Select(MsSqlSelectManager<TableType> manager) : base(manager)
		{
		}

		public SelectWhere<TableType> Where(Expression<Func<TableType, bool>> expression)
		{
			Manager.SetWhereExpression(expression);
			return new SelectWhere<TableType>(Manager);
		}

		public OrderedSelect<TableType> OrderBy(Expression<Func<TableType, object>> expression, bool descending = false)
		{
			var ascending = !descending;
			Manager.OrderByExpression(expression, ascending);
			return new OrderedSelect<TableType>(Manager);
		}
	}
}
