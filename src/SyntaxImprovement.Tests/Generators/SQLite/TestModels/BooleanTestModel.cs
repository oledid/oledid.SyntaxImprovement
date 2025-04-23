using oledid.SyntaxImprovement.Generators.Sql;

namespace oledid.SyntaxImprovement.Tests.Generators.Sqlite.TestModels
{
	public class BooleanTestModel : DatabaseTable
	{
		public override string GetTableName() => nameof(BooleanTestModel);

		public override DatabaseType GetDatabaseType() => DatabaseType.SQLite;

		[IsPrimaryKey]
		public string IdStr { get; set; }

		public bool IsActive { get; set; }
	}
}
