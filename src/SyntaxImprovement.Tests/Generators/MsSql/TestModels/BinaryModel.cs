using oledid.SyntaxImprovement.Generators.Sql;

namespace oledid.SyntaxImprovement.Tests.Generators.MsSql.TestModels
{
	public class BinaryModel : DatabaseTable
	{
		public override string GetTableName()
		{
			return nameof(BinaryModel);
		}

		[IsPrimaryKey, IsIdentity]
		public long Id { get; set; }

		public byte[] Data { get; set; }
	}
}
