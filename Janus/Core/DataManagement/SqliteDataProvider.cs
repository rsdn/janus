using System;
using System.Data;
using System.Data.Common;

using BLToolkit.Data.DataProvider;

using System.Data.SQLite;

namespace Rsdn.Janus.Core.DataManagement
{
	internal class SqliteDataProvider : DataProviderBase
	{
		public override Type ConnectionType
		{
			get { return typeof (SQLiteConnection); }
		}

		public override IDbConnection CreateConnectionObject()
		{
			return new SQLiteConnection();
		}

		public override DbDataAdapter CreateDataAdapterObject()
		{
			return new SQLiteDataAdapter();
		}

		public override bool DeriveParameters(IDbCommand command)
		{
			//SQLiteCommandBuilder.DeriveParameters((SQLiteCommand) command);

			return false;
		}

		public override string Name
		{
			get { return "SQLite"; }
		}
	}
}