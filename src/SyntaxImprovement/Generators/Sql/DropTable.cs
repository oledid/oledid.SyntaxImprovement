using oledid.SyntaxImprovement.Generators.Sql.Internal;

namespace oledid.SyntaxImprovement.Generators.Sql
{
	public class DropTable<TableType> where TableType : DatabaseTable, new()
	{
		public SqlQuery ToQuery()
		{
			var tableManager = new DropManager<TableType>();
			return tableManager.ToQuery();
		}
	}
}
