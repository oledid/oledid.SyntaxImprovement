using oledid.SyntaxImprovement.Generators.Sql.Internal;

namespace oledid.SyntaxImprovement.Generators.Sql
{
	public class SelectWhere<TableType> : SelectBase<TableType> where TableType : DatabaseTable, new()
	{
		internal SelectWhere(SelectManager<TableType> manager) : base(manager)
		{
		}
	}
}
