using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using ADOX;

namespace Rsdn.Janus.Jet
{
	internal sealed class JetSchemaDriver : DBSchemaDriverBase
	{
		// !RULE: Foreign Key may be to PK,UI,UC and column without constraint(fuck!)
		// !IMP: UC override UI

		public override void CreateDatabase(string constr)
		{
			var catalog = (Catalog)new CatalogClass();
			try
			{
				catalog.Create(constr);
			}
			finally
			{
				Marshal.ReleaseComObject(catalog);
			}
		}

		/// <summary>
		/// Создать схему метаданных из исходной базы
		/// </summary>
		/// <param name="connStr"></param>
		public override DBSchema LoadExistingSchema(string connStr)
		{
			return JetSchemaLoader.LoadSchema(connStr);
		}

		public override IDbConnection CreateConnection(string connStr)
		{
			return new OleDbConnection(connStr);
		}

		protected override string MakeParameterName(TableColumnSchema column)
		{
			// Бедный несчастный Jet умирает на Timestamp'ах с долями секунд.
			// Но при преобразовании в строку, дата обрезается до секунд,
			// что и требовалось.
			//
			if (column.Type == ColumnType.Timestamp)
				return @"CDate(" + base.MakeParameterName(column) + @")";

			return base.MakeParameterName(column);
		}

		public override IDbDataParameter ConvertToDbParameter(TableColumnSchema column,
			IDbDataParameter parameter)
		{
			if (column.Type == ColumnType.Timestamp)
			{
				// Бедный несчастный Jet умирает на Timestamp'ах с долями секунд.
				// Но при преобразовании в строку, дата обрезается до секунд,
				// что и требовалось.
				//
				parameter.DbType = DbType.String;
				parameter.ParameterName = base.MakeParameterName(column);

				return parameter;
			}

			return base.ConvertToDbParameter(column, parameter);
		}

		#region Schema prepare metods
		protected override DBSchema DbscCopyPrepare(DBSchema schema)
		{
			var newSchema = schema.Copy();
			return newSchema;
		}

		protected override void AfterSchemaComparision(DBSchema existingSchema, DBSchema targetSchema)
		{
			var i = 1;
			foreach (var mTable in targetSchema.Tables)
			{
				var eTable = existingSchema.GetTable(mTable.Name);
				if (eTable != null)
					//bool recreate = false;
					//bool toident = false;
					foreach (var mColumn in mTable.Columns)
					{
						var eColumn = eTable.GetColumn(mColumn.Name);
						if (eColumn != null)
							if (!mTable.IsExactColumn(eColumn))
							{
								foreach (var eTable1 in existingSchema.Tables)
								{
									var eKey1 = eTable1.GetKey2(ConstraintType.KeyForeign, eTable.Name, eColumn.Name);
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

								AddDdlCommand(i,
									eTable.Name + @"." + mColumn.Name,
									"Column",
									MakeDdlColumnAlter(mColumn, eColumn, eTable));
							}
					}
				i++;
			}
		}

		protected override bool CheckKeyTypeForRecreate(KeySchema eKey)
		{
			return eKey.KeyType == ConstraintType.Unique;
			// || eKey.keyType == DBSM.ConstraintType.KeyPrimary)
		}

		protected override bool CheckIndexTypeForRecreate(IndexSchema eIndex)
		{
			return eIndex.Unique;
		}

		protected override void ExecuteDdlCommands(IEnumerable<string> commands, string connStr)
		{
			using (var con = new OleDbConnection(connStr))
			{
				con.Open();

				var cmd = con.CreateCommand();
				cmd.CommandTimeout = 0;

				using (var trans = con.BeginTransaction())
				{
					cmd.Transaction = trans;

					foreach (var command in commands)
					{
						cmd.CommandText = command + @";";
						SchemaHelper.SafeRunCommand(command, () => cmd.ExecuteNonQuery());
					}

					trans.Commit();
				}
			}
		}

		protected override void WriteDdlCommands(TextWriter wr, IEnumerable<string> commands)
		{
			foreach (var command in commands)
				wr.WriteLine(command + @";");
		}
		#endregion

		#region  DDL Make Metods
		protected override string MakeDdlColumnAlter(TableColumnSchema mColumn, TableColumnSchema eColumn,
			TableSchema table)
		{
			return string.Format(@"ALTER TABLE {0} ALTER COLUMN {1}",
				MakeDdlElementName(table.Name), ParseColumnAlter(mColumn, eColumn));
		}

		protected override string MakeDdlIndexCreate(IndexSchema eindex, TableSchema etable)
		{
			var stat = new StringBuilder();

			if (eindex.PrimaryKey || eindex.NullAllowances != IndexNullAllowance.Allow)
			{
				stat.Append(@" WITH");
				if (eindex.PrimaryKey)
					stat.Append(@" PRIMARY");
				switch (eindex.NullAllowances)
				{
					case IndexNullAllowance.Disallow:
						stat.Append(@" DISALLOW NULL");
						break;
					case IndexNullAllowance.Ignore:
						stat.Append(@" IGNORE NULL");
						break;
				}
			}

			return string.Format(@"CREATE {0} INDEX {1} ON {2} ({3}) {4}",
				eindex.Unique ? @"UNIQUE" : string.Empty,
				MakeDdlElementName(eindex.Name),
				MakeDdlElementName(etable.Name),
				ParseColumnListIndex(eindex.Columns).JoinStrings(@", "),
				stat);
		}

		protected override string ParseColumn(TableColumnSchema ecolumn)
		{
			var stat = new StringBuilder();
			stat.Append(@"[" + ecolumn.Name + @"]");
			if (ecolumn.AutoIncrement)
				stat.Append(@" IDENTITY(" + ecolumn.Seed.ToString(CultureInfo.CurrentCulture) + @"," +
					ecolumn.Increment.ToString(CultureInfo.InvariantCulture) + @")");
			else
			{
				stat.Append(@" " + JetSchemaLoader.TypeDbsmToJet(ecolumn));
				if (!String.IsNullOrEmpty(ecolumn.DefaultValue))
					stat.Append(@" DEFAULT " + ecolumn.DefaultValue);
			}
			if (!ecolumn.Nullable)
				stat.Append(@" NOT NULL");
			return stat.ToString();
		}

		protected override string ParseColumnAlter(TableColumnSchema mColumn, TableColumnSchema eColumn)
		{
			return ParseColumn(mColumn);
		}
		#endregion
	}
}