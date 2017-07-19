using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Rsdn.Janus.Mssql {
	public partial class MssqlConfigControl : DBConfigControlBase {
		private readonly SqlConnectionStringBuilder _csb = new SqlConnectionStringBuilder();

		public MssqlConfigControl() {
			InitializeComponent();
		}

		protected override DbConnectionStringBuilder ConnectionStringBuilder {
			get { return _csb; }
		}

		public override bool ConnectSuccess {
			get { return base.ConnectSuccess && (chIsSqlExpress.Checked || !string.IsNullOrEmpty(_csb.InitialCatalog)); }
		}

		public override void CustomInitialize(bool localize) {
			var versionsInstalled = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			using (var regRoot = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server Local DB\Installed Versions")) {
				foreach (var subKey in regRoot.GetSubKeyNames()) {
					versionsInstalled.Add(subKey);
				}
			}
			if (Environment.Is64BitOperatingSystem) {
				using (var regRoot = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Microsoft SQL Server Local DB\Installed Versions")) {
					if (regRoot != null) {
						foreach (var subKey in regRoot.GetSubKeyNames()) {
							versionsInstalled.Add(subKey);
						}
					}
				}
			}

			if (versionsInstalled.Any()) {
				chIsSqlExpress.Enabled = true;
				cmbExpressInstances.Items.AddRange(versionsInstalled.ToArray());
				cmbExpressInstances.SelectedIndex = 0;
			}
			else {
				chIsSqlExpress.Enabled = false;
			}

			rbtMsSqlWinAuth.Checked = true;
			cmbMsSqlServersExist.Items.Clear();

			if (ConfigSelDb.Instance.MssqlServers.Count > 0) {
				cmbMsSqlServersExist.Items.AddRange(ConfigSelDb.Instance.MssqlServers.ToArray());
			}
			else {
				cmbMsSqlServersExist.Items.Add(Environment.MachineName);
			}

			cmbMsSqlServersExist.SelectedIndex = cmbMsSqlServersExist.Items.Count - 1;

			//if (localize)
			//	_selectDatabasePathLabel.Text = SR.Database.ConfigControls.Mssql.DatabasePath;
		}

		public override void BuildConnectionString() {
			base.BuildConnectionString();

			if (chIsSqlExpress.Checked) {
				var filePath = txDbFilePath.Text;
				_csb.ConnectionString = string.Format(
					@"Data Source=(LocalDB)\v" + cmbExpressInstances.SelectedItem + @";AttachDbFilename={0};Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30;",
					filePath);
				if (!System.IO.File.Exists(filePath)) {
					if (MessageBox.Show(string.Format(Resources.FileNotExists, filePath), Resources.ConfirmationHeader, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
						using (var sqlC = new SqlConnection(@"Data Source=(LocalDb)\MSSQLLocalDB;")) {
							sqlC.Open();
							using (var sqlCommand = new SqlCommand(string.Format(
@"
CREATE DATABASE
    [{2}]
ON PRIMARY (
    NAME={2}_data,
    FILENAME = '{0}'
)
LOG ON (
    NAME={2}_log,
    FILENAME = '{1}'
)",
								filePath, System.IO.Path.ChangeExtension(filePath, "log"), System.IO.Path.GetFileNameWithoutExtension(filePath)), sqlC)) {
								sqlCommand.ExecuteNonQuery();
							}
						}
					}
				}
				OnConnectSucceeded();
			}
			else {
				var eServer = true;
				var eLoginAndPass = true;
				if (string.IsNullOrEmpty(cmbMsSqlServersExist.Text)) {
					eServer = false;
				}

				if (rbtMsSqlServerAuth.Checked) {
					if (string.IsNullOrEmpty(txtMsSqlLogin.Text) || string.IsNullOrEmpty(txtMsSqlPassword.Text)) {
						eLoginAndPass = false;
					}
				}

				if (!eServer || !eLoginAndPass) {
					var msg = "Fill next fileds: \n" +
								(!eServer ? " - server name or IP\n" : string.Empty) +
								(!eLoginAndPass ? " - login and password\n" : string.Empty);
					throw new InvalidOperationException(msg);
				}

				_csb.DataSource = cmbMsSqlServersExist.Text;
				if (lsbMsSqlBasesExist.SelectedIndex > -1) {
					_csb.InitialCatalog = lsbMsSqlBasesExist.SelectedItem.ToString();
				}

				if (rbtMsSqlServerAuth.Checked) {
					_csb.UserID = txtMsSqlLogin.Text;
					_csb.Password = txtMsSqlPassword.Text;
				}
				else {
					_csb.IntegratedSecurity = true;
				}
			}
		}

		public override bool PrepareCreateConnectionString() {
			if (!base.PrepareCreateConnectionString()) {
				return false;
			}

			if (chIsSqlExpress.Checked) {
				var _fileChoosen = false;
				CancelEventHandler checkChoose = (sender, args) => { _fileChoosen = true; };
				_selectDatabaseDialog.FileOk += checkChoose;
				btBrowseDatabaseFile.PerformClick();
				_selectDatabaseDialog.FileOk -= checkChoose;
				if (_fileChoosen) {
					return true;
				}
				else {
					return false;
				}
			}
			else {
				using (var sef = new StringEnterForm("Enter new database name:")) {
					if (sef.ShowDialog() != DialogResult.OK) {
						return false;
					}

					_csb.InitialCatalog = sef.String;
				}
			}
			return true;
		}

		public override void OnConnectSucceeded() {
			base.OnConnectSucceeded();
			if (chIsSqlExpress.Checked) {
				return;
			}
			else {
				if (!string.IsNullOrEmpty(cmbMsSqlServersExist.Text) &&
					!ConfigSelDb.Instance.MssqlServers.Contains(cmbMsSqlServersExist.Text)) {
					ConfigSelDb.Instance.MssqlServers.Add(cmbMsSqlServersExist.Text);
				}

				var bases = new List<string>(GetDatabasesList(_csb.ConnectionString));
				bases.Sort();
				lsbMsSqlBasesExist.Items.Clear();
				lsbMsSqlBasesExist.Items.AddRange(bases.ToArray());
			}
		}

		private void RbtMsSqlWinAuthCheckedChanged(object sender, EventArgs e) {
			var needLogin = !rbtMsSqlWinAuth.Checked;
			txtMsSqlLogin.Enabled = needLogin;
			txtMsSqlPassword.Enabled = needLogin;
		}

		private void LsbMsSqlBasesExistSelectedIndexChanged(object sender, EventArgs e) {
			if (lsbMsSqlBasesExist.SelectedIndex > -1) {
				_csb.InitialCatalog = lsbMsSqlBasesExist.SelectedItem.ToString();
			}

			OnConnectionStringChanged();
		}

		private static IEnumerable<string> GetDatabasesList(string constr) {
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
			using (var adapter = new SqlDataAdapter(cmd)) {
				result = new DataTable { Locale = CultureInfo.InvariantCulture };

				cmd.CommandText = sql;
				adapter.Fill(result);
			}

			return result.GetTableColumn<string>("name");
		}

		private void chIsSqlExpress_CheckedChanged(object sender, EventArgs e) {
			notExpressControls.Visible = !(expressControls.Visible = chIsSqlExpress.Checked);
			notExpressControls.Enabled = !(expressControls.Enabled = chIsSqlExpress.Checked);
		}

		private void btBrowseDatabaseFile_Click(object sender, EventArgs e) {
			_selectDatabaseDialog.FileName = txDbFilePath.Text;
			if (_selectDatabaseDialog.ShowDialog() == DialogResult.OK) {
				txDbFilePath.Text = _selectDatabaseDialog.FileName;
				BuildConnectionString();
				OnConnectionStringChanged();
				txConnectionString.Text = ConnectionString;
			}
		}
	}
}