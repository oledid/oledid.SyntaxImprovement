using Dapper;
using System;
using System.Data;

namespace oledid.SyntaxImprovement.Tests.Generators.SQLite
{
	internal static class DapperTypeHandlerRegistration
	{
		private static bool _registered = false;

		public static void Register()
		{
			if (_registered) return;

			SqlMapper.AddTypeHandler(new GuidTypeHandler());
			SqlMapper.AddTypeHandler(new DateTimeTypeHandler());

			_registered = true;
		}
	}

	internal class DateTimeTypeHandler : SqlMapper.TypeHandler<object>
	{
		public override object Parse(object value)
		{
			return DateTime.Parse(value.ToString());
		}

		public override void SetValue(IDbDataParameter parameter, object value)
		{
			if (value is DateTime dateTime)
			{
				parameter.Value = dateTime;
			}
			else
			{
				throw new ArgumentException("Value must be a DateTime.");
			}
		}
	}

	internal class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
	{
		public override Guid Parse(object value)
			=> new((string)value);

		public override void SetValue(System.Data.IDbDataParameter parameter, Guid value)
			=> parameter.Value = value.ToString();
	}
}
