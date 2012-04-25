using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Rsdn.Janus
{
	/// <summary>
	/// Класс-интерфейс
	/// общая сущность менеджеров схем разных engine
	/// </summary>
	[Localizable(false)]
	public abstract class DBSchemaDriverBase : IDBSchemaDriver
	{
		private readonly DataTable _ddlCmdTable; // DataTable операторы DDL к исполнению
		private List<string> _ddlCommands = new List<string>();

		protected DBSchemaDriverBase()
		{
			_ddlCmdTable = new DataTable("DDL_CS");
			_ddlCmdTable.Columns.Add("Priority", typeof (int));
			_ddlCmdTable.Columns.Add("Name", typeof (string));
			_ddlCmdTable.Columns.Add("ObjectType", typeof (string));
			_ddlCmdTable.Columns.Add("Statement", typeof (string));
		}

		#region Public Metods
		public abstract void CreateDatabase(string constr);

		/// <summary>
		/// Создать схему метаданных из исходной базы
		/// </summary>
		/// <param name="connStr"></param>
		public abstract DBSchema LoadExistingSchema(string connStr);

		/// <summary>
		/// Сравнение схемы с эталонной и выдача во внутреннюю хеш таблицу DDL комманд,
		/// выполнение общих для всех движков действий.
		/// </summary>
		public void CompareDbsc(DBSchema mbDbsc, string targetConnStr)
		{
			// TODO : эту кашу надо откомментировать и отрефакторить по человечески.

			var existingSchema = LoadExistingSchema(targetConnStr);
			var targetSchema = DbscCopyPrepare(mbDbsc);

			existingSchema.Normalize();
			targetSchema.Normalize();
			_ddlCmdTable.Rows.Clear();

			#region Tables
			var i = 1;
			foreach (var eTable in existingSchema.Tables)
				if (!targetSchema.IsTableExist(eTable.Name))
				{
					HelperTableDrop(eTable, existingSchema.Tables, i);
					i++;
				}

			i = 1;
			foreach (var mTable in targetSchema.Tables)
			{
				var eTable = existingSchema.GetTable(mTable.Name);
				if (eTable == null)
					HelperTableCreate(mTable, i);
				else if (mTable.GetDiffColumnsCount(eTable) == eTable.Columns.Length)
				{
					HelperTableDrop(eTable, existingSchema.Tables, i);
					HelperTableCreate(mTable, i);
				}
				else
				{
					#region Column scan
					foreach (var eColumn in eTable.Columns)
						if (!mTable.IsColumnExist(eColumn.Name))
							AddDdlCommand(i,
								eTable.Name + @"." + eColumn.Name,
								"Column",
								MakeDdlColumnDrop(eColumn, eTable));
					foreach (var mColumn in mTable.Columns)
					{
						var eColumn = eTable.GetColumn(mColumn.Name);
						if (eColumn == null)
							AddDdlCommand(i,
								mTable.Name + @"." + mColumn.Name,
								"Column",
								MakeDdlColumnCreate(mColumn, mTable));
						// to post processing in all workers AlterColumn
					}
					#endregion

					#region Indexes scan
					foreach (var eIndex in eTable.Indexes)
						if (!mTable.IsIndexExist(eIndex.Name))
						{
							AddDdlCommand(i,
								eTable.Name + @"." + eIndex.Name,
								"Index",
								MakeDdlIndexDrop(eIndex, eTable));

							if (CheckIndexTypeForRecreate(eIndex))
								foreach (var eTable1 in existingSchema.Tables)
								{
									var eKey1 = eTable1.GetKey(ConstraintType.KeyForeign, eTable.Name,
										ParseColumnListIndexClear(eIndex.Columns));
									if (eKey1 != null)
									{
										AddDdlCommand(i,
											eTable.Name + @"." + eKey1.Name,
											eKey1.KeyType.ToString(),
											MakeDdlKeyDrop(eKey1, eTable1));

										AddDdlCommand(i,
											eTable.Name + @"." + eKey1.Name,
											eKey1.KeyType.ToString(),
											MakeDdlKeyCreateByAlter(eKey1, eTable1));
									}
								}
						}
					foreach (var mIndex in mTable.Indexes)
						if (eTable.IsIndexExist(mIndex.Name))
						{
							var eIndex = eTable.GetIndex(mIndex.Name);
							if (!eIndex.Equals(mIndex))
							{
								AddDdlCommand(i,
									eTable.Name + @"." + eIndex.Name,
									"Index",
									MakeDdlIndexDrop(eIndex, eTable));

								AddDdlCommand(i,
									eTable.Name + @"." + mIndex,
									"Index",
									MakeDdlIndexCreate(mIndex, mTable));

								if (CheckIndexTypeForRecreate(eIndex))
									foreach (var eTable1 in existingSchema.Tables)
									{
										var eKey1 = eTable1.GetKey(ConstraintType.KeyForeign, eTable.Name,
											ParseColumnListIndexClear(eIndex.Columns));
										if (eKey1 != null)
										{
											AddDdlCommand(i,
												eTable1.Name + @"." + eKey1.Name,
												eKey1.KeyType.ToString(),
												MakeDdlKeyDrop(eKey1, eTable1));

											AddDdlCommand(i,
												eTable1.Name + @"." + eKey1.Name,
												eKey1.KeyType.ToString(),
												MakeDdlKeyCreateByAlter(eKey1, eTable1));
										}
									}
							}
						}
						else
							AddDdlCommand(i,
								mTable.Name + @"." + mIndex.Name,
								"Index",
								MakeDdlIndexCreate(mIndex, mTable));
					#endregion

					#region Keys scan
					foreach (var eKey in eTable.Keys)
					{
						if (eKey.KeyType == ConstraintType.Default)
							continue;
						if (!mTable.IsKeyExist(eKey.Name))
						{
							AddDdlCommand(i,
								eTable.Name + @"." + eKey.Name,
								eKey.KeyType.ToString(),
								MakeDdlKeyDrop(eKey, eTable));

							if (CheckKeyTypeForRecreate(eKey))
								foreach (var eTable1 in existingSchema.Tables)
								{
									var eKey1 = eTable1.GetKey(ConstraintType.KeyForeign, eTable.Name, eKey.Columns);
									if (eKey1 != null)
									{
										AddDdlCommand(i,
											eTable1.Name + @"." + eKey1.Name,
											eKey1.KeyType.ToString(),
											MakeDdlKeyDrop(eKey1, eTable1));

										AddDdlCommand(i,
											eTable1.Name + @"." + eKey1.Name,
											eKey1.KeyType.ToString(),
											MakeDdlKeyCreateByAlter(eKey1, eTable1));
									}
								}
						}
					}
					foreach (var mKey in mTable.Keys)
					{
						if (mKey.KeyType == ConstraintType.Default)
							continue;

						if (eTable.IsKeyExist(mKey.Name))
						{
							var eKey = eTable.GetKey(mKey.Name);
							if (!eKey.Equals(mKey))
							{
								AddDdlCommand(i,
									eTable.Name + @"." + mKey.Name,
									mKey.KeyType.ToString(),
									MakeDdlKeyDrop(eKey, eTable));

								AddDdlCommand(i,
									eTable.Name + @"." + mKey.Name,
									mKey.KeyType.ToString(),
									MakeDdlKeyCreateByAlter(mKey, eTable));

								if (CheckKeyTypeForRecreate(eKey))
									foreach (var eTable1 in existingSchema.Tables)
									{
										var eKey1 = eTable1.GetKey(ConstraintType.KeyForeign, eTable.Name, eKey.Columns);
										if (eKey1 != null)
										{
											AddDdlCommand(i,
												eTable1.Name + @"." + eKey1.Name,
												eKey1.KeyType.ToString(),
												MakeDdlKeyDrop(eKey1, eTable1));

											AddDdlCommand(i,
												eTable1.Name + @"." + eKey1.Name,
												eKey1.KeyType.ToString(),
												MakeDdlKeyCreateByAlter(eKey1, eTable1));
										}
									}
							}
						}
						else
							AddDdlCommand(i,
								eTable.Name + @"." + mKey.Name,
								mKey.KeyType.ToString(),
								MakeDdlKeyCreateByAlter(mKey, eTable));
					}
					#endregion
				}
				i++;
			}
			#endregion

			AfterSchemaComparision(existingSchema, targetSchema);

			_ddlCommands = Reparse();

			_ddlCmdTable.Rows.Clear();
		}

		public void Prepare(string connStr)
		{
			try
			{
				ExecuteDdlCommands(_ddlCommands, connStr);
			}
			catch (Exception ex)
			{
				throw new DBSchemaException(@"Error while preparing SqlDiff", ex);
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
		protected void AddDdlCommand(
			int priority,
			[Localizable(false)]string name,
			[Localizable(false)]string type,
			[Localizable(false)]string statement)
		{
			_ddlCmdTable.Rows.Add(priority, name, type, statement);
		}

		/// <summary>
		/// Удалить DDL команды по заданному фильтру.
		/// </summary>
		/// <param name="filter">фильтр</param>
		protected void DeleteDdlCommandsByFilter([Localizable(false)]string filter)
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
		private IEnumerable<string> GetFilteredDdlStatements(
			[Localizable(false)]string filter,
			[Localizable(false)]string sort)
		{
			var dw = _ddlCmdTable.DefaultView;
			dw.RowFilter = filter;
			dw.Sort = sort;

			return dw.Cast<DataRowView>().Select(t => t["Statement"].ToString());
		}

		/// <summary>
		/// Так как схема - референс тип, создание его глубокой копии.
		/// Которая при приведение к искомому движку бедет правиться.
		/// И приведение её к исходному движку
		/// </summary>
		/// <param name="schema">Эталоная схема</param>
		/// <returns></returns>
		protected abstract DBSchema DbscCopyPrepare(DBSchema schema);

		/// <summary>
		/// Препроцесинг схем, детали для разных движков
		/// </summary>
		/// <param name="existingSchema">Существующая схема</param>
		/// <param name="targetSchema">Эталоная схема</param>
		protected virtual void AfterSchemaComparision(DBSchema existingSchema, DBSchema targetSchema)
		{}

		/// <summary>
		/// Проверка необходимости пересоздания связанных Foreign Key
		/// при удаление констрента
		/// </summary>
		/// <param name="eKey"></param>
		/// <returns></returns>
		protected abstract bool CheckKeyTypeForRecreate(KeySchema eKey);

		/// <summary>
		/// Проверка необходимости пересоздания связанных Foreign Key
		/// при удалении индекса
		/// </summary>
		/// <param name="eIndex"></param>
		/// <returns></returns>
		protected abstract bool CheckIndexTypeForRecreate(IndexSchema eIndex);

		private void HelperTableCreate(TableSchema mTable, int i)
		{
			AddDdlCommand(i,
				mTable.Name,
				"Table",
				MakeDdlTableCreate(mTable, false));

			foreach (var mIndex in mTable.Indexes)
				AddDdlCommand(i,
					mTable.Name + @"." + mIndex.Name,
					"Index",
					MakeDdlIndexCreate(mIndex, mTable));
			foreach (var mKey in mTable.Keys)
				AddDdlCommand(i,
					mTable.Name + @"." + mKey.Name,
					mKey.KeyType.ToString(),
					MakeDdlKeyCreateByAlter(mKey, mTable));
		}

		private void HelperTableDrop(TableSchema eTable, IEnumerable<TableSchema> eTables, int i)
		{
			foreach (var table in eTables)
				foreach (var t in table.Keys)
					if (t.KeyType == ConstraintType.KeyForeign &&
						t.RelTable == eTable.Name)
						AddDdlCommand(i,
							table.Name + @"." + t.Name,
							t.KeyType.ToString(),
							MakeDdlKeyDrop(t, table));
			AddDdlCommand(i,
				eTable.Name,
				"Table",
				MakeDdlTableDrop(eTable));
		}

		/// <summary>
		/// Выполнить набор DDL команд.
		/// </summary>
		/// <param name="commands">набор DDL команд</param>
		/// <param name="connStr"></param>
		protected abstract void ExecuteDdlCommands(IEnumerable<string> commands, string connStr);

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
				throw new OperationCanceledException(@"Not all commands reparsed");

			#region Delete duplicates
			var hash = new HashSet<string>();
			ddlCommands =
				new List<string>(ddlCommands.Where(
					cmd =>
					{
						if (hash.Contains(cmd))
							return false;
						hash.Add(cmd);
						return true;
					}));
			#endregion

			return ddlCommands;
		}
		#endregion

		#region Migration
		public virtual IDbDataParameter ConvertToDbParameter(TableColumnSchema column,
			IDbDataParameter parameter)
		{
			parameter.ParameterName = MakeParameterName(column);

			switch (column.Type)
			{
				case ColumnType.Integer:
					parameter.DbType = DbType.Int32;
					break;
				case ColumnType.NCharacterVaring:
				case ColumnType.BlobSubtypeNText:
					parameter.DbType = DbType.String;
					break;
				case ColumnType.Timestamp:
					parameter.DbType = DbType.DateTime;
					break;
				case ColumnType.Boolean:
					parameter.DbType = DbType.Boolean;
					break;
				case ColumnType.TinyInt:
					// Unsigned for some reason
					//
					parameter.DbType = DbType.Byte;
					break;
				case ColumnType.SmallInt:
					parameter.DbType = DbType.Int16;
					break;
				default:
					throw new NotSupportedException(@"Unsupported column type " + column.Type);
			}

			return parameter;
		}

		public abstract IDbConnection CreateConnection(string connStr);

		public virtual string MakeSelect(TableSchema table, bool orderedByPK)
		{
			var columns = Array.ConvertAll(table.Columns, column => MakeDdlElementName(column.Name));

			var query = string.Format("SELECT\n\t{0}\nFROM\n\t{1}",
				string.Join(",\n\t", columns), MakeDdlElementName(table.Name));

			if (orderedByPK && table.KeyPrimary() != null
				&& !string.IsNullOrEmpty(table.KeyPrimary().Columns))
			{
				var keyColumns = table.KeyPrimary().Columns.Split(new[] {',', ' '},
					StringSplitOptions.RemoveEmptyEntries);
				keyColumns = Array.ConvertAll(keyColumns, MakeDdlElementName);
				query += "\nORDER BY\n\t" + string.Join(",\n\t", keyColumns);
			}

			return query;
		}

		public virtual string MakeInsert(TableSchema table)
		{
			var columns = Array.ConvertAll(table.Columns, column => MakeDdlElementName(column.Name));
			var parameters = Array.ConvertAll(table.Columns, MakeParameterName);

			var query =
				string.Format("INSERT INTO\n\t{0}\n\t({1})\nVALUES\n\t({2})",
					MakeDdlElementName(table.Name),
					string.Join(",\n\t", columns),
					string.Join(",\n\t", parameters));

			return query;
		}

		public virtual void BeginTableLoad(IDbConnection connection, TableSchema table)
		{}

		public virtual void EndTableLoad(IDbConnection connection, TableSchema table)
		{}

		protected virtual string MakeParameterName(TableColumnSchema column)
		{
			return @"@" + column.Name;
		}
		#endregion

		#region DDL Generate Metods
		protected virtual string MakeDdlElementName(string name)
		{
			return @"[" + name + @"]";
		}

		protected virtual string MakeDdlTableCreate(TableSchema table, bool withConstraint)
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
					stat.Append(@" " + ParseKey(key));
			}

			return string.Format(@"CREATE TABLE {0} ({1})",
				MakeDdlElementName(table.Name), stat);
		}

		protected string MakeDdlTableDrop(TableSchema table)
		{
			return string.Format(@"DROP TABLE {0}",
				MakeDdlElementName(table.Name));
		}

		protected string MakeDdlTableRename(TableSchema table, string newName)
		{
			return string.Format(@"ALTER TABLE {0} RENAME TO {1}",
				MakeDdlElementName(table.Name), newName);
		}

		protected string MakeDdlTableCopy(TableSchema toTable, TableSchema fromTable)
		{
			return string.Format(@"INSERT INTO {0}({1}) SELECT {3} FROM {2}",
				MakeDdlElementName(toTable.Name), toTable.ColumnsList(MakeDdlElementName),
				MakeDdlElementName(fromTable.Name), fromTable.ColumnsList(MakeDdlElementName));
		}

		private string MakeDdlColumnCreate(TableColumnSchema column, TableSchema table)
		{
			return string.Format(@"ALTER TABLE {0} ADD {1}",
				MakeDdlElementName(table.Name), ParseColumn(column));
		}

		protected virtual string MakeDdlColumnAlter(TableColumnSchema mColumn, TableColumnSchema eColumn,
			TableSchema table)
		{
			return string.Format(@"ALTER TABLE {0} ALTER COLUMN {1}",
				MakeDdlElementName(table.Name), ParseColumnAlter(mColumn, eColumn));
		}

		protected virtual string MakeDdlColumnDrop(TableColumnSchema column, TableSchema table)
		{
			return string.Format(@"ALTER TABLE {0} DROP COLUMN {1}",
				MakeDdlElementName(table.Name), MakeDdlElementName(column.Name));
		}

		protected string MakeDdlDefaultDrop(KeySchema key, TableSchema table)
		{
			return string.Format(@"ALTER TABLE {0} DROP CONSTRAINT {1}",
				MakeDdlElementName(table.Name), MakeDdlElementName(key.Name));
		}

		protected string MakeDdlDefaultCreate(KeySchema key, TableSchema table)
		{
			return string.Format(@"ALTER TABLE {0} ADD CONSTRAINT {1} DEFAULT {2} FOR {3}",
				MakeDdlElementName(table.Name), MakeDdlElementName(key.Name),
				key.Source, key.Columns);
		}

		protected string MakeDdlKeyCreateByAlter(KeySchema key, TableSchema table)
		{
			return string.Format(@"ALTER TABLE {0} ADD CONSTRAINT {1} {2}",
				MakeDdlElementName(table.Name), MakeDdlElementName(key.Name),
				ParseKey(key));
		}

		protected string MakeDdlKeyDrop(KeySchema key, TableSchema table)
		{
			return string.Format(@"ALTER TABLE {0} DROP CONSTRAINT {1}",
				MakeDdlElementName(table.Name), MakeDdlElementName(key.Name));
		}

		protected abstract string MakeDdlIndexCreate(IndexSchema index, TableSchema table);

		protected virtual string MakeDdlIndexDrop(IndexSchema index, TableSchema table)
		{
			return string.Format(@"DROP INDEX {0} ON {1}",
				MakeDdlElementName(index.Name), MakeDdlElementName(table.Name));
		}

		protected abstract string ParseColumn(TableColumnSchema column);

		protected string ParseKey(KeySchema key)
		{
			var stat = new StringBuilder();
			switch (key.KeyType)
			{
				case ConstraintType.KeyForeign:
					stat.AppendFormat(@"FOREIGN KEY ({0}) REFERENCES {1}",
						ParseColumnListIndex(key.Columns).JoinStrings(),
						MakeDdlElementName(key.RelTable));

					if (!string.IsNullOrEmpty(key.RelColumns))
						stat.AppendFormat(@" ({0})", ParseColumnListIndex(key.RelColumns).JoinStrings());

					switch (key.DeleteRule)
					{
						case LinkRule.Cascade:
							stat.Append(@" ON DELETE CASCADE");
							break;
						case LinkRule.SetNull:
							stat.Append(@" ON DELETE SET NULL");
							break;
					}
					switch (key.UpdateRule)
					{
						case LinkRule.Cascade:
							stat.Append(@" ON UPDATE CASCADE");
							break;
						case LinkRule.SetNull:
							stat.Append(@" ON UPDATE SET NULL");
							break;
					}
					break;
				case ConstraintType.KeyPrimary:
					//TODO проверить на DB отличных от MS SQL
					stat.AppendFormat(@"PRIMARY KEY ({0})", ParseColumnListIndex(key.Columns).JoinStrings(@","));
					break;
				case ConstraintType.Unique:
					stat.AppendFormat(@"UNIQUE ({0})", ParseColumnListIndex(key.Columns).JoinStrings());
					break;
				case ConstraintType.Check:
					stat.AppendFormat(@"CHECK ({0})", key.Source);
					break;
				case ConstraintType.Default:
					stat.AppendFormat(@"DEFAULT {0} FOR {1}", key.Source, key.Columns);
					break;
			}
			return stat.ToString();
		}

		protected abstract string ParseColumnAlter(TableColumnSchema mColumn, TableColumnSchema eColumn);

		private static string ParseColumnListIndexClear(string cList)
		{
			return ParseColumnListIndexClear(cList, IndexClearType.All).JoinStrings();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="iclist"></param>
		/// <param name="ctype">Clear type: 3 - all, 2 - ASC, 1 - DESC</param>
		/// <returns></returns>
		private static IEnumerable<string> ParseColumnListIndexClear(string iclist, IndexClearType ctype)
		{
			foreach (var column in iclist.Split(','))
			{
				var name = column.Trim();
				if (string.IsNullOrEmpty(name))
					continue;

				if (name.EndsWith(@" ASC") && (ctype & IndexClearType.Asc) == IndexClearType.Asc)
					yield return name.Replace(@" ASC", string.Empty).Trim();
				else if (name.EndsWith(@" DESC") && (ctype & IndexClearType.Desc) == IndexClearType.Desc)
					yield return name.Replace(@" DESC", string.Empty).Trim();
				else
					yield return name;
			}
		}

		protected IEnumerable<string> ParseColumnListIndex(string iclist)
		{
			foreach (var column in iclist.Split(','))
			{
				var name = column.Trim();
				if (string.IsNullOrEmpty(name))
					continue;

				if (name.EndsWith(@" ASC"))
					yield return MakeDdlElementName(name.Replace(@" ASC", string.Empty).Trim()) + @" ASC";
				else if (name.EndsWith(@" DESC"))
					yield return MakeDdlElementName(name.Replace(@" DESC", string.Empty).Trim()) + @" DESC";
				else
					yield return MakeDdlElementName(name.TrimStart(' '));
			}
		}
		#endregion
	}
}