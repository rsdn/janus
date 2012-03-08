using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace Rsdn.Janus.Sqlite
{
	public partial class SqliteConfigControl : DBConfigControlBase
	{
		private readonly SQLiteConnectionStringBuilder _csb = new SQLiteConnectionStringBuilder();

		public SqliteConfigControl()
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
			foreach (var path in ConfigSelDb.Instance.SqliteDatabasePathList)
				if (File.Exists(path))
					existDatabasePathList.Add(path);
			ConfigSelDb.Instance.SqliteDatabasePathList = existDatabasePathList;

			if (ConfigSelDb.Instance.SqliteDatabasePathList.Count > 0)
			{
				_selectDatabasePathComboBox.Items.Clear();
				_selectDatabasePathComboBox.Items.AddRange(ConfigSelDb.Instance.SqliteDatabasePathList.ToArray());
				_selectDatabasePathComboBox.SelectedIndex = 0;
			}

			if (localize)
			{
				_selectDatabasePathLabel.Text = Resources.DatabasePath;
				_selectDatabasePathOpenFileDialog.Filter = Resources.OpenFileDialogFilter;
			}
		}

		public override void BuildConnectionString()
		{
			base.BuildConnectionString();

			_csb.DataSource = _selectDatabasePathComboBox.Text;
		}

		public override void OnConnectSucceeded()
		{
			base.OnConnectSucceeded();

			if (!_selectDatabasePathComboBox.Items.Contains(_selectDatabasePathComboBox.Text))
			{
				_selectDatabasePathComboBox.Items.Add(_selectDatabasePathComboBox.Text);
				ConfigSelDb.Instance.SqliteDatabasePathList.Add(_csb.DataSource);
			}
		}

		private void SelectDatabasePathButton_Click(object sender, EventArgs e)
		{
			if (_selectDatabasePathOpenFileDialog.ShowDialog(this) == DialogResult.OK)
				_selectDatabasePathComboBox.Text = _selectDatabasePathOpenFileDialog.FileName;
		}
	}
}