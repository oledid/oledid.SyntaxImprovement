﻿using oledid.SyntaxImprovement.Generators.Sql;
using System;

namespace oledid.SyntaxImprovement.Tests.Generators.Sql.TestModels
{
	public class GuidTestEntity : DatabaseTable
	{
		public int Id { get; set; }
		public Guid? Fk { get; set; }
		public bool IsDeleted { get; set; }

		public override string GetTableName() => nameof(GuidTestEntity);
	}
}
