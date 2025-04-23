using oledid.SyntaxImprovement.Generators.Sql;

namespace oledid.SyntaxImprovement.Tests.Generators.PostgreSQL.TestModels
{
	public class BooleanTestModel : DatabaseTable
	{
		public override string GetTableName() => nameof(BooleanTestModel);

		[IsPrimaryKey]
		public string IdStr { get; set; }

		public bool IsActive { get; set; }
	}
}
