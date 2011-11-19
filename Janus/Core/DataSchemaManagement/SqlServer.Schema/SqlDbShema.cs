using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace Rsdn.Janus
{

	internal abstract class SqlDbSchema
	{
		private static SqlConnection _hcon;
		protected static SqlServerVersion _version = new SqlServerVersion();
		protected DataTable _sqlClauses;

		public SqlDbSchema()
		{
			Stream xmlStream = Assembly.GetExecutingAssembly()
				.GetManifestResourceStream(SqlSchemaFactory.ResName);

			DataSet ds = new DataSet();
			ds.ReadXml(xmlStream);
			_sqlClauses = ds.Tables["SqlClauses"];
		}

		protected abstract SqlClause GetCommandText(object[] restrictions);

		public DataTable GetSchema(SqlConnection con, string collectionName, object[] restrictions)
		{
			if (con != _hcon)
			{
				_hcon = con;
				if(!SqlServerVersion.Parse(_hcon.ServerVersion, out _version))
					throw new ArgumentException("Invalid server version", "con");
			}

			DataTable dataTable = new DataTable(collectionName);

			using(SqlCommand command = BuildCommand(con, collectionName, restrictions))
			using(SqlDataAdapter adapter = new SqlDataAdapter(command))
			{
				adapter.Fill(dataTable);
			}

			return ProcessResult(dataTable);
		}

		protected SqlCommand BuildCommand(SqlConnection connection, string collectionName, object[] restrictions)
		{
			//string filter = String.Format("CollectionName='{0}'", collectionName);
			//DataRow[] restriction = connection.GetSchema(DbMetaDataCollectionNames.Restrictions).Select(filter);

			SqlCommand cmd = connection.CreateCommand();

			cmd.CommandText = GetCommandText(restrictions).ToString();

			if (restrictions != null && restrictions.Length > 0)
			{
				// Add parameters
				int index = 0;
				for (int i = 0; i < restrictions.Length; i++)
				{
					if (restrictions[i] != null )
					{
						string pname = String.Format(CultureInfo.CurrentCulture, "@p{0}", index++);
						cmd.Parameters.Add(pname, SqlDbType.VarChar, 255).Value = restrictions[i].ToString();
					}
				}
			}

			return cmd;
		}

		protected virtual DataTable ProcessResult(DataTable schema)
		{
			return schema;
		}

		#region Static methods-helpers

		protected static string HelperGetObjectDefinition(int id)
		{
			if (_hcon == null)
				throw new InvalidOperationException("The conneciton not set.");

			using (SqlCommand cmd = _hcon.CreateCommand())
			{
				cmd.CommandText = string.Format(@"
					SELECT
						sc.[text],
						sc.[encrypted]
					FROM
						[dbo].[syscomments] sc
					WHERE
						sc.[id] = {0}",
					id);

				DataTable table = new DataTable();
				table.Locale = CultureInfo.InvariantCulture;

				using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
					adapter.Fill(table);

				StringBuilder result = new StringBuilder();
				for (int i = 0; i < table.Rows.Count; i++)
				{
					if (!Convert.ToBoolean(table.Rows[i]["encrypted"], CultureInfo.InvariantCulture))
						result.Append(table.Rows[i]["text"]);
				}
				return result.ToString();
			}
		}

		protected static string HelperGetComputed(int tid, int cid)
		{
			if (_hcon == null)
				throw new InvalidOperationException("The conneciton not set.");

			using (SqlCommand cmd = _hcon.CreateCommand())
			{
				cmd.CommandText = string.Format(@"
					SELECT
						sc.[text]
					FROM
						[dbo].[syscomments] sc
					WHERE
						sc.[id] = {0} AND sc.[number] = {1}",
					tid, cid);

				DataTable table = new DataTable();
				table.Locale = CultureInfo.InvariantCulture;

				using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
					adapter.Fill(table);

				StringBuilder result = new StringBuilder();
				for (int i = 0; i < table.Rows.Count; i++)
					result.Append(table.Rows[i]["text"]);
				return result.ToString();
			}
		}

		protected static object HelperGetExtendedProperty(string property_name, string level0_object_type, string level0_object_name,
			string level1_object_type, string level1_object_name, string level2_object_type, string level2_object_name)
		{
			if (_hcon == null)
				throw new InvalidOperationException("The conneciton not set.");

			using (SqlCommand cmd = _hcon.CreateCommand())
			{
				cmd.CommandText = string.Format(
					@"SELECT [value] FROM ::fn_listextendedproperty ({0}, {1}, {2}, {3}, {4}, {5}, {6})",
					QuotedStringOrNull(property_name),
					QuotedStringOrNull(level0_object_type),
					QuotedStringOrNull(level0_object_name),
					QuotedStringOrNull(level1_object_type),
					QuotedStringOrNull(level1_object_name),
					QuotedStringOrNull(level2_object_type),
					QuotedStringOrNull(level2_object_name));

				return cmd.ExecuteScalar();
			}
		}

		private static string QuotedStringOrNull(string str)
		{
			return str == null ? "null" : "'" + str + "'";
		}

		#endregion
	}

}
