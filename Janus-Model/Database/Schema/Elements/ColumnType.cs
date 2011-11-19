namespace Rsdn.Janus
{
	public enum ColumnType
	{
		// ANSI SQL-2003 Types:
		// CHARACTER, CHARACTER VARYING, CHARACTER LARGE OBJECT, BINARY LARGE OBJECT,
		// NUMERIC, DECIMAL, SMALLINT,
		// INTEGER, BIGINT, FLOAT, REAL, DOUBLE PRECISION, BOOLEAN, DATE, TIME,
		// TIMESTAMP, INTERVAL
		Character,
		CharacterVaring,
		CharacterLargeObject,
		NCharacter,
		NCharacterVaring,
		NCharacterLargeObject,
		BinaryLargeObject,
		Numeric,
		Decimal,
		SmallInt, // 16 bit
		Integer, // 32 bit
		BigInt, // 64 bit
		Float,
		Real, //REAL specifies the data type approximate numeric, with implementation-defined precision.
		DoublePrecision,
		//DOUBLE PRECISION specifies the data type approximate numeric, with implementation-defined precision
		//	that is greater than the implementation-defined precision of REAL.
		Boolean,
		Date,
		Time, // comprises values of the datetime fields HOUR, MINUTE and SECOND.
		Timestamp,
		// comprises values of the datetime fields YEAR (between 0001 and 9999), MONTH, DAY, HOUR, MINUTE and SECOND.
		Interval,
		//
		Unknown,
		// None ANSI Types
		Money,
		Binary,
		BinaryVaring,
		TinyInt, // 8 bit
		Guid,
		//Ntext,
		// --- ACCESS specific Types
		// Memo
		// Money -> [decimal(19,4)]
		// --- MsSql Server Data types
		// bigint int smallint tinyint
		// bit
		// decimal numeric
		// money -> [decimal(19,4)]  smallmoney -> [decimal(8,4)]
		// float real
		// datetime				- 8 bytes.Date and time data from January 1, 1753 through December 31, 9999, to an accuracy of one three-hundredth of a second 
		//								(equivalent to 3.33 milliseconds or 0.00333 seconds). 
		// smalldatetime	- 4 bytes.Date and time data from January 1, 1900, through June 6, 2079, with accuracy to the minute. 
		//									smalldatetime values with 29.998 seconds or lower are rounded down to the nearest minute; 
		//									values with 29.999 seconds or higher are rounded up to the nearest minute.
		// timestamp			- 8 bytes.
		// char varchar text
		// nchar nvarchar ntext
		// binary varbinary image
		// cursor sql_variant table uniqueidentifier
		// --- MsSql Server specific Types
		//DateTime,
		MsTimestamp,
		SqlVariant,
		SmallMoney,
		SmallDateTime,
		Cursor,
		Table,
		Xml,
		// ---- Firebird Data Types
		// INTEGER - 32 bits; –2,147,483,648 to 2,147,483,647; Signed long (longword)
		// SMALLINT - 16 bits; –32,768 to 32,767; Signed short (word)
		// FLOAT - 32 bits 1.175 x 10–38 to 3.402 x 1038 IEEE single precision: 7 digits
		// DOUBLE PRECISION - 64 bits 2.225 x 10–308 to 1.797 x 10308 IEEE double precision: 15 digits
		// NUMERIC and DECIMAL
		// DATE - 64 bits 1 Jan 100 a.d. to 29 Feb 32768 a.d.
		// TIME - 64 bits 0:00 AM-23:59.9999 PM
		// TIMESTAMP - 64 bits 1 Jan 100 a.d. to 29 Feb 32768 a.d. Also includes time information
		// CHARACTER and VARYING CHARACTER [CHAR(n), VARCHAR(n), NCHAR(n),NVARCHAR(n)]
		// BLOB
		// --- Firebird specific types
		Array,
		// --BLOB Subtypes
		// 0 Unstructured, generally applied to binary data or data of an indeterminate type
		// 1 Text
		// 2 Binary language representation (BLR)
		// 3 Access control list
		// 4 (Reserved for future use)
		// 5 Encoded description of a table’s current metadata
		// 6 Description of multi-database transaction that finished irregularly
		BlobSubtypeImage, // image
		BlobSubtypeText, // text
		BlobSubtypeNText, // ntext
		BlobSubtype1, // 
		BlobSubtype2
	}
}