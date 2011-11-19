using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using ADODB;
using ADOX;

namespace Rsdn.Janus
{
	internal class DbscProviderJet: IDbscProvider
	{
		#region IDbscProvider Members

		public DbsmSchema MakeDbsc(string constr)
		{
			DbsmSchema dbsc = new DbsmSchema();

			Catalog catalog = (Catalog)new CatalogClass();
			ConnectionClass conn = new ConnectionClass();
			try
			{
				JetConnectionStringBuilder csb = new JetConnectionStringBuilder(constr);

				List<DbsmKey> keysList = new List<DbsmKey>();
				List<DbsmIndex> indexesList = new List<DbsmIndex>();

				conn.Open(csb.ConnectionString, string.Empty, string.Empty, 0);
				catalog.ActiveConnection = conn;

				int slashPos = csb.DataSource.LastIndexOf(@"\");

				dbsc.Name = csb.DataSource.Substring(slashPos == -1 ? 0 : slashPos + 1);
				dbsc.DbEngine = DbEngineType.JetDB;

				foreach (Table xTable in GetTables(catalog))
				{
					keysList.Clear();
					indexesList.Clear();

					DbsmTable eTable = new DbsmTable();

					eTable.Name = xTable.Name;
					eTable.Columns = GetColumns(xTable).ToArray();
					eTable.Keys = GetKeys(xTable).ToArray();
					eTable.Indexes = GetIndexes(xTable).ToArray();

					// Фильтрация ключей
					foreach (DbsmKey key in eTable.Keys)
					{
						// Долбаный Аксес создает при создании внешних ключей уникальные индексы.
						if (key.KeyType == DbsmConstraintType.KeyForeign && !eTable.IsIndexExist(key.Name))
							continue;
						if (key.KeyType == DbsmConstraintType.Unique && eTable.IsKeyExist(key.Name, DbsmConstraintType.KeyForeign))
							continue;

						keysList.Add(key);
					}
					eTable.Keys = keysList.ToArray();

					// Фильтрация индексов
					foreach (DbsmIndex index in eTable.Indexes)
					{
						if (eTable.IsKeyExist(index.Name, DbsmConstraintType.Unique) ||
							eTable.IsKeyExist(index.Name, DbsmConstraintType.KeyPrimary) ||
							eTable.IsKeyExist(index.Name, DbsmConstraintType.KeyForeign))
							continue;

						indexesList.Add(index);
					}
					eTable.Indexes = indexesList.ToArray();

					dbsc.Tables.Add(eTable);
				}
			}
			finally
			{
				Marshal.ReleaseComObject(conn);
				Marshal.ReleaseComObject(catalog);
			}

			return dbsc;
		}

		#endregion

		#region Private Methods

		private static DbsmRule GetDbsmRule(RuleEnum key)
		{
			switch (key)
			{
				case RuleEnum.adRICascade: return DbsmRule.Cascade;
				case RuleEnum.adRISetNull: return DbsmRule.SetNull;
				case RuleEnum.adRISetDefault: return DbsmRule.SetDefault;
				default: return DbsmRule.None;
			}
		}

		private static IEnumerable<Table> GetTables(Catalog catalog)
		{
			for (int i = 0; i < catalog.Tables.Count; i++)
			{
				Table xTable = catalog.Tables[i];

				if (xTable.Type != "TABLE" || xTable.Name.StartsWith("~TMP"))
					continue;

				yield return xTable;
			}
		}

		private static List<DbsmColumn> GetColumns(Table xTable)
		{
			List<DbsmColumn> columns = new List<DbsmColumn>();
			for (int j = 0; j < xTable.Columns.Count; j++)
			{
				Column xColumn = xTable.Columns[j];

				DbsmColumn column = new DbsmColumn();
				column.Name = xColumn.Name;
				column.Type = TypeHelper.TypeJetToDbsm(xColumn.Type);
				column.Size = xColumn.DefinedSize;
				column.DecimalPrecision = xColumn.Precision;
				column.DecimalScale = xColumn.NumericScale;

				//Hashtable hprops = new Hashtable();
				//for (int x = 0; x < column.Properties.Count; x++)
				//    hprops.Add(column.Properties[x].Name, column.Properties[x].Value);

				column.AutoIncrement = (bool)xColumn.Properties["Autoincrement"].Value;
				if (column.AutoIncrement)
				{
					column.Seed = 1;// (int)xTable.Columns[j].Properties["Seed"].Value;
					column.Increment = (int)xColumn.Properties["Increment"].Value;
				}

				//eColumn.FixedLength = (bool)xTable.Columns[j].Properties["Fixed Length"].Value;
				column.Nullable = (bool)xColumn.Properties["Nullable"].Value;
				if (xColumn.Properties["Default"].Value != null)
					column.DefaultValue = xColumn.Properties["Default"].Value.ToString();
				if (column.Type == DbsmColumnType.Boolean && column.DefaultValue == null)
					column.DefaultValue = "0";

				columns.Add(column);
			}

			return columns;
		}

		private static List<DbsmKey> GetKeys(Table xTable)
		{
			List<DbsmKey> keys = new List<DbsmKey>();

			for (int j = 0; j < xTable.Keys.Count; j++)
			{
				Key xKey = xTable.Keys[j];

				StringBuilder cols = new StringBuilder();
				StringBuilder rcols = new StringBuilder();

				for (int k = 0; k < xKey.Columns.Count; k++)
				{
					cols.Append(xKey.Columns[k].Name);
					cols.Append(k == xKey.Columns.Count - 1 ? string.Empty : ", ");
				}

				DbsmKey key = new DbsmKey();

				key.Name = xKey.Name;
				key.RelTable = xKey.RelatedTable;
				key.DeleteRule = GetDbsmRule(xKey.DeleteRule);
				key.UpdateRule = GetDbsmRule(xKey.UpdateRule);
				switch (xKey.Type)
				{
					case KeyTypeEnum.adKeyPrimary:
						key.KeyType = DbsmConstraintType.KeyPrimary;
						break;
					case KeyTypeEnum.adKeyForeign:
						for (int k = 0; k < xKey.Columns.Count; k++)
						{
							rcols.Append(xKey.Columns[k].RelatedColumn);
							rcols.Append(k == xKey.Columns.Count - 1 ? string.Empty : ", ");
						}
						key.KeyType = DbsmConstraintType.KeyForeign;
						break;
					case KeyTypeEnum.adKeyUnique:
						key.KeyType = DbsmConstraintType.Unique;
						break;
				}
				key.Columns = cols.ToString();
				key.RelColumns = rcols.Length != 0 ? rcols.ToString() : null;

				keys.Add(key);
			}

			return keys;
		}

		private static List<DbsmIndex> GetIndexes(Table xTable)
		{
			List<DbsmIndex> indexes = new List<DbsmIndex>();

			for (int j = 0; j < xTable.Indexes.Count; j++)
			{
				Index xIndex = xTable.Indexes[j];

				StringBuilder cols = new StringBuilder();
				for (int k = 0; k < xIndex.Columns.Count; k++)
				{
					cols.Append(xIndex.Columns[k].Name);
					cols.Append(xIndex.Columns[k].SortOrder == SortOrderEnum.adSortAscending ? string.Empty : " DESC");
					cols.Append(k == xIndex.Columns.Count - 1 ? string.Empty : ", ");
				}

				DbsmIndex index = new DbsmIndex();

				index.Name = xIndex.Name;
				index.Columns = cols.ToString();
				index.Clustered = xIndex.Clustered;
				switch (xIndex.IndexNulls)
				{
					case AllowNullsEnum.adIndexNullsAllow:
						index.AllowNulls = DbsmAllowNulls.IndexNullsAllow;
						break;
					case AllowNullsEnum.adIndexNullsDisallow:
						index.AllowNulls = DbsmAllowNulls.IndexNullsDisallow;
						break;
					case AllowNullsEnum.adIndexNullsIgnore:
					default:
						index.AllowNulls = DbsmAllowNulls.IndexNullsIgnore;
						break;
				}
				index.PrimaryKey = xIndex.PrimaryKey;
				index.Unique = xIndex.Unique;

				indexes.Add(index);
			}

			return indexes;
		}

		#endregion
	}
}
