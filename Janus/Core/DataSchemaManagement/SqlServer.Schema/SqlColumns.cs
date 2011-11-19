using System;
using System.Data;
using System.Globalization;
using System.Text;

namespace Rsdn.Janus
{
	internal class SqlColumns : SqlDbSchema
	{
		protected override SqlClause GetCommandText(object[] restrictions)
		{
			SqlClause sql = new SqlClause();

			DataRow[] col = _sqlClauses.Select("CollectionName = 'Columns' and Version=" + _version.ver1);

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

				// COLUMN_NAME
				if (restrictions.Length >= 4 && restrictions[3] != null)
				{
					sql.AppendWhereFormat(CultureInfo.CurrentCulture, col[0]["Where4"].ToString(), index++);
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

				if (row["NUMERIC_PRECISION"] == DBNull.Value)
					row["NUMERIC_PRECISION"] = 0;


				if (row["NUMERIC_SCALE"] == DBNull.Value)
					row["NUMERIC_SCALE"] = 0;

				int helperDid = Convert.ToInt32(row["HELPER_DID"], CultureInfo.InvariantCulture);
				if (helperDid != 0)
					row["COLUMN_DEFAULT"] = HelperGetObjectDefinition(helperDid);

				if(Convert.ToBoolean(row["IS_COMPUTED"]))
				{
					row["COMPUTED_SOURCE"] = HelperGetComputed(
						Convert.ToInt32(row["HELPER_OID"], CultureInfo.InvariantCulture),
						Convert.ToInt32(row["HELPER_COLID"], CultureInfo.InvariantCulture));
				}

			}
			schema.EndLoadData();
			schema.AcceptChanges();

			schema.Columns.Remove("HELPER_DID");
			schema.Columns.Remove("HELPER_OID");
			//schema.Columns.Remove("HELPER_COLID");
			schema.Columns["HELPER_COLID"].Caption = "ORDINAL_POSITION";

			return schema;
		}
	}
}
