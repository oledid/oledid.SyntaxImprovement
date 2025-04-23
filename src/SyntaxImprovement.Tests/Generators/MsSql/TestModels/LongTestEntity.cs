using oledid.SyntaxImprovement.Generators.Sql;

namespace oledid.SyntaxImprovement.Tests.Generators.MsSql.TestModels
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
