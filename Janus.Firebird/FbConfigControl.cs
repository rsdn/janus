using System;
using System.Data.Common;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;

namespace Rsdn.Janus.Firebird
{
	public partial class FbConfigControl : DBConfigControlBase
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

			if (ConfigSelDb.Instance.FbServers.Count == 0 ||
				!ConfigSelDb.Instance.FbServers.Contains("localhost"))
				ConfigSelDb.Instance.FbServers.Insert(0, "localhost");
			cmbFbServerName.Items.Clear();
			cmbFbServerName.Items.AddRange(ConfigSelDb.Instance.FbServers.ToArray());

			if (ConfigSelDb.Instance.FbDatabases.Count > 0)
			{
				cmbFbSelectPath.Items.Clear();
				cmbFbSelectPath.Items.AddRange(ConfigSelDb.Instance.FbDatabases.ToArray());
				cmbFbSelectPath.SelectedIndex = 0;
			}

			cmbFbServerName.SelectedIndex = 0;
			cmbFbServerType.SelectedIndex = 0;

			//if (localize)
			//	_selectDatabasePathLabel.Text = SR.Database.ConfigControls.Jet.DatabasePath;
		}

		public override void BuildConnectionString()
		{
			base.BuildConnectionString();

			var eServer = true;
			var eLoginAndPass = true;
			if (string.IsNullOrEmpty(cmbFbSelectPath.Text))
				eServer = false;

			if (string.IsNullOrEmpty(txtFbUserID.Text) || string.IsNullOrEmpty(txtFbPassword.Text))
				eLoginAndPass = false;

			if (!eServer || !eLoginAndPass)
			{
				var msg = "Fill next fields:" + Environment.NewLine +
							(!eServer ? "- server name or path to db file" + Environment.NewLine : string.Empty) +
							(!eLoginAndPass ? "- login and password" + Environment.NewLine : string.Empty);
				throw new InvalidOperationException(msg);
			}

			_csb.Pooling = false;
			_csb.Database = cmbFbSelectPath.Text;
			_csb.Dialect = 3;
			_csb.UserID = txtFbUserID.Text;
			_csb.DataSource = cmbFbServerName.Text;
			_csb.Password = txtFbPassword.Text;
			_csb.ServerType = cmbFbServerType.SelectedIndex == 0
								? FbServerType.Embedded
								: FbServerType.Default;
		}

		public override void OnConnectSucceeded()
		{
			base.OnConnectSucceeded();

			if (!cmbFbServerName.Items.Contains(cmbFbServerName.Text))
				ConfigSelDb.Instance.FbServers.Add(_csb.DataSource);
			if (!cmbFbSelectPath.Items.Contains(cmbFbSelectPath.Text))
				ConfigSelDb.Instance.FbDatabases.Add(_csb.Database);
		}

		private void BtnFbSelectPathClick(object sender, EventArgs e)
		{
			if (_selectDatabaseDialog.ShowDialog(this) == DialogResult.OK)
				cmbFbSelectPath.Text = _selectDatabaseDialog.FileName;
		}

		private void CmbFbServerTypeSelectedIndexChanged(object sender, EventArgs e)
		{
			if (cmbFbServerType.SelectedIndex == 0)
			{
				cmbFbServerName.SelectedIndex = 0;
				cmbFbServerName.Enabled = false;
			}
			else
				cmbFbServerName.Enabled = true;
		}
	}
}