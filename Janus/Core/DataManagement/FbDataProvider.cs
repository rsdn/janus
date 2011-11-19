using System;
using System.Data;
using System.Data.Common;

using BLToolkit.Data.DataProvider;

using FirebirdSql.Data.FirebirdClient;

namespace Rsdn.Janus.Core.DataManagement
{
	internal class FbDataProvider : DataProviderBase
	{
		public override Type ConnectionType
		{
			get { return typeof (FbConnection); }
		}

		public override IDbConnection CreateConnectionObject()
		{
			return new FbConnection();
		}

		public override DbDataAdapter CreateDataAdapterObject()
		{
			return new FbDataAdapter();
		}

		public override bool DeriveParameters(IDbCommand command)
		{
			FbCommandBuilder.DeriveParameters((FbCommand) command);

			return true;
		}

		public override string Name
		{
			get { return "Fb"; }
		}
	}
}