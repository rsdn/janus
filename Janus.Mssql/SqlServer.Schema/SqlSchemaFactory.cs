using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;

using Rsdn.Janus.Mssql;

namespace Rsdn.Janus
{
	internal static class SqlSchemaFactory
	{
		public static DataTable GetSchema(SqlConnection con, string collectionName, object[] restrictions)
		{
			var filter = String.Format("CollectionName = '{0}'", collectionName);

			var ds = new DataSet();
			ds.ReadXml(new StringReader(Resources.MetaData));

			var collection = ds.Tables[DbMetaDataCollectionNames.MetaDataCollections].Select(filter);

			if (collection.Length != 1)
				throw new NotSupportedException("Unsupported collection name.");

			if (restrictions != null && restrictions.Length > (int)collection[0]["NumberOfRestrictions"])
				throw new InvalidOperationException("The number of specified restrictions is not valid.");

			if (ds.Tables[DbMetaDataCollectionNames.Restrictions].Select(filter).Length !=
				(int)collection[0]["NumberOfRestrictions"])
				throw new InvalidOperationException("Incorrect restriction definition.");

			SqlDbSchema returnSchema;
			switch (collectionName.ToLower(CultureInfo.CurrentCulture))
			{
				case "checkconstraints":
					returnSchema = new SqlCheckConstraints();
					break;
				case "columns":
					returnSchema = new SqlColumns();
					break;
				case "defaultconstraints":
					returnSchema = new SqlDefaultConstraints();
					break;
				case "foreignkeys":
					returnSchema = new SqlForeignKeys();
					break;
				case "indexes":
					returnSchema = new SqlIndexes();
					break;
				case "primarykeys":
					returnSchema = new SqlPrimaryKeys();
					break;
				case "tables":
					returnSchema = new SqlTables();
					break;
				case "triggers":
					returnSchema = new SqlTriggers();
					break;
				case "uniquekeys":
					returnSchema = new SqlUniqueKeys();
					break;
				default:
					throw new NotSupportedException("The specified metadata collection is not supported.");
			}

			return returnSchema.GetSchema(con, collectionName, restrictions);
		}
	}
}