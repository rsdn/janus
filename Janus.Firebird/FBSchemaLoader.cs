using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using FirebirdSql.Data.FirebirdClient;

namespace Rsdn.Janus.Firebird
{
	internal static class FBSchemaLoader
	{
		public static DBSchema LoadSchema(string constr)
		{
			var csb = new FbConnectionStringBuilder(constr);
			//csb.Pooling = false;
			using (var con = new FbConnection(csb.ConnectionString))
			{
				con.Open();

				var dbsc =
					new DBSchema
					{
						Name = csb.Database,
						Tables = GetTables(con)
					};

				foreach (var table in dbsc.Tables)
				{
					table.Keys = GetKeys(con, table).ToArray();
					table.Indexes = GetIndexes(con, table).ToArray();
				}
				dbsc.Generators = GetGenerators(con);

				return dbsc;
			}
		}

		#region Private Methods
		private static List<TableSchema> GetTables(FbConnection con)
		{
			string[] restrict3 = {null, null, null};
			string[] restrict4 = {null, null, null, null};
			var aStore = new List<TableSchema>();

			restrict4[0] = null;
			restrict4[1] = null;
			restrict4[2] = null;
			restrict4[3] = "TABLE";
			var dtTables = con.GetSchema("Tables", restrict4);
			for (var i = 0; i < dtTables.Rows.Count; i++)
			{
				var tRow = dtTables.Rows[i];
				var eTable = new TableSchema {Name = tRow["TABLE_NAME"].ToString()};
				// Columns
				restrict3[0] = null;
				restrict3[1] = null;
				restrict3[2] = eTable.Name;
				var dtShema = con.GetSchema("Columns", restrict3);
				if (dtShema.Rows.Count > 0)
					eTable.Columns = new TableColumnSchema[dtShema.Rows.Count];
				for (var j = 0; j < dtShema.Rows.Count; j++)
				{
					var cRow = dtShema.Rows[j];

					var eColumn = new TableColumnSchema
									{
									Name = cRow["COLUMN_NAME"].ToString(),
									Size = Convert.ToInt32(cRow["COLUMN_SIZE"], CultureInfo.InvariantCulture),
									Type = TypeFbToDbsm(cRow["COLUMN_DATA_TYPE"].ToString()),
									Nullable = Convert.ToBoolean(cRow["IS_NULLABLE"], CultureInfo.InvariantCulture)
									};
					eColumn.DefaultValue = HelpDbscColumnDefault(con, eColumn.Name, eTable.Name);
					eColumn.DefaultValue = string.IsNullOrEmpty(eColumn.DefaultValue) ? null : eColumn.DefaultValue;
					eColumn.DecimalPrecision = cRow["NUMERIC_PRECISION"] == DBNull.Value
												? 0
												: Convert.ToInt32(cRow["NUMERIC_PRECISION"], CultureInfo.InvariantCulture);
					eColumn.DecimalScale = Convert.ToInt32(cRow["NUMERIC_SCALE"], CultureInfo.InvariantCulture);

					eTable.Columns[j] = eColumn;
				}
				aStore.Add(eTable);
			}
			return aStore;
		}

		private static List<KeySchema> GetKeys(DbConnection con, SchemaNamedElement eTable)
		{
			var aStore = new List<KeySchema>();
			var aHash = new List<string>();

			string[] restrict3 = {null, null, null};

			#region Primary
			restrict3[0] = null;
			restrict3[1] = null;
			restrict3[2] = eTable.Name;
			var dtShema = con.GetSchema("PrimaryKeys", restrict3);
			aHash.Clear();
			for (var x = 0; x < dtShema.Rows.Count; x++)
			{
				var cRow = dtShema.Rows[x];
				var cName = cRow["PK_NAME"].ToString();
				if (aHash.Contains(cName))
					continue;
				var eKey = new KeySchema();
				var columns = "";
				aHash.Add(cName);
				eKey.KeyType = ConstraintType.KeyPrimary;
				eKey.Name = cName;
				var dtv = dtShema.DefaultView;
				dtv.RowFilter = "PK_NAME = '" + cName + "'";
				dtv.Sort = "ORDINAL_POSITION ASC";
				for (var y = 0; y < dtv.Count; y++)
					columns += (dtv[y]["COLUMN_NAME"] + ", ");
				columns = columns.Remove(columns.Length - 2, 2);
				eKey.Columns = columns;
				aStore.Add(eKey);
			}
			#endregion

			#region Foreign
			restrict3[0] = null;
			restrict3[1] = null;
			restrict3[2] = eTable.Name;
			dtShema = con.GetSchema("ForeignKeys", restrict3);
			var dtShemaCols = con.GetSchema("ForeignKeyColumns", restrict3);
			for (var x = 0; x < dtShema.Rows.Count; x++)
			{
				var cRow = dtShema.Rows[x];
				var cName = cRow["INDEX_NAME"].ToString();
				var eKey = new KeySchema();
				string columns = "", rcolumns = "";
				eKey.KeyType = ConstraintType.KeyForeign;
				eKey.Name = cName;
				eKey.RelTable = cRow["REFERENCED_TABLE_NAME"].ToString();
				var uRule = cRow["UPDATE_RULE"].ToString();
				var dRule = cRow["DELETE_RULE"].ToString();
				switch (uRule)
				{
					case "RESTRICT":
						eKey.UpdateRule = LinkRule.None;
						break;
					case "CASCADE":
						eKey.UpdateRule = LinkRule.Cascade;
						break;
					case "SET NULL":
						eKey.UpdateRule = LinkRule.SetNull;
						break;
					case "SET DEFAULT":
						eKey.UpdateRule = LinkRule.SetDefault;
						break;
				}

				switch (dRule)
				{
					case "RESTRICT":
						eKey.DeleteRule = LinkRule.None;
						break;
					case "CASCADE":
						eKey.DeleteRule = LinkRule.Cascade;
						break;
					case "SET NULL":
						eKey.DeleteRule = LinkRule.SetNull;
						break;
					case "SET DEFAULT":
						eKey.DeleteRule = LinkRule.SetDefault;
						break;
				}

				var dtv = dtShemaCols.DefaultView;
				dtv.RowFilter = "CONSTRAINT_NAME = '" + cName + "'";
				dtv.Sort = "ORDINAL_POSITION ASC";
				for (var y = 0; y < dtv.Count; y++)
				{
					columns += (dtv[y]["COLUMN_NAME"] + ", ");
					rcolumns += (dtv[y]["REFERENCED_COLUMN_NAME"] + ", ");
				}
				columns = columns.Remove(columns.Length - 2, 2);
				rcolumns = rcolumns.Remove(rcolumns.Length - 2, 2);
				eKey.Columns = columns;
				eKey.RelColumns = rcolumns;
				aStore.Add(eKey);
			}
			#endregion

			#region Unique
			restrict3[0] = null;
			restrict3[1] = null;
			restrict3[2] = eTable.Name;
			dtShema = con.GetSchema("UniqueKeys", restrict3);
			aHash.Clear();
			for (var x = 0; x < dtShema.Rows.Count; x++)
			{
				var cRow = dtShema.Rows[x];
				var cName = cRow["UK_NAME"].ToString();
				if (aHash.Contains(cName))
					continue;
				var eKey = new KeySchema();
				var columns = "";
				aHash.Add(cName);
				eKey.KeyType = ConstraintType.Unique;
				eKey.Name = cName;
				var dtv = dtShema.DefaultView;
				dtv.RowFilter = "UK_NAME = '" + cName + "'";
				dtv.Sort = "ORDINAL_POSITION ASC";
				for (var y = 0; y < dtv.Count; y++)
					columns += (dtv[y]["COLUMN_NAME"] + ", ");
				columns = columns.Remove(columns.Length - 2, 2);
				eKey.Columns = columns;
				aStore.Add(eKey);
			}
			#endregion

			return aStore;
		}

		private static List<IndexSchema> GetIndexes(DbConnection con, TableSchema eTable)
		{
			var aStore = new List<IndexSchema>();

			string[] restrict3 = {null, null, eTable.Name};
			string[] restrict4 = {null, null, eTable.Name, null};

			// INDEX_TYPE = 0 - ascending, 1 - descending
			restrict4[0] = null;
			restrict4[1] = null;
			restrict4[2] = eTable.Name;
			restrict4[3] = null;
			var dtShema = con.GetSchema("Indexes", restrict3);
			aStore.Clear();
			for (var x = 0; x < dtShema.Rows.Count; x++)
			{
				var cRow = dtShema.Rows[x];
				var cName = cRow["INDEX_NAME"].ToString();
				if (eTable.IsKeyExist(cName, ConstraintType.Unique) ||
					eTable.IsKeyExist(cName, ConstraintType.KeyPrimary) ||
					eTable.IsKeyExist(cName, ConstraintType.KeyForeign))
					continue;

				var eIndex = new IndexSchema();
				var columns = "";
				eIndex.Name = cName;
				eIndex.Unique = Convert.ToBoolean(cRow["IS_UNIQUE"], CultureInfo.InvariantCulture);
				if (cRow["INDEX_TYPE"] == DBNull.Value)
					eIndex.Sort = SortOrder.Ascending;
				else
					eIndex.Sort = Convert.ToInt32(cRow["INDEX_TYPE"], CultureInfo.InvariantCulture) == 0
									? SortOrder.Ascending
									: SortOrder.Descending;
				eIndex.IsActive = !Convert.ToBoolean(cRow["IS_INACTIVE"], CultureInfo.InvariantCulture);

				restrict4[3] = cName;
				var dtShemaCols = con.GetSchema("IndexColumns", restrict4);
				var dtv = dtShemaCols.DefaultView;
				//dtv.RowFilter = "INDEX_NAME = '" + cName + "'";
				dtv.Sort = "ORDINAL_POSITION ASC";
				for (var y = 0; y < dtv.Count; y++)
					columns += (dtv[y]["COLUMN_NAME"] + ", ");
				columns = columns.Remove(columns.Length - 2, 2);
				eIndex.Columns = columns;
				aStore.Add(eIndex);
			}
			return aStore;
		}

		private static List<DBGenerator> GetGenerators(FbConnection con)
		{
			using (var cmd = con.CreateCommand())
			{
				var generators = new List<DBGenerator>();

				string[] restrict4 = {null, null, null, null};

				restrict4[0] = null;
				restrict4[1] = null;
				restrict4[2] = null;
				restrict4[3] = null;

				var dtGenerators = con.GetSchema("Generators", restrict4);
				for (var i = 0; i < dtGenerators.Rows.Count; i++)
				{
					var trRow = dtGenerators.Rows[i];
					if (Convert.ToBoolean(trRow["IS_SYSTEM_GENERATOR"], CultureInfo.InvariantCulture))
						continue;

					var eGenerator = new DBGenerator
										{
											Name = trRow["GENERATOR_NAME"].ToString()
										};
					cmd.CommandText = $"SELECT gen_id(\"{eGenerator.Name}\", 0) FROM rdb$database";
					eGenerator.StartValue = Convert.ToInt32(cmd.ExecuteScalar(), CultureInfo.InvariantCulture);

					generators.Add(eGenerator);
				}

				return generators;
			}
		}

		private static string HelpDbscColumnDefault(FbConnection con, string cname, string tname)
		{
			using (var cmd = con.CreateCommand())
			using (var result = new DataTable())
			{
				result.Locale = CultureInfo.InvariantCulture;

				cmd.CommandText =
					$@"
					SELECT
						rfr.rdb$default_source AS DEFAULT_SOURCE 
					FROM
						rdb$relation_fields rfr  
					WHERE
						rfr.rdb$relation_name = '{tname}' AND rfr.rdb$field_name='{cname}';";

				using (var adapter = new FbDataAdapter(cmd))
					adapter.Fill(result);

				if (result.Rows.Count > 1)
					throw new DBSchemaException("Ambiguous column");

				return result.Rows[0]["DEFAULT_SOURCE"].ToString();
			}
		}
		#endregion

		#region Преобразование типов Firebird <-> Dbsm.Types

		private static ColumnType TypeFbToDbsm(string typeName)
		{
			switch (typeName.ToUpper(CultureInfo.CurrentCulture))
			{
				case "ARRAY":
					return ColumnType.Array;
				case "BLOB":
					return ColumnType.BinaryLargeObject;
				case "BLOB SUB_TYPE 1":
					return ColumnType.BlobSubtype1;
				case "CHAR":
					return ColumnType.Character;
				case "VARCHAR":
					return ColumnType.CharacterVaring;
				case "SMALLINT":
					return ColumnType.SmallInt;
				case "INTEGER":
					return ColumnType.Integer;
				case "FLOAT":
					return ColumnType.Float;
				case "DOUBLE PRECISION":
					return ColumnType.DoublePrecision;
				case "BIGINT":
					return ColumnType.BigInt;
				case "NUMERIC":
					return ColumnType.Numeric;
				case "DECIMAL":
					return ColumnType.Decimal;
				case "DATE":
					return ColumnType.Date;
				case "TIME":
					return ColumnType.Time;
				case "TIMESTAMP":
					return ColumnType.Timestamp;
				default:
					throw new ArgumentException("Unsupported data type for " + FBDriver.DriverName);
			}
		}

		/// <summary>
		/// Преобразование общего типа к firebird type, только однозначные отношения!!!
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public static string TypeDbsmToFb(TableColumnSchema column)
		{
			switch (column.Type)
			{
				case ColumnType.Array:
					return "ARRAY";
				case ColumnType.BinaryLargeObject:
					return "BLOB";
				case ColumnType.BlobSubtype1:
					return "BLOB SUB_TYPE 1";
				case ColumnType.Character:
					return "CHAR(" + column.Size + ")";
				case ColumnType.CharacterVaring:
					return "VARCHAR(" + column.Size + ")";
				case ColumnType.NCharacterVaring:
					return $"VARCHAR({column.Size}) CHARACTER SET UNICODE_FSS";
				case ColumnType.SmallInt:
				case ColumnType.TinyInt:
					return "SMALLINT";
				case ColumnType.Integer:
					return "INTEGER";
				case ColumnType.Float:
					return "FLOAT";
				case ColumnType.DoublePrecision:
					return "DOUBLE PRECISION";
				case ColumnType.BigInt:
					return "BIGINT";
				case ColumnType.Numeric:
					return "NUMERIC(" + column.DecimalPrecision + ", " + column.DecimalScale + ")";
				case ColumnType.Decimal:
					return "DECIMAL(" + column.DecimalPrecision + ", " + column.DecimalScale + ")";
				case ColumnType.Date:
					return "DATE";
				case ColumnType.Time:
					return "TIME";
				case ColumnType.Timestamp:
					return "TIMESTAMP";
				case ColumnType.BlobSubtypeText:
				case ColumnType.BlobSubtypeNText:
					return "BLOB SUB_TYPE TEXT";
				case ColumnType.Boolean:
					return "SMALLINT";
				default:
					throw new ArgumentException("Unsupported data type for " + FBDriver.DriverName);
			}
		}
		#endregion
	}
}