namespace oledid.SyntaxImprovement.Generators.Sql.Internal
{
	public class DropManager<TableType> where TableType : DatabaseTable, new()
	{
		public SqlQuery ToQuery()
		{
			var tableInstance = new TableType();
			var tableName = "[" + tableInstance.GetTableName() + "]";
			var parameterFactory = new ParameterFactory();
			var query = "DROP TABLE " + tableName + ";";
			return new SqlQuery(query, parameterFactory.GetParameters());
		}
	}
}
