using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rsdn.Janus.Sqlite
{
	internal sealed class SqliteSchemaDriver : DBSchemaDriverBase
	{
		internal const int PageSize = 8192;

		// !RULE: Foreign Key may be to PK,UI,UC
		//
		public SqliteSchemaDriver(IServiceProvider serviceProvider) : base()
		{}

		public override IDbConnection CreateConnection(string connStr)
		{
			return new SQLiteConnection(connStr);
		}

		public override void CreateDatabase(string constr)
		{
			// SQLite молча пересоздаст файл если такой уже есть.
			//
			var csb = new SQLiteConnectionStringBuilder(constr);
			if (!File.Exists(csb.DataSource)
				|| MessageBox.Show(
					string.Format(Resources.FileExistedMessage, Path.GetFileName(csb.DataSource)),
					@"SQLite",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question,
					MessageBoxDefaultButton.Button2) == DialogResult.Yes)
			{
				// Create file
				SQLiteConnection.CreateFile(csb.DataSource);

				// Change default page size
				using (var con = new SQLiteConnection(constr))
				using (var cmd = con.CreateCommand())
				{
					con.Open();
					cmd.CommandText = @"pragma page_size=" + PageSize + @"; VACUUM;";
					cmd.ExecuteNonQuery();
				}
			}
		}

		/// <summary>
		/// Создать схему метаданных из исходной базы
		/// </summary>
		/// <param name="connStr"></param>
		public override DBSchema LoadExistingSchema(string connStr)
		{
			var schema = SqliteSchemaLoader.LoadSchema(connStr);
			var keys = new List<KeySchema>();
			foreach (var table in schema.Tables)
			{
				keys.Clear();
				keys.AddRange(table.Keys.Where(key => key.KeyType != ConstraintType.Default));
				table.Keys = keys.ToArray();
			}
			return schema;
		}

		#region Schema prepare metods
		protected override DBSchema DbscCopyPrepare(DBSchema schema)
		{
			return schema.Copy();
		}

		protected override void AfterSchemaComparision(DBSchema existingSchema, DBSchema targetSchema)
		{
			// По-хорошему, так делать нельзя.
			// Тут нужно поменять консерваторию, чтобы конкретный движок
			// мог сказать, что эту операцию он не поддерживает, но если тупо
			// пересоздать эту таблицу и перелить в неё данные то всё будет ок.
			// А пока что тупая затычка, чтобы колонки, которые должны быть
			// autoincrement, но не являются таковыми, всё-таки стали 
			// autoincrement'ными.

			DeleteDdlCommandsByFilter("ObjectType='KeyPrimary' OR ObjectType='KeyForeign'");

			var i = 1;
			foreach (var mTable in targetSchema.Tables)
			{
				var eTable = existingSchema.GetTable(mTable.Name);
				if (eTable == null)
					continue;

				// ReSharper disable AccessToModifiedClosure
				var recreate =
					mTable
						.Columns
						.Select(mColumn => eTable.GetColumn(mColumn.Name))
						.Where(eColumn => eColumn != null)
						.Any(eColumn => !mTable.IsExactColumn(eColumn));
				// ReSharper restore AccessToModifiedClosure

				if (!recreate)
					continue;

				var tTable = mTable.Clone();
				tTable.Name = mTable.Name + @"_tmp";

				AddDdlCommand(i, tTable.Name, "RecreateTable", MakeDdlTableCreate(tTable, true));

				foreach (var tIndex in tTable.Indexes)
					AddDdlCommand(i, tTable.Name + @"." + tIndex.Name, "RecreateIndex", MakeDdlIndexCreate(tIndex, tTable));

				AddDdlCommand(i, tTable.Name + @"." + tTable.Name, "RecreateInsert", MakeDdlTableCopy(tTable, mTable));

				AddDdlCommand(i, tTable.Name, "RecreateRename", MakeDdlTableRename(tTable, mTable.Name));

				AddDdlCommand(i, mTable.Name, "RecreateTable", MakeDdlTableDrop(mTable));

				i++;
			}
		}

		protected override bool CheckKeyTypeForRecreate(KeySchema eKey)
		{
			return eKey.KeyType == ConstraintType.Unique ||
				eKey.KeyType == ConstraintType.KeyPrimary;
		}

		protected override bool CheckIndexTypeForRecreate(IndexSchema eIndex)
		{
			return eIndex.Unique;
		}

		protected override void ExecuteDdlCommands(IEnumerable<string> commands, string connStr)
		{
			using (var con = new SQLiteConnection(connStr))
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
		protected override string MakeDdlTableCreate(TableSchema table, bool withConstraint)
		{
			// На параметр 'withConstraint' приходится забить, т.к. AUTOINCREMENT
			// может быть задан только "по месту".
			// Лишние ALTER TABLE потом тупо удаляются в AfterSchemaComparision.

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
							@"SQLite does not support auto increment columns with the seed other then 1");

					if (column.Increment != 1)
						throw new NotSupportedException(
							@"SQLite does not support auto increment columns with the increment other then 1");

					if (pk == null || pk.Columns != column.Name || column.Type != ColumnType.Integer)
						throw new NotSupportedException(
							@"SQLite supports only autoincrement for integer primary keys");

					stat.Append(@" PRIMARY KEY AUTOINCREMENT");
					pk = null;
				}
			}

			foreach (var key in table.Keys)
			{
				if (key.KeyType == ConstraintType.KeyPrimary && pk == null)
					// Primary key was already processed by an autoincrement column.
					//
					continue;

				if (stat.Length > 0)
					stat.Append(",\n\t");

				stat.Append(ParseKey(key));
			}

			return string.Format(@"CREATE TABLE {0} ({1})", MakeDdlElementName(table.Name), stat);
		}

		protected override string MakeDdlIndexCreate(IndexSchema index, TableSchema table)
		{
			return string.Format(
				@"CREATE {0} {1} INDEX {2} ON {3} ({4})",
				index.Unique ? @" UNIQUE" : string.Empty,
				string.Empty, //index.Clustered ? " CLUSTERED" : string.Empty,
				MakeDdlElementName(index.Name),
				MakeDdlElementName(table.Name),
				ParseColumnListIndex(index.Columns).JoinStrings(@", "));
		}

		protected override string MakeDdlIndexDrop(IndexSchema eIndex, TableSchema etable)
		{
			return string.Format(@"DROP INDEX {0}", MakeDdlElementName(eIndex.Name));
		}

		protected override string ParseColumn(TableColumnSchema eColumn)
		{
			var stat = new StringBuilder();
			stat.AppendFormat(@" [" + eColumn.Name + @"] " + SqliteSchemaLoader.TypeDbsmToSqlite(eColumn));

			if (!String.IsNullOrEmpty(eColumn.DefaultValue))
				stat.Append(@" DEFAULT " + eColumn.DefaultValue);

			if (!eColumn.Nullable)
				stat.Append(@" NOT NULL");
			return stat.ToString();
		}

		protected override string ParseColumnAlter(TableColumnSchema mColumn, TableColumnSchema eColumn)
		{
			return string.Format(@" [{0}] {1}{2} ",
				mColumn.Name,
				SqliteSchemaLoader.TypeDbsmToSqlite(mColumn),
				mColumn.Nullable ? string.Empty : @" NOT NULL");
		}
		#endregion
	}
}