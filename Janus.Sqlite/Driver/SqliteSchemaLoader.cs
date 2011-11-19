using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;

namespace Rsdn.Janus.Sqlite
{
	internal static class SqliteSchemaLoader
	{
		public static DBSchema LoadSchema(string constr)
		{
			var csb = new SQLiteConnectionStringBuilder(constr);
			using (var con = new SQLiteConnection(csb.ConnectionString))
			{
				con.Open();

				var dbsc = new DBSchema
				{
					Name = csb.DataSource,
					Tables = GetTables(con)
				};

				foreach (var eTable in dbsc.Tables)
				{
					eTable.Keys = GetKeys(con, eTable).ToArray();
					eTable.Indexes = GetIndexes(con, eTable).ToArray();
				}

				return dbsc;
			}
		}

		#region Private Methods
		private static List<TableSchema> GetTables(SQLiteConnection con)
		{
			string[] restrict3 = {null, null, null};
			string[] restrict4 = {null, null, null, "TABLE"};
			var aStore = new List<TableSchema>();

			// Текущая версия SQLite.Net (1.0.52.0) не позволяет узнать,
			// какие колонки являются autoincrement.
			// Вытаскиваем эту информацию вручную.
			// Для этого сначала проверяем наличие системной таблицы
			// sqlite_sequence (её может вообще не быть) и считываем
			// из неё имена всех autoincrement колонок.
			//
			var tablesWithAutoIncrementPKs = new List<string>();
			restrict3[2] = "sqlite_sequence";
			if (con.GetSchema("Columns", restrict3).Rows.Count > 0)
				using (var cmd = con.CreateCommand())
				{
					cmd.CommandTimeout = 0;
					cmd.CommandText = "SELECT name FROM sqlite_sequence";
					using (IDataReader r = cmd.ExecuteReader())
						while (r.Read())
							tablesWithAutoIncrementPKs.Add(r.GetString(0));
				}

			var dtTables = con.GetSchema("Tables", restrict4);
			for (var i = 0; i < dtTables.Rows.Count; i++)
			{
				var tRow = dtTables.Rows[i];
				var eTable = new TableSchema {Name = tRow["TABLE_NAME"].ToString()};

				// Columns
				//
				restrict3[2] = eTable.Name;
				var dtShema = con.GetSchema("Columns", restrict3);
				if (dtShema.Rows.Count > 0)
					eTable.Columns = new TableColumnSchema[dtShema.Rows.Count];
				for (var j = 0; j < dtShema.Rows.Count; j++)
				{
					var eColumn = new TableColumnSchema();
					var cRow = dtShema.Rows[j];
					eColumn.Name = cRow["COLUMN_NAME"].ToString();
					eColumn.Size = Convert.ToInt32(cRow["CHARACTER_MAXIMUM_LENGTH"], CultureInfo.InvariantCulture);
					eColumn.Type = TypeSqliteToDbsm(cRow["DATA_TYPE"].ToString());
					eColumn.Nullable = Convert.ToBoolean(cRow["IS_NULLABLE"], CultureInfo.InvariantCulture);

					eColumn.AutoIncrement = Convert.ToBoolean(cRow["PRIMARY_KEY"], CultureInfo.InvariantCulture) &&
						tablesWithAutoIncrementPKs.Contains(eTable.Name);

					if (eColumn.AutoIncrement)
					{
						// В SQLite нет тонкой настройки автоинкремента
						//
						eColumn.Seed = 1;
						eColumn.Increment = 1;
					}

					var hasDefault = Convert.ToBoolean(cRow["COLUMN_HASDEFAULT"], CultureInfo.InvariantCulture);
					eColumn.DefaultValue = hasDefault ? cRow["COLUMN_DEFAULT"].ToString() : null;

					eColumn.DecimalPrecision = cRow["NUMERIC_PRECISION"] == DBNull.Value
						? 0
						: Convert.ToInt32(cRow["NUMERIC_PRECISION"], CultureInfo.InvariantCulture);
					eColumn.DecimalScale = cRow["NUMERIC_SCALE"] == DBNull.Value
						? 0
						: Convert.ToInt32(cRow["NUMERIC_SCALE"], CultureInfo.InvariantCulture);
					eTable.Columns[j] = eColumn;
				}
				aStore.Add(eTable);
			}
			return aStore;
		}

		private static List<KeySchema> GetKeys(DbConnection con, SchemaNamedElement eTable)
		{
			var dbsmKeys = new Dictionary<string, KeySchema>();

			string[] restrict3 = {null, null, eTable.Name};

			#region Primary
			var dtShema = con.GetSchema("Columns", restrict3);
			var dtv = dtShema.DefaultView;
			dtv.RowFilter = "PRIMARY_KEY = 'True'";
			dtv.Sort = "ORDINAL_POSITION ASC";

			if (dtv.Count > 0)
			{
				string columns = null;
				for (var y = 0; y < dtv.Count; y++)
					if (columns == null)
						columns = dtv[y]["COLUMN_NAME"].ToString();
					else
						columns += (", " + dtv[y]["COLUMN_NAME"]);

				var eKey = new KeySchema
				{
					KeyType = ConstraintType.KeyPrimary,
					Name = ("PK_" + eTable.Name),
					Columns = columns
				};
				dbsmKeys.Add(eKey.Name, eKey);
			}
			#endregion

			#region Unique (не поддерживается SQLite)
			//dtv.RowFilter = "PRIMARY_KEY = 'False' AND UNIQUE = 'True'";
			//dtv.Sort = "ORDINAL_POSITION ASC";
			//
			//for (int y = 0; y < dtv.Count; y++)
			//{
			//	string cName = dtv[y]["COLUMN_NAME"].ToString();
			//	if (!dbsmKeys.ContainsKey(cName))
			//	{
			//		DBKey eKey = new DBKey();
			//		eKey.Name = cName;
			//		eKey.KeyType = ConstraintType.Unique;
			//		eKey.ColumnList = cName;
			//		dbsmKeys.Add(cName, eKey);
			//	}
			//}
			#endregion

			#region Foreign
			dtShema = con.GetSchema("ForeignKeys", restrict3);

			for (var x = 0; x < dtShema.Rows.Count; x++)
			{
				var cRow = dtShema.Rows[x];
				var cName = cRow["CONSTRAINT_NAME"].ToString();

				if (dbsmKeys.ContainsKey(cName))
					continue;
				var eKey = new KeySchema();
				string columns = null;
				string rcolumns = null;

				eKey.KeyType = ConstraintType.KeyForeign;
				eKey.Name = cName;
				eKey.RelTable = cRow["TABLE_NAME"].ToString();

				dtv = dtShema.DefaultView;
				dtv.RowFilter = "CONSTRAINT_NAME = '" + cName + "'";
				dtv.Sort = "FKEY_FROM_ORDINAL_POSITION ASC";

				for (var y = 0; y < dtv.Count; y++)
					if (columns == null)
					{
						Debug.Assert(rcolumns == null);
						columns = dtv[y]["FKEY_FROM_COLUMN"].ToString();
						rcolumns = dtv[y]["FKEY_TO_COLUMN"].ToString();
					}
					else
					{
						Debug.Assert(rcolumns != null);
						columns += (", " + dtv[y]["FKEY_FROM_COLUMN"]);
						rcolumns += (", " + dtv[y]["FKEY_TO_COLUMN"]);
					}

				eKey.Columns = columns;
				eKey.RelColumns = rcolumns;
				dbsmKeys.Add(cName, eKey);
			}
			#endregion

			return new List<KeySchema>(dbsmKeys.Values);
		}

		private static List<IndexSchema> GetIndexes(DbConnection con, TableSchema eTable)
		{
			var aStore = new List<IndexSchema>();
			var aHash = new List<string>();

			string[] restrict4 = {null, null, null, null};
			string[] restrict5 = {null, null, null, null, null};

			// INDEX_TYPE = 0 - ascending, 1 - descending
			restrict4[2] = eTable.Name;
			restrict5[2] = eTable.Name;
			var dtShema = con.GetSchema("Indexes", restrict4);
			aStore.Clear();
			aHash.Clear();
			for (var x = 0; x < dtShema.Rows.Count; x++)
			{
				var cRow = dtShema.Rows[x];
				var cName = cRow["INDEX_NAME"].ToString();
				if (eTable.IsKeyExist(cName, ConstraintType.Unique) ||
					eTable.IsKeyExist(cName, ConstraintType.KeyPrimary) ||
						eTable.IsKeyExist(cName, ConstraintType.KeyForeign) ||
							Convert.ToBoolean(cRow["PRIMARY_KEY"], CultureInfo.InvariantCulture))
					continue;
				if (aHash.Contains(cName))
					continue;
				var eIndex = new IndexSchema();
				var columns = String.Empty;
				aHash.Add(cName);
				eIndex.Name = cName;
				eIndex.Unique = Convert.ToBoolean(cRow["UNIQUE"], CultureInfo.InvariantCulture);
				if (cRow["TYPE"] == DBNull.Value)
					eIndex.Sort = SortOrder.Ascending;
				else
					eIndex.Sort = Convert.ToInt32(cRow["TYPE"], CultureInfo.InvariantCulture) == 0
						? SortOrder.Ascending
						: SortOrder.Descending;
				eIndex.IsActive = true;
				// !Convert.ToBoolean(cRow["IS_INACTIVE"], CultureInfo.InvariantCulture);

				restrict5[3] = cName;
				var dtIndexColumns = con.GetSchema("IndexColumns", restrict5);

				var first = true;
				for (var y = 0; y < dtIndexColumns.Rows.Count; y++)
				{
					if (first)
						first = false;
					else
						columns += ", ";

					columns += dtIndexColumns.Rows[y]["COLUMN_NAME"];
				}

				eIndex.Columns = columns;
				aStore.Add(eIndex);
			}
			return aStore;
		}
		#endregion

		#region Преобразование типов SQLite <-> Dbsm.Types
		private static ColumnType TypeSqliteToDbsm(string typeName)
		{
			var typeNameParts = typeName.ToUpper(CultureInfo.CurrentCulture).Split(' ');

			switch (typeNameParts[0])
			{
				// Boolean types
				//
				case "BIT":
				case "BOOL":
				case "YESNO":
				case "LOGICAL":
					return ColumnType.Boolean;

				// Integer types
				//
				case "TINYINT":
					return ColumnType.TinyInt;
				case "SMALLINT":
					return ColumnType.SmallInt;
				case "INT":
				case "INTEGER": // The "Integer" type is 64 bit in SQLite, but all others wants it to be 32-bit.
					return ColumnType.Integer;
				case "AUTOINCREMENT":
				case "BIGINT":
				case "COUNTER":
				case "IDENTITY":
				case "LONG":
					return ColumnType.BigInt;

				// Floating point types
				//
				case "REAL":
					return ColumnType.Float;
				case "DOUBLE":
				case "FLOAT":
					return ColumnType.DoublePrecision;

				// Decimal types
				//
				case "CURRENCY":
				case "MONEY":
					return ColumnType.Money;
				case "DECIMAL":
				case "NUMERIC":
					return ColumnType.Decimal;

				// Blob types
				//
				case "BINARY":
				case "BLOB":
					return ColumnType.Binary;
				case "GENERAL":
				case "OLEOBJECT":
				case "VARBINARY":
					return ColumnType.BinaryVaring;
				case "IMAGE":
					return ColumnType.BlobSubtypeImage;

				// String types
				//
				case "CHAR":
					return ColumnType.Character;
				case "NCHAR":
					return ColumnType.NCharacter;
				case "TEXT":
				case "VARCHAR":
					return ColumnType.CharacterVaring;
				case "NVARCHAR":
				case "MEMO":
				case "NOTE":
				case "STRING":
					return ColumnType.NCharacterVaring;
				case "NTEXT":
					return ColumnType.BlobSubtypeNText;

				// Date & Time types
				//
				case "DATE":
				case "DATETIME":
					return ColumnType.Date;
				case "TIME":
					return ColumnType.Time;
				case "TIMESTAMP":
					return ColumnType.Timestamp;

				// Guid types
				//
				case "GUID":
				case "UNIQUEIDENTIFIER":
					return ColumnType.Guid;

				default:
					throw new NotSupportedException("Data type '" + typeName + "' is not supported by " +
						SqliteDriver.DriverName);
			}
		}

		public static string TypeDbsmToSqlite(TableColumnSchema eColumn)
		{
			switch (eColumn.Type)
			{
				case ColumnType.Boolean:
					return "BIT";
				case ColumnType.BigInt:
					return "BIGINT";
				case ColumnType.Binary:
					return "BINARY";
				case ColumnType.Character:
					return "CHAR(" + eColumn.Size + ")";
				case ColumnType.CharacterVaring:
					return "VARCHAR(" + eColumn.Size + ")";
				case ColumnType.Timestamp:
					return "TIMESTAMP";
				case ColumnType.Decimal:
					return "DECIMAL(" + eColumn.DecimalPrecision + ", " + eColumn.DecimalScale + ")";
				case ColumnType.Integer:
					// There should be "INT" but column type Integer is widelly used as an autoincrement
					// type which must be Int64 for SQLite. So there is an implicit 32-64 bit conversion.
					//
					return "INTEGER";
				case ColumnType.Float:
					return "REAL(" + eColumn.DecimalPrecision + ")";
				case ColumnType.DoublePrecision:
					return "DOUBLE(" + eColumn.DecimalPrecision + ")";
				case ColumnType.Money:
					return "MONEY";
				case ColumnType.NCharacter:
					return "NCHAR(" + eColumn.Size + ")";
				case ColumnType.NCharacterVaring:
					return "NVARCHAR(" + eColumn.Size + ")";
				case ColumnType.Real:
					return "REAL";
				case ColumnType.SmallInt:
					return "SMALLINT";
				case ColumnType.TinyInt:
					return "TINYINT";
				case ColumnType.Guid:
					return "UNIQUEIDENTIFIER";
				case ColumnType.BinaryVaring:
					return "VARBINARY";
				case ColumnType.BlobSubtypeNText:
					return "NTEXT";
				case ColumnType.BlobSubtypeImage:
					return "IMAGE";
				default:
					throw new NotSupportedException("Data type '" + eColumn.Type + "' is not supported by " +
						SqliteDriver.DriverName);
			}
		}
		#endregion
	}
}