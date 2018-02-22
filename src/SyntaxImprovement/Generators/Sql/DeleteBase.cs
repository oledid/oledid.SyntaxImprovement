using oledid.SyntaxImprovement.Generators.Sql.Internal;

namespace oledid.SyntaxImprovement.Generators.Sql
{
	public class DeleteBase<TableType> where TableType : DatabaseTable, new()
	{
		internal readonly DeleteManager<TableType> manager;

		internal DeleteBase(DeleteManager<TableType> manager)
		{
			this.manager = manager;
		}

		public SqlQuery ToQuery()
		{
			return manager.ToQuery();
		}
	}
}
