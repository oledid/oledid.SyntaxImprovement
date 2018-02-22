using oledid.SyntaxImprovement.Generators.Sql.Internal;

namespace oledid.SyntaxImprovement.Generators.Sql
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
