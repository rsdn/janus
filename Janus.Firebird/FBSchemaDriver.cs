#pragma warning disable 1692
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

using CodeJam;

using FirebirdSql.Data.FirebirdClient;

namespace Rsdn.Janus.Firebird
{
	internal sealed class FBSchemaDriver : DBSchemaDriverBase
	{
		private const string _generatorPrefix = "GEN";

		// !RULE: Foreign Key may be to PK,UC
		// !RULE: Same set of columns cannot be used in more than one PRIMARY KEY and/or UNIQUE constraint definition.

#pragma warning disable 649
		private static readonly bool _fieldNameToUpper;
#pragma warning restore 649

		public override void CreateDatabase(string constr)
		{
			var csb = new FbConnectionStringBuilder(constr) { Pooling = false };

			FbConnection.CreateDatabase(csb.ConnectionString, 16384, false, true);

			using (var con = new FbConnection(csb.ConnectionString))
			using (var cmd = con.CreateCommand())
			{
				con.Open();

				#region bug drug block
				//cmd.CommandText = @"CREATE TABLE crdb (tid INTEGER, name CHAR(120));";
				//cmd.ExecuteScalar();
				//cmd.CommandText = @"DROP TABLE crdb;";
				//cmd.ExecuteScalar();
				#endregion

				#region init actions: register udf functions
				cmd.CommandText =
					@"
					DECLARE EXTERNAL FUNCTION strlen 
						CSTRING(32767)
						RETURNS INTEGER BY VALUE
						ENTRY_POINT 'IB_UDF_strlen' MODULE_NAME 'ib_udf';";
				cmd.ExecuteScalar();
				#endregion
			}
		}

		public override IDbConnection CreateConnection(string connStr)
		{
			return new FbConnection(connStr);
		}

		public override void EndTableLoad(IDbConnection connection, TableSchema table)
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
					cmd.CommandText = @"SELECT MAX(" + MakeDdlElementName(column.Name) +
						@") FROM " + MakeDdlElementName(table.Name);
					cmd.CommandTimeout = 0;

					var maxValue = Convert.ToInt32(cmd.ExecuteScalar());

					if (maxValue <= 0)
						continue;
					var genName = ParseName(_generatorPrefix, table.Name, column.Name);

					var dbsmGenerator = new DBGenerator {Name = genName, StartValue = maxValue};

					cmd.CommandText = MakeDdlGeneratorSet(dbsmGenerator);
					cmd.ExecuteNonQuery();
				}
			}

			base.EndTableLoad(connection, table);
		}

		#region Schema prepare metods

		protected override DBSchema DbscCopyPrepare(DBSchema schema)
		{
			return schema.Copy();
		}

		protected override void AfterSchemaComparision(DBSchema existingSchema, DBSchema targetSchema)
		{
			// Drop Generators
			var i = 1;
			foreach (var eGen in existingSchema.Generators)
			{
				if (!targetSchema.IsGeneratorExist(eGen.Name))
				{
					AddDdlCommand(i,
						eGen.Name,
						"Generator",
						MakeDdlGeneratorDrop(eGen));
				}
				i++;
			}
			// Create Generators
			i = 1;
			foreach (var mGenerator in targetSchema.Generators)
			{
				if (!existingSchema.IsGeneratorExist(mGenerator.Name))
				{
					AddDdlCommand(i,
						mGenerator.Name,
						"Generator",
						MakeDdlGeneratorCreate(mGenerator));

					AddDdlCommand(i,
						mGenerator.Name,
						"Generator",
						MakeDdlGeneratorSet(mGenerator));
				}
				i++;
			}

			foreach (var mTable in targetSchema.Tables)
			{
				var eTable = existingSchema.GetTable(mTable.Name);
				if (eTable != null)
				{
					foreach (var mColumn in mTable.Columns)
					{
						var eColumn = eTable.GetColumn(mColumn.Name);
						if (eColumn == null)
							continue;
						if (mTable.IsExactColumn(eColumn) || eColumn.Type == ColumnType.BlobSubtype1 ||
							eColumn.Type == ColumnType.BinaryLargeObject)
							continue;
						if (mColumn.Type != eColumn.Type)
						{
							AddDdlCommand(i,
								eTable.Name + @"." + eColumn.Name,
								"Column",
								MakeDdlColumnAlter(mColumn, eColumn, eTable));
						}
						if (mColumn.Nullable && !eColumn.Nullable)
						{
							AddDdlCommand(i,
								eTable.Name + @"." + eColumn.Name,
								"Column",
								$@"
											UPDATE RDB$RELATION_FIELDS
											SET
												RDB$NULL_FLAG = NULL
											WHERE
												(RDB$FIELD_NAME = '{eColumn
									.Name}') AND (RDB$RELATION_NAME = '{eTable.Name}')");
						}
						if (mColumn.DefaultValue == eColumn.DefaultValue)
							continue;
						if (mColumn.DefaultValue != null)
						{
							AddDdlCommand(i,
								eTable.Name + @"." + eColumn.Name,
								"Column",
								$@"
												ALTER TABLE {MakeDdlElementName(eTable.Name)}
												ADD IBE$$TEMP_COLUMN {FBSchemaLoader
									.TypeDbsmToFb(eColumn)} DEFAULT {mColumn.DefaultValue}");

							AddDdlCommand(i++,
								eTable.Name + @"." + eColumn.Name,
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
								eTable.Name + @"." + eColumn.Name,
								"Column",
								$@"ALTER TABLE {MakeDdlElementName(eTable.Name)} DROP IBE$$TEMP_COLUMN");
						}
						else
						{
							AddDdlCommand(i,
								eTable.Name + @"." + eColumn.Name,
								"Column",
								$@"
												UPDATE RDB$RELATION_FIELDS F1
												SET
													F1.RDB$DEFAULT_VALUE = NULL,
													F1.RDB$DEFAULT_SOURCE = NULL
												WHERE
													(F1.RDB$RELATION_NAME = '{eTable
									.Name}') AND (F1.RDB$FIELD_NAME = '{eColumn.Name}')");
						}
					}
				}
				i++;
			}
		}

		protected override bool CheckKeyTypeForRecreate(KeySchema eKey)
		{
			return eKey.KeyType == ConstraintType.Unique;
		}

		protected override bool CheckIndexTypeForRecreate(IndexSchema eIndex)
		{
			return false;
		}

		protected override void ExecuteDdlCommands(IEnumerable<string> commands, string connStr)
		{
			using (var con = new FbConnection(connStr))
			{
				con.Open();

				using (var transaction = con.BeginTransaction(FbTransactionOptions.Exclusive))
				{
					foreach (var command in commands)
					{
						var cmd = new FbCommand(command + @";", con, transaction) {CommandTimeout = 0};
						SchemaHelper.SafeRunCommand(command, () => cmd.ExecuteNonQuery());
					}

					transaction.Commit();
				}
			}
		}

		protected override void WriteDdlCommands(TextWriter wr, IEnumerable<string> commands)
		{
			foreach (var command in commands)
				wr.WriteLine(command + @";");
		}

		#endregion

		#region DDL Make Metods

		protected override string MakeDdlElementName(string name)
		{
			return @"""" + name + @"""";
		}

		protected override string MakeDdlColumnDrop(TableColumnSchema column, TableSchema table)
		{
			return $@"ALTER TABLE {MakeDdlElementName(table.Name)} DROP {MakeDdlElementName(column.Name)}";
		}

		protected override string MakeDdlIndexCreate(IndexSchema index, TableSchema table)
		{
			var stat = new StringBuilder();
			if (index.Unique)
				stat.Append(@" UNIQUE");

			switch (index.Sort)
			{
				case SortOrder.Descending:
					stat.Append(@" DESC");
					break;
				case SortOrder.Ascending:
					stat.Append(@" ASC");
					break;
			}

			return
				$@"CREATE {stat} INDEX {MakeDdlElementName(index.Name)} ON {MakeDdlElementName(table.Name)} ({ParseColumnListIndex(
					index.Columns).JoinStrings(@", ")})";
		}

		protected override string MakeDdlIndexDrop(IndexSchema index, TableSchema table)
		{
			return $@"DROP INDEX {MakeDdlElementName(index.Name)}";
		}

		private string MakeDdlGeneratorCreate(SchemaNamedElement gen)
		{
			return $@"CREATE GENERATOR {MakeDdlElementName(gen.Name)}";
		}

		private string MakeDdlGeneratorSet(DBGenerator gen)
		{
			return $@"SET GENERATOR {MakeDdlElementName(gen.Name)} TO {gen.StartValue}";
		}

		private string MakeDdlGeneratorDrop(SchemaNamedElement gen)
		{
			return $@"DROP GENERATOR {MakeDdlElementName(gen.Name)}";
		}

		protected override string ParseColumn(TableColumnSchema column)
		{
			return
				$@"{MakeDdlElementName(column.Name)} {FBSchemaLoader.TypeDbsmToFb(column)} {string.Empty} {(column.DefaultValue
					.IsNullOrEmpty()
					? string.Empty
					: $"DEFAULT {column.DefaultValue}")} {(column.Nullable ? string.Empty : @" NOT NULL")}";
		}

		protected override string ParseColumnAlter(TableColumnSchema mColumn, TableColumnSchema eColumn)
		{
			return $@"{MakeDdlElementName(eColumn.Name)} TYPE {FBSchemaLoader.TypeDbsmToFb(mColumn)}";
		}

		private static string ParseName(string pre, string tname, string oname)
		{
			var colSize = 32 - pre.Length - 2;
			string res;
			if ((oname.Length + tname.Length) > colSize)
			{
				if (oname.Length < colSize)
					res = pre + @"_" + tname.Substring(0, (colSize - 2 - oname.Length)) + @"_" + oname;
				else
					res = pre + oname.Substring(oname.Length - colSize);
			}
			else
				res = pre + @"_" + tname + @"_" + oname;
			return _fieldNameToUpper ? res.ToUpper(CultureInfo.CurrentCulture) : res;
		}
		#endregion

		/// <summary>
		/// Создать схему метаданных из исходной базы
		/// </summary>
		/// <param name="connStr"></param>
		public override DBSchema LoadExistingSchema(string connStr)
		{
			return FBSchemaLoader.LoadSchema(connStr);
		}
	}
}