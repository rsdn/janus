using System;
using System.Collections.ObjectModel;

namespace Rsdn.Janus
{
	public static class SchemaHelper
	{
		#region DbsmSchema
		public static bool IsTableExist(this DbsmSchema schema, string tableName)
		{
			if (schema == null)
				throw new ArgumentNullException("schema");
			if (tableName == null)
				throw new ArgumentNullException("tableName");

			foreach (var table in schema.Tables)
				if (tableName == table.Name)
					return true;
			return false;
		}

		public static DbsmTable GetTable(this DbsmSchema schema, string tableName)
		{
			if (schema == null)
				throw new ArgumentNullException("schema");
			if (tableName == null)
				throw new ArgumentNullException("tableName");

			foreach (var table in schema.Tables)
				if (tableName == table.Name)
					return table;
			return null;
		}

		public static bool IsGeneratorExist(this DbsmSchema schema, string gname)
		{
			if (schema == null)
				throw new ArgumentNullException("schema");
			if (gname == null)
				throw new ArgumentNullException("gname");

			foreach (var gen in schema.Generators)
				if (gname == gen.Name)
					return true;
			return false;
		}
		#endregion

		#region DbsmTable

		#region Columns
		public static int GetDiffColumnsCount(this DbsmTable table1, DbsmTable table2)
		{
			if (table1 == null)
				throw new ArgumentNullException("table1");
			if (table2 == null)
				throw new ArgumentNullException("table2");

			var cdf = 0;
			foreach (var column in table1.Columns)
				if (!table2.IsColumnExist(column.Name))
					cdf++;
			return cdf;
		}

		public static string ColumnsList(this DbsmTable table)
		{
			if (table == null)
				throw new ArgumentNullException("table");

			return table.Columns.Project(col => col.Name).Join(", ");
		}

		public static bool IsColumnExist(this DbsmTable table, string cname)
		{
			if (table == null)
				throw new ArgumentNullException("table");
			if (cname == null)
				throw new ArgumentNullException("cname");

			foreach (var column in table.Columns)
				if (column.Name == cname)
					return true;
			return false;
		}

		public static DbsmColumn GetColumn(this DbsmTable table, string cname)
		{
			if (table == null)
				throw new ArgumentNullException("table");
			if (cname == null)
				throw new ArgumentNullException("cname");

			foreach (var column in table.Columns)
				if (column.Name == cname)
					return column;
			return null;
		}

		public static bool IsExactColumn(this DbsmTable table, DbsmColumn inputColumn)
		{
			if (inputColumn == null)
				return false;
			var column = table.GetColumn(inputColumn.Name);
			return column != null && column.Equals(inputColumn);
		}
		#endregion

		#region Keys
		public static bool IsKeyExist(this DbsmTable table, string keyName)
		{
			if (table == null)
				throw new ArgumentNullException("table");
			if (keyName == null)
				throw new ArgumentNullException("keyName");

			foreach (var key in table.Keys)
				if (key.Name == keyName)
					return true;
			return false;
		}

		public static DbsmKey GetKey(this DbsmTable table, string keyName)
		{
			if (table == null)
				throw new ArgumentNullException("table");
			if (keyName == null)
				throw new ArgumentNullException("keyName");

			foreach (var key in table.Keys)
				if (key.Name == keyName)
					return key;
			return null;
		}

		public static bool IsKeyExist(this DbsmTable table, string keyName, DbsmConstraintType keyType)
		{
			if (table == null)
				throw new ArgumentNullException("table");
			if (keyName == null)
				throw new ArgumentNullException("keyName");

			foreach (var key in table.Keys)
				if (keyName == key.Name && keyType == key.KeyType)
					return true;
			return false;
		}

		public static bool IsKeyExist(this DbsmTable table, DbsmConstraintType keyType, string columnList)
		{
			if (table == null)
				throw new ArgumentNullException("table");
			if (columnList == null)
				throw new ArgumentNullException("columnList");

			foreach (var key in table.Keys)
				if (keyType == key.KeyType && columnList == key.Columns)
					return true;
			return false;
		}

		public static DbsmKey GetKey(this DbsmTable table, DbsmConstraintType keyType, string columnList)
		{
			if (table == null)
				throw new ArgumentNullException("table");
			if (columnList == null)
				throw new ArgumentNullException("columnList");

			foreach (var key in table.Keys)
				if (key.KeyType == keyType && key.Columns == columnList)
					return key;
			return null;
		}

		public static DbsmKey GetKey(
			this DbsmTable table,
			DbsmConstraintType keyType,
			string relTable,
			string relColumns)
		{
			if (table == null)
				throw new ArgumentNullException("table");
			if (relTable == null)
				throw new ArgumentNullException("relTable");
			if (relColumns == null)
				throw new ArgumentNullException("relColumns");

			foreach (var key in table.Keys)
				if (keyType == key.KeyType && relColumns == key.RelColumns && relTable == key.RelTable)
					return key;
			return null;
		}

		public static DbsmKey GetKey2(
			this DbsmTable table,
			DbsmConstraintType keyType,
			string relTable,
			string relColumns)
		{
			if (table == null)
				throw new ArgumentNullException("table");

			var rt = relTable != null ? relTable.Trim() : null;
			var rc = relColumns != null ? relColumns.Trim() : null;
			if (table.Keys == null || string.IsNullOrEmpty(rt) || string.IsNullOrEmpty(rc))
				return null;

			foreach (var key in table.Keys)
			{
				if (keyType != key.KeyType || relTable != key.RelTable)
					continue;
				var rcols2 = new Collection<string>(key.RelColumns.Split(','));
				var isf = true;
				foreach (var column in relColumns.Split(','))
					if (!rcols2.Contains(column))
					{
						isf = false;
						break;
					}
				if (isf)
					return key;
			}
			return null;
		}

		public static DbsmKey KeyPrimary(this DbsmTable table)
		{
			if (table == null)
				throw new ArgumentNullException("table");

			foreach (var key in table.Keys)
				if (key.KeyType == DbsmConstraintType.KeyPrimary)
					return key;
			return null;
		}
		#endregion

		#region Indexes
		public static bool IsIndexExist(this DbsmTable table, string name)
		{
			if (table == null)
				throw new ArgumentNullException("table");
			if (name == null)
				throw new ArgumentNullException("name");

			foreach (var index in table.Indexes)
				if (index.Name == name)
					return true;
			return false;
		}

		public static DbsmIndex GetIndex(this DbsmTable table, string name)
		{
			if (table == null)
				throw new ArgumentNullException("table");
			if (name == null)
				throw new ArgumentNullException("name");

			foreach (var index in table.Indexes)
				if (index.Name == name)
					return index;
			return null;
		}
		#endregion
		#endregion
	}
}
