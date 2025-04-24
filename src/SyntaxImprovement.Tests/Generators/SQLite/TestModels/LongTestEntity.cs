using oledid.SyntaxImprovement.Generators.Sql;

namespace oledid.SyntaxImprovement.Tests.Generators.Sqlite.TestModels
{
	public class LongTestEntity : DatabaseTable
	{
		[IsPrimaryKey]
		[IsIdentity]
		public long Id { get; set; }
		public bool IsDeleted { get; set; }

		public override string GetTableName() => nameof(LongTestEntity).RemoveFromEnd("Entity");

		public override DatabaseType GetDatabaseType() => DatabaseType.SQLite;
	}
}
