using oledid.SyntaxImprovement.Generators.TsFromCs;
using System;
using Xunit;

namespace oledid.SyntaxImprovement.Tests.Generators.TsFromCs
{
	public class TsFromCsGeneratorTests
	{
		[Fact]
		public void It_can_generate_dtos()
		{
			var result = TsFromCsGenerator.GenerateTypescriptInterfaceFromCsharpClass(typeof(PersonEntity), typeof(TypesEntity));
			Assert.Equal("", result);
		}

		public class PersonEntity
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public bool IsActive { get; set; }
			public Guid UniqueId { get; set; }
			public DateTime CreatedOn { get; set; }
			public DateTime? DeletedOn { get; set; }

			public TypesEntity FavoriteTypes { get; set; }
			public PersonEntity Parent { get; set; }
		}

		public class TypesEntity
		{
			public int Int { get; set; }
			public int? NullableInt { get; set; }
			public bool? NullableBool { get; set; }
			public long Long { get; set; }
			public long? NullableLong { get; set; }
			public short Short { get; set; }
			public short? NullableShort { get; set; }
			public string String { get; set; }
			public Guid? NullableGuid { get; set; }
		}
	}
}
