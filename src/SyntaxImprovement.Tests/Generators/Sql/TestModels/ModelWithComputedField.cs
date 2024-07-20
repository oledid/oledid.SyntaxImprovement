using oledid.SyntaxImprovement.Generators.Db;

namespace oledid.SyntaxImprovement.Tests.Generators.Sql.TestModels
{
	public class ModelWithComputedField : DatabaseTable
	{
		public int Id { get; set; }

		[IsComputed]
		public int DoubleOfId { get; set; }

		public override string GetTableName()
		{
			return nameof(ModelWithComputedField);
		}
	}
}
