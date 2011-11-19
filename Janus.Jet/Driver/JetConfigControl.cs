using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Windows.Forms;

using Rsdn.Janus.Jet;

namespace Rsdn.Janus
{
	public partial class JetConfigControl : DBConfigControlBase
	{
		private readonly JetConnectionStringBuilder _csb = new JetConnectionStringBuilder();

		public JetConfigControl()
		{
			InitializeComponent();
		}

		protected override DbConnectionStringBuilder ConnectionStringBuilder
		{
			get { return _csb; }
		}

		public override void CustomInitialize(bool localize)
		{
			var existDatabasePathList = new List<string>();
			foreach (var path in ConfigSelDb.Instance.JetServers)
				if (File.Exists(path))
					existDatabasePathList.Add(path);
			ConfigSelDb.Instance.JetServers = existDatabasePathList;

			if (ConfigSelDb.Instance.JetServers.Count > 0)
			{
				_selectDatabasePathComboBox.Items.Clear();
				_selectDatabasePathComboBox.Items.AddRange(ConfigSelDb.Instance.JetServers.ToArray());
				_selectDatabasePathComboBox.Text = _selectDatabasePathComboBox.Items[0].ToString();
			}

			if (localize)
			{
				_selectDatabasePathLabel.Text = Resources.DatabasePath;
				_selectDatabaseDialog.Filter = Resources.OpenFileDialogFilter;
			}
		}

		public override void BuildConnectionString()
		{
			base.BuildConnectionString();

			_csb.Provider = "Microsoft.Jet.OLEDB.4.0";
			_csb.Mode = 16;
			_csb.DataSource = _selectDatabasePathComboBox.Text;
		}

		public override void OnConnectSucceeded()
		{
			base.OnConnectSucceeded();

			if (!_selectDatabasePathComboBox.Items.Contains(_selectDatabasePathComboBox.Text))
			{
				_selectDatabasePathComboBox.Items.Add(_selectDatabasePathComboBox.Text);
				ConfigSelDb.Instance.JetServers.Add(_csb.DataSource);
			}
		}

		private void SelectDatabasePathButton_Click(object sender, EventArgs e)
		{
			if (_selectDatabaseDialog.ShowDialog(this) == DialogResult.OK)
				_selectDatabasePathComboBox.Text = _selectDatabaseDialog.FileName;
		}
	}
}