#pragma warning disable 1692
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

using FirebirdSql.Data.FirebirdClient;
using Rsdn.Janus.Framework;

using IServiceProvider=Rsdn.SmartApp.IServiceProvider;

namespace Rsdn.Janus
{
	internal sealed class FBSchemaDriver : DBSchemaDriverBase
	{
		private const string GENERATOR_PREFIX = "GEN";

		// !RULE: Foreign Key may be to PK,UC
		// !RULE: Same set of columns cannot be used in more than one PRIMARY KEY and/or UNIQUE constraint definition.

#pragma warning disable ConvertToConstant
		private static readonly bool _fieldNameToUpper = false;
#pragma warning restore ConvertToConstant

		public FBSchemaDriver(IServiceProvider serviceProvider, string constr)
			: base(serviceProvider, FbSupportModule.DriverName, constr)
		{ }

		public override IDbConnection CreateConnection()
		{
			return new FbConnection(ConnectionString);
		}

		public override void EndTableLoad(IDbConnection connection, DbsmTable table)
		{
			foreach (var column in table.Columns)
			{
				// Update sequence to make sure that next generated value
				// will not cause a constraint violation.
				//
				if (!column.AutoIncrement)
					continue;
				using (var cmd = connection.CreateCommand())
				{
					cmd.CommandText = "SELECT MAX(" + MakeDDLElementName(column.Name) +
						") FROM " + MakeDDLElementName(table.Name);
					cmd.CommandTimeout = 0;

					var maxValue = Convert.ToInt32(cmd.ExecuteScalar());

					if (maxValue <= 0)
						continue;
					var genName = ParseName(GENERATOR_PREFIX, table.Name, column.Name);

					var dbsmGenerator = new DbsmGenerator {Name = genName, StartValue = maxValue};

					cmd.CommandText = MakeDDLGeneratorSet(dbsmGenerator);
					cmd.ExecuteNonQuery();
				}
			}

			base.EndTableLoad(connection, table);
		}

		#region Dbsc prepare metods

		protected override DbsmSchema DbscCopyPrepare(DbsmSchema schema)
		{
			var newSchema = schema.Copy();
			switch (schema.DbEngine)
			{
				case DbEngineType.JetDB:
				case DbEngineType.MsSqlDB:
				case DbEngineType.SQLiteDB:
					return ConvertFromSql(newSchema);
				case DbEngineType.FireBirdDB:
					return newSchema;
			}

			throw new NotSupportedException("Engine type '" + schema.DbEngine + "' is not supported");
		}

		protected override void CompareDbscPost(DbsmSchema mDbsc)
		{
			var eDbsc = _dbsc;
			// Drop Generators
			var i = 1;
			foreach (var eGen in eDbsc.Generators)
			{
				if (!mDbsc.IsGeneratorExist(eGen.Name))
				{
					AddDdlCommand(i,
						eGen.Name,
						"Generator",
						MakeDDLGeneratorDrop(eGen));
				}
				i++;
			}
			// Create Generators
			i = 1;
			foreach (var mGenerator in mDbsc.Generators)
			{
				if (!eDbsc.IsGeneratorExist(mGenerator.Name))
				{
					AddDdlCommand(i,
						mGenerator.Name,
						"Generator",
						MakeDDLGeneratorCreate(mGenerator));

					AddDdlCommand(i,
						mGenerator.Name,
						"Generator",
						MakeDDLGeneratorSet(mGenerator));
				}
				i++;
			}

			foreach (var mTable in mDbsc.Tables)
			{
				var eTable = eDbsc.GetTable(mTable.Name);
				if (eTable != null)
				{
					foreach (var mColumn in mTable.Columns)
					{
						var eColumn = eTable.GetColumn(mColumn.Name);
						if (eColumn == null)
							continue;
						if (mTable.IsExactColumn(eColumn) || eColumn.Type == DbsmColumnType.BlobSubtype1 ||
								eColumn.Type == DbsmColumnType.BinaryLargeObject)
							continue;
						if (mColumn.Type != eColumn.Type)
						{
							AddDdlCommand(i,
								eTable.Name + "." + eColumn.Name,
								"Column",
								MakeDDLColumnAlter(mColumn, eColumn, eTable));
						}
						if (mColumn.Nullable && !eColumn.Nullable)
						{
							AddDdlCommand(i,
								eTable.Name + "." + eColumn.Name,
								"Column",
								string.Format(@"
											UPDATE RDB$RELATION_FIELDS
											SET
												RDB$NULL_FLAG = NULL
											WHERE
												(RDB$FIELD_NAME = '{0}') AND (RDB$RELATION_NAME = '{1}')",
									eColumn.Name,
									eTable.Name));
						}
						if (mColumn.DefaultValue == eColumn.DefaultValue)
							continue;
						if (mColumn.DefaultValue != null)
						{
							AddDdlCommand(i,
								eTable.Name + "." + eColumn.Name,
								"Column",
								string.Format(@"
												ALTER TABLE {0}
												ADD IBE$$TEMP_COLUMN {1} {2}",
									MakeDDLElementName(eTable.Name),
									eColumn.TypeDbsmToFb(),
									mColumn.DefaultValue));

							AddDdlCommand(i++,
								eTable.Name + "." + eColumn.Name,
								"Column", 
								string.Format(@"
												UPDATE RDB$RELATION_FIELDS F1
												SET
													F1.RDB$DEFAULT_VALUE =
														(SELECT
															F2.RDB$DEFAULT_VALUE
														FROM
															RDB$RELATION_FIELDS F2
														WHERE
															(F2.RDB$RELATION_NAME = '{0}') AND (F2.RDB$FIELD_NAME = 'IBE$$TEMP_COLUMN')),
													F1.RDB$DEFAULT_SOURCE =
														(SELECT
															F3.RDB$DEFAULT_SOURCE
														FROM
															RDB$RELATION_FIELDS F3
														WHERE
															(F3.RDB$RELATION_NAME = '{0}') AND (F3.RDB$FIELD_NAME = 'IBE$$TEMP_COLUMN'))
												WHERE
													(F1.RDB$RELATION_NAME = '{0}') AND (F1.RDB$FIELD_NAME = '{0}')",
									eTable.Name));

							AddDdlCommand(i++,
								eTable.Name + "." + eColumn.Name,
								"Column",
								string.Format(@"ALTER TABLE {0} DROP IBE$$TEMP_COLUMN",
									MakeDDLElementName(eTable.Name)));
						}
						else
						{
							AddDdlCommand(i,
								eTable.Name + "." + eColumn.Name,
								"Column",
								string.Format(@"
												UPDATE RDB$RELATION_FIELDS F1
												SET
													F1.RDB$DEFAULT_VALUE = NULL,
													F1.RDB$DEFAULT_SOURCE = NULL
												WHERE
													(F1.RDB$RELATION_NAME = '{0}') AND (F1.RDB$FIELD_NAME = '{1}')",
									eTable.Name,
									eColumn.Name));
						}
					}
				}
				i++;
			}
		}

		protected override bool CheckKeyTypeForRecreate(DbsmKey eKey)
		{
			return eKey.KeyType == DbsmConstraintType.Unique;
		}

		protected override bool CheckIndexTypeForRecreate(DbsmIndex eIndex)
		{
			return false;
		}

		protected override void ExecuteDdlCommands(IEnumerable<string> commands)
		{
			using (var con = new FbConnection(ConnectionString))
			{
				con.Open();

				using (var transaction = con.BeginTransaction(FbTransactionOptions.Exclusive))
				{
					foreach (var command in commands)
					{
						var cmd = new FbCommand(command + ";", con, transaction) {CommandTimeout = 0};
						cmd.ExecuteNonQuery();
					}

					transaction.Commit();
				}
			}
		}

		protected override void WriteDdlCommands(TextWriter wr, IEnumerable<string> commands)
		{
			foreach (var command in commands)
				wr.WriteLine(command + ";");
		}

		#endregion

		#region DDL Make Metods

		protected override string MakeDDLElementName(string name)
		{
			return "\"" + name + "\"";
		}

		protected override string MakeDDLColumnDrop(DbsmColumn column, DbsmTable table)
		{
			return string.Format("ALTER TABLE {0} DROP {1}",
				MakeDDLElementName(table.Name), MakeDDLElementName(column.Name));
		}

		protected override string MakeDDLIndexCreate(DbsmIndex index, DbsmTable table)
		{
			var stat = new StringBuilder();
			if (index.Unique)
				stat.Append(" UNIQUE");

			switch (index.Sort)
			{
				case DbsmSortOrder.SortDescending:
					stat.Append(" DESC");
					break;
				case DbsmSortOrder.SortAscending:
					stat.Append(" ASC");
					break;
			}

			return string.Format("CREATE {0} INDEX {1} ON {2} ({3})",
				stat,
				MakeDDLElementName(index.Name),
				MakeDDLElementName(table.Name),
				Algorithms.ToString(ParseColumnListIndex(index.Columns), ", "));
		}

		protected override string MakeDDLIndexDrop(DbsmIndex index, DbsmTable table)
		{
			return string.Format("DROP INDEX {0}",
				MakeDDLElementName(index.Name));
		}

		private string MakeDDLGeneratorCreate(DbsmNamedElement gen)
		{
			return string.Format("CREATE GENERATOR {0}",
				MakeDDLElementName(gen.Name));
		}

		private string MakeDDLGeneratorSet(DbsmGenerator gen)
		{
			return string.Format("SET GENERATOR {0} TO {1}",
				MakeDDLElementName(gen.Name),
				gen.StartValue);
		}

		private string MakeDDLGeneratorDrop(DbsmNamedElement gen)
		{
			return string.Format("DROP GENERATOR {0}",
				MakeDDLElementName(gen.Name));
		}

		protected override string ParseColumn(DbsmColumn column)
		{
			return string.Format("{0} {1} {2} {3} {4}",
				MakeDDLElementName(column.Name),
				column.TypeDbsmToFb(),
				string.Empty, // column.computedBy == null ? string.Empty : ("COMPUTED BY " + column.computedBy),
				column.DefaultValue ?? string.Empty,
				column.Nullable ? string.Empty : " NOT NULL");
		}

		protected override string ParseColumnAlter(DbsmColumn mColumn, DbsmColumn eColumn)
		{
			return string.Format("{0} TYPE {1}",
				MakeDDLElementName(eColumn.Name),
				mColumn.TypeDbsmToFb());
		}

		private static string ParseName(string tname, string oname)
		{
			var colSize = 32 - 1;
			string res;
			if ((oname.Length + tname.Length) > colSize)
			{
				if (oname.Length < colSize)
					res = tname.Substring(0, (colSize - 1 - oname.Length)) + "_" + oname;
				else
					res = oname.Substring(oname.Length - colSize);
			}
			else
				res = tname + "_" + oname;
			return _fieldNameToUpper ? res.ToUpper(CultureInfo.CurrentCulture) : res;
		}

		private static string ParseName(string pre, string tname, string oname)
		{
			var colSize = 32 - pre.Length - 2;
			string res;
			if ((oname.Length + tname.Length) > colSize)
			{
				if (oname.Length < colSize)
					res = pre + "_" + tname.Substring(0, (colSize - 2 - oname.Length)) + "_" + oname;
				else
					res = pre + oname.Substring(oname.Length - colSize);
			}
			else
				res = pre + "_" + tname + "_" + oname;
			return _fieldNameToUpper ? res.ToUpper(CultureInfo.CurrentCulture) : res;
		}
		#endregion

		#region Пребразование схем

		/// <summary>
		/// Конвертация MsSql, Jet схем баз к FireBird-овской
		/// </summary>
		/// <param name="dbsc">Глубокая копия конвертируемой схемы</param>
		/// <returns></returns>
		private DbsmSchema ConvertFromSql(DbsmSchema dbsc)
		{
			// PROBLEMS
			// - CHECK constraint syntax
			// - Many Object name in Firebird have len limit - table.name:32, key.name:32, index.name:32
			using (var con = new FbConnection(ConnectionString))
			{
				con.Open();

				var aStore = new List<DbsmKey>();

				foreach (var table in dbsc.Tables)
				{
					aStore.Clear();
					if (_fieldNameToUpper)
						table.Name = table.Name.ToUpper(CultureInfo.CurrentCulture);
					for (var i = 0; i < table.Columns.Length; i++)
					{
						var eColumn = table.Columns[i];
						if (_fieldNameToUpper)
							eColumn.Name = eColumn.Name.ToUpper(CultureInfo.CurrentCulture);
						if (eColumn.AutoIncrement)
						{
							var genName = ParseName(GENERATOR_PREFIX, table.Name, eColumn.Name);
							//cmd.CommandText = @"SELECT Count(*) FROM rdb$generators WHERE rdb$generator_name = '" + genName + "'";
							//int cnt = (int)cmd.ExecuteScalar();

							var dbsmGenerator = new DbsmGenerator {Name = genName, StartValue = 0};
							dbsc.Generators.Add(dbsmGenerator);
						}
						// foreign type conversion must be in transform dbsc metod!
						switch (eColumn.Type)
						{
							case DbsmColumnType.SmallDateTime:
								eColumn.Type = DbsmColumnType.Timestamp;
								break;
							case DbsmColumnType.TinyInt:
							case DbsmColumnType.Boolean:
								eColumn.Type = DbsmColumnType.SmallInt;
								break;
							case DbsmColumnType.NCharacterVaring:
								eColumn.Type = DbsmColumnType.CharacterVaring;
								break;
							case DbsmColumnType.NCharacter:
								eColumn.Type = DbsmColumnType.Character;
								break;
							case DbsmColumnType.BlobSubtypeImage:
								eColumn.Type = DbsmColumnType.BinaryLargeObject;
								break;
							case DbsmColumnType.BlobSubtypeText:
							case DbsmColumnType.BlobSubtypeNtext:
								eColumn.Type = DbsmColumnType.BlobSubtype1;
								break;
							case DbsmColumnType.Real:
								eColumn.Type = DbsmColumnType.Float;
								eColumn.DecimalPrecision = 0;
								break;
							case DbsmColumnType.Float:
								if (eColumn.DecimalPrecision < 25)
								{
									eColumn.Type = DbsmColumnType.Float;
									eColumn.DecimalPrecision = 0;
								}
								else
								{
									eColumn.Type = DbsmColumnType.DoublePrecision;
									eColumn.DecimalPrecision = 0;
								}
								break;
							case DbsmColumnType.Money:
								eColumn.Type = DbsmColumnType.Numeric;
								eColumn.DecimalPrecision = 19;
								eColumn.DecimalScale = 4;
								break;
						}
						if (eColumn.DefaultValue != null)
							switch (eColumn.DefaultValue.ToLower(CultureInfo.CurrentCulture))
							{
								case "getdate()":
									eColumn.DefaultValue = "DEFAULT 'NOW'";
									break;
								default:
									eColumn.DefaultValue = "DEFAULT " + UnBracket.ParseUnBracket(eColumn.DefaultValue);
									break;
							}
					}
					for (var i = 0; i < table.Indexes.Length; i++)
					{
						// !RULE - in Firebird char size for indexed column cannot be > 252!
						var index = table.Indexes[i];
						var casd = 0;
						index.Columns = ParseColumnListIndexClear(index.Columns);
						if (_fieldNameToUpper)
							index.Columns = index.Columns.ToUpper(CultureInfo.CurrentCulture);
						index.Name = ParseName(table.Name, index.Name);
						if (_fieldNameToUpper)
							index.Name = index.Name.ToUpper(CultureInfo.CurrentCulture);
						index.Sort = casd >= 0 ? DbsmSortOrder.SortAscending : DbsmSortOrder.SortDescending;
					}
					for (var i = 0; i < table.Keys.Length; i++)
					{
						var key = table.Keys[i];
						key.Name = ParseName(table.Name, key.Name);
						if (_fieldNameToUpper)
							key.Name = key.Name.ToUpper(CultureInfo.CurrentCulture);
						key.Columns = ParseColumnListIndexClear(key.Columns);
						if (_fieldNameToUpper)
							key.Columns = key.Columns.ToUpper(CultureInfo.CurrentCulture);
						if (key.KeyType == DbsmConstraintType.KeyForeign)
						{
							if (_fieldNameToUpper)
								key.RelTable = key.RelTable.ToUpper(CultureInfo.CurrentCulture);
							key.RelColumns = ParseColumnListIndexClear(key.RelColumns);
							if (_fieldNameToUpper)
								key.RelColumns = key.RelColumns.ToUpper(CultureInfo.CurrentCulture);
						}
						if (key.KeyType == DbsmConstraintType.KeyPrimary)
						{
							foreach(var name in ParseColumnList(key.Columns))
							{
								var tColumn = table.GetColumn(name);
								if (tColumn != null)
									tColumn.Nullable = false;
							}
						}
						aStore.Add(key);
					}
					for (var i = aStore.Count - 1; i >= 0; i--)
					{
						var eKey = aStore[i];
						if (eKey.KeyType == DbsmConstraintType.Unique && table.IsKeyExist(DbsmConstraintType.KeyPrimary, eKey.Columns))
							aStore.RemoveAt(i);
					}
					table.Keys = new DbsmKey[aStore.Count];
					table.Keys = aStore.ToArray();
				}
			}

			return dbsc;
		}

		#endregion
	}
}
