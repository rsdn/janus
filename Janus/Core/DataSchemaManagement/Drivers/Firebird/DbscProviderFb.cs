using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

using FirebirdSql.Data.FirebirdClient;

namespace Rsdn.Janus
{
	internal class DbscProviderFb: IDbscProvider
	{
		#region IDbscProvider Members

		public DbsmSchema MakeDbsc(string constr)
		{
			FbConnectionStringBuilder csb = new FbConnectionStringBuilder(constr);
			//csb.Pooling = false;
			using (FbConnection con = new FbConnection(csb.ConnectionString))
			{
				con.Open();

				DbsmSchema dbsc = new DbsmSchema();

				dbsc.Name     = csb.Database;
				dbsc.DbEngine = DbEngineType.FireBirdDB;

				dbsc.Tables = GetTables(con);
				foreach (DbsmTable table in dbsc.Tables)
				{
					table.Keys = GetKeys(con, table).ToArray();
					table.Indexes = GetIndexes(con, table).ToArray();
				}
				dbsc.Generators = GetGenerators(con);

				return dbsc;
			}
		}

		#endregion

		#region Private Methods

		private static List<DbsmTable> GetTables(FbConnection con)
		{
			string[] restrict3 = { null, null, null };
			string[] restrict4 = { null, null, null, null };
			List<DbsmTable> aStore = new List<DbsmTable>();

			restrict4[0] = null; restrict4[1] = null; restrict4[2] = null; restrict4[3] = "TABLE";
			DataTable dtTables = con.GetSchema("Tables", restrict4);
			for (int i = 0; i < dtTables.Rows.Count; i++)
			{
				DataRow tRow = dtTables.Rows[i];
				DbsmTable eTable = new DbsmTable();
				eTable.Type = DbsmTableType.Table;
				eTable.Name = tRow["TABLE_NAME"].ToString();
				// Columns
				restrict3[0] = null;
				restrict3[1] = null;
				restrict3[2] = eTable.Name;
				DataTable dtShema = con.GetSchema("Columns", restrict3);
				if (dtShema.Rows.Count > 0)
					eTable.Columns = new DbsmColumn[dtShema.Rows.Count];
				for (int j = 0; j < dtShema.Rows.Count; j++)
				{
					DataRow cRow = dtShema.Rows[j];

					DbsmColumn eColumn = new DbsmColumn();
					eColumn.Name = cRow["COLUMN_NAME"].ToString();
					eColumn.Size = Convert.ToInt32(cRow["COLUMN_SIZE"], CultureInfo.InvariantCulture);
					eColumn.Type = TypeHelper.TypeFbToDbsm(cRow["COLUMN_DATA_TYPE"].ToString());
					eColumn.Nullable = Convert.ToBoolean(cRow["IS_NULLABLE"], CultureInfo.InvariantCulture);
					eColumn.DefaultValue = HelpDbscColumnDefault(con, eColumn.Name, eTable.Name);
					eColumn.DefaultValue = string.IsNullOrEmpty(eColumn.DefaultValue) ? null : eColumn.DefaultValue;
					if (cRow["NUMERIC_PRECISION"] == DBNull.Value)
						eColumn.DecimalPrecision = 0;
					else
						eColumn.DecimalPrecision = Convert.ToInt32(cRow["NUMERIC_PRECISION"], CultureInfo.InvariantCulture);
					eColumn.DecimalScale = Convert.ToInt32(cRow["NUMERIC_SCALE"], CultureInfo.InvariantCulture);

					eTable.Columns[j] = eColumn;
				}
				aStore.Add(eTable);
			}
			return aStore;
		}

		private static List<DbsmKey> GetKeys(FbConnection con, DbsmTable eTable)
		{
			List<DbsmKey> aStore = new List<DbsmKey>();
			List<string> aHash = new List<string>();

			string[] restrict3 = { null, null, null };

			#region Primary
			restrict3[0] = null;
			restrict3[1] = null;
			restrict3[2] = eTable.Name;
			DataTable dtShema = con.GetSchema("PrimaryKeys", restrict3);
			aHash.Clear();
			for (int x = 0; x < dtShema.Rows.Count; x++)
			{
				DataRow cRow = dtShema.Rows[x];
				string cName = cRow["PK_NAME"].ToString();
				if (!aHash.Contains(cName))
				{
					DbsmKey eKey = new DbsmKey();
					string columns = "";
					aHash.Add(cName);
					eKey.KeyType = DbsmConstraintType.KeyPrimary;
					eKey.Name = cName;
					DataView dtv = dtShema.DefaultView;
					dtv.RowFilter = "PK_NAME = '" + cName + "'";
					dtv.Sort = "ORDINAL_POSITION ASC";
					for (int y = 0; y < dtv.Count; y++)
					{
						columns += (dtv[y]["COLUMN_NAME"] + ", ");
					}
					columns = columns.Remove(columns.Length - 2, 2);
					eKey.Columns = columns;
					aStore.Add(eKey);
				}
			}
			#endregion

			#region Foreign
			restrict3[0] = null;
			restrict3[1] = null;
			restrict3[2] = eTable.Name;
			dtShema = con.GetSchema("ForeignKeys", restrict3);
			DataTable dtShemaCols = con.GetSchema("ForeignKeyColumns", restrict3);
			for (int x = 0; x < dtShema.Rows.Count; x++)
			{
				DataRow cRow = dtShema.Rows[x];
				string cName = cRow["INDEX_NAME"].ToString();
				DbsmKey eKey = new DbsmKey();
				string columns = "", rcolumns = "";
				eKey.KeyType = DbsmConstraintType.KeyForeign;
				eKey.Name = cName;
				eKey.RelTable = cRow["REFERENCED_TABLE_NAME"].ToString();
				string uRule = cRow["UPDATE_RULE"].ToString();
				string dRule = cRow["DELETE_RULE"].ToString();
				switch (uRule)
				{
					case "RESTRICT":
						eKey.UpdateRule = DbsmRule.None;
						break;
					case "CASCADE":
						eKey.UpdateRule = DbsmRule.Cascade;
						break;
					case "SET NULL":
						eKey.UpdateRule = DbsmRule.SetNull;
						break;
					case "SET DEFAULT":
						eKey.UpdateRule = DbsmRule.SetDefault;
						break;
				}

				switch (dRule)
				{
					case "RESTRICT":
						eKey.DeleteRule = DbsmRule.None;
						break;
					case "CASCADE":
						eKey.DeleteRule = DbsmRule.Cascade;
						break;
					case "SET NULL":
						eKey.DeleteRule = DbsmRule.SetNull;
						break;
					case "SET DEFAULT":
						eKey.DeleteRule = DbsmRule.SetDefault;
						break;
				}

				DataView dtv = dtShemaCols.DefaultView;
				dtv.RowFilter = "CONSTRAINT_NAME = '" + cName + "'";
				dtv.Sort = "ORDINAL_POSITION ASC";
				for (int y = 0; y < dtv.Count; y++)
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
			for (int x = 0; x < dtShema.Rows.Count; x++)
			{
				DataRow cRow = dtShema.Rows[x];
				string cName = cRow["UK_NAME"].ToString();
				if (!aHash.Contains(cName))
				{
					DbsmKey eKey = new DbsmKey();
					string columns = "";
					aHash.Add(cName);
					eKey.KeyType = DbsmConstraintType.Unique;
					eKey.Name = cName;
					DataView dtv = dtShema.DefaultView;
					dtv.RowFilter = "UK_NAME = '" + cName + "'";
					dtv.Sort = "ORDINAL_POSITION ASC";
					for (int y = 0; y < dtv.Count; y++)
					{
						columns += (dtv[y]["COLUMN_NAME"] + ", ");
					}
					columns = columns.Remove(columns.Length - 2, 2);
					eKey.Columns = columns;
					aStore.Add(eKey);
				}
			}
			#endregion

			return aStore;
		}

		private static List<DbsmIndex> GetIndexes(FbConnection con, DbsmTable eTable)
		{
			List<DbsmIndex> aStore = new List<DbsmIndex>();

			string[] restrict3 = { null, null, eTable.Name };
			string[] restrict4 = { null, null, eTable.Name, null };

			// INDEX_TYPE = 0 - ascending, 1 - descending
			restrict4[0] = null;
			restrict4[1] = null;
			restrict4[2] = eTable.Name;
			restrict4[3] = null;
			DataTable dtShema = con.GetSchema("Indexes", restrict3);
			aStore.Clear();
			for (int x = 0; x < dtShema.Rows.Count; x++)
			{
				DataRow cRow = dtShema.Rows[x];
				string cName = cRow["INDEX_NAME"].ToString();
				if (eTable.IsKeyExist(cName, DbsmConstraintType.Unique) ||
					eTable.IsKeyExist(cName, DbsmConstraintType.KeyPrimary) ||
					eTable.IsKeyExist(cName, DbsmConstraintType.KeyForeign))
					continue;

				DbsmIndex eIndex = new DbsmIndex();
				string columns = "";
				eIndex.Name = cName;
				eIndex.Unique = Convert.ToBoolean(cRow["IS_UNIQUE"], CultureInfo.InvariantCulture);
				if (cRow["INDEX_TYPE"] == DBNull.Value)
					eIndex.Sort = DbsmSortOrder.SortAscending;
				else
					eIndex.Sort = Convert.ToInt32(cRow["INDEX_TYPE"], CultureInfo.InvariantCulture) == 0 ? DbsmSortOrder.SortAscending : DbsmSortOrder.SortDescending;
				eIndex.IsActive = !Convert.ToBoolean(cRow["IS_INACTIVE"], CultureInfo.InvariantCulture);

				restrict4[3] = cName;
				DataTable dtShemaCols = con.GetSchema("IndexColumns", restrict4);
				DataView dtv = dtShemaCols.DefaultView;
				//dtv.RowFilter = "INDEX_NAME = '" + cName + "'";
				dtv.Sort = "ORDINAL_POSITION ASC";
				for (int y = 0; y < dtv.Count; y++)
				{
					columns += (dtv[y]["COLUMN_NAME"] + ", ");
				}
				columns = columns.Remove(columns.Length - 2, 2);
				eIndex.Columns = columns;
				aStore.Add(eIndex);
			}
			return aStore;
		}

		private static List<DbsmGenerator> GetGenerators(FbConnection con)
		{
			using (FbCommand cmd = con.CreateCommand())
			{
				List<DbsmGenerator> generators = new List<DbsmGenerator>();

				string[] restrict4 = { null, null, null, null };

				restrict4[0] = null;
				restrict4[1] = null;
				restrict4[2] = null;
				restrict4[3] = null;

				DataTable dtGenerators = con.GetSchema("Generators", restrict4);
				for (int i = 0; i < dtGenerators.Rows.Count; i++)
				{
					DataRow trRow = dtGenerators.Rows[i];
					if (Convert.ToBoolean(trRow["IS_SYSTEM_GENERATOR"], CultureInfo.InvariantCulture))
						continue;

					DbsmGenerator eGenerator = new DbsmGenerator();
					eGenerator.Name = trRow["GENERATOR_NAME"].ToString();
					cmd.CommandText = string.Format("SELECT gen_id(\"{0}\", 0) FROM rdb$database", eGenerator.Name);
					eGenerator.StartValue = Convert.ToInt32(cmd.ExecuteScalar(), CultureInfo.InvariantCulture);

					generators.Add(eGenerator);
				}

				return generators;
			}
		}

		private static string HelpDbscColumnDefault(FbConnection con, string cname, string tname)
		{
			using (FbCommand cmd = con.CreateCommand())
			using (DataTable result = new DataTable())
			{
				result.Locale = CultureInfo.InvariantCulture;

				cmd.CommandText = string.Format(@"
					SELECT
						rfr.rdb$default_source AS DEFAULT_SOURCE 
					FROM
						rdb$relation_fields rfr  
					WHERE
						rfr.rdb$relation_name = '{0}' AND rfr.rdb$field_name='{1}';",
					tname, cname);

				using (FbDataAdapter adapter = new FbDataAdapter(cmd))
					adapter.Fill(result);

				if (result.Rows.Count > 1)
					throw new DbsmException("Ambiguous column");

				return result.Rows[0]["DEFAULT_SOURCE"].ToString();
			}
		}

		#endregion
	}
}
