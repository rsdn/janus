using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Rsdn.Janus.Framework;
using Rsdn.SmartApp;

using IServiceProvider=Rsdn.SmartApp.IServiceProvider;

namespace Rsdn.Janus
{
	/// <summary>
	/// Класс-интерфейс
	/// общая сущность менеджеров схем разных engine
	/// </summary>
	internal abstract class DBSchemaDriverBase : IDBSchemaDriver //IDisposable
	{
		#region fileds

		private readonly IDBDriver _dbDriver;
		private readonly DbConnectionStringBuilder _csb;

		protected DbsmSchema _dbsc;
		private List<string> _ddlCommands = new List<string>();
		private readonly DataTable _ddlCmdTable; // DataTable операторы DDL к исполнению

		#endregion

		protected DBSchemaDriverBase(IServiceProvider serviceProvider, string driverName)
			: this(serviceProvider, driverName, string.Empty)
		{ }

		protected DBSchemaDriverBase(IServiceProvider serviceProvider, string driverName, string constr)
		{
			_ddlCmdTable = new DataTable("DDL_CS");
			_ddlCmdTable.Columns.Add("Priority", typeof(int));
			_ddlCmdTable.Columns.Add("Name", typeof(string));
			_ddlCmdTable.Columns.Add("ObjectType", typeof(string));
			_ddlCmdTable.Columns.Add("Statement", typeof(string));

			_dbDriver = serviceProvider
				.GetRequiredService<IDBDriverManager>()
				.GetDriver(driverName);

			_csb = _dbDriver.CreateConnectionString(constr);
		}

		public virtual DbsmSchema Dbsc
		{
			get { return _dbsc; }
			set { _dbsc = value; }
		}

		public string ConnectionString
		{
			get { return _csb.ToString(); }
			set { _csb.ConnectionString = value; }
		}

		#region Public Metods

		//public static void CompareTable(string name)

		/// <summary>
		/// Создать схему на основании загруженной ранее
		/// connection string в ConnectionStringBuilder
		/// </summary>
		public virtual void MakeDbsc()
		{
			_dbsc = _dbDriver.CreateSchemaProvider().MakeDbsc(ConnectionString);
		}

		/// <summary>
		/// Сравнение схемы с эталонной и выдача во внутреннюю хеш таблицу DDL комманд,
		/// выполнение общих для всех движков действий.
		/// </summary>
		/// <param name="mbDbsc"></param>
		public void CompareDbsc(DbsmSchema mbDbsc)
		{
			var eDbsc = _dbsc;
			var mDbsc = DbscCopyPrepare(mbDbsc);

			eDbsc.Normalize();
			mDbsc.Normalize();
			_ddlCmdTable.Rows.Clear();

			#region Tables
			var i = 1;
			foreach (var eTable in eDbsc.Tables)
			{
				if (!mDbsc.IsTableExist(eTable.Name))
				{
					HelperTableDrop(eTable, eDbsc.Tables, i);
					i++;
				}
			}

			i = 1;
			foreach (var mTable in mDbsc.Tables)
			{
				var eTable = eDbsc.GetTable(mTable.Name);
				if (eTable == null)
				{
					HelperTableCreate(mTable, i);
				}
				else if (mTable.GetDiffColumnsCount(eTable) == eTable.Columns.Length)
				{
					HelperTableDrop(eTable, eDbsc.Tables, i);
					HelperTableCreate(mTable, i);
				}
				else
				{
					#region Column scan
					foreach (var eColumn in eTable.Columns)
					{
						if (!mTable.IsColumnExist(eColumn.Name))
						{
							AddDdlCommand(i,
								eTable.Name + "." + eColumn.Name,
								"Column",
								MakeDDLColumnDrop(eColumn, eTable));
						}
					}
					foreach (var mColumn in mTable.Columns)
					{
						var eColumn = eTable.GetColumn(mColumn.Name);
						if (eColumn == null)
						{
							AddDdlCommand(i,
								mTable.Name + "." + mColumn.Name,
								"Column",
								MakeDDLColumnCreate(mColumn, mTable));
						}
						// to post processing in all workers AlterColumn
					}
					#endregion

					#region Indexes scan
					foreach (var eIndex in eTable.Indexes)
					{
						if (!mTable.IsIndexExist(eIndex.Name))
						{
							AddDdlCommand(i,
								eTable.Name + "." + eIndex.Name,
								"Index",
								MakeDDLIndexDrop(eIndex, eTable));

							if (CheckIndexTypeForRecreate(eIndex))
							{
								foreach (var eTable1 in eDbsc.Tables)
								{
									var eKey1 = eTable1.GetKey(DbsmConstraintType.KeyForeign, eTable.Name, ParseColumnListIndexClear(eIndex.Columns));
									if (eKey1 != null)
									{
										AddDdlCommand(i,
											eTable.Name + "." + eKey1.Name,
											eKey1.KeyType.ToString(),
											MakeDDLKeyDrop(eKey1, eTable1));

										AddDdlCommand(i,
											eTable.Name + "." + eKey1.Name,
											eKey1.KeyType.ToString(),
											MakeDDLKeyCreateByAlter(eKey1, eTable1));
									}
								}
							}
						}
					}
					foreach (var mIndex in mTable.Indexes)
					{
						if (eTable.IsIndexExist(mIndex.Name))
						{
							var eIndex = eTable.GetIndex(mIndex.Name);
							if (!eIndex.Equals(mIndex))
							{
								AddDdlCommand(i,
									eTable.Name + "." + eIndex.Name,
									"Index",
									MakeDDLIndexDrop(eIndex, eTable));

								AddDdlCommand(i,
									eTable.Name + "." + mIndex,
									"Index",
									MakeDDLIndexCreate(mIndex, mTable));

								if (CheckIndexTypeForRecreate(eIndex))
								{
									foreach (var eTable1 in eDbsc.Tables)
									{
										var eKey1 = eTable1.GetKey(DbsmConstraintType.KeyForeign, eTable.Name, ParseColumnListIndexClear(eIndex.Columns));
										if (eKey1 != null)
										{
											AddDdlCommand(i,
												eTable1.Name + "." + eKey1.Name,
												eKey1.KeyType.ToString(),
												MakeDDLKeyDrop(eKey1, eTable1));

											AddDdlCommand(i,
												eTable1.Name + "." + eKey1.Name,
												eKey1.KeyType.ToString(),
												MakeDDLKeyCreateByAlter(eKey1, eTable1));
										}
									}
								}
							}
						}
						else
						{
							AddDdlCommand(i,
								mTable.Name + "." + mIndex.Name,
								"Index",
								MakeDDLIndexCreate(mIndex, mTable));
						}
					}
					#endregion

					#region Keys scan
					foreach (var eKey in eTable.Keys)
					{
						if (eKey.KeyType == DbsmConstraintType.Default)
							continue;
						if (!mTable.IsKeyExist(eKey.Name))
						{
							AddDdlCommand(i,
								eTable.Name + "." + eKey.Name,
								eKey.KeyType.ToString(),
								MakeDDLKeyDrop(eKey, eTable));

							if (CheckKeyTypeForRecreate(eKey))
							{
								foreach (var eTable1 in eDbsc.Tables)
								{
									var eKey1 = eTable1.GetKey(DbsmConstraintType.KeyForeign, eTable.Name, eKey.Columns);
									if (eKey1 != null)
									{
										AddDdlCommand(i,
											eTable1.Name + "." + eKey1.Name,
											eKey1.KeyType.ToString(),
											MakeDDLKeyDrop(eKey1, eTable1));

										AddDdlCommand(i,
											eTable1.Name + "." + eKey1.Name,
											eKey1.KeyType.ToString(),
											MakeDDLKeyCreateByAlter(eKey1, eTable1));
									}
								}
							}
						}
					}
					foreach (var mKey in mTable.Keys)
					{
						if (mKey.KeyType == DbsmConstraintType.Default)
							continue;

						if (eTable.IsKeyExist(mKey.Name))
						{
							var eKey = eTable.GetKey(mKey.Name);
							if (!eKey.Equals(mKey))
							{
								AddDdlCommand(i,
									eTable.Name + "." + mKey.Name,
									mKey.KeyType.ToString(),
									MakeDDLKeyDrop(eKey, eTable));

								AddDdlCommand(i,
									eTable.Name + "." + mKey.Name,
									mKey.KeyType.ToString(),
									MakeDDLKeyCreateByAlter(mKey, eTable));

								if (CheckKeyTypeForRecreate(eKey))
								{
									foreach (var eTable1 in eDbsc.Tables)
									{
										var eKey1 = eTable1.GetKey(DbsmConstraintType.KeyForeign, eTable.Name, eKey.Columns);
										if (eKey1 != null)
										{
											AddDdlCommand(i,
												eTable1.Name + "." + eKey1.Name,
												eKey1.KeyType.ToString(),
												MakeDDLKeyDrop(eKey1, eTable1));

											AddDdlCommand(i,
												eTable1.Name + "." + eKey1.Name,
												eKey1.KeyType.ToString(),
												MakeDDLKeyCreateByAlter(eKey1, eTable1));
										}
									}
								}
							}
						}
						else
						{
							AddDdlCommand(i,
								eTable.Name + "." + mKey.Name,
								mKey.KeyType.ToString(),
								MakeDDLKeyCreateByAlter(mKey, eTable));
						}
					}
					#endregion
				}
				i++;
			}
			#endregion

			CompareDbscPost(mDbsc);

			_ddlCommands = Reparse();

			_ddlCmdTable.Rows.Clear();
		}

		public void Prepare()
		{
			try
			{
				ExecuteDdlCommands(_ddlCommands);
			}
			catch (Exception ex)
			{
				throw new DbsmException("Error while preparing SqlDiff", ex);
			}
		}

		public void PrepareToSqlFile(string path)
		{
			using (var wr = new StreamWriter(path, false, Encoding.Unicode))
				WriteDdlCommands(wr, _ddlCommands);
		}

		#endregion

		#region Private Metods

		/// <summary>
		/// Добавить DDL команду.
		/// </summary>
		protected void AddDdlCommand(int priority, string name, string type, string statement)
		{
			_ddlCmdTable.Rows.Add(priority, name, type, statement);
		}

		/// <summary>
		/// Удалить DDL команды по заданному фильтру.
		/// </summary>
		/// <param name="filter">фильтр</param>
		protected void DeleteDdlCommandsByFilter(string filter)
		{
			var dwr = _ddlCmdTable.DefaultView;
			dwr.RowFilter = filter;
			while (dwr.Count > 0)
				dwr[0].Delete();
			_ddlCmdTable.AcceptChanges();
		}

		/// <summary>
		/// Получить набор DDL операторов по заданному фильтру.
		/// </summary>
		protected IEnumerable<string> GetFilteredDdlStatements(string filter, string sort)
		{
			var dw = _ddlCmdTable.DefaultView;
			dw.RowFilter = filter;
			dw.Sort = sort;

			for (var i = 0; i < dw.Count; i++)
				yield return dw[i]["Statement"].ToString();
		}

		/// <summary>
		/// Так как схема - референс тип, создание его глубокой копии.
		/// Которая при приведение к искомому движку бедет правиться.
		/// И приведение её к исходному движку
		/// </summary>
		/// <param name="schema">Эталоная схема</param>
		/// <returns></returns>
		protected abstract DbsmSchema DbscCopyPrepare(DbsmSchema schema);

		/// <summary>
		/// Препроцесинг схем, детали для разных движков
		/// </summary>
		/// <param name="mDbsc">Эталоная схема</param>
		//protected abstract void CompareDbscPre(DbsmSchema mDbsc);

		/// <summary>
		/// Постпроцесинг схем, детали для разных движков
		/// </summary>
		/// <param name="mDbsc">Эталоная схема</param>
		protected abstract void CompareDbscPost(DbsmSchema mDbsc);

		/// <summary>
		/// Проверка необходимости пересоздания связанных Foreign Key
		/// при удаление констрента
		/// </summary>
		/// <param name="eKey"></param>
		/// <returns></returns>
		protected abstract bool CheckKeyTypeForRecreate(DbsmKey eKey);

		/// <summary>
		/// Проверка необходимости пересоздания связанных Foreign Key
		/// при удалении индекса
		/// </summary>
		/// <param name="eIndex"></param>
		/// <returns></returns>
		protected abstract bool CheckIndexTypeForRecreate(DbsmIndex eIndex);

		private void HelperTableCreate(DbsmTable mTable, int i)
		{
			AddDdlCommand(i,
				mTable.Name,
				"Table",
				MakeDDLTableCreate(mTable, false));

			foreach (var mIndex in mTable.Indexes)
				AddDdlCommand(i,
					mTable.Name + "." + mIndex.Name,
					"Index",
					MakeDDLIndexCreate(mIndex, mTable));
			foreach (var mKey in mTable.Keys)
				AddDdlCommand(i,
					mTable.Name + "." + mKey.Name,
					mKey.KeyType.ToString(),
					MakeDDLKeyCreateByAlter(mKey, mTable));
		}

		private void HelperTableDrop(DbsmTable eTable, IEnumerable<DbsmTable> eTables, int i)
		{
			foreach (var table in eTables)
			{
				for (var j = 0; j < table.Keys.Length; j++)
				{
					if (table.Keys[j].KeyType == DbsmConstraintType.KeyForeign &&
						table.Keys[j].RelTable == eTable.Name)
					{
						AddDdlCommand(i,
							table.Name + "." + table.Keys[j].Name,
							table.Keys[j].KeyType.ToString(),
							MakeDDLKeyDrop(table.Keys[j], table));
					}
				}
			}
			AddDdlCommand(i,
				eTable.Name,
				"Table",
				MakeDDLTableDrop(eTable));
		}

		/// <summary>
		/// Выполнить набор DDL команд.
		/// </summary>
		/// <param name="commands">набор DDL команд</param>
		protected abstract void ExecuteDdlCommands(IEnumerable<string> commands);

		/// <summary>
		/// Записать набор DDL команд с помощью писателя.
		/// </summary>
		/// <param name="wr">писатель</param>
		/// <param name="commands">набор DDL команд</param>
		protected abstract void WriteDdlCommands(TextWriter wr, IEnumerable<string> commands);

		/// <summary>
		/// Метод ресортирует операторы DDL из кучи в соответствии с логикой 
		/// и очередностью их выполнения. Очень важный момент!
		/// </summary>
		/// <returns>Набор DDL команд в порядке их выполнения.</returns>
		private List<string> Reparse()
		{
			var ddlCommands = new List<string>();

			#region Recreate block
			ddlCommands.AddRange(GetFilteredDdlStatements(
				"ObjectType IN ('RecreateKeyForeign','RecreateUnique','RecreateKeyPrimary') AND Statement LIKE '* DROP CONSTRAINT *'",
				"Priority ASC"));
			ddlCommands.AddRange(GetFilteredDdlStatements(
				"ObjectType = 'RecreateTable' AND Statement NOT LIKE 'DROP *'",
				"Priority ASC"));
			// Insert
			ddlCommands.AddRange(GetFilteredDdlStatements(
				"ObjectType = 'RecreateInsert'",
				"Priority ASC"));
			// Drop recreated table
			ddlCommands.AddRange(GetFilteredDdlStatements(
				"ObjectType = 'RecreateTable' AND Statement LIKE 'DROP *'",
				"Priority ASC"));
			// Indexes
			ddlCommands.AddRange(GetFilteredDdlStatements(
				"ObjectType = 'RecreateIndex' AND Statement NOT LIKE 'DROP *'",
				"Priority ASC"));
			// Keys Primary and Unique
			ddlCommands.AddRange(GetFilteredDdlStatements(
				"ObjectType IN ('RecreateUnique','RecreateKeyPrimary','RecreateCheck') AND Statement NOT LIKE '* DROP CONSTRAINT *'",
				"Priority ASC"));
			// Rename
			ddlCommands.AddRange(GetFilteredDdlStatements(
				"ObjectType = 'RecreateRename'",
				"Priority ASC"));
			#endregion

			#region Drop Block
			// Keys drop
			ddlCommands.AddRange(GetFilteredDdlStatements(
				"ObjectType IN ('KeyForeign') AND Statement LIKE '* DROP CONSTRAINT *'",
				"Priority ASC"));
			ddlCommands.AddRange(GetFilteredDdlStatements(
				"ObjectType IN ('Unique','KeyPrimary','Check','Default') AND Statement LIKE '* DROP CONSTRAINT *'",
				"Priority ASC"));
			// Index drop
			ddlCommands.AddRange(GetFilteredDdlStatements(
				"ObjectType = 'Index' AND Statement LIKE 'DROP *'",
				"Priority ASC"));
			// Column drop
			ddlCommands.AddRange(GetFilteredDdlStatements(
				"ObjectType = 'Column' AND Statement LIKE 'DROP *'",
				"Priority ASC"));
			// Table drop
			ddlCommands.AddRange(GetFilteredDdlStatements(
				"ObjectType = 'Table' AND Statement LIKE 'DROP *'",
				"Priority ASC"));
			// Trigges drop
			ddlCommands.AddRange(GetFilteredDdlStatements(
				"ObjectType = 'Trigger' AND Statement LIKE 'DROP *'",
				"Priority ASC"));
			// Generators drop
			ddlCommands.AddRange(GetFilteredDdlStatements(
				"ObjectType = 'Generator' AND Statement LIKE 'DROP *'",
				"Priority ASC"));
			// Procedures drop
			ddlCommands.AddRange(GetFilteredDdlStatements(
				"ObjectType = 'Procedure' AND Statement LIKE 'DROP *'",
				"Priority ASC"));
			#endregion

			#region Create block
			// Tables
			ddlCommands.AddRange(GetFilteredDdlStatements(
				"ObjectType = 'Table' AND Statement NOT LIKE 'DROP *'",
				"Priority ASC"));
			// Column create alter
			ddlCommands.AddRange(GetFilteredDdlStatements(
				"ObjectType = 'Column' AND Statement NOT LIKE 'DROP *'",
				"Priority ASC"));
			// Indexes
			ddlCommands.AddRange(GetFilteredDdlStatements(
				"ObjectType = 'Index' AND Statement NOT LIKE 'DROP *'",
				"Priority ASC"));
			// Keys Primary and Unique
			ddlCommands.AddRange(GetFilteredDdlStatements(
				"ObjectType IN ('Unique','KeyPrimary','Check','Default') AND Statement NOT LIKE '* DROP CONSTRAINT *'",
				"Priority ASC"));
			// Keys Foreign
			ddlCommands.AddRange(GetFilteredDdlStatements(
				"ObjectType IN ('KeyForeign') AND Statement NOT LIKE '* DROP CONSTRAINT *'",
				"Priority ASC"));
			// Keys Foreign
			ddlCommands.AddRange(GetFilteredDdlStatements(
				"ObjectType IN ('RecreateKeyForeign') AND Statement NOT LIKE '* DROP CONSTRAINT *'",
				"Priority ASC"));
			// Trigges create
			ddlCommands.AddRange(GetFilteredDdlStatements(
				"ObjectType = 'Trigger' AND Statement NOT LIKE 'DROP *'",
				"Priority ASC"));
			// Generators create
			ddlCommands.AddRange(GetFilteredDdlStatements(
				"ObjectType = 'Generator' AND Statement NOT LIKE 'DROP *'",
				"Priority ASC"));
			// Procedures create
			ddlCommands.AddRange(GetFilteredDdlStatements(
				"ObjectType = 'Procedure' AND Statement NOT LIKE 'DROP *'",
				"Priority ASC"));
			#endregion

			if (ddlCommands.Count != _ddlCmdTable.Rows.Count)
				throw new OperationCanceledException("Not all commands reparsed");

			return ddlCommands;
		}

		#endregion

		#region Migration

		public virtual string MakeParameterName(DbsmColumn column)
		{
			return "@" + column.Name;
		}

		public virtual IDbDataParameter ConvertToDbParameter(DbsmColumn column, IDbDataParameter parameter)
		{
			parameter.ParameterName = MakeParameterName(column);

			switch (column.Type)
			{
				case DbsmColumnType.Integer:
					parameter.DbType = DbType.Int32;
					break;
				case DbsmColumnType.NCharacterVaring:
				case DbsmColumnType.BlobSubtypeNtext:
					parameter.DbType = DbType.String;
					break;
				case DbsmColumnType.Timestamp:
					parameter.DbType = DbType.DateTime;
					break;
				case DbsmColumnType.Boolean:
					parameter.DbType = DbType.Boolean;
					break;
				case DbsmColumnType.TinyInt:
					// Unsigned for some reason
					//
					parameter.DbType = DbType.Byte;
					break;
				case DbsmColumnType.SmallInt:
					parameter.DbType = DbType.Int16;
					break;
				default:
					throw new NotSupportedException("Unsupported column type " + column.Type);
			}

			return parameter;
		}

		public abstract IDbConnection CreateConnection();

		public virtual string MakeSelect(DbsmTable table, bool orderedByPK)
		{
			var columns = Array.ConvertAll(table.Columns, column => MakeDDLElementName(column.Name));

			var query = string.Format("SELECT\n\t{0}\nFROM\n\t{1}",
				string.Join(",\n\t", columns), MakeDDLElementName(table.Name));

			if (orderedByPK && table.KeyPrimary() != null
				&& !string.IsNullOrEmpty(table.KeyPrimary().Columns))
			{
				var keyColumns = table.KeyPrimary().Columns.Split(new[]{',', ' '},
						StringSplitOptions.RemoveEmptyEntries);

				keyColumns = Array.ConvertAll<string, string>(keyColumns, MakeDDLElementName);

				query += "\nORDER BY\n\t" + string.Join(",\n\t", keyColumns);
			}

			return query;
		}

		public virtual string MakeInsert(DbsmTable table)
		{
			var columns = Array.ConvertAll(table.Columns, column => MakeDDLElementName(column.Name));
			var parameters = Array.ConvertAll<DbsmColumn, string>(table.Columns, MakeParameterName);

			var query =
				string.Format("INSERT INTO\n\t{0}\n\t({1})\nVALUES\n\t({2})",
					MakeDDLElementName(table.Name),
					string.Join(",\n\t", columns),
					string.Join(",\n\t", parameters));

			return query;
		}

		public virtual void BeginTableLoad(IDbConnection connection, DbsmTable table)
		{
		}

		public virtual void EndTableLoad(IDbConnection connection, DbsmTable table)
		{
		}

		#endregion

		#region DDL Generate Metods

		protected virtual string MakeDDLElementName(string name)
		{
			return "[" + name + "]";
		}

		protected virtual string MakeDDLTableCreate(DbsmTable table, bool withConstraint)
		{
			var stat = new StringBuilder();

			var firstColumn = true;
			foreach (var column in table.Columns)
			{
				if (!firstColumn)
					stat.Append(",\n\t");
				stat.Append(ParseColumn(column));
				firstColumn = false;
			}

			if (withConstraint)
			{
				if (!firstColumn)
					stat.Append(",\n\t");
				foreach (var key in table.Keys)
					stat.Append(" " + ParseKey(key));
			}

			return string.Format("CREATE TABLE {0} ({1})",
				MakeDDLElementName(table.Name), stat);
		}

		protected virtual string MakeDDLTableDrop(DbsmTable table)
		{
			return string.Format("DROP TABLE {0}",
				MakeDDLElementName(table.Name));
		}

		protected virtual string MakeDDLTableRename(DbsmTable table, string newName)
		{
			return string.Format("ALTER TABLE {0} RENAME TO {1}",
				MakeDDLElementName(table.Name), newName);
		}

		protected virtual string MakeDDLTableCopy(DbsmTable toTable, DbsmTable fromTable)
		{
			return string.Format("INSERT INTO {0}({1}) SELECT {3} FROM {2}",
				MakeDDLElementName(toTable.Name), toTable.ColumnsList(),
				MakeDDLElementName(fromTable.Name), fromTable.ColumnsList());
		}

		protected virtual string MakeDDLColumnCreate(DbsmColumn column, DbsmTable table)
		{
			return string.Format("ALTER TABLE {0} ADD {1}",
				MakeDDLElementName(table.Name), ParseColumn(column));
		}

		protected virtual string MakeDDLColumnAlter(DbsmColumn mColumn, DbsmColumn eColumn, DbsmTable table)
		{
			return string.Format("ALTER TABLE {0} ALTER COLUMN {1}",
				MakeDDLElementName(table.Name), ParseColumnAlter(mColumn, eColumn));
		}

		protected virtual string MakeDDLColumnDrop(DbsmColumn column, DbsmTable table)
		{
			return string.Format("ALTER TABLE {0} DROP COLUMN {1}",
				MakeDDLElementName(table.Name), MakeDDLElementName(column.Name));
		}

		protected virtual string MakeDDLDefaultDrop(DbsmKey key, DbsmTable table)
		{
			return string.Format("ALTER TABLE {0} DROP CONSTRAINT {1}",
				MakeDDLElementName(table.Name), MakeDDLElementName(key.Name));
		}

		protected virtual string MakeDDLDefaultCreate(DbsmKey key, DbsmTable table)
		{
			return string.Format("ALTER TABLE {0} ADD CONSTRAINT {1} DEFAULT {2} FOR {3}",
				MakeDDLElementName(table.Name), MakeDDLElementName(key.Name),
				key.Source, key.Columns);
		}

		protected virtual string MakeDDLKeyCreateByAlter(DbsmKey key, DbsmTable table)
		{
			return string.Format("ALTER TABLE {0} ADD CONSTRAINT {1} {2}",
				MakeDDLElementName(table.Name), MakeDDLElementName(key.Name),
				ParseKey(key));
		}

		protected virtual string MakeDDLKeyDrop(DbsmKey key, DbsmTable table)
		{
			return string.Format("ALTER TABLE {0} DROP CONSTRAINT {1}",
				MakeDDLElementName(table.Name), MakeDDLElementName(key.Name));
		}

		protected abstract string MakeDDLIndexCreate(DbsmIndex index, DbsmTable table);

		protected virtual string MakeDDLIndexDrop(DbsmIndex index, DbsmTable table)
		{
			return string.Format("DROP INDEX {0} ON {1}",
				MakeDDLElementName(index.Name), MakeDDLElementName(table.Name));
		}

		protected abstract string ParseColumn(DbsmColumn column);

		protected virtual string ParseKey(DbsmKey key)
		{
			var stat = new StringBuilder();
			switch (key.KeyType)
			{
				case DbsmConstraintType.KeyForeign:
					stat.AppendFormat("FOREIGN KEY ({0}) REFERENCES {1}",
						Algorithms.ToString(ParseColumnListIndex(key.Columns), ", "),
						MakeDDLElementName(key.RelTable));

					if (!string.IsNullOrEmpty(key.RelColumns))
						stat.AppendFormat(" ({0})",
							Algorithms.ToString(ParseColumnListIndex(key.RelColumns), ", "));

					switch (key.DeleteRule)
					{
						case DbsmRule.Cascade:
							stat.Append(" ON DELETE CASCADE");
							break;
						case DbsmRule.SetNull:
							stat.Append(" ON DELETE SET NULL");
							break;
					}
					switch (key.UpdateRule)
					{
						case DbsmRule.Cascade:
							stat.Append(" ON UPDATE CASCADE");
							break;
						case DbsmRule.SetNull:
							stat.Append(" ON UPDATE SET NULL");
							break;
					}
					break;
				case DbsmConstraintType.KeyPrimary:
					stat.AppendFormat("PRIMARY KEY ({0})",
						Algorithms.ToString(ParseColumnListIndex(key.Columns), ", "));
					break;
				case DbsmConstraintType.Unique:
					stat.AppendFormat("UNIQUE ({0})",
						Algorithms.ToString(ParseColumnListIndex(key.Columns), ", "));
					break;
				case DbsmConstraintType.Check:
					stat.AppendFormat("CHECK ({0})", key.Source);
					break;
			}
			return stat.ToString();
		}

		protected abstract string ParseColumnAlter(DbsmColumn mColumn, DbsmColumn eColumn);

		protected virtual string ParseColumnListIndexClear(string cList)
		{
			return Algorithms.ToString(ParseColumnListIndexClear(cList, IndexClearType.All), ", ");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="iclist"></param>
		/// <param name="ctype">Clear type: 3 - all, 2 - ASC, 1 - DESC</param>
		/// <returns></returns>
		public static IEnumerable<string> ParseColumnListIndexClear(string iclist, IndexClearType ctype)
		{
			foreach (var column in iclist.Split(','))
			{
				var name = column.Trim();
				if (string.IsNullOrEmpty(name))
					continue;

				if (name.EndsWith(" ASC") && (ctype & IndexClearType.Asc) == IndexClearType.Asc)
					yield return name.Replace(" ASC", string.Empty).Trim();
				else if (name.EndsWith(" DESC") && (ctype & IndexClearType.Desc) == IndexClearType.Desc)
					yield return name.Replace(" DESC", string.Empty).Trim();
				else
					yield return name;
			}
		}

		protected virtual IEnumerable<string> ParseColumnListIndex(string iclist)
		{
			foreach(var column in iclist.Split(','))
			{
				var name = column.Trim();
				if (string.IsNullOrEmpty(name))
					continue;

				if (name.EndsWith(" ASC"))
					yield return MakeDDLElementName(name.Replace(" ASC", string.Empty).Trim()) + " ASC";
				else if (name.EndsWith(" DESC"))
					yield return MakeDDLElementName(name.Replace(" DESC", string.Empty).Trim()) + " DESC";
				else
					yield return MakeDDLElementName(name.TrimStart(' '));
			}
		}

		public static IEnumerable<string> ParseColumnList(string cList)
		{
			foreach (var column in cList.Split(','))
			{
				var name = column.Trim();
				if (name.EndsWith(" ASC"))
					yield return name.Replace(" ASC", string.Empty).Trim();
				if (name.EndsWith(" DESC"))
					yield return name.Replace(" DESC", string.Empty).Trim();
				yield return name;
			}
		}

		#endregion

	}

	internal static class UnBracket
	{
		public static string ParseUnBracket(string vl)
		{
			var _curp = 0;
			//
			var _lit = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefjhijklmnopqrstuvwxyz1234567890";
			var _bop = "()";
			// чистые операции
			var _cop = "+-";
			// грязные операции
			var _dop = "*/";
			while (true)
			{
				if (vl.Substring(_curp, 1) == "(")
				{
					var varr = new int[4];
					varr[0] = 0;
					varr[1] = 0;
					var bd = 0;
					for (var i = _curp + 1; i < vl.Length; i++)
					{
						if (vl.Substring(i, 1) == ")" && bd == 0)
						{
							varr[0] = _curp;
							varr[1] = i;
							break;
						}

						if (vl.Substring(i, 1) == "(")
							bd++;
						else if (vl.Substring(i, 1) == ")")
							bd--;
					}
					if (varr[0] < varr[1])
					{
						var i = varr[0] - 1;
						while (i >= 0)
						{
							if (vl[i] != ' ')
							{
								if (_lit.IndexOf(vl[i]) != -1)
								{
									varr[0] = -1;
									varr[1] = -1;
								}
								break;
							}
							i--;
						}
						if (varr[0] < varr[1])
						{
							var df = false;
							if (vl.Substring(varr[0] + 1, varr[1] - varr[0] - 1).IndexOfAny(_cop.ToCharArray()) != -1 ||
								vl.Substring(varr[0] + 1, varr[1] - varr[0] - 1).IndexOfAny(_dop.ToCharArray()) != -1)
							{
								for (var j = varr[0] - 1; j >= 0; j--)
								{
									if (_dop.Contains(vl.Substring(j, 1)))
										df = true;

									if (_cop.Contains(vl.Substring(j, 1)))
										break;
								}
								for (var j = varr[1] + 1; j < vl.Length; j++)
								{
									if (_dop.Contains(vl.Substring(j, 1)))
										df = true;

									if (_cop.Contains(vl.Substring(j, 1)) || _bop.Contains(vl.Substring(j, 1)))
										break;
								}
							}
							if (!df)
							{
								vl = vl.Remove(varr[1], 1);
								vl = vl.Remove(varr[0], 1);
								_curp -= (varr[0] <= _curp ? 1 : 0) + (varr[1] <= _curp ? 1 : 0);
							}
						}
					}
				}
				_curp += 1;
				if (_curp >= vl.Length)
					break;
			}
			return vl;
		}
	}

}
