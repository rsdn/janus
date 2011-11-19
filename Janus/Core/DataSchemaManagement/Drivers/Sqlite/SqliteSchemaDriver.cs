using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;

using System.Data.SQLite;
using Rsdn.Janus.Framework;

using IServiceProvider=Rsdn.SmartApp.IServiceProvider;

namespace Rsdn.Janus
{
	internal sealed class SqliteSchemaDriver : DBSchemaDriverBase
	{
		// !RULE: Foreign Key may be to PK,UI,UC
		//
		public SqliteSchemaDriver(IServiceProvider serviceProvider, string constr)
			: base(serviceProvider, SqliteSupportModule.DriverName, constr)
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

		public override IDbConnection CreateConnection()
		{
			return new SQLiteConnection(ConnectionString);
		}

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
			// По-хорошему, так делать нельзя.
			// Тут нужно поменять консерваторию, чтобы конкретный движок
			// мог сказать, что эту операцию он не поддерживает, но если тупо
			// пересоздать эту таблицу и перелить в неё данные то всё будет ок.
			// А пока что тупая затычка, чтобы колонки, которые должны быть
			// autoincrement, но не являются таковыми, всё-таки стали 
			// autoincrement'ными.

			DeleteDdlCommandsByFilter("ObjectType='KeyPrimary' OR ObjectType='KeyForeign'");

			var eDbsc = _dbsc;
			var i = 1;
			foreach (var mTable in mDbsc.Tables)
			{
				var eTable = eDbsc.GetTable(mTable.Name);
				if (eTable == null)
					continue;

				var recreate = false;

				foreach (var mColumn in mTable.Columns)
				{
					var eColumn = eTable.GetColumn(mColumn.Name);
					if (eColumn != null)
					{
						if (!mTable.IsExactColumn(eColumn))
						{
							recreate = true;
							break;
						}
					}
				}

				if (!recreate)
					continue;

				var tTable = mTable.Clone();
				tTable.Name = mTable.Name + "_tmp";

				AddDdlCommand(i, tTable.Name, "RecreateTable",
					MakeDDLTableCreate(tTable, true));

				foreach (var tIndex in tTable.Indexes)
				{
					AddDdlCommand(i, tTable.Name + "." + tIndex.Name, "RecreateIndex",
						MakeDDLIndexCreate(tIndex, tTable));
				}

				AddDdlCommand(i, tTable.Name + "." + tTable.Name, "RecreateInsert",
					MakeDDLTableCopy(tTable, mTable));

				AddDdlCommand(i, tTable.Name, "RecreateRename",
					MakeDDLTableRename(tTable, mTable.Name));

				AddDdlCommand(i, mTable.Name, "RecreateTable",
					MakeDDLTableDrop(mTable));

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
			using (var con = new SQLiteConnection(ConnectionString))
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

		protected override string MakeDDLTableCreate(DbsmTable table, bool withConstraint)
		{
			// На параметр 'withConstraint' приходится забить, т.к. AUTOINCREMENT
			// может быть задан только "по месту".
			// Лишние ALTER TABLE потом тупо удаляются в CompareDbscPost.

			var stat = new StringBuilder();
			var pk = table.KeyPrimary();

			foreach (var column in table.Columns)
			{
				if (stat.Length > 0)
					stat.Append(",\n\t");

				stat.Append(ParseColumn(column));

				if (column.AutoIncrement)
				{
					if (column.Seed != 1)
						throw new NotSupportedException(
							"SQLite does not support auto increment columns with the seed other then 1");

					if (column.Increment != 1)
						throw new NotSupportedException(
							"SQLite does not support auto increment columns with the increment other then 1");

					if (pk == null || pk.Columns != column.Name || column.Type != DbsmColumnType.Integer)
						throw new NotSupportedException(
							"SQLite supports only autoincrement for integer primary keys");

					stat.Append(" PRIMARY KEY AUTOINCREMENT");
					pk = null;
				}
			}

			foreach (var key in table.Keys)
			{
				if (key.KeyType == DbsmConstraintType.KeyPrimary && pk == null)
				{
					// Primary key was already processed by an autoincrement column.
					//
					continue;
				}

				if (stat.Length > 0)
					stat.Append(",\n\t");

				stat.Append(ParseKey(key));
			}

			return string.Format("CREATE TABLE {0} ({1})",
				MakeDDLElementName(table.Name), stat);
		}

		protected override string MakeDDLIndexCreate(DbsmIndex index, DbsmTable table)
		{
			return string.Format("CREATE {0} {1} INDEX {2} ON {3} ({4})",
				index.Unique ? " UNIQUE" : string.Empty,
				string.Empty,//index.Clustered ? " CLUSTERED" : string.Empty,
				MakeDDLElementName(index.Name),
				MakeDDLElementName(table.Name),
				Algorithms.ToString(ParseColumnListIndex(index.Columns), ", "));
		}

		protected override string MakeDDLIndexDrop(DbsmIndex eIndex, DbsmTable etable)
		{
			return string.Format("DROP INDEX {0}",
				MakeDDLElementName(eIndex.Name));
		}

		protected override string ParseColumn(DbsmColumn eColumn)
		{
			var stat = new StringBuilder();
			stat.AppendFormat(" [" + eColumn.Name + "] " + eColumn.TypeDbsmToSqlite());
			
			if (!String.IsNullOrEmpty(eColumn.DefaultValue))
				stat.Append(" DEFAULT " + eColumn.DefaultValue);

			if (!eColumn.Nullable)
				stat.Append(" NOT NULL");
			return stat.ToString();
		}

		protected override string ParseColumnAlter(DbsmColumn mColumn, DbsmColumn eColumn)
		{
			return string.Format(" [{0}] {1}{2} ",
				mColumn.Name,
				mColumn.TypeDbsmToSqlite(),
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
								var keyName = string.Format("FK_{0}_{1}",
									eTable.Name, eTable.Keys[j].RelTable);
								var post = 0;
								if (eTable.IsKeyExist(keyName))
								{
									while (eTable.IsKeyExist(keyName + post))
										post++;
								}
								eTable.Keys[j].Name = keyName + (post == 0 ? string.Empty : post.ToString());
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
