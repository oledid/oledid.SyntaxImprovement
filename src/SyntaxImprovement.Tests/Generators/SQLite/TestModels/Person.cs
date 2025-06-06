﻿using oledid.SyntaxImprovement.Generators.Sql;
using System;

namespace oledid.SyntaxImprovement.Tests.Generators.Sqlite.TestModels
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

		public override DatabaseType GetDatabaseType() => DatabaseType.SQLite;
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

		public override DatabaseType GetDatabaseType() => DatabaseType.SQLite;
	}

	public class PersonWithComputedField : DatabaseTable
	{
		[IsPrimaryKey]
		[IsIdentity]
		public int Id { get; set; }

		public string Name { get; set; }

		[IsComputed]
		public DateTime CreatedUtc { get; set; }

		public override string GetTableName()
		{
			return nameof(Person);
		}

		public override DatabaseType GetDatabaseType() => DatabaseType.SQLite;
	}
}
