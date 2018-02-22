using oledid.SyntaxImprovement.Generators.Sql.Internal;

namespace oledid.SyntaxImprovement.Generators.Sql
{
	public class DeleteWhere<TableType> : DeleteBase<TableType> where TableType : DatabaseTable, new()
	{
		internal DeleteWhere(DeleteManager<TableType> manager) : base(manager)
		{
		}
	}
}
