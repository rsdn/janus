using System;
using System.Data.Common;
using System.Windows.Forms;

using FirebirdSql.Data.FirebirdClient;

namespace Rsdn.Janus
{
	public partial class FbConfigControl: DBConfigControlBase
	{
		private readonly FbConnectionStringBuilder _csb = new FbConnectionStringBuilder();

		public FbConfigControl()
		{
			InitializeComponent();
		}

		protected override DbConnectionStringBuilder ConnectionStringBuilder
		{
			get { return _csb; }
		}

		public override void CustomInitialize(bool localize)
		{
			//if(ConfigSelDb.Instance.FbServerTypes.Count == 0)
			//{
			ConfigSelDb.Instance.FbServerTypes.Clear();
			ConfigSelDb.Instance.FbServerTypes.Insert(0, "Local");
			ConfigSelDb.Instance.FbServerTypes.Insert(1, "Remote");
			//}
			cmbFbServerType.Items.Clear();
			cmbFbServerType.Items.AddRange(ConfigSelDb.Instance.FbServerTypes.ToArray());

			if (ConfigSelDb.Instance.FbDialects.Count == 0)
			{
				ConfigSelDb.Instance.FbDialects.Add("Dialect 2");
				ConfigSelDb.Instance.FbDialects.Add("Dialect 3");
				ConfigSelDb.Instance.Save();
			}
			cmbFbDialect.Items.Clear();
			cmbFbDialect.Items.AddRange(ConfigSelDb.Instance.FbDialects.ToArray());

			if (ConfigSelDb.Instance.FbServers.Count == 0 || !ConfigSelDb.Instance.FbServers.Contains("localhost"))
			{
				ConfigSelDb.Instance.FbServers.Insert(0, "localhost");
			}
			cmbFbServerName.Items.Clear();
			cmbFbServerName.Items.AddRange(ConfigSelDb.Instance.FbServers.ToArray());

			if (ConfigSelDb.Instance.FbDatabases.Count > 0)
			{
				cmbFbSelectPath.Items.Clear();
				cmbFbSelectPath.Items.AddRange(ConfigSelDb.Instance.FbDatabases.ToArray());
				cmbFbSelectPath.SelectedIndex = 0;
			}

			cmbFbServerName.SelectedIndex = 0;
			cmbFbDialect.SelectedIndex = 1;
			cmbFbServerType.SelectedIndex = 0;

			//if (localize)
			//    _selectDatabasePathLabel.Text = SR.Database.ConfigControls.Jet.DatabasePath;
		}

		public override void BuildConnectionString()
		{
			base.BuildConnectionString();

			bool eServer = true;
			bool eLoginAndPass = true;
			if (string.IsNullOrEmpty(cmbFbSelectPath.Text))
				eServer = false;

			if (string.IsNullOrEmpty(txtFbUserID.Text) || string.IsNullOrEmpty(txtFbPassword.Text))
				eLoginAndPass = false;

			if (!eServer || !eLoginAndPass)
			{
				string _msg = "Fill next fileds: \n" +
					(!eServer ? " - server name or path to db file\n" : string.Empty) +
					(!eLoginAndPass ? " - login and password\n" : string.Empty);
				throw new InvalidOperationException(_msg);
			}

			_csb.Pooling    = false;
			_csb.Database   = cmbFbSelectPath.Text;
			_csb.Dialect    = Convert.ToByte(cmbFbDialect.Text.Replace("Dialect ", string.Empty));
			_csb.UserID     = txtFbUserID.Text;
			_csb.DataSource = cmbFbServerName.Text;
			_csb.Password   = txtFbPassword.Text;
			_csb.ServerType = cmbFbServerType.SelectedIndex == 0 ? FbServerType.Embedded : FbServerType.Default;
		}

		public override void OnConnectSucceeded()
		{
			base.OnConnectSucceeded();

			if (!cmbFbServerName.Items.Contains(cmbFbServerName.Text))
				ConfigSelDb.Instance.FbServers.Add(_csb.DataSource);
			if (!cmbFbSelectPath.Items.Contains(cmbFbSelectPath.Text))
				ConfigSelDb.Instance.FbDatabases.Add(_csb.Database);
		}

		private void btnFbSelectPath_Click(object sender, EventArgs e)
		{
			if (_selectDatabaseDialog.ShowDialog(this) == DialogResult.OK)
				cmbFbSelectPath.Text = _selectDatabaseDialog.FileName;
		}

		private void cmbFbServerType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cmbFbServerType.SelectedIndex == 0)
			{
				cmbFbServerName.SelectedIndex = 0;
				cmbFbServerName.Enabled = false;
			}
			else
			{
				cmbFbServerName.Enabled = true;
			}
		}
	}
}
