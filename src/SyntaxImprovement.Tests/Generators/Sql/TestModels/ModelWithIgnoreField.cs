using oledid.SyntaxImprovement.Generators.Db;

namespace oledid.SyntaxImprovement.Tests.Generators.Sql.TestModels
{
	public class ModelWithIgnoreField : DatabaseTable
	{
		[IsPrimaryKey]
		[IsIdentity]
		public int Id { get; set; }

		public string Name { get; set; }

		[Ignore]
		public string Nope { get; set; }

		public override string GetTableName() => nameof(ModelWithIgnoreField);
	}
}
