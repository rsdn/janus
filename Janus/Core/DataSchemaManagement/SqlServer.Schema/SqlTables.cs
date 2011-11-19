using System;
using System.Data;
using System.Globalization;

namespace Rsdn.Janus
{
	internal class SqlTables : SqlDbSchema
	{
		protected override SqlClause GetCommandText(object[] restrictions)
		{
			SqlClause sql = new SqlClause();

			DataRow[] col = _sqlClauses.Select("CollectionName = 'Tables' and Version=" + _version.ver1);

			sql.Select = col[0]["SelectMain"].ToString();

			sql.AppendWhere(col[0]["Where1"].ToString());

			if (restrictions != null)
			{
				int index = 0;

				// TABLE_CATALOG
				if (restrictions.Length >= 1 && restrictions[0] != null)
				{
				}

				// TABLE_SCHEMA
				if (restrictions.Length >= 2 && restrictions[1] != null)
				{
					sql.AppendWhereFormat(CultureInfo.CurrentCulture, col[0]["Where2"].ToString(), index++);
				}

				// TABLE_NAME
				if (restrictions.Length >= 3 && restrictions[2] != null)
				{
					sql.AppendWhereFormat(CultureInfo.CurrentCulture, col[0]["Where3"].ToString(), index++);
				}

				// TABLE_TYPE
				if (restrictions.Length >= 4 && restrictions[3] != null)
				{
					switch (restrictions[3].ToString())
					{
						case "VIEW":
							sql.AppendWhere(col[0]["Where4"].ToString());
							break;
						case "SYSTEM TABLE":
							sql.AppendWhere(col[0]["Where5"].ToString());
							break;
						case "BASE TABLE":
						default:
							sql.AppendWhere(col[0]["Where6"].ToString());
							break;
					}
				}
			}

			sql.AppendOrder(col[0]["Order1"].ToString());

			return sql;
		}

		protected override DataTable ProcessResult(DataTable schema)
		{
			schema.BeginLoadData();

			foreach (DataRow row in schema.Rows)
			{
				// ToBoolean
				foreach (DataColumn col in schema.Columns)
				{
					if (col.Caption.StartsWith("IS_"))
					{
						row[col.Caption] =
							row[col.Caption] != DBNull.Value &&
							Convert.ToInt32(row[col.Caption], CultureInfo.InvariantCulture) != 0;
					}
				}

				row["MS_DBTOOLS_SUPPORT"] = (Convert.ToInt32(
					HelperGetExtendedProperty(
						"microsoft_database_tools_support", "schema", "dbo", "table",
						row["TABLE_NAME"].ToString(), null, null),
					CultureInfo.InvariantCulture) == 1);
			}

			schema.EndLoadData();
			schema.AcceptChanges();

			return schema;
		}
	}
}
