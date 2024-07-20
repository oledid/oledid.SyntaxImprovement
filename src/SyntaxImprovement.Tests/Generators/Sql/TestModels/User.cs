﻿using System;
using oledid.SyntaxImprovement.Generators.Sql;

namespace oledid.SyntaxImprovement.Tests.Generators.Sql.TestModels
{
	public class User : DatabaseTable
	{
		[IsPrimaryKey]
		public Guid Id { get; set; }

		public string ExternalId { get; set; }
		public string ExternalIdType { get; set; }
		public Guid? PersonId { get; set; }
		public DateTime? LastLogin { get; set; }
		public bool IsActive { get; set; }
		public bool IsAdmin { get; set; }
		public bool CanWrite { get; set; }
		public string ExternalName { get; set; }

		public override string GetTableName()
		{
			return nameof(User);
		}

		public override string GetSchemaName()
		{
			return "userschema";
		}
	}
}
