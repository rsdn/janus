using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;
using Rsdn.Janus.Framework;

namespace Rsdn.Janus
{
	public partial class MssqlConfigControl: DBConfigControlBase
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
			//    _selectDatabasePathLabel.Text = SR.Database.ConfigControls.Mssql.DatabasePath;
		}

		public override void BuildConnectionString()
		{
			base.BuildConnectionString();

			bool eServer = true;
			bool eLoginAndPass = true;
			if (string.IsNullOrEmpty(cmbMsSqlServersExist.Text))
				eServer = false;

			if (rbtMsSqlServerAuth.Checked)
			{
				if (string.IsNullOrEmpty(txtMsSqlLogin.Text) || string.IsNullOrEmpty(txtMsSqlPassword.Text))
					eLoginAndPass = false;
			}

			if (!eServer || !eLoginAndPass)
			{
				string _msg = "Fill next fileds: \n" +
					(!eServer ? " - server name or IP\n" : string.Empty) +
					(!eLoginAndPass ? " - login and password\n" : string.Empty);
				throw new InvalidOperationException(_msg);
			}

			_csb.DataSource = cmbMsSqlServersExist.Text;
			if (lsbMsSqlBasesExist.SelectedIndex > -1)
				_csb.InitialCatalog = lsbMsSqlBasesExist.SelectedItem.ToString();

			if (rbtMsSqlServerAuth.Checked)
			{
				_csb.UserID   = txtMsSqlLogin.Text;
				_csb.Password = txtMsSqlPassword.Text;
			}
			else
				_csb.IntegratedSecurity = true;
		}

		public override bool PrepareCreateConnectionString()
		{
			if (!base.PrepareCreateConnectionString())
				return false;

			using (StringEnterForm sef = new StringEnterForm())
			{
				sef.Description = "Enter new database name:";
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
			{
				ConfigSelDb.Instance.MssqlServers.Add(cmbMsSqlServersExist.Text);
			}

			List<string> bases = new List<string>(GetDatabasesList(_csb.ConnectionString));
			bases.Sort();
			lsbMsSqlBasesExist.Items.Clear();
			lsbMsSqlBasesExist.Items.AddRange(bases.ToArray());
		}

		private void rbtMsSqlWinAuth_CheckedChanged(object sender, EventArgs e)
		{
			bool needLogin = !rbtMsSqlWinAuth.Checked;
			txtMsSqlLogin.Enabled = needLogin;
			txtMsSqlPassword.Enabled = needLogin;
		}

		private void lsbMsSqlBasesExist_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lsbMsSqlBasesExist.SelectedIndex > -1)
				_csb.InitialCatalog = lsbMsSqlBasesExist.SelectedItem.ToString();

			OnConnectionStringChanged();
		}

		private static IEnumerable<string> GetDatabasesList(string constr)
		{
			const string sql = @"
				SELECT
					[name]
				FROM
					[master].[dbo].[sysdatabases]
				WHERE
					[dbid] > 4";

			SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder(constr);
			csb.ConnectTimeout = Config.Instance.ConnectTimeout;
			//csbCheck.Pooling = false;

			DataTable result;

			using (SqlConnection con = new SqlConnection(csb.ConnectionString))
			using (SqlCommand cmd = con.CreateCommand())
			using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
			{
				result = new DataTable();
				result.Locale = CultureInfo.InvariantCulture;

				cmd.CommandText = sql;
				adapter.Fill(result);
			}

			return Algorithms.GetTableColumn<string>(result, "name");
		}
	}
}
