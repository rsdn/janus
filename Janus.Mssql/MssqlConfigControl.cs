using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;

namespace Rsdn.Janus.Mssql
{
	public partial class MssqlConfigControl : DBConfigControlBase
	{
		private readonly SqlConnectionStringBuilder _csb = new SqlConnectionStringBuilder();

		public MssqlConfigControl()
		{
			InitializeComponent();
		}

		protected override DbConnectionStringBuilder ConnectionStringBuilder
		{
			get { return _csb; }
		}

		public override bool ConnectSuccess
		{
			get { return base.ConnectSuccess && !string.IsNullOrEmpty(_csb.InitialCatalog); }
		}

		public override void CustomInitialize(bool localize)
		{
			rbtMsSqlWinAuth.Checked = true;
			cmbMsSqlServersExist.Items.Clear();

			if (ConfigSelDb.Instance.MssqlServers.Count > 0)
				cmbMsSqlServersExist.Items.AddRange(ConfigSelDb.Instance.MssqlServers.ToArray());
			else
				cmbMsSqlServersExist.Items.Add(Environment.MachineName);

			cmbMsSqlServersExist.SelectedIndex = cmbMsSqlServersExist.Items.Count - 1;

			//if (localize)
			//	_selectDatabasePathLabel.Text = SR.Database.ConfigControls.Mssql.DatabasePath;
		}

		public override void BuildConnectionString()
		{
			base.BuildConnectionString();

			var eServer = true;
			var eLoginAndPass = true;
			if (string.IsNullOrEmpty(cmbMsSqlServersExist.Text))
				eServer = false;

			if (rbtMsSqlServerAuth.Checked)
				if (string.IsNullOrEmpty(txtMsSqlLogin.Text) || string.IsNullOrEmpty(txtMsSqlPassword.Text))
					eLoginAndPass = false;

			if (!eServer || !eLoginAndPass)
			{
				var msg = "Fill next fileds: \n" +
							(!eServer ? " - server name or IP\n" : string.Empty) +
							(!eLoginAndPass ? " - login and password\n" : string.Empty);
				throw new InvalidOperationException(msg);
			}

			_csb.DataSource = cmbMsSqlServersExist.Text;
			if (lsbMsSqlBasesExist.SelectedIndex > -1)
				_csb.InitialCatalog = lsbMsSqlBasesExist.SelectedItem.ToString();

			if (rbtMsSqlServerAuth.Checked)
			{
				_csb.UserID = txtMsSqlLogin.Text;
				_csb.Password = txtMsSqlPassword.Text;
			}
			else
				_csb.IntegratedSecurity = true;
		}

		public override bool PrepareCreateConnectionString()
		{
			if (!base.PrepareCreateConnectionString())
				return false;

			using (var sef = new StringEnterForm("Enter new database name:"))
			{
				if (sef.ShowDialog() != DialogResult.OK)
					return false;
				_csb.InitialCatalog = sef.String;
			}
			return true;
		}

		public override void OnConnectSucceeded()
		{
			base.OnConnectSucceeded();

			if (!string.IsNullOrEmpty(cmbMsSqlServersExist.Text) &&
				!ConfigSelDb.Instance.MssqlServers.Contains(cmbMsSqlServersExist.Text))
				ConfigSelDb.Instance.MssqlServers.Add(cmbMsSqlServersExist.Text);

			var bases = new List<string>(GetDatabasesList(_csb.ConnectionString));
			bases.Sort();
			lsbMsSqlBasesExist.Items.Clear();
			lsbMsSqlBasesExist.Items.AddRange(bases.ToArray());
		}

		private void RbtMsSqlWinAuthCheckedChanged(object sender, EventArgs e)
		{
			var needLogin = !rbtMsSqlWinAuth.Checked;
			txtMsSqlLogin.Enabled = needLogin;
			txtMsSqlPassword.Enabled = needLogin;
		}

		private void LsbMsSqlBasesExistSelectedIndexChanged(object sender, EventArgs e)
		{
			if (lsbMsSqlBasesExist.SelectedIndex > -1)
				_csb.InitialCatalog = lsbMsSqlBasesExist.SelectedItem.ToString();

			OnConnectionStringChanged();
		}

		private static IEnumerable<string> GetDatabasesList(string constr)
		{
			const string sql =
				@"
				SELECT
					[name]
				FROM
					[master].[dbo].[sysdatabases]
				WHERE
					[dbid] > 4";

			var csb = new SqlConnectionStringBuilder(constr);
			// TODO : Поправить
			//csb.ConnectTimeout = Config.Instance.ConnectTimeout;
			//csbCheck.Pooling = false;

			DataTable result;

			using (var con = new SqlConnection(csb.ConnectionString))
			using (var cmd = con.CreateCommand())
			using (var adapter = new SqlDataAdapter(cmd))
			{
				result = new DataTable {Locale = CultureInfo.InvariantCulture};

				cmd.CommandText = sql;
				adapter.Fill(result);
			}

			return result.GetTableColumn<string>("name");
		}
	}
}