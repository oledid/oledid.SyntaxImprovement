using System.Collections.Generic;
using oledid.SyntaxImprovement.Generators.Sqlite.Internal;

namespace oledid.SyntaxImprovement.Generators.Sqlite
{
	/// <summary>
	/// Used to generate a sql INSERT-query
	/// </summary>
	/// <typeparam name="TableType">A class which inherits from <see cref="SqliteDatabaseTable"/></typeparam>
	public class SqliteInsert<TableType> where TableType : SqliteDatabaseTable, new()
	{
		private readonly List<TableType> instances;

		public SqliteInsert()
		{
			instances = new List<TableType>();
		}

		private SqliteInsert(List<TableType> instances)
		{
			this.instances = instances;
		}

		public SqliteInsert<TableType> Add(TableType instance, params TableType[] otherInstances)
		{
			var list = new List<TableType>();
			list.AddRange(instances);
			list.Add(instance);
			list.AddRange(otherInstances);
			return new SqliteInsert<TableType>(list);
		}

		public SqliteInsert<TableType> Add(IEnumerable<TableType> instancesToAdd)
		{
			var list = new List<TableType>();
			list.AddRange(instances);
			list.AddRange(instancesToAdd);
			return new SqliteInsert<TableType>(list);
		}

		public SqliteQuery ToQuery()
		{
			var manager = new SqliteInsertManager<TableType>();
			return manager.ToQuery(instances);
		}

		/// <summary>
		/// Returns true if any item has been added to the query, false if not.
		/// </summary>
		public bool HasValue => instances.Count > 0;
	}
}
