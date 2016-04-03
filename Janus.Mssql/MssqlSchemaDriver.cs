using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

using CodeJam;

namespace Rsdn.Janus.Mssql
{
	internal sealed class MssqlSchemaDriver : DBSchemaDriverBase
	{
		// !RULE: Foreign Key may be to PK,UI,UC
		//

		internal static SqlConnection ConnectToMaster(string connectionString, out string dbName)
		{
			var csb = new SqlConnectionStringBuilder(connectionString);

			dbName = csb.InitialCatalog;
			csb.InitialCatalog = string.Empty;

			return new SqlConnection(csb.ConnectionString);
		}

		public override void CreateDatabase(string connectionString)
		{
			string dbName;

			using (var con = ConnectToMaster(connectionString, out dbName))
			using (var cmd = con.CreateCommand())
			{
				con.Open();
				cmd.CommandText = $@"CREATE DATABASE {dbName}";
				cmd.ExecuteNonQuery();
			}
		}

		#region Migration
		public override IDbConnection CreateConnection(string connStr)
		{
			return new SqlConnection(connStr);
		}

		private static bool TableHasIdentityColumns(TableSchema table)
		{
			return Array.Exists(table.Columns, column => column.AutoIncrement);
		}

		public override void BeginTableLoad(IDbConnection connection, TableSchema table)
		{
			if (TableHasIdentityColumns(table))
				using (var cmd = connection.CreateCommand())
				{
					cmd.CommandText = @"SET IDENTITY_INSERT " + MakeDdlElementName(table.Name) + @" ON";
					cmd.ExecuteNonQuery();
				}

			base.BeginTableLoad(connection, table);
		}

		public override void EndTableLoad(IDbConnection connection, TableSchema table)
		{
			if (TableHasIdentityColumns(table))
				using (var cmd = connection.CreateCommand())
				{
					cmd.CommandText = @"SET IDENTITY_INSERT " + MakeDdlElementName(table.Name) + @" OFF";
					cmd.ExecuteNonQuery();
				}

			base.EndTableLoad(connection, table);
		}
		#endregion

		#region Schema prepare metods
		protected override DBSchema DbscCopyPrepare(DBSchema schema)
		{
			return schema.Copy();
		}

		protected override void AfterSchemaComparision(DBSchema existingSchema, DBSchema targetSchema)
		{
			var i = 1;
			foreach (var mTable in targetSchema.Tables)
			{
				var eTable = existingSchema.GetTable(mTable.Name);
				if (eTable != null)
				{
					var recreate = false;
					var toident = false;
					var notexact = false;
					var extracolumns = false;

					#region Columns scan
					foreach (var mColumn in mTable.Columns)
					{
						var eColumn = eTable.GetColumn(mColumn.Name);
						if (eColumn == null)
						{
							extracolumns = true;
							continue;
						}
						if (mTable.IsExactColumn(eColumn))
							continue;
						notexact = true;
						if (eColumn.Type == ColumnType.BlobSubtypeImage && eColumn.Type == ColumnType.BlobSubtypeText &&
							eColumn.Type == ColumnType.BlobSubtypeNText)
						{}
							//else if (mColumn.computedBy != null && eColumn.computedBy == null)
							//{
							//}
						else if (mColumn.DefaultValue != eColumn.DefaultValue)
						{}
						else if (mColumn.AutoIncrement != eColumn.AutoIncrement)
						{
							recreate = true;
							toident = mColumn.AutoIncrement;
						}
						else
							recreate = true;
					}
					if (!recreate)
					{
						if (notexact)
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
									var eKey = eTable.GetKey(ConstraintType.Default, eColumn.Name);
									if (eKey != null)
										AddDdlCommand(i,
													  eTable.Name + @"." + eKey.Name,
													  eKey.KeyType.ToString(),
													  MakeDdlDefaultDrop(eKey, eTable));
								}
								else if (eColumn.DefaultValue == null)
								{
									var mKey = mTable.GetKey(ConstraintType.Default, mColumn.Name);
									if (mKey != null)
										AddDdlCommand(i,
													  mTable.Name + @"." + mKey.Name,
													  mKey.KeyType.ToString(),
													  MakeDdlDefaultCreate(mKey, eTable));
								}
								else
								{
									var eKey = eTable.GetKey(ConstraintType.Default, eColumn.Name);
									eKey.Source = mColumn.DefaultValue;

									AddDdlCommand(i,
												  eTable.Name + @"." + eKey.Name,
												  eKey.KeyType.ToString(),
												  MakeDdlDefaultDrop(eKey, eTable));

									AddDdlCommand(i,
												  eTable.Name + @"." + eKey.Name,
												  eKey.KeyType.ToString(),
												  MakeDdlDefaultCreate(eKey, eTable));
								}
							}
						}
						if (extracolumns)
						{
							foreach (var mColumn in mTable.Columns)
							{
								if (eTable.IsColumnExist(mColumn.Name))
									continue;
								if (mColumn.DefaultValue == null)
									continue;

								var mKey = mTable.GetKey(ConstraintType.Default, mColumn.Name);
								if (mKey != null)
									AddDdlCommand(i,
												  mTable.Name + @"." + mKey.Name,
												  mKey.KeyType.ToString(),
												  MakeDdlDefaultCreate(mKey, eTable));
							}
						}
					}
					foreach (var eColumn in eTable.Columns)
					{
						var mColumn = mTable.GetColumn(eColumn.Name);
						if (mColumn != null)
							continue;
						var eKey = eTable.GetKey(ConstraintType.Default, eColumn.Name);
						if (eKey != null)
							AddDdlCommand(i,
										  eTable.Name + @"." + eKey.Name,
										  eKey.KeyType.ToString(),
										  MakeDdlDefaultDrop(eKey, eTable));
					}
					#endregion

					#region Keys scan
					//foreach(DBKey eKey in eTable.Keys)
					//{
					//  if(eKey.KeyType == ConstraintType.Default)
					//  {
					//    DBKey mKey = mTable.GetKey(eKey.Name);
					//    if(mKey == null)
					//    {
					//       AddDdlCommand(i, eTable.Name + "." + eKey.Name, eKey.KeyType.ToString(), MakeDDLDefaultDrop(eKey, eTable.Name));
					//    }
					//    else if(!DBKey.CompareKeys(eKey, mKey))
					//    {
					//       AddDdlCommand(i, eTable.Name + "." + eKey.Name, eKey.KeyType.ToString(), MakeDDLDefaultDrop(eKey, eTable.Name));
					//       AddDdlCommand(i, eTable.Name + "." + eKey.Name, eKey.KeyType.ToString(), MakeDDLDefaultCreate(mKey, eTable.Name));
					//    }
					//  }
					//}
					//foreach(DBKey mKey in mTable.Keys)
					//{
					//  if(mKey.KeyType == ConstraintType.Default)
					//  {
					//    DBKey eKey = eTable.GetKey(mKey.Name);
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
						DeleteDdlCommandsByFilter($@"Name Like '{eTable.Name}.%'");

						foreach (var eTable1 in existingSchema.Tables)
							foreach (var t in eTable1.Keys)
								if (t.KeyType == ConstraintType.KeyForeign && t.RelTable == eTable.Name)
								{
									AddDdlCommand(i,
												  eTable1.Name + @"." + t.Name,
												  @"Recreate" + t.KeyType,
												  MakeDdlKeyDrop(t, eTable1));

									AddDdlCommand(i,
												  eTable1.Name + @"." + t.Name,
												  @"Recreate" + t.KeyType,
												  MakeDdlKeyCreateByAlter(t, eTable1));
								}

						var tTable = mTable.Clone();
						tTable.Name = @"Tmp_" + eTable.Name;

						AddDdlCommand(i,
									  tTable.Name,
									  "RecreateTable",
									  MakeDdlTableCreate(tTable, false));

						foreach (var tIndex in tTable.Indexes)
							AddDdlCommand(i,
										  tTable.Name + @"." + tIndex.Name,
										  "RecreateIndex",
										  MakeDdlIndexCreate(tIndex, tTable));

						foreach (var tKey in tTable.Keys)
						{
							if (tKey.KeyType == ConstraintType.Default)
								continue;

							AddDdlCommand(i,
										  tTable.Name + @"." + tKey.Name,
										  @"Recreate" + tKey.KeyType,
										  MakeDdlKeyCreateByAlter(tKey, tTable));
						}

						if (toident)
							AddDdlCommand(i,
										  tTable.Name,
										  "RecreateInsert",
								$@"SET IDENTITY_INSERT {tTable.Name} ON");

						// Подготовить набор колонок данных.
						var eColumns = tTable
							.Columns
							.Select(col =>
										{
											if (eTable.IsColumnExist(col.Name))
												return MakeDdlElementName(col.Name);
											if (col.DefaultValue == null)
											{
												if (!col.Nullable)
													throw new DBSchemaException(@"Default value is required but not provided.");
												return @"NULL";
											}
											return @"''" + col.DefaultValue + @"''";
										})
							.JoinStrings(@", ");

						AddDdlCommand(i++,
									  tTable.Name,
									  "RecreateInsert",
							$@"
								IF EXISTS(SELECT * FROM {eTable.Name})
									EXEC('INSERT INTO {tTable.Name} ({tTable
								.ColumnsList(MakeDdlElementName)}) SELECT {eColumns} FROM {eTable.Name} WITH (HOLDLOCK TABLOCKX)')");

						if (toident)
							AddDdlCommand(i++,
										  tTable.Name,
										  "RecreateInsert",
								$@"SET IDENTITY_INSERT {tTable.Name} OFF");

						AddDdlCommand(i,
									  eTable.Name,
									  "RecreateTable",
									  MakeDdlTableDrop(eTable));

						AddDdlCommand(i,
									  tTable.Name,
									  "RecreateRename",
							$@"EXECUTE sp_rename N'{tTable.Name}', N'{eTable.Name}', 'OBJECT'");
					}
					#endregion
				}
				i++;
			}
		}

		protected override bool CheckKeyTypeForRecreate(KeySchema eKey)
		{
			return  eKey.KeyType == ConstraintType.Unique ||
					eKey.KeyType == ConstraintType.KeyPrimary;
		}

		protected override bool CheckIndexTypeForRecreate(IndexSchema eIndex)
		{
			return eIndex.Unique;
		}

		protected override void ExecuteDdlCommands(IEnumerable<string> commands, string connStr)
		{
			using (var con = new SqlConnection(connStr))
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
						SchemaHelper.SafeRunCommand(command, () => cmd.ExecuteNonQuery());
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
		protected override string MakeDdlIndexCreate(IndexSchema index, TableSchema table)
		{
			return
				@"CREATE {0} {1} INDEX {2} ON {3} ({4})"
					.FormatWith(
						index.Unique ? @" UNIQUE" : string.Empty,
						index.Clustered ? @" CLUSTERED" : string.Empty,
						MakeDdlElementName(index.Name),
						MakeDdlElementName(table.Name),
						ParseColumnListIndex(index.Columns).JoinStrings(@", "));
		}

		protected override string ParseColumn(TableColumnSchema column)
		{
			var stat = new StringBuilder();

			stat.Append(MakeDdlElementName(column.Name) + MssqlSchemaLoader.TypeDbsmToSql(column));

			if (column.AutoIncrement)
				stat.AppendFormat(@" IDENTITY ({0},{1})", column.Seed, column.Increment);
			else if (!string.IsNullOrEmpty(column.DefaultValue))
				stat.Append(@" DEFAULT " + column.DefaultValue);

			if (!column.Nullable)
				stat.Append(@" NOT NULL");

			return stat.ToString();
		}

		protected override string ParseColumnAlter(TableColumnSchema mColumn, TableColumnSchema eColumn)
		{
			return
				$@" [{mColumn.Name}] {MssqlSchemaLoader.TypeDbsmToSql(mColumn)}{(mColumn.Nullable ? string.Empty : @" NOT NULL")} ";
		}
		#endregion

		/// <summary>
		/// Создать схему метаданных из исходной базы
		/// </summary>
		/// <param name="connStr"></param>
		public override DBSchema LoadExistingSchema(string connStr)
		{
			var schema = MssqlSchemaLoader.LoadSchema(connStr);
			foreach (var table in schema.Tables)
				table.Keys = table.Keys
					.Where(key => key.KeyType != ConstraintType.Default)
					.ToArray();
			return schema;
		}
	}
}