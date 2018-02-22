﻿using oledid.SyntaxImprovement.Generators.Sql.Internal;

namespace oledid.SyntaxImprovement.Generators.Sql
{
	public class UpdateBase<TableType> where TableType : DatabaseTable, new()
	{
		internal readonly UpdateManager<TableType> manager;

		internal UpdateBase(UpdateManager<TableType> manager)
		{
			this.manager = manager;
		}
	}
}
