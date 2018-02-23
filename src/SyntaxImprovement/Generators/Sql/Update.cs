﻿using System;
using System.Linq.Expressions;
using oledid.SyntaxImprovement.Generators.Sql.Internal;

namespace oledid.SyntaxImprovement.Generators.Sql
{
	public class Update<TableType> : UpdateBase<TableType> where TableType : DatabaseTable, new()
	{
		public Update() : base(new UpdateManager<TableType>())
		{
		}

		internal Update(UpdateManager<TableType> manager) : base(manager)
		{
		}

		public UpdateSet<TableType> Set(Expression<Func<TableType, object>> columnSelector, object value)
		{
			manager.AddSetExpression(new SetExpression<TableType>
			{
				ColumnExpression = columnSelector,
				Value = value
			});
			return new UpdateSet<TableType>(manager);
		}
	}
}