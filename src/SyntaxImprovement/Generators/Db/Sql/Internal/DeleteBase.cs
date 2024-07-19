namespace oledid.SyntaxImprovement.Generators.Db.Sql.Internal
{
	public class DeleteBase<TableType> where TableType : DatabaseTable, new()
	{
		internal readonly DeleteManager<TableType> manager;

		internal DeleteBase(DeleteManager<TableType> manager)
		{
			this.manager = manager;
		}
	}
}
