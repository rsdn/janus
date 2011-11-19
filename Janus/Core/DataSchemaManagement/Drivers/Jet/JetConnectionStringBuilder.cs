using System;
using System.Data;
using System.Data.Common;
using System.Globalization;

namespace Rsdn.Janus
{
	public class JetConnectionStringBuilder: DbConnectionStringBuilder
	{
		public JetConnectionStringBuilder()
		{ }

		public JetConnectionStringBuilder(string constr)
		{
			ConnectionString = constr;
		}

		public string DataSource
		{
			get { return GetString("data source"); }
			set { this["data source"] = value; }
		}

		public int? Mode
		{
			get { return GetInt32("mode"); }
			set { this["mode"] = value; }
		}

		public string Password
		{
			get { return GetString("password"); }
			set { this["password"] = value; }
		}

		public string Provider
		{
			get { return GetString("provider");}
			set { this["provider"] = value; }
		}

		public string UserID
		{
			get { return GetString("user id"); }
			set { this["user id"] = value; }
		}

		#region Private	Methods

		private string GetString(string key)
		{
			object value;
			return TryGetValue(key, out value)? Convert.ToString(value): null;
		}

		private int? GetInt32(string key)
		{
			object value;
			return TryGetValue(key, out value)? Convert.ToInt32(value): (int?)null;
		}

		private IsolationLevel GetIsolationLevel(string key)
		{
			switch (GetString(key).ToLower(CultureInfo.CurrentCulture))
			{
				default:
				case "readcommitted":
					return IsolationLevel.ReadCommitted;

				case "readuncommitted":
					return IsolationLevel.ReadUncommitted;

				case "repeatableread":
					return IsolationLevel.RepeatableRead;

				case "serializable":
					return IsolationLevel.Serializable;

				case "chaos":
					return IsolationLevel.Chaos;

				case "unspecified":
					return IsolationLevel.Unspecified;
			}
		}

		#endregion
	}
}
