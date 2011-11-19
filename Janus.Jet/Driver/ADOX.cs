using System.Runtime.InteropServices;

namespace ADOX
{
	[ComImport, Guid("00000602-0000-0010-8000-00AA006D2EA4")]
	public class CatalogClass
	{
	}

	[ComImport, Guid("00000603-0000-0010-8000-00AA006D2EA4")]
	public interface Catalog
	{
		Tables Tables { get; }
		object ActiveConnection { get; set; }
		void let_ActiveConnection(object pVal);
		object Procedures { get; }
		object Views { get; }
		object Groups { get; }
		object Users { get; }
		object Create(string ConnectString);
		string GetObjectOwner(string ObjectName, ObjectTypeEnum ObjectType, object ObjectTypeId);
		void SetObjectOwner(string ObjectName, ObjectTypeEnum ObjectType, string UserName, object ObjectTypeId);
	}

	[ComImport, Guid("00000610-0000-0010-8000-00AA006D2EA4")]
	public interface Table
	{
		Columns Columns { get; }
		string Name { get; set; }
		string Type { get; }
		Indexes Indexes { get; }
		Keys Keys { get; }
		Properties Properties { get; }
		object DateCreated { get; }
		object DateModified { get; }
		Catalog ParentCatalog { get; set; }
	}

	[ComImport, Guid("00000611-0000-0010-8000-00AA006D2EA4")]
	public interface Tables
	{
		int Count { get; }
		object GetEnumerator();
		void Refresh();
		Table this[object Item] { get; }
	}

	[ComImport, Guid("0000061C-0000-0010-8000-00AA006D2EA4")]
	public interface Column
	{
		string Name { get; set; }
		ColumnAttributesEnum Attributes { get; set; }
		int DefinedSize { get; set; }
		byte NumericScale { get; set; }
		int Precision { get; set; }
		string RelatedColumn { get; set; }
		SortOrderEnum SortOrder { get; set; }
		DataTypeEnum Type { get; set; }
		Properties Properties { get; }
		Catalog ParentCatalog { get; set; }
	}

	[ComImport, Guid("0000061D-0000-0010-8000-00AA006D2EA4")]
	public interface Columns
	{
		int Count { get; }
		object GetEnumerator();
		void Refresh();
		Column this[object Item] { get; }
	}


	[ComImport, Guid("0000061F-0000-0010-8000-00AA006D2EA4")]
	public interface Index
	{
		string Name { get; set; }
		bool Clustered { get; set; }
		AllowNullsEnum IndexNulls { get; set; }
		bool PrimaryKey { get; set; }
		bool Unique { get; set; }
		Columns Columns { get; }
		Properties Properties { get; }
	}

	[ComImport, Guid("00000620-0000-0010-8000-00AA006D2EA4")]
	public interface Indexes
	{
		int Count { get; }
		object GetEnumerator();
		void Refresh();
		Index this[object Item] { get; }
	}


	[ComImport, Guid("00000622-0000-0010-8000-00AA006D2EA4")]
	public interface Key
	{
		string Name { get; set; }
		RuleEnum DeleteRule { get; set; }
		KeyTypeEnum Type { get; set; }
		string RelatedTable { get; set; }
		RuleEnum UpdateRule { get; set; }
		Columns Columns { get; }
	}

	[ComImport, Guid("00000623-0000-0010-8000-00AA006D2EA4")]
	public interface Keys
	{
		int Count { get; }
		object GetEnumerator();
		void Refresh();
		Key this[object Item] { get; }
	}

	[ComImport, Guid("00000503-0000-0010-8000-00AA006D2EA4")]
	public interface Property
	{
		object Value { get; set; }
		string Name { get; }
		DataTypeEnum Type { get; }
		int Attributes { get; set; }
	}

	[ComImport, Guid("00000504-0000-0010-8000-00AA006D2EA4")]
	public interface Properties
	{
		int Count { get; }
		object GetEnumerator();
		void Refresh();
		Property this[object Item] { get; }
	}

	#region Enums

	public enum AllowNullsEnum
	{
		adIndexNullsAllow = 0,
		adIndexNullsDisallow = 1,
		adIndexNullsIgnore = 2,
		adIndexNullsIgnoreAny = 4
	}

	public enum ColumnAttributesEnum
	{
		adColFixed = 1,
		adColNullable = 2
	}

	public enum DataTypeEnum
	{
		adBigInt = 20,
		adBinary = 128,
		adBoolean = 11,
		adBSTR = 8,
		adChapter = 136,
		adChar = 129,
		adCurrency = 6,
		adDate = 7,
		adDBDate = 133,
		adDBTime = 134,
		adDBTimeStamp = 135,
		adDecimal = 14,
		adDouble = 5,
		adEmpty = 0,
		adError = 10,
		adFileTime = 64,
		adGUID = 72,
		adIDispatch = 9,
		adInteger = 3,
		adIUnknown = 13,
		adLongVarBinary = 205,
		adLongVarChar = 201,
		adLongVarWChar = 203,
		adNumeric = 131,
		adPropVariant = 138,
		adSingle = 4,
		adSmallInt = 2,
		adTinyInt = 16,
		adUnsignedBigInt = 21,
		adUnsignedInt = 19,
		adUnsignedSmallInt = 18,
		adUnsignedTinyInt = 17,
		adUserDefined = 132,
		adVarBinary = 204,
		adVarChar = 200,
		adVariant = 12,
		adVarNumeric = 139,
		adVarWChar = 202,
		adWChar = 130
	}

	public enum KeyTypeEnum
	{
		adKeyForeign = 2,
		adKeyPrimary = 1,
		adKeyUnique = 3
	}

	public enum ObjectTypeEnum
	{
		adPermObjColumn = 2,
		adPermObjDatabase = 3,
		adPermObjProcedure = 4,
		adPermObjProviderSpecific = -1,
		adPermObjTable = 1,
		adPermObjView = 5
	}

	public enum RuleEnum
	{
		adRINone,
		adRICascade,
		adRISetNull,
		adRISetDefault
	}


	public enum SortOrderEnum
	{
		adSortAscending = 1,
		adSortDescending = 2
	}

	#endregion
}