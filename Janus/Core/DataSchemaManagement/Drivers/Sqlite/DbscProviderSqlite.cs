using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;

using System.Data.SQLite;

namespace Rsdn.Janus
{
	internal class DbscProviderSqlite: IDbscProvider
	{
		#region IDbscProvider Members

		public DbsmSchema MakeDbsc(string constr)
		{
			SQLiteConnectionStringBuilder csb = new SQLiteConnectionStringBuilder(constr);
			using (SQLiteConnection con = new SQLiteConnection(csb.ConnectionString))
			{
				con.Open();

				DbsmSchema dbsc = new DbsmSchema();

				dbsc.Name     = csb.DataSource;
				dbsc.DbEngine = DbEngineType.SQLiteDB;

				dbsc.Tables = GetTables(con);
				foreach (DbsmTable eTable in dbsc.Tables)
				{
					eTable.Keys    = GetKeys   (con, eTable).ToArray();
					eTable.Indexes = GetIndexes(con, eTable).ToArray();
				}

				return dbsc;
			}
		}

		#endregion

		#region Private Methods

		private static List<DbsmTable> GetTables(SQLiteConnection con)
		{
			string[] restrict3 = { null, null, null };
			string[] restrict4 = { null, null, null, "TABLE" };
			List<DbsmTable> aStore = new List<DbsmTable>();
			List<string>    tablesWithAutoIncrementPKs = new List<string>();

			// Текущая версия SQLite.Net (1.0.46.0) не позволяет узнать,
			// какие колонки являются autoincrement.
			// Вытаскиваем эту информацию ручками.
			//
			restrict3[2] = "sqlite_sequence";
			if (con.GetSchema("Columns", restrict3).Rows.Count > 0)
			{
				using (SQLiteCommand cmd = con.CreateCommand())
				{
					cmd.CommandTimeout = 0;
					cmd.CommandText = "SELECT name FROM sqlite_sequence";

					using (IDataReader r = cmd.ExecuteReader())
					{
						while (r.Read())
						{
							tablesWithAutoIncrementPKs.Add(r.GetString(0));
						}
					}
				}
			}

			DataTable dtTables = con.GetSchema("Tables", restrict4);
			for (int i = 0; i < dtTables.Rows.Count; i++)
			{
				DataRow tRow = dtTables.Rows[i];
				DbsmTable eTable = new DbsmTable();
				eTable.Type = DbsmTableType.Table;
				eTable.Name = tRow["TABLE_NAME"].ToString();

				// Columns
				//
				restrict3[2] = eTable.Name;
				DataTable dtShema = con.GetSchema("Columns", restrict3);
				if (dtShema.Rows.Count > 0)
					eTable.Columns = new DbsmColumn[dtShema.Rows.Count];
				for (int j = 0; j < dtShema.Rows.Count; j++)
				{
					DbsmColumn eColumn = new DbsmColumn();
					DataRow cRow = dtShema.Rows[j];
					eColumn.Name = cRow["COLUMN_NAME"].ToString();
					eColumn.Size = Convert.ToInt32(cRow["CHARACTER_MAXIMUM_LENGTH"], CultureInfo.InvariantCulture);
					eColumn.Type = TypeHelper.TypeSqliteToDbsm(cRow["DATA_TYPE"].ToString());
					eColumn.Nullable = Convert.ToBoolean(cRow["IS_NULLABLE"], CultureInfo.InvariantCulture);

					eColumn.AutoIncrement = Convert.ToBoolean(cRow["PRIMARY_KEY"], CultureInfo.InvariantCulture) &&
						tablesWithAutoIncrementPKs.Contains(eTable.Name);

					if (eColumn.AutoIncrement)
					{
						// В SQLite нет тонкой настройки автоинкремента
						//
						eColumn.Seed      = 1;
						eColumn.Increment = 1;
					}

					bool hasDefault = Convert.ToBoolean(cRow["COLUMN_HASDEFAULT"], CultureInfo.InvariantCulture);
					eColumn.DefaultValue = hasDefault? cRow["COLUMN_DEFAULT"].ToString(): null;

					if (cRow["NUMERIC_PRECISION"] == DBNull.Value)
						eColumn.DecimalPrecision = 0;
					else
						eColumn.DecimalPrecision = Convert.ToInt32(cRow["NUMERIC_PRECISION"], CultureInfo.InvariantCulture);
					if (cRow["NUMERIC_SCALE"] == DBNull.Value)
						eColumn.DecimalScale = 0;
					else
						eColumn.DecimalScale = Convert.ToInt32(cRow["NUMERIC_SCALE"], CultureInfo.InvariantCulture);
					eTable.Columns[j] = eColumn;
				}
				aStore.Add(eTable);
			}
			return aStore;
		}

		private static List<DbsmKey> GetKeys(SQLiteConnection con, DbsmTable eTable)
		{
			Dictionary<string, DbsmKey> dbsmKeys = new Dictionary<string, DbsmKey>();

			string[] restrict3 = { null, null, eTable.Name };

			#region Primary

			DataTable dtShema = con.GetSchema("Columns", restrict3);
			DataView dtv = dtShema.DefaultView;
			dtv.RowFilter = "PRIMARY_KEY = 'True'";
			dtv.Sort = "ORDINAL_POSITION ASC";

			if (dtv.Count > 0)
			{
				string columns = null;
				for (int y = 0; y < dtv.Count; y++)
				{
					if (columns == null)
						columns = dtv[y]["COLUMN_NAME"].ToString();
					else
						columns += (", " + dtv[y]["COLUMN_NAME"]);
				}

				DbsmKey eKey = new DbsmKey();
				eKey.KeyType = DbsmConstraintType.KeyPrimary;
				eKey.Name = "PK_" + eTable.Name;
				eKey.Columns = columns;
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
			//		DbsmKey eKey = new DbsmKey();
			//		eKey.Name = cName;
			//		eKey.KeyType = DbsmConstraintType.Unique;
			//		eKey.ColumnList = cName;
			//		dbsmKeys.Add(cName, eKey);
			//	}
			//}

			#endregion

			#region Foreign

			dtShema = con.GetSchema("ForeignKeys", restrict3);

			for (int x = 0; x < dtShema.Rows.Count; x++)
			{
				DataRow cRow = dtShema.Rows[x];
				string cName = cRow["CONSTRAINT_NAME"].ToString();

				if (!dbsmKeys.ContainsKey(cName))
				{
					DbsmKey eKey = new DbsmKey();
					string columns  = null;
					string rcolumns = null;

					eKey.KeyType = DbsmConstraintType.KeyForeign;
					eKey.Name = cName;
					eKey.RelTable = cRow["TABLE_NAME"].ToString();

					dtv = dtShema.DefaultView;
					dtv.RowFilter = "CONSTRAINT_NAME = '" + cName + "'";
					dtv.Sort = "FKEY_FROM_ORDINAL_POSITION ASC";

					for (int y = 0; y < dtv.Count; y++)
					{
						if (columns == null)
						{
							Debug.Assert(rcolumns == null);
							columns = dtv[y]["FKEY_FROM_COLUMN"].ToString();
							rcolumns = dtv[y]["FKEY_TO_COLUMN"].ToString();
						}
						else
						{
							Debug.Assert(rcolumns != null);
							columns  += (", " + dtv[y]["FKEY_FROM_COLUMN"]);
							rcolumns += (", " + dtv[y]["FKEY_TO_COLUMN"]);
						}
					}

					eKey.Columns = columns;
					eKey.RelColumns = rcolumns;
					dbsmKeys.Add(cName, eKey);
				}
			}
			#endregion

			return new List<DbsmKey>(dbsmKeys.Values);
		}

		private static List<DbsmIndex> GetIndexes(SQLiteConnection con, DbsmTable eTable)
		{
			List<DbsmIndex> aStore = new List<DbsmIndex>();
			List<string> aHash = new List<string>();

			string[] restrict4 = { null, null, null, null };
			string[] restrict5 = { null, null, null, null, null };

			// INDEX_TYPE = 0 - ascending, 1 - descending
			restrict4[2] = eTable.Name;
			restrict5[2] = eTable.Name;
			DataTable dtShema = con.GetSchema("Indexes", restrict4);
			aStore.Clear();
			aHash.Clear();
			for (int x = 0; x < dtShema.Rows.Count; x++)
			{
				DataRow cRow = dtShema.Rows[x];
				string cName = cRow["INDEX_NAME"].ToString();
				if (eTable.IsKeyExist(cName, DbsmConstraintType.Unique) ||
					eTable.IsKeyExist(cName, DbsmConstraintType.KeyPrimary) ||
					eTable.IsKeyExist(cName, DbsmConstraintType.KeyForeign) ||
					Convert.ToBoolean(cRow["PRIMARY_KEY"], CultureInfo.InvariantCulture))
					continue;
				if (!aHash.Contains(cName))
				{
					DbsmIndex eIndex = new DbsmIndex();
					string columns = String.Empty;
					aHash.Add(cName);
					eIndex.Name = cName;
					eIndex.Unique = Convert.ToBoolean(cRow["UNIQUE"], CultureInfo.InvariantCulture);
					if (cRow["TYPE"] == DBNull.Value)
						eIndex.Sort = DbsmSortOrder.SortAscending;
					else
						eIndex.Sort = Convert.ToInt32(cRow["TYPE"], CultureInfo.InvariantCulture) == 0 ? DbsmSortOrder.SortAscending : DbsmSortOrder.SortDescending;
					eIndex.IsActive = true; // !Convert.ToBoolean(cRow["IS_INACTIVE"], CultureInfo.InvariantCulture);

					restrict5[3] = cName;
					DataTable dtIndexColumns = con.GetSchema("IndexColumns", restrict5);

					bool first = true;
					for (int y = 0; y < dtIndexColumns.Rows.Count; y++)
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
			}
			return aStore;
		}

		#endregion
	}
}
