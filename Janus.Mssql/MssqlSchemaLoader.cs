using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace Rsdn.Janus.Mssql
{
	internal static class MssqlSchemaLoader
	{
		public static DBSchema LoadSchema(string constr)
		{
			var csb = new SqlConnectionStringBuilder(constr);
			using (var con = new SqlConnection(csb.ConnectionString))
			{
				con.Open();

				var dbsc = new DBSchema
				{
					Name = csb.InitialCatalog,
					Tables = GetTables(con)
				};

				foreach (var table in dbsc.Tables)
				{
					table.Keys = GetKeys(con, table).ToArray();
					table.Indexes = GetIndexes(con, table).ToArray();
				}

				return dbsc;
			}
		}

		#region Private Methods
		private static LinkRule GetDbsmRule(string rule)
		{
			switch (rule)
			{
				case "CASCADE":
					return LinkRule.Cascade;
				case "SET NULL":
					return LinkRule.SetNull;
				case "SET DEFAULT":
					return LinkRule.SetDefault;
				default:
					return LinkRule.None;
			}
		}

		private static List<TableSchema> GetTables(SqlConnection con)
		{
			var tables = new List<TableSchema>();
			string[] restrict4 = {null, null, null, "TABLE"};
			string[] restrict3 = {null, null, null};

			var dtTables = SqlSchemaFactory.GetSchema(con, "Tables", restrict4);
			for (var i = 0; i < dtTables.Rows.Count; i++)
			{
				var tRow = dtTables.Rows[i];
				var eTable = new TableSchema {Name = tRow["TABLE_NAME"].ToString()};
				// Columns
				restrict3[0] = null;
				restrict3[1] = null;
				restrict3[2] = eTable.Name;

				var dtShema = SqlSchemaFactory.GetSchema(con, "Columns", restrict3);
				if (dtShema.Rows.Count > 0)
				{
					eTable.Columns = new TableColumnSchema[dtShema.Rows.Count];

					for (var j = 0; j < dtShema.Rows.Count; j++)
					{
						var cRow = dtShema.Rows[j];

						var eColumn = new TableColumnSchema
						{
							Name = cRow["COLUMN_NAME"].ToString(),
							Size = Convert.ToInt32(cRow["COLUMN_SIZE"], CultureInfo.InvariantCulture),
							Type = TypeSqlToDbsm(cRow["COLUMN_DATA_TYPE"].ToString()),
							Nullable = Convert.ToBoolean(cRow["IS_NULLABLE"], CultureInfo.InvariantCulture),
							DefaultValue = cRow["COLUMN_DEFAULT"].ToString(),
							Increment = Convert.ToInt32(cRow["IDENTITY_INCREMENT"], CultureInfo.InvariantCulture),
							Seed = Convert.ToInt32(cRow["IDENTITY_SEED"], CultureInfo.InvariantCulture),
							AutoIncrement = Convert.ToBoolean(cRow["IS_IDENTITY"], CultureInfo.InvariantCulture),
							DecimalPrecision = Convert.ToInt32(cRow["NUMERIC_PRECISION"], CultureInfo.InvariantCulture),
							DecimalScale = Convert.ToInt32(cRow["NUMERIC_SCALE"], CultureInfo.InvariantCulture)
						};
						eColumn.DefaultValue = string.IsNullOrEmpty(eColumn.DefaultValue)
												? null
												: UnBracket.ParseUnBracket(eColumn.DefaultValue);

						eTable.Columns[j] = eColumn;
					}
				}

				tables.Add(eTable);
			}
			return tables;
		}

		private static List<KeySchema> GetKeys(SqlConnection con, SchemaNamedElement eTable)
		{
			var keys = new List<KeySchema>();
			var aHash = new Dictionary<string, bool>();

			string[] restrict3 = {null, null, null};
			string[] restrict4 = {null, null, null, null};

			#region Primary keys
			restrict3[0] = null;
			restrict3[1] = null;
			restrict3[2] = eTable.Name;
			var dtShema = SqlSchemaFactory.GetSchema(con, "PrimaryKeys", restrict3);
			aHash.Clear();
			for (var x = 0; x < dtShema.Rows.Count; x++)
			{
				var cRow = dtShema.Rows[x];
				var cName = cRow["CONSTRAINT_NAME"].ToString();
				if (aHash.ContainsKey(cName))
					continue;
				aHash.Add(cName, true);

				var eKey = new KeySchema
				{
					KeyType = ConstraintType.KeyPrimary,
					Name = cName,
					Clustered = Convert.ToBoolean(cRow["IS_CLUSTERED"], CultureInfo.InvariantCulture)
				};

				var columns = new StringBuilder();

				var dtv = dtShema.DefaultView;
				dtv.RowFilter = string.Format("CONSTRAINT_NAME = '{0}'", cName);
				dtv.Sort = "ORDINAL_POSITION ASC";
				for (var y = 0; y < dtv.Count; y++)
				{
					columns.Append(dtv[y]["COLUMN_NAME"]);
					columns.Append(y == dtv.Count - 1 ? string.Empty : ", ");
				}

				eKey.Columns = columns.ToString();
				keys.Add(eKey);
			}
			#endregion

			#region Foreign keys
			restrict4[0] = null;
			restrict4[1] = null;
			restrict4[2] = eTable.Name;
			restrict4[3] = null;
			dtShema = SqlSchemaFactory.GetSchema(con, "ForeignKeys", restrict4);
			aHash.Clear();
			for (var x = 0; x < dtShema.Rows.Count; x++)
			{
				var cRow = dtShema.Rows[x];
				if (Convert.ToBoolean(cRow["IS_DISABLED"], CultureInfo.InvariantCulture))
					continue;
				var cName = cRow["CONSTRAINT_NAME"].ToString();
				if (aHash.ContainsKey(cName))
					continue;
				aHash.Add(cName, true);

				var eKey = new KeySchema
				{
					KeyType = ConstraintType.KeyForeign,
					Name = cName,
					RelTable = cRow["PK_TABLE_NAME"].ToString(),
					UpdateRule = GetDbsmRule(cRow["UPDATE_RULE"].ToString()),
					DeleteRule = GetDbsmRule(cRow["DELETE_RULE"].ToString())
				};

				var fcolumns = new StringBuilder();
				var rcolumns = new StringBuilder();

				var dtv = dtShema.DefaultView;
				dtv.RowFilter = string.Format("CONSTRAINT_NAME = '{0}'", cName);
				dtv.Sort = "ORDINAL_POSITION ASC";
				for (var y = 0; y < dtv.Count; y++)
				{
					fcolumns.Append(dtv[y]["FK_COLUMN_NAME"]);
					fcolumns.Append(y == dtv.Count - 1 ? string.Empty : ", ");

					rcolumns.Append(dtv[y]["PK_COLUMN_NAME"]);
					rcolumns.Append(y == dtv.Count - 1 ? string.Empty : ", ");
				}

				eKey.Columns = fcolumns.ToString();
				eKey.RelColumns = rcolumns.ToString();
				keys.Add(eKey);
			}
			#endregion

			#region Checks
			restrict3[0] = null;
			restrict3[1] = null;
			restrict3[2] = eTable.Name;
			dtShema = SqlSchemaFactory.GetSchema(con, "CheckConstraints", restrict3);
			for (var x = 0; x < dtShema.Rows.Count; x++)
			{
				var row = dtShema.Rows[x];

				var eKey = new KeySchema
				{
					KeyType = ConstraintType.Check,
					Name = row["CONSTRAINT_NAME"].ToString(),
					Source = row["SOURCE"].ToString()
				};

				keys.Add(eKey);
			}
			#endregion

			#region Unique
			restrict3[0] = null;
			restrict3[1] = null;
			restrict3[2] = eTable.Name;
			dtShema = SqlSchemaFactory.GetSchema(con, "UniqueKeys", restrict3);
			aHash.Clear();
			for (var x = 0; x < dtShema.Rows.Count; x++)
			{
				var cRow = dtShema.Rows[x];
				var cName = cRow["CONSTRAINT_NAME"].ToString();
				if (aHash.ContainsKey(cName))
					continue;
				var eKey = new KeySchema();
				aHash.Add(cName, true);
				eKey.KeyType = ConstraintType.Unique;
				eKey.Name = cName;

				var columns = new StringBuilder();

				var dtv = dtShema.DefaultView;
				dtv.RowFilter = "CONSTRAINT_NAME = '" + cName + "'";
				dtv.Sort = "ORDINAL_POSITION ASC";
				for (var y = 0; y < dtv.Count; y++)
				{
					columns.Append(dtv[y]["COLUMN_NAME"]);
					columns.Append(y == dtv.Count - 1 ? string.Empty : ", ");
				}

				eKey.Columns = columns.ToString();
				keys.Add(eKey);
			}
			#endregion

			#region Default constraints
			restrict3[0] = null;
			restrict3[1] = null;
			restrict3[2] = eTable.Name;
			dtShema = SqlSchemaFactory.GetSchema(con, "DefaultConstraints", restrict3);
			for (var x = 0; x < dtShema.Rows.Count; x++)
			{
				var cRow = dtShema.Rows[x];
				var eKey = new KeySchema
				{
					KeyType = ConstraintType.Default,
					Name = cRow["CONSTRAINT_NAME"].ToString(),
					Columns = cRow["COLUMN_NAME"].ToString(),
					Source = UnBracket.ParseUnBracket(cRow["SOURCE"].ToString())
				};
				keys.Add(eKey);
			}
			#endregion

			return keys;
		}

		private static List<IndexSchema> GetIndexes(SqlConnection con, TableSchema eTable)
		{
			var indexes = new List<IndexSchema>();
			var aHash = new Dictionary<string, bool>();
			string[] restrict4 = {null, null, null, null};

			// Indexes
			restrict4[0] = null;
			restrict4[1] = null;
			restrict4[2] = eTable.Name;
			restrict4[3] = null;
			var dtShema = SqlSchemaFactory.GetSchema(con, "Indexes", restrict4);
			for (var i = 0; i < dtShema.Rows.Count; i++)
			{
				var row = dtShema.Rows[i];
				if (Convert.ToBoolean(row["IS_STATISTICS"], CultureInfo.InvariantCulture) ||
					Convert.ToBoolean(row["IS_AUTOSTATISTICS"], CultureInfo.InvariantCulture) ||
					Convert.ToBoolean(row["IS_HYPOTTETICAL"], CultureInfo.InvariantCulture))
					continue;

				var cName = row["INDEX_NAME"].ToString();
				if (eTable.IsKeyExist(cName, ConstraintType.Unique) ||
					eTable.IsKeyExist(cName, ConstraintType.KeyPrimary) ||
					eTable.IsKeyExist(cName, ConstraintType.KeyForeign))
					continue;

				if (aHash.ContainsKey(cName))
					continue;
				var eIndex = new IndexSchema();
				aHash.Add(cName, true);
				eIndex.Name = cName;
				eIndex.Unique = Convert.ToBoolean(row["IS_UNIQUE"], CultureInfo.InvariantCulture);
				eIndex.Clustered = Convert.ToBoolean(row["IS_CLUSTERED"], CultureInfo.InvariantCulture);
				//eIndex.isActive = !Convert.ToBoolean(cRow["IS_INACTIVE"], CultureInfo.InvariantCulture);

				var dtv = dtShema.DefaultView;
				dtv.RowFilter = string.Format("INDEX_NAME = '{0}'", cName);
				dtv.Sort = "COLUMN_ORDINAL_POSITION ASC";

				var columns = "";
				for (var y = 0; y < dtv.Count; y++)
					columns += (dtv[y]["COLUMN_NAME"] +
								(Convert.ToBoolean(dtv[y]["IS_DESCENDING"], CultureInfo.InvariantCulture) ? " DESC" : "") +
								", ");
				columns = columns.Remove(columns.Length - 2, 2);
				eIndex.Columns = columns;
				indexes.Add(eIndex);
			}
			return indexes;
		}
		#endregion

		#region Преобразование типов MsSql Server <-> Dbsm.Types

		private static ColumnType TypeSqlToDbsm(string typeName)
		{
			switch (typeName.ToUpper(CultureInfo.CurrentCulture))
			{
				case "BIT":
					return ColumnType.Boolean;
				case "BIGINT":
					return ColumnType.BigInt;
				case "BINARY":
					return ColumnType.Binary;
				case "CHAR":
					return ColumnType.Character;
				case "CURSOR":
					return ColumnType.Cursor;
				case "DATETIME":
					return ColumnType.Timestamp;
				case "DECIMAL":
					return ColumnType.Decimal;
				case "INT":
					return ColumnType.Integer;
				case "IMAGE":
					return ColumnType.BlobSubtypeImage;
				case "FLOAT":
					return ColumnType.Float;
				case "MONEY":
					return ColumnType.Money;
				case "NCHAR":
					return ColumnType.Character;
				case "NVARCHAR":
					return ColumnType.NCharacterVaring;
				case "NUMERIC":
					return ColumnType.Numeric;
				case "NTEXT":
					return ColumnType.BlobSubtypeNText;
				case "REAL":
					return ColumnType.Real;
				case "SMALLMONEY":
					return ColumnType.SmallMoney;
				case "SMALLDATETIME":
					return ColumnType.SmallDateTime;
				case "SMALLINT":
					return ColumnType.SmallInt;
				case "SQL_VARIANT":
					return ColumnType.SqlVariant;
				case "TABLE":
					return ColumnType.Table;
				case "TEXT":
					return ColumnType.BlobSubtypeText;
				case "TIMESTAMP":
					return ColumnType.MsTimestamp;
				case "TINYINT":
					return ColumnType.TinyInt;
				case "UNIQUEIDENTIFIER":
					return ColumnType.Guid;
				case "VARBINARY":
					return ColumnType.BinaryVaring;
				case "VARCHAR":
					return ColumnType.CharacterVaring;
				case "XML":
					return ColumnType.Xml;
				default:
					throw new ArgumentException("Unsupported data type for " + MssqlDriver.DriverName);
			}
		}

		public static string TypeDbsmToSql(TableColumnSchema eColumn)
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
				case ColumnType.Cursor:
					return "CURSOR";
				case ColumnType.Timestamp:
					return "DATETIME";
				case ColumnType.Decimal:
					return "DECIMAL(" + eColumn.DecimalPrecision + ", " + eColumn.DecimalScale + ")";
				case ColumnType.Integer:
					return "INT";
				case ColumnType.BlobSubtypeImage:
					return "IMAGE";
				case ColumnType.Float:
					return "FLOAT(" + eColumn.DecimalPrecision + ")";
				case ColumnType.Money:
					return "MONEY";
				case ColumnType.NCharacter:
					return "NCHAR(" + eColumn.Size + ")";
				case ColumnType.NCharacterVaring:
					return "NVARCHAR(" + eColumn.Size + ")";
				case ColumnType.Numeric:
					return "NUMERIC(" + eColumn.DecimalPrecision + ", " + eColumn.DecimalScale + ")";
				case ColumnType.BlobSubtypeNText:
					return "NTEXT";
				case ColumnType.Real:
					return "REAL";
				case ColumnType.SmallMoney:
					return "SMALLMONEY";
				case ColumnType.SmallDateTime:
					return "SMALLDATETIME";
				case ColumnType.SmallInt:
					return "SMALLINT";
				case ColumnType.SqlVariant:
					return "SQL_VARIANT";
				case ColumnType.Table:
					return "TABLE";
				case ColumnType.BlobSubtypeText:
					return "TEXT";
				case ColumnType.MsTimestamp:
					return "TIMESTAMP";
				case ColumnType.TinyInt:
					return "TINYINT";
				case ColumnType.Guid:
					return "UNIQUEIDENTIFIER";
				case ColumnType.BinaryVaring:
					return "VARBINARY";
				default:
					throw new ArgumentException("Unsupported data type for " + MssqlDriver.DriverName);
			}
		}
		#endregion
	}
}