using System;
using System.Drawing;
using System.Windows.Forms;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Summary description for SelectDB.
	/// </summary>
	public partial class SelectDB: JanusBaseForm
	{
		#region Helper classes

		private class DbDriverInfo
		{
			private readonly string _driverName;

			internal DbDriverInfo(string driverName)
			{
				_driverName = driverName;
			}

			public string DriverName
			{
				get { return _driverName; }
			}

			public IDBDriver Driver { get; internal set; }

			public IDBConfigControl ConfigControl { get; internal set; }
		}

		#endregion

		private DbDriverInfo _curDriverInfo;
		private readonly IDBDriverManager _dbDriverManager;

		/// <summary>
		/// Для дизайнера. Не использовать.
		/// </summary>
		[Obsolete]
		public SelectDB()
		{ }

		public SelectDB(IServiceProvider provider)
		{
			InitializeComponent();
			ConfigSelDb.Instance.Save();

			tbcEngine.SuspendLayout();
			try
			{
				_dbDriverManager = provider.GetService<IDBDriverManager>();
				foreach (var info in provider.GetRequiredService<IDBDriverManager>().GetRegisteredDriverInfos())
				{
					var tabPage  = new TabPage(info.GetDisplayName())
						{
							Location = Point.Empty,
							Size = Size.Empty,
							Tag = new DbDriverInfo(info.Name)
						};
					tbcEngine.TabPages.Add(tabPage);
				}
			}
			finally
			{
				tbcEngine.ResumeLayout(false);
			}

			if (tbcEngine.SelectedTab == null)
				throw new InvalidOperationException("There are no db drivers found");

			TearoffDbDriver(tbcEngine, new TabControlCancelEventArgs(tbcEngine.SelectedTab, 0, false, TabControlAction.Selecting));
		}

		public string DbDriver
		{
			get { return _curDriverInfo.DriverName; }
		}

		private string _connectionString = string.Empty;
		public string  ConnectionString
		{
			get { return _connectionString; }
		}

		private void tbcEngine_SelectedIndexChanged(object sender, EventArgs e)
		{
			_curDriverInfo = (DbDriverInfo)tbcEngine.SelectedTab.Tag;
			UpdateState(_curDriverInfo.ConfigControl);
		}

		private void btnConnect_Click(object sender, EventArgs e)
		{
			ConnectDatabase(_curDriverInfo);
		}

		private void btnCreateDatabase_Click(object sender, EventArgs e)
		{
			CreateDatabase(_curDriverInfo);
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			ConfigSelDb.Instance.Save();
		}

		private void ConfigConnectionStringChanged(object sender, EventArgs e)
		{
			UpdateState((IDBConfigControl)sender);
		}

		private void UpdateState(IDBConfigControl configControl)
		{
			_connectionString = configControl.ConnectionString;

			btnConnect.BackColor = configControl.ConnectSuccess
				? Color.FromArgb(220, 255, 220)
				: Color.FromArgb(255, 192, 192);
			btnOK.Enabled = configControl.ConnectSuccess;
		}

		private void ConnectDatabase(DbDriverInfo info)
		{
			try
			{
				info.ConfigControl.BuildConnectionString();
				if (info.Driver.CheckConnectionString(info.ConfigControl.ConnectionString))
					info.ConfigControl.OnConnectSucceeded();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message,
					ApplicationInfo.ApplicationName,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			UpdateState(info.ConfigControl);
		}

		private void CreateDatabase(DbDriverInfo info)
		{
			try
			{
				if (!info.ConfigControl.PrepareCreateConnectionString())
					return;
				info.Driver.CreateSchemaDriver().CreateDatabase(info.ConfigControl.ConnectionString);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message,
					ApplicationInfo.ApplicationName,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			ConnectDatabase(info);

			MessageBox.Show(this, "Database created successfully.",
				ApplicationInfo.ApplicationName,
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void TearoffDbDriver(object sender, TabControlCancelEventArgs e)
		{
			if (e.TabPage.Controls.Count != 0)
			{
				// Already loaded.
				//
				return;
			}

			try
			{
				var info  = (DbDriverInfo) e.TabPage.Tag;
				info.Driver        = _dbDriverManager.GetDriver(info.DriverName);
				info.ConfigControl = info.Driver.CreateConfigControl();

				info.ConfigControl.DockInPanel(e.TabPage);

				info.ConfigControl.CustomInitialize(true);
				info.ConfigControl.ConnectionStringChanged += ConfigConnectionStringChanged;

				_curDriverInfo = (DbDriverInfo)tbcEngine.SelectedTab.Tag;
				UpdateState(info.ConfigControl);
			}
			catch (Exception ex)
			{
				var errorLabel = new Label
					{
						Text = ex.GetBaseException().Message,
						Dock = DockStyle.Fill
					};

				e.TabPage.Controls.Add(errorLabel);
			}
		}
	}
}
