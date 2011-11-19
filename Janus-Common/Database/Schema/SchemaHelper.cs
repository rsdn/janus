using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Rsdn.Janus
{
	public static class SchemaHelper
	{
		#region DBSchema

		public static bool IsTableExist(this DBSchema schema, string tableName)
		{
			if (schema == null)
				throw new ArgumentNullException("schema");
			if (tableName == null)
				throw new ArgumentNullException("tableName");

			return schema.Tables.Any(table => tableName == table.Name);
		}

		public static TableSchema GetTable(this DBSchema schema, string tableName)
		{
			if (schema == null)
				throw new ArgumentNullException("schema");
			if (tableName == null)
				throw new ArgumentNullException("tableName");

			return schema.Tables.FirstOrDefault(table => tableName == table.Name);
		}

		public static bool IsGeneratorExist(this DBSchema schema, string gname)
		{
			if (schema == null)
				throw new ArgumentNullException("schema");
			if (gname == null)
				throw new ArgumentNullException("gname");

			return schema.Generators.Any(gen => gname == gen.Name);
		}

		#endregion

		#region TableSchema

		#region Columns

		public static int GetDiffColumnsCount(this TableSchema table1, TableSchema table2)
		{
			if (table1 == null)
				throw new ArgumentNullException("table1");
			if (table2 == null)
				throw new ArgumentNullException("table2");

			return table1.Columns.Count(column => !table2.IsColumnExist(column.Name));
		}

		public static string ColumnsList(this TableSchema table, Func<string, string> nameDecorator)
		{
			if (table == null)
				throw new ArgumentNullException("table");

			return
				table
					.Columns
					.Select(col => nameDecorator(col.Name))
					.JoinStrings(", ");
		}

		public static bool IsColumnExist(this TableSchema table, string cname)
		{
			if (table == null)
				throw new ArgumentNullException("table");
			if (cname == null)
				throw new ArgumentNullException("cname");

			return table.Columns.Any(column => column.Name == cname);
		}

		public static TableColumnSchema GetColumn(this TableSchema table, string cname)
		{
			if (table == null)
				throw new ArgumentNullException("table");
			if (cname == null)
				throw new ArgumentNullException("cname");

			return table.Columns.FirstOrDefault(column => column.Name == cname);
		}

		public static bool IsExactColumn(this TableSchema table, TableColumnSchema inputColumn)
		{
			if (inputColumn == null)
				return false;
			var column = table.GetColumn(inputColumn.Name);
			return column != null && column.Equals(inputColumn);
		}

		#endregion

		#region Keys

		public static bool IsKeyExist(this TableSchema table, string keyName)
		{
			if (table == null)
				throw new ArgumentNullException("table");
			if (keyName == null)
				throw new ArgumentNullException("keyName");

			return table.Keys.Any(key => key.Name == keyName);
		}

		public static KeySchema GetKey(this TableSchema table, string keyName)
		{
			if (table == null)
				throw new ArgumentNullException("table");
			if (keyName == null)
				throw new ArgumentNullException("keyName");

			return table.Keys.FirstOrDefault(key => key.Name == keyName);
		}

		public static bool IsKeyExist(this TableSchema table, string keyName, ConstraintType keyType)
		{
			if (table == null)
				throw new ArgumentNullException("table");
			if (keyName == null)
				throw new ArgumentNullException("keyName");

			return table.Keys.Any(key => keyName == key.Name && keyType == key.KeyType);
		}

		public static bool IsKeyExist(this TableSchema table, ConstraintType keyType, string columnList)
		{
			if (table == null)
				throw new ArgumentNullException("table");
			if (columnList == null)
				throw new ArgumentNullException("columnList");

			return table.Keys.Any(key => keyType == key.KeyType && columnList == key.Columns);
		}

		public static KeySchema GetKey(this TableSchema table, ConstraintType keyType, string columnList)
		{
			if (table == null)
				throw new ArgumentNullException("table");
			if (columnList == null)
				throw new ArgumentNullException("columnList");

			return table.Keys.FirstOrDefault(key => key.KeyType == keyType && key.Columns == columnList);
		}

		public static KeySchema GetKey(
			this TableSchema table,
			ConstraintType keyType,
			string relTable,
			string relColumns)
		{
			if (table == null)
				throw new ArgumentNullException("table");
			if (relTable == null)
				throw new ArgumentNullException("relTable");
			if (relColumns == null)
				throw new ArgumentNullException("relColumns");

			return
				table.Keys.FirstOrDefault(
					key => keyType == key.KeyType && relColumns == key.RelColumns && relTable == key.RelTable);
		}

		public static KeySchema GetKey2(
			this TableSchema table,
			ConstraintType keyType,
			string relTable,
			string relColumns)
		{
			if (table == null)
				throw new ArgumentNullException("table");

			var rt = relTable != null ? relTable.Trim() : null;
			var rc = relColumns != null ? relColumns.Trim() : null;
			if (table.Keys == null || string.IsNullOrEmpty(rt) || string.IsNullOrEmpty(rc))
				return null;

			return
				(from key in table.Keys
				 where keyType == key.KeyType && relTable == key.RelTable
				 let rcols2 = new Collection<string>(key.RelColumns.Split(','))
				 let isf = relColumns.Split(',').All(rcols2.Contains)
				 where isf
				 select key).FirstOrDefault();
		}

		public static KeySchema KeyPrimary(this TableSchema table)
		{
			if (table == null)
				throw new ArgumentNullException("table");

			return table.Keys.FirstOrDefault(key => key.KeyType == ConstraintType.KeyPrimary);
		}

		#endregion

		#region Indexes

		public static bool IsIndexExist(this TableSchema table, string name)
		{
			if (table == null)
				throw new ArgumentNullException("table");
			if (name == null)
				throw new ArgumentNullException("name");

			return table.Indexes.Any(index => index.Name == name);
		}

		public static IndexSchema GetIndex(this TableSchema table, string name)
		{
			if (table == null)
				throw new ArgumentNullException("table");
			if (name == null)
				throw new ArgumentNullException("name");

			return table.Indexes.FirstOrDefault(index => index.Name == name);
		}

		#endregion

		#endregion

		public static void SafeRunCommand(string cmdText, Action runner)
		{
			try
			{
				runner();
			}
			catch (Exception ex)
			{
				throw new SchemaChangeException(cmdText, ex);
			}
		}
	}
}