using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace Rsdn.Janus
{
	internal class DbscProviderMssql: IDbscProvider
	{
		#region IDbscProvider Members

		public DbsmSchema MakeDbsc(string constr)
		{
			SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder(constr);
			using (SqlConnection con = new SqlConnection(csb.ConnectionString))
			{
				con.Open();

				DbsmSchema dbsc = new DbsmSchema();

				dbsc.Name = csb.InitialCatalog;
				dbsc.DbEngine = DbEngineType.MsSqlDB;

				dbsc.Tables = GetTables(con);
				foreach (DbsmTable table in dbsc.Tables)
				{
					table.Keys = GetKeys(con, table).ToArray();
					table.Indexes = GetIndexes(con, table).ToArray();
				}

				return dbsc;
			}
		}

		#endregion

		#region Private Methods

		private static DbsmRule GetDbsmRule(string rule)
		{
			switch (rule)
			{
				default:
				case "RESTRICT": return DbsmRule.None;
				case "CASCADE": return DbsmRule.Cascade;
				case "SET NULL": return DbsmRule.SetNull;
				case "SET DEFAULT": return DbsmRule.SetDefault;
			}
		}

		private static List<DbsmTable> GetTables(SqlConnection con)
		{
			List<DbsmTable> tables = new List<DbsmTable>();
			string[] restrict4 = { null, null, null, "TABLE" };
			string[] restrict3 = { null, null, null };

			DataTable dtTables = SqlSchemaFactory.GetSchema(con, "Tables", restrict4);
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

				DataTable dtShema = SqlSchemaFactory.GetSchema(con, "Columns", restrict3);
				if (dtShema.Rows.Count > 0)
				{
					eTable.Columns = new DbsmColumn[dtShema.Rows.Count];

					for (int j = 0; j < dtShema.Rows.Count; j++)
					{
						DataRow cRow = dtShema.Rows[j];

						DbsmColumn eColumn = new DbsmColumn();
						eColumn.Name = cRow["COLUMN_NAME"].ToString();
						eColumn.Size = Convert.ToInt32(cRow["COLUMN_SIZE"], CultureInfo.InvariantCulture);
						eColumn.Type = TypeHelper.TypeSqlToDbsm(cRow["COLUMN_DATA_TYPE"].ToString());
						eColumn.Nullable = Convert.ToBoolean(cRow["IS_NULLABLE"], CultureInfo.InvariantCulture);
						eColumn.DefaultValue = cRow["COLUMN_DEFAULT"].ToString();
						eColumn.DefaultValue = string.IsNullOrEmpty(eColumn.DefaultValue) ? null : UnBracket.ParseUnBracket(eColumn.DefaultValue);
						eColumn.Increment = Convert.ToInt32(cRow["IDENTITY_INCREMENT"], CultureInfo.InvariantCulture);
						eColumn.Seed = Convert.ToInt32(cRow["IDENTITY_SEED"], CultureInfo.InvariantCulture);
						eColumn.AutoIncrement = Convert.ToBoolean(cRow["IS_IDENTITY"], CultureInfo.InvariantCulture);
						eColumn.DecimalPrecision = Convert.ToInt32(cRow["NUMERIC_PRECISION"], CultureInfo.InvariantCulture);
						eColumn.DecimalScale = Convert.ToInt32(cRow["NUMERIC_SCALE"], CultureInfo.InvariantCulture);

						eTable.Columns[j] = eColumn;
					}
				}

				tables.Add(eTable);
			}
			return tables;
		}

		private static List<DbsmKey> GetKeys(SqlConnection con, DbsmTable eTable)
		{
			List<DbsmKey> keys = new List<DbsmKey>();
			Dictionary<string, bool> aHash = new Dictionary<string, bool>();

			string[] restrict3 = { null, null, null };
			string[] restrict4 = { null, null, null, null };

			#region Primary keys
			restrict3[0] = null;
			restrict3[1] = null;
			restrict3[2] = eTable.Name;
			DataTable dtShema = SqlSchemaFactory.GetSchema(con, "PrimaryKeys", restrict3);
			aHash.Clear();
			for (int x = 0; x < dtShema.Rows.Count; x++)
			{
				DataRow cRow = dtShema.Rows[x];
				string cName = cRow["CONSTRAINT_NAME"].ToString();
				if (!aHash.ContainsKey(cName))
				{
					aHash.Add(cName, true);

					DbsmKey eKey = new DbsmKey();
					eKey.KeyType = DbsmConstraintType.KeyPrimary;
					eKey.Name = cName;
					eKey.Clustered = Convert.ToBoolean(cRow["IS_CLUSTERED"], CultureInfo.InvariantCulture);

					StringBuilder columns = new StringBuilder();

					DataView dtv = dtShema.DefaultView;
					dtv.RowFilter = string.Format("CONSTRAINT_NAME = '{0}'", cName);
					dtv.Sort = "ORDINAL_POSITION ASC";
					for (int y = 0; y < dtv.Count; y++)
					{
						columns.Append(dtv[y]["COLUMN_NAME"]);
						columns.Append(y == dtv.Count - 1 ? string.Empty : ", ");
					}

					eKey.Columns = columns.ToString();
					keys.Add(eKey);
				}
			}
			#endregion

			#region Foreign keys
			restrict4[0] = null;
			restrict4[1] = null;
			restrict4[2] = eTable.Name;
			restrict4[3] = null;
			dtShema = SqlSchemaFactory.GetSchema(con, "ForeignKeys", restrict4);
			aHash.Clear();
			for (int x = 0; x < dtShema.Rows.Count; x++)
			{
				DataRow cRow = dtShema.Rows[x];
				if (Convert.ToBoolean(cRow["IS_DISABLED"], CultureInfo.InvariantCulture))
					continue;
				string cName = cRow["CONSTRAINT_NAME"].ToString();
				if (!aHash.ContainsKey(cName))
				{
					aHash.Add(cName, true);

					DbsmKey eKey = new DbsmKey();
					eKey.KeyType = DbsmConstraintType.KeyForeign;
					eKey.Name = cName;
					eKey.RelTable = cRow["PK_TABLE_NAME"].ToString();
					eKey.UpdateRule = GetDbsmRule(cRow["UPDATE_RULE"].ToString());
					eKey.DeleteRule = GetDbsmRule(cRow["DELETE_RULE"].ToString());

					StringBuilder fcolumns = new StringBuilder();
					StringBuilder rcolumns = new StringBuilder();

					DataView dtv = dtShema.DefaultView;
					dtv.RowFilter = string.Format("CONSTRAINT_NAME = '{0}'", cName);
					dtv.Sort = "ORDINAL_POSITION ASC";
					for (int y = 0; y < dtv.Count; y++)
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
			}
			#endregion

			#region Checks
			restrict3[0] = null;
			restrict3[1] = null;
			restrict3[2] = eTable.Name;
			dtShema = SqlSchemaFactory.GetSchema(con, "CheckConstraints", restrict3);
			for (int x = 0; x < dtShema.Rows.Count; x++)
			{
				DataRow row = dtShema.Rows[x];

				DbsmKey eKey = new DbsmKey();
				eKey.KeyType = DbsmConstraintType.Check;
				eKey.Name = row["CONSTRAINT_NAME"].ToString();
				eKey.Source = row["SOURCE"].ToString();

				keys.Add(eKey);
			}
			#endregion

			#region Unique
			restrict3[0] = null;
			restrict3[1] = null;
			restrict3[2] = eTable.Name;
			dtShema = SqlSchemaFactory.GetSchema(con, "UniqueKeys", restrict3);
			aHash.Clear();
			for (int x = 0; x < dtShema.Rows.Count; x++)
			{
				DataRow cRow = dtShema.Rows[x];
				string cName = cRow["CONSTRAINT_NAME"].ToString();
				if (!aHash.ContainsKey(cName))
				{
					DbsmKey eKey = new DbsmKey();
					aHash.Add(cName, true);
					eKey.KeyType = DbsmConstraintType.Unique;
					eKey.Name = cName;

					StringBuilder columns = new StringBuilder();

					DataView dtv = dtShema.DefaultView;
					dtv.RowFilter = "CONSTRAINT_NAME = '" + cName + "'";
					dtv.Sort = "ORDINAL_POSITION ASC";
					for (int y = 0; y < dtv.Count; y++)
					{
						columns.Append(dtv[y]["COLUMN_NAME"]);
						columns.Append(y == dtv.Count - 1 ? string.Empty : ", ");
					}

					eKey.Columns = columns.ToString();
					keys.Add(eKey);
				}
			}
			#endregion

			#region Default constraints
			restrict3[0] = null;
			restrict3[1] = null;
			restrict3[2] = eTable.Name;
			dtShema = SqlSchemaFactory.GetSchema(con, "DefaultConstraints", restrict3);
			for (int x = 0; x < dtShema.Rows.Count; x++)
			{
				DataRow cRow = dtShema.Rows[x];
				DbsmKey eKey = new DbsmKey();
				eKey.KeyType = DbsmConstraintType.Default;
				eKey.Name = cRow["CONSTRAINT_NAME"].ToString();
				eKey.Columns = cRow["COLUMN_NAME"].ToString();
				eKey.Source = UnBracket.ParseUnBracket(cRow["SOURCE"].ToString());
				keys.Add(eKey);
			}
			#endregion

			return keys;
		}

		private static List<DbsmIndex> GetIndexes(SqlConnection con, DbsmTable eTable)
		{
			List<DbsmIndex> indexes = new List<DbsmIndex>();
			Dictionary<string, bool> aHash = new Dictionary<string, bool>();
			string[] restrict4 = { null, null, null, null };

			// Indexes
			restrict4[0] = null;
			restrict4[1] = null;
			restrict4[2] = eTable.Name;
			restrict4[3] = null;
			DataTable dtShema = SqlSchemaFactory.GetSchema(con, "Indexes", restrict4);
			for (int i = 0; i < dtShema.Rows.Count; i++)
			{
				DataRow row = dtShema.Rows[i];
				if (Convert.ToBoolean(row["IS_STATISTICS"], CultureInfo.InvariantCulture) ||
					Convert.ToBoolean(row["IS_AUTOSTATISTICS"], CultureInfo.InvariantCulture) ||
					Convert.ToBoolean(row["IS_HYPOTTETICAL"], CultureInfo.InvariantCulture))
					continue;

				string cName = row["INDEX_NAME"].ToString();
				if (eTable.IsKeyExist(cName, DbsmConstraintType.Unique) ||
					eTable.IsKeyExist(cName, DbsmConstraintType.KeyPrimary) ||
					eTable.IsKeyExist(cName, DbsmConstraintType.KeyForeign))
					continue;

				if (!aHash.ContainsKey(cName))
				{
					DbsmIndex eIndex = new DbsmIndex();
					aHash.Add(cName, true);
					eIndex.Name = cName;
					eIndex.Unique = Convert.ToBoolean(row["IS_UNIQUE"], CultureInfo.InvariantCulture);
					eIndex.Clustered = Convert.ToBoolean(row["IS_CLUSTERED"], CultureInfo.InvariantCulture);
					//eIndex.isActive = !Convert.ToBoolean(cRow["IS_INACTIVE"], CultureInfo.InvariantCulture);

					DataView dtv = dtShema.DefaultView;
					dtv.RowFilter = string.Format("INDEX_NAME = '{0}'", cName);
					dtv.Sort = "COLUMN_ORDINAL_POSITION ASC";

					string columns = "";
					for (int y = 0; y < dtv.Count; y++)
					{
						columns += (dtv[y]["COLUMN_NAME"] + (Convert.ToBoolean(dtv[y]["IS_DESCENDING"], CultureInfo.InvariantCulture) ? " DESC" : "") + ", ");
					}
					columns = columns.Remove(columns.Length - 2, 2);
					eIndex.Columns = columns;
					indexes.Add(eIndex);
				}
			}
			return indexes;
		}

		#endregion
	}
}
