using oledid.SyntaxImprovement.Generators.Sql;

namespace oledid.SyntaxImprovement.Tests.Generators.MsSql.TestModels
{
	public class BooleanTestModel : DatabaseTable
	{
		public override string GetTableName() => nameof(BooleanTestModel);

		public override DatabaseType GetDatabaseType() => DatabaseType.MSSQL;

		[IsPrimaryKey]
		public string IdStr { get; set; }

		public bool IsActive { get; set; }
	}
}
