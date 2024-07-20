namespace oledid.SyntaxImprovement.Generators.Db.Sql.Internal
{
	public abstract class SelectBase<TableType> where TableType : DatabaseTable, new()
	{
		internal readonly SelectManager<TableType> Manager;

		internal SelectBase(SelectManager<TableType> manager)
		{
			Manager = manager;
		}

		public SqlQuery ToQuery()
		{
			return Manager.ToQuery();
		}
	}
}
