using System;
using System.Threading.Tasks;
using oledid.SyntaxImprovement.Generators.TsFromCs;
using Xunit;

namespace oledid.SyntaxImprovement.Tests.Generators.TsFromCs
{
	public class InterfaceGeneratorTests
	{
		[Fact]
		[Obsolete]
		public void It_can_generate_interfaces()
		{
			const string expected = @"export interface IHub {
	OnMessage(message: string): void;
	OnMessageAsync(message: string): Promise<void>;
	Create(person: IPersonEntity): IPersonEntity;
}

export interface IPersonEntity {
	id: number;
	name?: string;
	isActive: boolean;
	uniqueId: string;
	createdOn: Date;
	deletedOn?: Date;
	parent?: IPersonEntity;
}
";

			var actual = TsFromCsGenerator.GenerateTypescriptInterfaceFromCsharpClass(typeof(IHub), typeof(PersonEntity));
			Assert.Equal(expected.Replace("\r", ""), actual.Replace("\r", ""));
		}

		public interface IHub
		{
			void OnMessage(string message);
			Task OnMessageAsync(string message);
			PersonEntity Create(PersonEntity person);
		}

		public class PersonEntity
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public bool IsActive { get; set; }
			public Guid UniqueId { get; set; }
			public DateTime CreatedOn { get; set; }
			public DateTime? DeletedOn { get; set; }

			public PersonEntity Parent { get; set; }
		}
	}
}
