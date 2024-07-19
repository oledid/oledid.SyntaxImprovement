namespace oledid.SyntaxImprovement.Generators.Db.Sql.Internal
{
	public class DeleteWhere<TableType> : DeleteBase<TableType> where TableType : DatabaseTable, new()
	{
		internal DeleteWhere(DeleteManager<TableType> manager) : base(manager)
		{
		}

		public SqlQuery ToQuery()
		{
			return manager.ToQuery();
		}
	}
}
