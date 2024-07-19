namespace oledid.SyntaxImprovement.Generators.Sql.Internal
{
	public class UpdateBase<TableType> where TableType : DatabaseTable, new()
	{
		internal readonly MsSqlUpdateManager<TableType> manager;

		internal UpdateBase(MsSqlUpdateManager<TableType> manager)
		{
			this.manager = manager;
		}
	}
}
