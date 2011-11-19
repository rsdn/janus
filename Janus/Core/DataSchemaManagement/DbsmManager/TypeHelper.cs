using System;

namespace Rsdn.Janus
{
	internal static class TypeHelper
	{
		#region Преобразование типов MsSql Server <-> Dbsm.Types
		public static DbsmColumnType TypeSqlToDbsm(string typeName)
		{
			switch (typeName.ToUpper(System.Globalization.CultureInfo.CurrentCulture))
			{
				case "BIT": return DbsmColumnType.Boolean;
				case "BIGINT": return DbsmColumnType.BigInt;
				case "BINARY": return DbsmColumnType.Binary;
				case "CHAR": return DbsmColumnType.Character;
				case "CURSOR": return DbsmColumnType.Cursor;
				case "DATETIME": return DbsmColumnType.Timestamp;
				case "DECIMAL": return DbsmColumnType.Decimal;
				case "INT": return DbsmColumnType.Integer;
				case "IMAGE": return DbsmColumnType.BlobSubtypeImage;
				case "FLOAT": return DbsmColumnType.Float;
				case "MONEY": return DbsmColumnType.Money;
				case "NCHAR": return DbsmColumnType.Character;
				case "NVARCHAR": return DbsmColumnType.NCharacterVaring;
				case "NUMERIC": return DbsmColumnType.Numeric;
				case "NTEXT": return DbsmColumnType.BlobSubtypeNtext;
				case "REAL": return DbsmColumnType.Real;
				case "SMALLMONEY": return DbsmColumnType.SmallMoney;
				case "SMALLDATETIME": return DbsmColumnType.SmallDateTime;
				case "SMALLINT": return DbsmColumnType.SmallInt;
				case "SQL_VARIANT": return DbsmColumnType.SqlVariant;
				case "TABLE": return DbsmColumnType.Table;
				case "TEXT": return DbsmColumnType.BlobSubtypeText;
				case "TIMESTAMP": return DbsmColumnType.MsTimestamp;
				case "TINYINT": return DbsmColumnType.TinyInt;
				case "UNIQUEIDENTIFIER": return DbsmColumnType.Guid;
				case "VARBINARY": return DbsmColumnType.BinaryVaring;
				case "VARCHAR": return DbsmColumnType.CharacterVaring;
				case "XML": return DbsmColumnType.Xml;
				default:
					throw new ArgumentException("Unsupported data type for " + DbEngineType.MsSqlDB);
			}
		}

		public static string TypeDbsmToSql(this DbsmColumn eColumn)
		{
			switch (eColumn.Type)
			{
				case DbsmColumnType.Boolean: return "BIT";
				case DbsmColumnType.BigInt: return "BIGINT";
				case DbsmColumnType.Binary: return "BINARY";
				case DbsmColumnType.Character: return "CHAR(" + eColumn.Size + ")";
				case DbsmColumnType.CharacterVaring: return "VARCHAR(" + eColumn.Size + ")";
				case DbsmColumnType.Cursor: return "CURSOR";
				case DbsmColumnType.Timestamp: return "DATETIME";
				case DbsmColumnType.Decimal: return "DECIMAL(" + eColumn.DecimalPrecision + ", " + eColumn.DecimalScale + ")";
				case DbsmColumnType.Integer: return "INT";
				case DbsmColumnType.BlobSubtypeImage: return "IMAGE";
				case DbsmColumnType.Float: return "FLOAT(" + eColumn.DecimalPrecision + ")";
				case DbsmColumnType.Money: return "MONEY";
				case DbsmColumnType.NCharacter: return "NCHAR(" + eColumn.Size + ")";
				case DbsmColumnType.NCharacterVaring: return "NVARCHAR(" + eColumn.Size + ")";
				case DbsmColumnType.Numeric: return "NUMERIC(" + eColumn.DecimalPrecision + ", " + eColumn.DecimalScale + ")";
				case DbsmColumnType.BlobSubtypeNtext: return "NTEXT";
				case DbsmColumnType.Real: return "REAL";
				case DbsmColumnType.SmallMoney: return "SMALLMONEY";
				case DbsmColumnType.SmallDateTime: return "SMALLDATETIME";
				case DbsmColumnType.SmallInt: return "SMALLINT";
				case DbsmColumnType.SqlVariant: return "SQL_VARIANT";
				case DbsmColumnType.Table: return "TABLE";
				case DbsmColumnType.BlobSubtypeText: return "TEXT";
				case DbsmColumnType.MsTimestamp: return "TIMESTAMP";
				case DbsmColumnType.TinyInt: return "TINYINT";
				case DbsmColumnType.Guid: return "UNIQUEIDENTIFIER";
				case DbsmColumnType.BinaryVaring: return "VARBINARY";
				default:
					throw new ArgumentException("Unsupported data type for " + DbEngineType.MsSqlDB);
			}
		}
		#endregion

		#region Преобразование типов Jet ADOX <-> Dbsm.Types
		public static string TypeDbsmToJet(this DbsmColumn dbc)
		{
			switch (dbc.Type)
			{
				case DbsmColumnType.BinaryVaring:
					return "BINARY(" + dbc.Size + ")";
				case DbsmColumnType.Boolean:
					return "BIT";
				case DbsmColumnType.TinyInt:
					return "BYTE";
				case DbsmColumnType.NCharacter:
					return "CHAR(" + dbc.Size + ")";
				case DbsmColumnType.NCharacterVaring:
					return "TEXT(" + dbc.Size + ")";
				case DbsmColumnType.Timestamp:
					return "DATETIME";
				case DbsmColumnType.DoublePrecision:
					return "DOUBLE";
				case DbsmColumnType.Real:
					return "REAL";
				case DbsmColumnType.Guid:
					return "UNIQUEIDENTIFIER";
				case DbsmColumnType.BlobSubtypeImage:
					return "IMAGE";
				case DbsmColumnType.Integer:
					return "INTEGER";
				case DbsmColumnType.BlobSubtype1:
					return "MEMO";
				case DbsmColumnType.Money:
					return "MONEY";
				case DbsmColumnType.Decimal:
					return "DECIMAL(" + dbc.DecimalPrecision + ", " + dbc.DecimalScale + ")";
				case DbsmColumnType.SmallInt:
					return "SMALLINT";
				default:
					throw new ArgumentException("Unsupported data type for " + DbEngineType.JetDB);
			}
		}
		/// <summary>
		/// Преобразование общего типа к access type, только однозначные отношения!!!
		/// </summary>
		/// <param name="dbcType"></param>
		/// <returns></returns>
		public static DbsmColumnType TypeJetToDbsm(this ADOX.DataTypeEnum dbcType)
		{
			switch (dbcType)
			{
				case ADOX.DataTypeEnum.adVarBinary:					// ACCESS - binary; MSSQL - binary();
					return DbsmColumnType.BinaryVaring;
				case ADOX.DataTypeEnum.adBoolean:						// ACCESS - boolean; MSSQL - bit;  ANSI BOOLEAN
					return DbsmColumnType.Boolean;
				case ADOX.DataTypeEnum.adUnsignedTinyInt:		// ACCESS - byte; MSSQL - tinyint;  ANSI <none>
					return DbsmColumnType.TinyInt;
				case ADOX.DataTypeEnum.adWChar:							// ACCESS - char; MSSQL - char; ANSI - CHARACTER
					return DbsmColumnType.NCharacter;
				case ADOX.DataTypeEnum.adDate:							// ACCESS - datetime; MSSQL - datetime; ANSI - TIMESTAMP
					return DbsmColumnType.Timestamp;
				case ADOX.DataTypeEnum.adDouble:						// ACCESS - double; MSSQL - float(53); ANSI - DOUBLEPRECISION, FLOAT(53)
					return DbsmColumnType.DoublePrecision;
				case ADOX.DataTypeEnum.adSingle:						// ACCESS - single; MSSQL - real,float(24); ANSI - REAL, FLOAT(24)
					return DbsmColumnType.Real;
				case ADOX.DataTypeEnum.adGUID:							// ACCESS - guid; MSSQL - uniqueidentifier; ANSI - <none>
					return DbsmColumnType.Guid;
				case ADOX.DataTypeEnum.adLongVarBinary:			// ACCESS - image(OLE object); MSSQL - images; ANSI - BLOB SUB_TYPE -1
					return DbsmColumnType.BlobSubtypeImage;
				case ADOX.DataTypeEnum.adInteger:						// ACCESS - integer; MSSQL - int; ANSI - Integer
					return DbsmColumnType.Integer;
				case ADOX.DataTypeEnum.adLongVarWChar:			// ACCESS - memo; MSSQL - ntext; ANSI - BLOB SIB_TYPE 1
					return DbsmColumnType.BlobSubtype1;
				case ADOX.DataTypeEnum.adCurrency:					// ACCESS - money; MSSQL - money; ANSI - 
					return DbsmColumnType.Money;
				case ADOX.DataTypeEnum.adNumeric:						// ACCESS - numeric; MSSQL - decimal(p,s),numeric(p,s); ANSI - DECIMAL(p,s)
					return DbsmColumnType.Decimal;
				case ADOX.DataTypeEnum.adVarWChar:					// ACCESS - char(n); MSSQL - char(n); ANSI - CHARACTER(n)
					return DbsmColumnType.NCharacterVaring;
				case ADOX.DataTypeEnum.adSmallInt:
					return DbsmColumnType.SmallInt;
				#region Unused types
				//				case ADOX.DataTypeEnum.adBinary:
				//					return DbsmType.BinaryLargeObject;
				//				case ADOX.DataTypeEnum.adBigInt:
				//					return DbsmType.BigInt;
				//				case ADOX.DataTypeEnum.adBSTR:
				//					return DbsmType.CharacterVaring;
				//				case ADOX.DataTypeEnum.adChapter:
				//					return DbsmType.BigInt;
				//				case ADOX.DataTypeEnum.adChar:
				//					return DbsmType.Character;
				//				case ADOX.DataTypeEnum.adDBDate:
				//					return DbsmType.Date;
				//				case ADOX.DataTypeEnum.adDBTime:
				//					return DbsmType.Time;
				//				case ADOX.DataTypeEnum.adDBTimeStamp:
				//					return DbsmType.TimeStamp;
				//				case ADOX.DataTypeEnum.adDecimal:
				//					return DbsmType.Decimal;
				//				case ADOX.DataTypeEnum.adError:
				//					return DbsmType.Integer;
				//				case ADOX.DataTypeEnum.adFileTime:
				//					return DbsmType.BigInt;
				//				case ADOX.DataTypeEnum.adIDispatch:
				//					return DbsmType.Integer;
				//				case ADOX.DataTypeEnum.adIUnknown:
				//					return DbsmType.Integer;
				//				case ADOX.DataTypeEnum.adLongVarChar:
				//					return DbsmType.BinaryLargeObject;
				//				case ADOX.DataTypeEnum.adNumeric:
				//					return DbsmType.Numeric;
				//				case ADOX.DataTypeEnum.adPropVariant:
				//					return DbsmType.BigInt;
				//				case ADOX.DataTypeEnum.adTinyInt:
				//					return DbsmType.Character;
				//				case ADOX.DataTypeEnum.adUnsignedBigInt:
				//					return DbsmType.BigInt;
				//				case ADOX.DataTypeEnum.adUnsignedInt:
				//					return DbsmType.Integer;
				//				case ADOX.DataTypeEnum.adUnsignedSmallInt:
				//					return DbsmType.SmallInt;
				//				case ADOX.DataTypeEnum.adUserDefined:
				//					return DbsmType.Integer;
				//				case ADOX.DataTypeEnum.adVarChar:
				//					return DbsmType.CharacterVaring;
				//				case ADOX.DataTypeEnum.adVariant:
				//					return DbsmType.BigInt;
				//				case ADOX.DataTypeEnum.adVarNumeric:
				//					return DbsmType.Numeric;
				//				case ADOX.DataTypeEnum.adEmpty:
				//					return DbsmType.Empty;

				#endregion
				default:
					throw new ArgumentException("Unsupported data type for " + DbEngineType.JetDB);
			}
		}
		#endregion

		#region Преобразование типов Firebird <-> Dbsm.Types
		public static DbsmColumnType TypeFbToDbsm(string typeName)
		{
			switch (typeName.ToUpper(System.Globalization.CultureInfo.CurrentCulture))
			{
				case "ARRAY":
					return DbsmColumnType.Array;
				case "BLOB":
					return DbsmColumnType.BinaryLargeObject;
				case "BLOB SUB_TYPE 1":
					return DbsmColumnType.BlobSubtype1;
				case "CHAR":
					return DbsmColumnType.Character;
				case "VARCHAR":
					return DbsmColumnType.CharacterVaring;
				case "SMALLINT":
					return DbsmColumnType.SmallInt;
				case "INTEGER":
					return DbsmColumnType.Integer;
				case "FLOAT":
					return DbsmColumnType.Float;
				case "DOUBLE PRECISION":
					return DbsmColumnType.DoublePrecision;
				case "BIGINT":
					return DbsmColumnType.BigInt;
				case "NUMERIC":
					return DbsmColumnType.Numeric;
				case "DECIMAL":
					return DbsmColumnType.Decimal;
				case "DATE":
					return DbsmColumnType.Date;
				case "TIME":
					return DbsmColumnType.Time;
				case "TIMESTAMP":
					return DbsmColumnType.Timestamp;
				default:
					throw new ArgumentException("Unsupported data type for " + DbEngineType.FireBirdDB);
			}
		}

		/// <summary>
		/// Преобразование общего типа к firebird type, только однозначные отношения!!!
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public static string TypeDbsmToFb(this DbsmColumn column)
		{
			switch (column.Type)
			{
				case DbsmColumnType.Array:
					return "ARRAY";
				case DbsmColumnType.BinaryLargeObject:
					return "BLOB";
				case DbsmColumnType.BlobSubtype1:
					return "BLOB SUB_TYPE 1";
				case DbsmColumnType.Character:
					return "CHAR(" + column.Size + ")";
				case DbsmColumnType.CharacterVaring:
					return "VARCHAR(" + column.Size + ")";
				case DbsmColumnType.SmallInt:
					return "SMALLINT";
				case DbsmColumnType.Integer:
					return "INTEGER";
				case DbsmColumnType.Float:
					return "FLOAT";
				case DbsmColumnType.DoublePrecision:
					return "DOUBLE PRECISION";
				case DbsmColumnType.BigInt:
					return "BIGINT";
				case DbsmColumnType.Numeric:
					return "NUMERIC(" + column.DecimalPrecision + ", " + column.DecimalScale + ")";
				case DbsmColumnType.Decimal:
					return "DECIMAL(" + column.DecimalPrecision + ", " + column.DecimalScale + ")";
				case DbsmColumnType.Date:
					return "DATE";
				case DbsmColumnType.Time:
					return "TIME";
				case DbsmColumnType.Timestamp:
					return "TIMESTAMP";
				default:
					throw new ArgumentException("Unsupported data type for " + DbEngineType.FireBirdDB);
			}
		}
		#endregion

		#region Преобразование типов SQLite <-> Dbsm.Types
		public static DbsmColumnType TypeSqliteToDbsm(string typeName)
		{
			var typeNameParts = typeName.ToUpper(System.Globalization.CultureInfo.CurrentCulture).Split(' ');
			
			switch (typeNameParts[0])
			{
				case "BIT": return DbsmColumnType.Boolean;
				case "BIGINT": return DbsmColumnType.BigInt;
				case "BINARY": return DbsmColumnType.Binary;
				case "CHAR": return DbsmColumnType.Character;
				case "CURSOR": return DbsmColumnType.Cursor;
				case "DATETIME": return DbsmColumnType.Timestamp;
				case "DECIMAL": return DbsmColumnType.Decimal;
				case "INT": return DbsmColumnType.Integer;
				case "INTEGER": return DbsmColumnType.Integer;
				case "IMAGE": return DbsmColumnType.BlobSubtypeImage;
				case "FLOAT": return DbsmColumnType.Float;
				case "MONEY": return DbsmColumnType.Money;
				case "NCHAR": return DbsmColumnType.Character;
				case "NVARCHAR": return DbsmColumnType.NCharacterVaring;
				case "NUMERIC": return DbsmColumnType.Numeric;
				case "NTEXT": return DbsmColumnType.BlobSubtypeNtext;
				case "REAL": return DbsmColumnType.Real;
				case "SMALLMONEY": return DbsmColumnType.SmallMoney;
				case "SMALLDATETIME": return DbsmColumnType.SmallDateTime;
				case "SMALLINT": return DbsmColumnType.SmallInt;
				case "SQL_VARIANT": return DbsmColumnType.SqlVariant;
				case "TABLE": return DbsmColumnType.Table;
				case "TEXT": return DbsmColumnType.BlobSubtypeText;
				case "TIMESTAMP": return DbsmColumnType.MsTimestamp;
				case "TINYINT": return DbsmColumnType.TinyInt;
				case "UNIQUEIDENTIFIER": return DbsmColumnType.Guid;
				case "VARBINARY": return DbsmColumnType.BinaryVaring;
				case "VARCHAR": return DbsmColumnType.CharacterVaring;
				case "XML": return DbsmColumnType.Xml;
				default:
					throw new NotSupportedException("Data type '" + typeName + "' is not supported by " + DbEngineType.SQLiteDB);
			}
		}

		public static string TypeDbsmToSqlite(this DbsmColumn eColumn)
		{
			switch (eColumn.Type)
			{
				case DbsmColumnType.Boolean: return "BIT";
				case DbsmColumnType.BigInt: return "BIGINT";
				case DbsmColumnType.Binary: return "BINARY";
				case DbsmColumnType.Character: return "CHAR(" + eColumn.Size + ")";
				case DbsmColumnType.CharacterVaring: return "VARCHAR(" + eColumn.Size + ")";
				case DbsmColumnType.Cursor: return "CURSOR";
				case DbsmColumnType.Timestamp: return "DATETIME";
				case DbsmColumnType.Decimal: return "DECIMAL(" + eColumn.DecimalPrecision + ", " + eColumn.DecimalScale + ")";
				case DbsmColumnType.Integer: return "INTEGER";
				case DbsmColumnType.BlobSubtypeImage: return "IMAGE";
				case DbsmColumnType.Float: return "FLOAT(" + eColumn.DecimalPrecision + ")";
				case DbsmColumnType.Money: return "MONEY";
				case DbsmColumnType.NCharacter: return "NCHAR(" + eColumn.Size + ")";
				case DbsmColumnType.NCharacterVaring: return "NVARCHAR(" + eColumn.Size + ")";
				case DbsmColumnType.Numeric: return "NUMERIC(" + eColumn.DecimalPrecision + ", " + eColumn.DecimalScale + ")";
				case DbsmColumnType.BlobSubtypeNtext: return "NTEXT";
				case DbsmColumnType.Real: return "REAL";
				case DbsmColumnType.SmallMoney: return "SMALLMONEY";
				case DbsmColumnType.SmallDateTime: return "SMALLDATETIME";
				case DbsmColumnType.SmallInt: return "SMALLINT";
				case DbsmColumnType.SqlVariant: return "SQL_VARIANT";
				case DbsmColumnType.Table: return "TABLE";
				case DbsmColumnType.BlobSubtypeText: return "TEXT";
				case DbsmColumnType.MsTimestamp: return "TIMESTAMP";
				case DbsmColumnType.TinyInt: return "TINYINT";
				case DbsmColumnType.Guid: return "UNIQUEIDENTIFIER";
				case DbsmColumnType.BinaryVaring: return "VARBINARY";
				default:
					throw new NotSupportedException("Data type '" + eColumn.Type + "' is not supported by " + DbEngineType.SQLiteDB);
			}
		}
		#endregion
	}
}
