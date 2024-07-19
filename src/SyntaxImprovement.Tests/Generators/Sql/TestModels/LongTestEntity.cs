using oledid.SyntaxImprovement.Extensions;
using oledid.SyntaxImprovement.Generators.Db;

namespace oledid.SyntaxImprovement.Tests.Generators.Sql.TestModels
{
	public class LongTestEntity : DatabaseTable
	{
		[IsPrimaryKey]
		[IsIdentity]
		public long Id { get; set; }
		public bool IsDeleted { get; set; }

		public override string GetTableName() => nameof(LongTestEntity).RemoveFromEnd("Entity");
	}
}
