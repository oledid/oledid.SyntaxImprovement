using oledid.SyntaxImprovement.Generators.Sql;

namespace oledid.SyntaxImprovement.Tests.Generators.Sqlite.TestModels
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

		public override DatabaseType GetDatabaseType() => DatabaseType.SQLite;
	}
}
