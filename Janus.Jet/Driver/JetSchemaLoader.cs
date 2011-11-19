using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using ADODB;

using ADOX;

using DataTypeEnum=ADOX.DataTypeEnum;

namespace Rsdn.Janus.Jet
{
	internal static class JetSchemaLoader
	{
		public static DBSchema LoadSchema(string connStr)
		{
			var dbsc = new DBSchema();

			var catalog = (Catalog)new CatalogClass();
			var conn = new ConnectionClass();
			try
			{
				var csb = new JetConnectionStringBuilder(connStr);

				var keysList = new List<KeySchema>();
				var indexesList = new List<IndexSchema>();

				conn.Open(csb.ConnectionString, string.Empty, string.Empty, 0);
				catalog.ActiveConnection = conn;

				var slashPos = csb.DataSource.LastIndexOf(@"\");

				dbsc.Name = csb.DataSource.Substring(slashPos == -1 ? 0 : slashPos + 1);

				foreach (var xTable in GetTables(catalog))
				{
					keysList.Clear();
					indexesList.Clear();

					var eTable = new TableSchema
					{
						Name = xTable.Name,
						Columns = GetColumns(xTable).ToArray(),
						Keys = GetKeys(xTable).ToArray(),
						Indexes = GetIndexes(xTable).ToArray()
					};

					// Фильтрация ключей
					foreach (var key in eTable.Keys)
					{
						// Долбаный Аксес создает при создании внешних ключей уникальные индексы.
						if (key.KeyType == ConstraintType.KeyForeign && !eTable.IsIndexExist(key.Name))
							continue;
						if (key.KeyType == ConstraintType.Unique &&
							eTable.IsKeyExist(key.Name, ConstraintType.KeyForeign))
							continue;

						keysList.Add(key);
					}
					eTable.Keys = keysList.ToArray();

					// Фильтрация индексов
					foreach (var index in eTable.Indexes)
					{
						if (eTable.IsKeyExist(index.Name, ConstraintType.Unique) ||
							eTable.IsKeyExist(index.Name, ConstraintType.KeyPrimary) ||
								eTable.IsKeyExist(index.Name, ConstraintType.KeyForeign))
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

		#region Private Methods
		private static LinkRule GetDbsmRule(RuleEnum key)
		{
			switch (key)
			{
				case RuleEnum.adRICascade:
					return LinkRule.Cascade;
				case RuleEnum.adRISetNull:
					return LinkRule.SetNull;
				case RuleEnum.adRISetDefault:
					return LinkRule.SetDefault;
				default:
					return LinkRule.None;
			}
		}

		private static List<Table> GetTables(Catalog catalog)
		{
			var res = new List<Table>();
			var tables = catalog.Tables;
			for (var i = 0; i < tables.Count; i++)
			{
				var xTable = catalog.Tables[i];
				if (xTable.Type != "TABLE" || xTable.Name.StartsWith("~TMP"))
					continue;
				res.Add(xTable);
			}
			return res;
		}

		private static List<TableColumnSchema> GetColumns(Table xTable)
		{
			var columns = new List<TableColumnSchema>();
			for (var j = 0; j < xTable.Columns.Count; j++)
			{
				var xColumn = xTable.Columns[j];

				var column = new TableColumnSchema
				{
					Name = xColumn.Name,
					Type = TypeJetToDbsm(xColumn.Type),
					Size = xColumn.DefinedSize,
					DecimalPrecision = xColumn.Precision,
					DecimalScale = xColumn.NumericScale,
					AutoIncrement = ((bool)xColumn.Properties["Autoincrement"].Value)
				};

				//Hashtable hprops = new Hashtable();
				//for (int x = 0; x < column.Properties.Count; x++)
				//    hprops.Add(column.Properties[x].Name, column.Properties[x].Value);

				if (column.AutoIncrement)
				{
					column.Seed = 1; // (int)xTable.Columns[j].Properties["Seed"].Value;
					column.Increment = (int)xColumn.Properties["Increment"].Value;
				}

				//eColumn.FixedLength = (bool)xTable.Columns[j].Properties["Fixed Length"].Value;
				column.Nullable = (bool)xColumn.Properties["Nullable"].Value;
				if (xColumn.Properties["Default"].Value != null)
					column.DefaultValue = xColumn.Properties["Default"].Value.ToString();
				if (column.Type == ColumnType.Boolean && column.DefaultValue == null)
					column.DefaultValue = "0";

				columns.Add(column);
			}

			return columns;
		}

		private static List<KeySchema> GetKeys(Table xTable)
		{
			var keys = new List<KeySchema>();

			for (var j = 0; j < xTable.Keys.Count; j++)
			{
				var xKey = xTable.Keys[j];

				var cols = new StringBuilder();
				var rcols = new StringBuilder();

				for (var k = 0; k < xKey.Columns.Count; k++)
				{
					cols.Append(xKey.Columns[k].Name);
					cols.Append(k == xKey.Columns.Count - 1 ? string.Empty : ", ");
				}

				var key = new KeySchema
				{
					Name = xKey.Name,
					RelTable = xKey.RelatedTable,
					DeleteRule = GetDbsmRule(xKey.DeleteRule),
					UpdateRule = GetDbsmRule(xKey.UpdateRule)
				};

				switch (xKey.Type)
				{
					case KeyTypeEnum.adKeyPrimary:
						key.KeyType = ConstraintType.KeyPrimary;
						break;
					case KeyTypeEnum.adKeyForeign:
						for (var k = 0; k < xKey.Columns.Count; k++)
						{
							rcols.Append(xKey.Columns[k].RelatedColumn);
							rcols.Append(k == xKey.Columns.Count - 1 ? string.Empty : ", ");
						}
						key.KeyType = ConstraintType.KeyForeign;
						break;
					case KeyTypeEnum.adKeyUnique:
						key.KeyType = ConstraintType.Unique;
						break;
				}
				key.Columns = cols.ToString();
				key.RelColumns = rcols.Length != 0 ? rcols.ToString() : null;

				keys.Add(key);
			}

			return keys;
		}

		private static List<IndexSchema> GetIndexes(Table xTable)
		{
			var indexes = new List<IndexSchema>();

			for (var j = 0; j < xTable.Indexes.Count; j++)
			{
				var xIndex = xTable.Indexes[j];

				var cols = new StringBuilder();
				for (var k = 0; k < xIndex.Columns.Count; k++)
				{
					cols.Append(xIndex.Columns[k].Name);
					cols.Append(xIndex.Columns[k].SortOrder == SortOrderEnum.adSortAscending
						? string.Empty
						: " DESC");
					cols.Append(k == xIndex.Columns.Count - 1 ? string.Empty : ", ");
				}

				var index = new IndexSchema
				{
					Name = xIndex.Name,
					Columns = cols.ToString(),
					Clustered = xIndex.Clustered
				};

				switch (xIndex.IndexNulls)
				{
					case AllowNullsEnum.adIndexNullsAllow:
						index.NullAllowances = IndexNullAllowance.Allow;
						break;
					case AllowNullsEnum.adIndexNullsDisallow:
						index.NullAllowances = IndexNullAllowance.Disallow;
						break;
					default:
						index.NullAllowances = IndexNullAllowance.Ignore;
						break;
				}
				index.PrimaryKey = xIndex.PrimaryKey;
				index.Unique = xIndex.Unique;

				indexes.Add(index);
			}

			return indexes;
		}
		#endregion

		#region Преобразование типов Jet ADOX <-> Dbsm.Types
		public static string TypeDbsmToJet(TableColumnSchema dbc)
		{
			switch (dbc.Type)
			{
				case ColumnType.BinaryVaring:
					return "BINARY(" + dbc.Size + ")";
				case ColumnType.Boolean:
					return "BIT";
				case ColumnType.TinyInt:
					return "BYTE";
				case ColumnType.NCharacter:
					return "CHAR(" + dbc.Size + ")";
				case ColumnType.NCharacterVaring:
					return "TEXT(" + dbc.Size + ")";
				case ColumnType.Timestamp:
					return "DATETIME";
				case ColumnType.DoublePrecision:
					return "DOUBLE";
				case ColumnType.Real:
					return "REAL";
				case ColumnType.Guid:
					return "UNIQUEIDENTIFIER";
				case ColumnType.BlobSubtypeImage:
					return "IMAGE";
				case ColumnType.Integer:
					return "INTEGER";
				case ColumnType.BlobSubtype1:
				case ColumnType.BlobSubtypeNText:
					return "MEMO";
				case ColumnType.Money:
					return "MONEY";
				case ColumnType.Decimal:
					return "DECIMAL(" + dbc.DecimalPrecision + ", " + dbc.DecimalScale + ")";
				case ColumnType.SmallInt:
					return "SMALLINT";
				default:
					throw new ArgumentException("Unsupported data type for " + JetDriver.DriverName);
			}
		}

		/// <summary>
		/// Преобразование общего типа к access type, только однозначные отношения!!!
		/// </summary>
		/// <param name="dbcType"></param>
		/// <returns></returns>
		public static ColumnType TypeJetToDbsm(DataTypeEnum dbcType)
		{
			switch (dbcType)
			{
				case DataTypeEnum.adVarBinary: // ACCESS - binary; MSSQL - binary();
					return ColumnType.BinaryVaring;
				case DataTypeEnum.adBoolean: // ACCESS - boolean; MSSQL - bit;  ANSI BOOLEAN
					return ColumnType.Boolean;
				case DataTypeEnum.adUnsignedTinyInt: // ACCESS - byte; MSSQL - tinyint;  ANSI <none>
					return ColumnType.TinyInt;
				case DataTypeEnum.adWChar: // ACCESS - char; MSSQL - char; ANSI - CHARACTER
					return ColumnType.NCharacter;
				case DataTypeEnum.adDate: // ACCESS - datetime; MSSQL - datetime; ANSI - TIMESTAMP
					return ColumnType.Timestamp;
				case DataTypeEnum.adDouble:
					// ACCESS - double; MSSQL - float(53); ANSI - DOUBLEPRECISION, FLOAT(53)
					return ColumnType.DoublePrecision;
				case DataTypeEnum.adSingle: // ACCESS - single; MSSQL - real,float(24); ANSI - REAL, FLOAT(24)
					return ColumnType.Real;
				case DataTypeEnum.adGUID: // ACCESS - guid; MSSQL - uniqueidentifier; ANSI - <none>
					return ColumnType.Guid;
				case DataTypeEnum.adLongVarBinary:
					// ACCESS - image(OLE object); MSSQL - images; ANSI - BLOB SUB_TYPE -1
					return ColumnType.BlobSubtypeImage;
				case DataTypeEnum.adInteger: // ACCESS - integer; MSSQL - int; ANSI - Integer
					return ColumnType.Integer;
				case DataTypeEnum.adLongVarWChar: // ACCESS - memo; MSSQL - ntext; ANSI - BLOB SIB_TYPE 1
					return ColumnType.BlobSubtype1;
				case DataTypeEnum.adCurrency: // ACCESS - money; MSSQL - money; ANSI - 
					return ColumnType.Money;
				case DataTypeEnum.adNumeric:
					// ACCESS - numeric; MSSQL - decimal(p,s),numeric(p,s); ANSI - DECIMAL(p,s)
					return ColumnType.Decimal;
				case DataTypeEnum.adVarWChar: // ACCESS - char(n); MSSQL - char(n); ANSI - CHARACTER(n)
					return ColumnType.NCharacterVaring;
				case DataTypeEnum.adSmallInt:
					return ColumnType.SmallInt;

					#region Unused types
					//				case ADOX.DataTypeEnum.adBinary:
					//					return DbsmType.BinaryLargeObject;
					//				case ADOX.DataTypeEnum.adBigInt:
					//					return DbsmType.BigInt;
					//				case ADOX.DataTypeEnum.adBSTR:
					//					return DbsmType.CharacterVaring;
					//				case ADOX.DataTypeEnum.adChapter:
					//					return DbsmType.BigInt;
					//				case ADOX.DataTypeEnum.adChar:
					//					return DbsmType.Character;
					//				case ADOX.DataTypeEnum.adDBDate:
					//					return DbsmType.Date;
					//				case ADOX.DataTypeEnum.adDBTime:
					//					return DbsmType.Time;
					//				case ADOX.DataTypeEnum.adDBTimeStamp:
					//					return DbsmType.TimeStamp;
					//				case ADOX.DataTypeEnum.adDecimal:
					//					return DbsmType.Decimal;
					//				case ADOX.DataTypeEnum.adError:
					//					return DbsmType.Integer;
					//				case ADOX.DataTypeEnum.adFileTime:
					//					return DbsmType.BigInt;
					//				case ADOX.DataTypeEnum.adIDispatch:
					//					return DbsmType.Integer;
					//				case ADOX.DataTypeEnum.adIUnknown:
					//					return DbsmType.Integer;
					//				case ADOX.DataTypeEnum.adLongVarChar:
					//					return DbsmType.BinaryLargeObject;
					//				case ADOX.DataTypeEnum.adNumeric:
					//					return DbsmType.Numeric;
					//				case ADOX.DataTypeEnum.adPropVariant:
					//					return DbsmType.BigInt;
					//				case ADOX.DataTypeEnum.adTinyInt:
					//					return DbsmType.Character;
					//				case ADOX.DataTypeEnum.adUnsignedBigInt:
					//					return DbsmType.BigInt;
					//				case ADOX.DataTypeEnum.adUnsignedInt:
					//					return DbsmType.Integer;
					//				case ADOX.DataTypeEnum.adUnsignedSmallInt:
					//					return DbsmType.SmallInt;
					//				case ADOX.DataTypeEnum.adUserDefined:
					//					return DbsmType.Integer;
					//				case ADOX.DataTypeEnum.adVarChar:
					//					return DbsmType.CharacterVaring;
					//				case ADOX.DataTypeEnum.adVariant:
					//					return DbsmType.BigInt;
					//				case ADOX.DataTypeEnum.adVarNumeric:
					//					return DbsmType.Numeric;
					//				case ADOX.DataTypeEnum.adEmpty:
					//					return DbsmType.Empty;
					#endregion

				default:
					throw new ArgumentException("Unsupported data type for " + JetDriver.DriverName);
			}
		}
		#endregion
	}
}