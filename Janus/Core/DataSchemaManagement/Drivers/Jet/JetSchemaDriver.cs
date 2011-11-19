using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Text;

using Rsdn.Janus.Framework;

using IServiceProvider=Rsdn.SmartApp.IServiceProvider;

namespace Rsdn.Janus
{
	internal sealed class JetSchemaDriver : DBSchemaDriverBase
	{
		// !RULE: Foreign Key may be to PK,UI,UC and column without constraint(fuck!)
		// !IMP: UC override UI

		public JetSchemaDriver(IServiceProvider serviceProvider, string constr)
			: base(serviceProvider, JetSupportModule.DriverName, constr)
		{ }

		public override IDbConnection CreateConnection()
		{
			return new OleDbConnection(ConnectionString);
		}

		public override string MakeParameterName(DbsmColumn column)
		{
			// Бедный несчастный Jet умирает на Timestamp'ах с долями секунд.
			// Но при преобразовании в строку, дата обрезается до секунд,
			// что и требовалось.
			//
			if (column.Type == DbsmColumnType.Timestamp)
				return "CDate(" + base.MakeParameterName(column) + ")";

			return base.MakeParameterName(column);
		}

		public override IDbDataParameter ConvertToDbParameter(DbsmColumn column, IDbDataParameter parameter)
		{
			if (column.Type == DbsmColumnType.Timestamp)
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

		#region Dbsc prepare metods

		protected override DbsmSchema DbscCopyPrepare(DbsmSchema schema)
		{
			var newSchema = schema.Copy();
			switch (schema.DbEngine)
			{
				case DbEngineType.MsSqlDB:
					return ConvertFromSql(newSchema);
				case DbEngineType.FireBirdDB:
					throw new ArgumentException("Not supported Dbengine");
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
					//bool recreate = false;
					//bool toident = false;
					foreach (var mColumn in mTable.Columns)
					{
						var eColumn = eTable.GetColumn(mColumn.Name);
						if (eColumn != null)
						{
							if (!mTable.IsExactColumn(eColumn))
							{
								foreach (var eTable1 in eDbsc.Tables)
								{
									var eKey1 = eTable1.GetKey2(DbsmConstraintType.KeyForeign, eTable.Name, eColumn.Name);
									if (eKey1 != null)
									{
										AddDdlCommand(i,
											eTable1.Name + "." + eKey1.Name,
											eKey1.KeyType.ToString(),
											MakeDDLKeyDrop(eKey1,eTable1));

										AddDdlCommand(i,
											eTable1.Name + "." + eKey1.Name,
											eKey1.KeyType.ToString(),
											MakeDDLKeyCreateByAlter(eKey1,eTable1));
									}
								}

								AddDdlCommand(i,
									eTable.Name + "." + mColumn.Name,
									"Column",
									MakeDDLColumnAlter(mColumn, eColumn, eTable));
							}
						}
					}
				}
				i++;
			}
		}

		protected override bool CheckKeyTypeForRecreate(DbsmKey eKey)
		{
			return eKey.KeyType == DbsmConstraintType.Unique;// || eKey.keyType == DBSM.DbsmConstraintType.KeyPrimary)
		}

		protected override bool CheckIndexTypeForRecreate(DbsmIndex eIndex)
		{
			return eIndex.Unique;
		}

		protected override void ExecuteDdlCommands(IEnumerable<string> commands)
		{
			using (var con = new OleDbConnection(ConnectionString))
			{
				con.Open();

				var cmd = con.CreateCommand();
				cmd.CommandTimeout = 0;

				using (var trans = con.BeginTransaction())
				{
					cmd.Transaction = trans;

					foreach (var command in commands)
					{
						cmd.CommandText = command + ";";
						cmd.ExecuteNonQuery();
					}

					trans.Commit();
				}
			}
		}

		protected override void WriteDdlCommands(TextWriter wr, IEnumerable<string> commands)
		{
			foreach (var command in commands)
				wr.WriteLine(command + ";");
		}

		#endregion

		#region  DDL Make Metods

		protected override string MakeDDLColumnAlter(DbsmColumn mColumn, DbsmColumn eColumn, DbsmTable table)
		{
			return string.Format("ALTER TABLE {0} ALTER COLUMN {1}",
				MakeDDLElementName(table.Name), ParseColumnAlter(mColumn, eColumn));
		}

		protected override string MakeDDLIndexCreate(DbsmIndex eindex, DbsmTable etable)
		{
			var stat = new StringBuilder();

			if (eindex.PrimaryKey || eindex.AllowNulls != DbsmAllowNulls.IndexNullsAllow)
			{
				stat.Append(" WITH");
				if (eindex.PrimaryKey)
					stat.Append(" PRIMARY");
				switch (eindex.AllowNulls)
				{
					case DbsmAllowNulls.IndexNullsDisallow:
						stat.Append(" DISALLOW NULL");
						break;
					case DbsmAllowNulls.IndexNullsIgnore:
						stat.Append(" IGNORE NULL");
						break;
				}
			}

			return string.Format("CREATE {0} INDEX {1} ON {2} ({3}) {4}",
				eindex.Unique ? "UNIQUE" : string.Empty,
				MakeDDLElementName(eindex.Name),
				MakeDDLElementName(etable.Name),
				Algorithms.ToString(ParseColumnListIndex(eindex.Columns), ", "),
				stat);
		}

		protected override string ParseColumn(DbsmColumn ecolumn)
		{
			var stat = new StringBuilder();
			stat.Append("[" + ecolumn.Name + "]");
			if (ecolumn.AutoIncrement)
				stat.Append(" IDENTITY(" + ecolumn.Seed.ToString(CultureInfo.CurrentCulture) + "," + ecolumn.Increment.ToString(CultureInfo.InvariantCulture) + ")");
			else
			{
				stat.Append(" " + ecolumn.TypeDbsmToJet());
				if (!String.IsNullOrEmpty(ecolumn.DefaultValue))
				{
					stat.Append(" DEFAULT " + ecolumn.DefaultValue);
				}
			}
			if (!ecolumn.Nullable)
				stat.Append(" NOT NULL");
			return stat.ToString();
		}

		protected override string ParseColumnAlter(DbsmColumn mColumn, DbsmColumn eColumn)
		{
			return ParseColumn(mColumn);
		}

		#endregion

		#region Преобразование схем

		private static DbsmSchema ConvertFromSql(DbsmSchema dbsc)
		{
			foreach (var eTable in dbsc.Tables)
			{
				// Columns scan
				for (var i = 0; i < eTable.Columns.Length; i++)
				{
					var eColumn = eTable.Columns[i];
					// type conversion
					switch (eColumn.Type)
					{
						case DbsmColumnType.BlobSubtypeText: // TEXT
						case DbsmColumnType.BlobSubtypeNtext: // NTEXT
							eColumn.Type = DbsmColumnType.BlobSubtype1;
							break;
						case DbsmColumnType.SmallMoney:
							eColumn.Type = DbsmColumnType.Money;
							break;
						case DbsmColumnType.SmallDateTime:
							eColumn.Type = DbsmColumnType.Timestamp;
							break;
		 				case DbsmColumnType.Character:
							eColumn.Type = DbsmColumnType.NCharacter;
							break;
						case DbsmColumnType.CharacterVaring:
							eColumn.Type = DbsmColumnType.NCharacterVaring;
							break;
						case DbsmColumnType.Numeric:
							eColumn.Type = DbsmColumnType.Decimal;
							break;
						case DbsmColumnType.Float:
							if (eColumn.DecimalPrecision < 25)
							{
								eColumn.Type = DbsmColumnType.Real;
								eColumn.DecimalPrecision = 0;
							}
							else
							{
								eColumn.Type = DbsmColumnType.DoublePrecision;
								eColumn.DecimalPrecision = 0;
							}
							break;
					}
				}
			}

			return dbsc;
		}

		#endregion
	}
}
