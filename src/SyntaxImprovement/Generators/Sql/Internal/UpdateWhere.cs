﻿namespace oledid.SyntaxImprovement.Generators.Sql.Internal
{
	public class UpdateWhere<TableType> : UpdateBase<TableType> where TableType : DatabaseTable, new()
	{
		internal UpdateWhere(UpdateManager<TableType> manager)
			: base(manager)
		{
		}

		public SqlQuery ToQuery()
		{
			return manager.ToQuery();
		}
	}
}
