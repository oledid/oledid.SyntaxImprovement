namespace oledid.SyntaxImprovement.Generators.Sql.Internal
{
	public class DeleteBase<TableType> where TableType : DatabaseTable, new()
	{
		internal readonly MsSqlDeleteManager<TableType> manager;

		internal DeleteBase(MsSqlDeleteManager<TableType> manager)
		{
			this.manager = manager;
		}
	}
}
