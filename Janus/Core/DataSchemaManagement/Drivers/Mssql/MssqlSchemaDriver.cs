using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;

using System.Data.SqlClient;
using Rsdn.Janus.Framework;

using IServiceProvider=Rsdn.SmartApp.IServiceProvider;

namespace Rsdn.Janus
{
	internal sealed class MssqlSchemaDriver : DBSchemaDriverBase
	{
		// !RULE: Foreign Key may be to PK,UI,UC
		//

		public MssqlSchemaDriver(IServiceProvider serviceProvider, string constr)
			: base(serviceProvider, MssqlSupportModule.DriverName, constr)
		{ }

		public override DbsmSchema Dbsc
		{
			get
			{
				var schema = _dbsc.Copy();
				var keys = new List<DbsmKey>();
				foreach (var table in schema.Tables)
				{
					keys.Clear();
					foreach (var key in table.Keys)
					{
						if (key.KeyType != DbsmConstraintType.Default)
							keys.Add(key);
					}
					table.Keys = keys.ToArray();
				}
				return schema;
			}
			set { _dbsc = value; }
		}

		#region Migration

		public override IDbConnection CreateConnection()
		{
			return new SqlConnection(ConnectionString);
		}

		private static bool TableHasIdentityColumns(DbsmTable table)
		{
			return Array.Exists(table.Columns, column => column.AutoIncrement);
		}

		public override void BeginTableLoad(IDbConnection connection, DbsmTable table)
		{
			if (TableHasIdentityColumns(table))
			{
				using (var cmd = connection.CreateCommand())
				{
					cmd.CommandText = "SET IDENTITY_INSERT " + MakeDDLElementName(table.Name) + " ON";
					cmd.ExecuteNonQuery();
				}
			}

			base.BeginTableLoad(connection, table);
		}

		public override void EndTableLoad(IDbConnection connection, DbsmTable table)
		{
			if (TableHasIdentityColumns(table))
			{
				using (var cmd = connection.CreateCommand())
				{
					cmd.CommandText = "SET IDENTITY_INSERT " + MakeDDLElementName(table.Name) + " OFF";
					cmd.ExecuteNonQuery();
				}
			}

			base.EndTableLoad(connection, table);
		}

		#endregion

		#region Dbsc prepare metods

		protected override DbsmSchema DbscCopyPrepare(DbsmSchema schema)
		{
			var newSchema = schema.Copy();
			switch (schema.DbEngine)
			{
				case DbEngineType.JetDB:
					return ConvertFromJet(newSchema);
				case DbEngineType.FireBirdDB:
					throw new ArgumentException("Not supported DbEngine");
			}
			return newSchema;
		}

		protected override void CompareDbscPost(DbsmSchema mDbsc)
		{
			var eDbsc = _dbsc;
			var i = 1;
			foreach (var mTable in mDbsc.Tables)
			{
				var eTable = eDbsc.GetTable(mTable.Name);
				if (eTable != null)
				{
					var recreate = false;
					var toident = false;
					var notexact = false;

					#region Columns scan
					foreach (var mColumn in mTable.Columns)
					{
						var eColumn = eTable.GetColumn(mColumn.Name);
						if (eColumn == null)
							continue;
						if (mTable.IsExactColumn(eColumn))
							continue;
						notexact = true;
						if (eColumn.Type == DbsmColumnType.BlobSubtypeImage && eColumn.Type == DbsmColumnType.BlobSubtypeText && eColumn.Type == DbsmColumnType.BlobSubtypeNtext)
						{
						}
							//else if (mColumn.computedBy != null && eColumn.computedBy == null)
							//{
							//}
						else if (mColumn.DefaultValue != eColumn.DefaultValue)
						{
						}
						else if (mColumn.AutoIncrement != eColumn.AutoIncrement)
						{
							recreate = true;
							toident = mColumn.AutoIncrement;
						}
						else
						{
							recreate = true;
						}
					}
					if (!recreate && notexact)
					{
						foreach (var mColumn in mTable.Columns)
						{
							var eColumn = eTable.GetColumn(mColumn.Name);
							if (eColumn == null)
								continue;
							if (mTable.IsExactColumn(eColumn))
								continue;
							if (mColumn.DefaultValue == eColumn.DefaultValue)
								continue;
							if (mColumn.DefaultValue == null)
							{
								var eKey = eTable.GetKey(DbsmConstraintType.Default, eColumn.Name);
								if (eKey != null)
								{
									AddDdlCommand(i,
										eTable.Name + "." + eKey.Name,
										eKey.KeyType.ToString(),
										MakeDDLDefaultDrop(eKey, eTable));
								}
							}
							else if (eColumn.DefaultValue == null)
							{
								var mKey = mTable.GetKey(DbsmConstraintType.Default, mColumn.Name);
								if (mKey != null)
								{
									AddDdlCommand(i,
										mTable.Name + "." + mKey.Name,
										mKey.KeyType.ToString(),
										MakeDDLDefaultCreate(mKey, eTable));
								}
							}
							else
							{
								var eKey = eTable.GetKey(DbsmConstraintType.Default, eColumn.Name);
								eKey.Source = mColumn.DefaultValue;

								AddDdlCommand(i,
									eTable.Name + "." + eKey.Name,
									eKey.KeyType.ToString(),
									MakeDDLDefaultDrop(eKey, eTable));

								AddDdlCommand(i,
									eTable.Name + "." + eKey.Name,
									eKey.KeyType.ToString(),
									MakeDDLDefaultCreate(eKey, eTable));
							}
						}
					}
					foreach (var eColumn in eTable.Columns)
					{
						var mColumn = mTable.GetColumn(eColumn.Name);
						if (mColumn != null)
							continue;
						var eKey = eTable.GetKey(DbsmConstraintType.Default, eColumn.Name);
						if (eKey != null)
						{
							AddDdlCommand(i,
								eTable.Name + "." + eKey.Name,
								eKey.KeyType.ToString(),
								MakeDDLDefaultDrop(eKey, eTable));
						}
					}
					#endregion

					#region Keys scan
					//foreach(DbsmKey eKey in eTable.Keys)
					//{
					//  if(eKey.KeyType == DbsmConstraintType.Default)
					//  {
					//    DbsmKey mKey = mTable.GetKey(eKey.Name);
					//    if(mKey == null)
					//    {
					//       AddDdlCommand(i, eTable.Name + "." + eKey.Name, eKey.KeyType.ToString(), MakeDDLDefaultDrop(eKey, eTable.Name));
					//    }
					//    else if(!DbsmKey.CompareKeys(eKey, mKey))
					//    {
					//       AddDdlCommand(i, eTable.Name + "." + eKey.Name, eKey.KeyType.ToString(), MakeDDLDefaultDrop(eKey, eTable.Name));
					//       AddDdlCommand(i, eTable.Name + "." + eKey.Name, eKey.KeyType.ToString(), MakeDDLDefaultCreate(mKey, eTable.Name));
					//    }
					//  }
					//}
					//foreach(DbsmKey mKey in mTable.Keys)
					//{
					//  if(mKey.KeyType == DbsmConstraintType.Default)
					//  {
					//    DbsmKey eKey = eTable.GetKey(mKey.Name);
					//    if(eKey == null)
					//    {
					//      AddDdlCommand(i, eTable.Name + "." + eKey.Name, eKey.KeyType.ToString(), MakeDDLDefaultCreate(mKey, eTable.Name));
					//    }
					//  }
					//}
					#endregion

					#region Recreate block
					if (recreate)
					{
						DeleteDdlCommandsByFilter(string.Format("Name Like '{0}.%'", eTable.Name));

						foreach (var eTable1 in eDbsc.Tables)
							for (var j = 0; j < eTable1.Keys.Length; j++)
								if (eTable1.Keys[j].KeyType == DbsmConstraintType.KeyForeign &&
									eTable1.Keys[j].RelTable == eTable.Name)
								{
									AddDdlCommand(i,
										eTable1.Name + "." + eTable1.Keys[j].Name,
										"Recreate" + eTable1.Keys[j].KeyType,
										MakeDDLKeyDrop(eTable1.Keys[j], eTable1));

									AddDdlCommand(i,
										eTable1.Name + "." + eTable1.Keys[j].Name,
										"Recreate" + eTable1.Keys[j].KeyType,
										MakeDDLKeyCreateByAlter(eTable1.Keys[j], eTable1));
								}

						var tTable = mTable.Clone();
						tTable.Name = "Tmp_" + eTable.Name;

						AddDdlCommand(i,
							tTable.Name,
							"RecreateTable",
							MakeDDLTableCreate(tTable, false));

						foreach (var tIndex in tTable.Indexes)
							AddDdlCommand(i,
								tTable.Name + "." + tIndex.Name,
								"RecreateIndex",
								MakeDDLIndexCreate(tIndex, tTable));

						foreach (var tKey in tTable.Keys)
						{
							if (tKey.KeyType == DbsmConstraintType.Default)
								continue;

							AddDdlCommand(i,
								tTable.Name + "." + tKey.Name,
								"Recreate" + tKey.KeyType,
								MakeDDLKeyCreateByAlter(tKey, tTable));
						}

						if (toident)
							AddDdlCommand(i,
								tTable.Name,
								"RecreateInsert",
								string.Format("SET IDENTITY_INSERT {0} ON", tTable.Name));

						AddDdlCommand(i++,
							tTable.Name,
							"RecreateInsert",
							string.Format(@"
								IF EXISTS(SELECT * FROM {0})
									EXEC('INSERT INTO {1} ({2}) SELECT {3} FROM {4} WITH (HOLDLOCK TABLOCKX)')",
								eTable.Name,
								tTable.Name,
								tTable.ColumnsList(),
								eTable.ColumnsList(),
								eTable.Name));

						if (toident)
							AddDdlCommand(i++,
								tTable.Name,
								"RecreateInsert",
								string.Format("SET IDENTITY_INSERT {0} OFF", tTable.Name));

						AddDdlCommand(i,
							eTable.Name,
							"RecreateTable",
							MakeDDLTableDrop(eTable));

						AddDdlCommand(i,
							tTable.Name,
							"RecreateRename",
							string.Format("EXECUTE sp_rename N'{0}', N'{1}', 'OBJECT'",
								tTable.Name, eTable.Name));
					}
					#endregion
				}
				i++;
			}
		}

		protected override bool CheckKeyTypeForRecreate(DbsmKey eKey)
		{
			return eKey.KeyType == DbsmConstraintType.Unique ||
				eKey.KeyType == DbsmConstraintType.KeyPrimary;
		}

		protected override bool CheckIndexTypeForRecreate(DbsmIndex eIndex)
		{
			return eIndex.Unique;
		}

		protected override void ExecuteDdlCommands(IEnumerable<string> commands)
		{
			using (var con = new SqlConnection(ConnectionString))
			{
				con.Open();

				var cmd = con.CreateCommand();
				cmd.CommandTimeout = 0;

				using (var trans = con.BeginTransaction(IsolationLevel.Serializable))
				{
					cmd.Transaction = trans;

					foreach (var command in commands)
					{
						cmd.CommandText = command;
						cmd.ExecuteNonQuery();
					}

					trans.Commit();
				}
			}
		}

		protected override void WriteDdlCommands(TextWriter wr, IEnumerable<string> commands)
		{
			foreach (var command in commands)
				wr.WriteLine(command);
		}

		#endregion

		#region  DDL Make Metods

		protected override string MakeDDLIndexCreate(DbsmIndex index, DbsmTable table)
		{
			return string.Format("CREATE {0} {1} INDEX {2} ON {3} ({4})",
				index.Unique ? " UNIQUE" : string.Empty,
				index.Clustered ? " CLUSTERED" : string.Empty,
				MakeDDLElementName(index.Name),
				MakeDDLElementName(table.Name),
				Algorithms.ToString(ParseColumnListIndex(index.Columns), ", "));
		}

		protected override string ParseColumn(DbsmColumn column)
		{
			var stat = new StringBuilder();

			stat.Append(MakeDDLElementName(column.Name) + column.TypeDbsmToSql());

			if (column.AutoIncrement)
				stat.AppendFormat(" IDENTITY ({0},{1})", column.Seed, column.Increment);
			else if (!string.IsNullOrEmpty(column.DefaultValue))
				stat.Append(" DEFAULT " + column.DefaultValue);

			if (!column.Nullable)
				stat.Append(" NOT NULL");

			return stat.ToString();
		}

		protected override string ParseColumnAlter(DbsmColumn mColumn, DbsmColumn eColumn)
		{
			return string.Format(" [{0}] {1}{2} ",
				mColumn.Name,
				mColumn.TypeDbsmToSql(),
				mColumn.Nullable ? string.Empty : " NOT NULL");
		}

		#endregion

		#region Преобразование схем

		private static DbsmSchema ConvertFromJet(DbsmSchema dbsc)
		{
			var aStore = new List<DbsmKey>();
			foreach (var eTable in dbsc.Tables)
			{
				aStore.Clear();
				for (var i = 0; i < eTable.Columns.Length; i++)
				{
					var eColumn = eTable.Columns[i];
					switch (eColumn.Type)
					{
						case DbsmColumnType.BlobSubtype1:
							eColumn.Type = DbsmColumnType.BlobSubtypeNtext;
							break;
						case DbsmColumnType.Boolean:
							if (eColumn.DefaultValue != null)
								switch(eColumn.DefaultValue.ToLower(CultureInfo.CurrentCulture))
								{
									case "false":
										eColumn.DefaultValue = "0";
										break;
									case "true":
										eColumn.DefaultValue = "1";
										break;
								}
							break;
					}
				}
				for (var j = 0; j < eTable.Keys.Length; j++)
				{
					switch (eTable.Keys[j].KeyType)
					{
						case DbsmConstraintType.KeyPrimary:
							{
								eTable.Keys[j].Name = "PK_" + eTable.Name;
								foreach(var name in ParseColumnList(eTable.Keys[j].Columns))
								{
									var eColumn = eTable.GetColumn(name);
									if (eColumn != null)
										eColumn.Nullable = false;
								}
							}
							break;
						case DbsmConstraintType.KeyForeign:
							{
								//DbsmTable tTable = dbsc.GetTable(eTable.keys[j].relTable);
								//if (tTable == null ||
								//  !tTable.IsKeyExist(DbsmConstraintType.KeyPrimary, eTable.keys[j].relColumn) ||
								//  !tTable.IsKeyExist(DbsmConstraintType.Unique, eTable.keys[j].relColumn))
								//  continue;
								var keyName = string.Format("FK_{0}_{1}",eTable.Name, eTable.Keys[j].RelTable);
								var post = 0;
								if (eTable.IsKeyExist(keyName))
								{
									while (eTable.IsKeyExist(keyName + post))
										post++;
								}
								eTable.Keys[j].Name = keyName + (post == 0 ? "" : post.ToString());
							}
							break;
					}
					aStore.Add(eTable.Keys[j]);
				}

				eTable.Keys = aStore.ToArray();
				for (var j = 0; j < eTable.Indexes.Length; j++)
				{
					eTable.Indexes[j].Columns = Algorithms.ToString(ParseColumnListIndexClear(eTable.Indexes[j].Columns, IndexClearType.Asc), ", ");
					eTable.Indexes[j].AllowNulls = DbsmAllowNulls.IndexNullsAllow;
					var iName = string.Format("IX_{0}_{1}",
						eTable.Name,
						Algorithms.ToString(ParseColumnList(eTable.Indexes[j].Columns), "_"));
					var post = 0;
					if (eTable.IsIndexExist(iName))
					{
						while (eTable.IsIndexExist(iName + post))
							post++;
					}
					eTable.Indexes[j].Name = iName + (post == 0 ? string.Empty : post.ToString());
				}
			}

			return dbsc;
		}

		#endregion
	}
}
