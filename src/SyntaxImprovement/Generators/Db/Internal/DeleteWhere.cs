namespace oledid.SyntaxImprovement.Generators.Sql.Internal
{
	public class DeleteWhere<TableType> : DeleteBase<TableType> where TableType : DatabaseTable, new()
	{
		internal DeleteWhere(MsSqlDeleteManager<TableType> manager) : base(manager)
		{
		}

		public SqlQuery ToQuery()
		{
			return manager.ToQuery();
		}
	}
}
