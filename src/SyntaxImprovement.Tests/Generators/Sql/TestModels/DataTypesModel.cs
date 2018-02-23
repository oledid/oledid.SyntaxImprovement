using System;
using oledid.SyntaxImprovement.Generators.Sql;

namespace oledid.SyntaxImprovement.Tests.Generators.Sql.TestModels
{
	public class DataTypesModel : DatabaseTable
	{
		public override string GetTableName()
		{
			return nameof(DataTypesModel);
		}

		public override string GetSchemaName()
		{
			return "TestSchema";
		}

		public bool Boolean { get; set; }
		public long Long { get; set; }
		public decimal Decimal { get; set; }
		public DateTime DateTime { get; set; }
		public Guid Guid { get; set; }
	}
}
