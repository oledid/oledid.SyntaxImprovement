using oledid.SyntaxImprovement.Generators.Sql;

namespace oledid.SyntaxImprovement.Tests.Generators.Sql.TestModels
{
	public class Person : DatabaseTable
	{
		[IsPrimaryKey]
		[IsIdentity]
		public int Id { get; set; }

		public string Name { get; set; }

		public override string GetTableName()
		{
			return nameof(Person);
		}
	}

	public class PersonWithSchema : DatabaseTable
	{
		[IsPrimaryKey]
		[IsIdentity]
		public int Id { get; set; }

		public string Name { get; set; }

		public override string GetTableName()
		{
			return nameof(Person);
		}

		public override string GetSchemaName()
		{
			return "MySchema";
		}
	}
}
