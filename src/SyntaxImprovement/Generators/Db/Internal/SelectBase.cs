namespace oledid.SyntaxImprovement.Generators.Sql.Internal
{
	public abstract class SelectBase<TableType> where TableType : DatabaseTable, new()
	{
		internal readonly MsSqlSelectManager<TableType> Manager;

		internal SelectBase(MsSqlSelectManager<TableType> manager)
		{
			Manager = manager;
		}

		public SqlQuery ToQuery()
		{
			return Manager.ToQuery();
		}
	}
}
