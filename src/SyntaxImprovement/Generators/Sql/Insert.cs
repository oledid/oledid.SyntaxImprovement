using System.Collections.Generic;
using oledid.SyntaxImprovement.Generators.Sql.Internal;

namespace oledid.SyntaxImprovement.Generators.Sql
{
	/// <summary>
	/// Used to generate a sql INSERT-query
	/// </summary>
	/// <typeparam name="TableType">A class which inherits from <see cref="DatabaseTable"/></typeparam>
	public class Insert<TableType> where TableType : DatabaseTable, new()
	{
		private readonly List<TableType> instances;

		public Insert()
		{
			instances = new List<TableType>();
		}

		private Insert(List<TableType> instances)
		{
			this.instances = instances;
		}

		public Insert<TableType> Add(TableType instance, params TableType[] otherInstances)
		{
			var list = new List<TableType>();
			list.AddRange(instances);
			list.Add(instance);
			list.AddRange(otherInstances);
			return new Insert<TableType>(list);
		}

		public Insert<TableType> Add(IEnumerable<TableType> instancesToAdd)
		{
			var list = new List<TableType>();
			list.AddRange(instances);
			list.AddRange(instancesToAdd);
			return new Insert<TableType>(list);
		}

		public SqlQuery ToQuery()
		{
			var manager = new InsertManager<TableType>();
			return manager.ToQuery(instances);
		}
	}
}
