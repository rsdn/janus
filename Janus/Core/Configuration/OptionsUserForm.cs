#pragma warning disable 1692
using System;
using System.IO;
using System.Windows.Forms;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Summary description for OptionsUserForm.
	/// </summary>
	public partial class OptionsUserForm : Form
	{
		#region Declarations

		private readonly IServiceProvider _serviceProvider;

		/// <summary>
		/// для хранения флага определяющего запущено ли приложение впервые
		/// </summary>
		private readonly bool _isFirstRun;

		private string _dbDriver;
		private string _connStr;
		#endregion

		#region Constructor(s)

		/// <summary>
		/// Конструктор формы
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <param name="firstrun">флаг определяющий запущено ли приложение впервые</param>
		public OptionsUserForm(IServiceProvider serviceProvider, bool firstrun)
		{
			InitializeComponent();

			_serviceProvider = serviceProvider;
			_isFirstRun	  = firstrun;

			_dbDriver = Config.Instance.DbDriver;
			_connStr   = Config.Instance.ConnectionString;

			CustomInitializeComponent();
		}

		#endregion

		#region Дополнительная инициализация

		/// <summary>
		/// дополнительная инициализация компонентов
		/// Config.OptionsUserForm.
		/// </summary>
		private void CustomInitializeComponent()
		{
			txtUserPsw.PasswordChar = (char)0x25CF;
			if (_isFirstRun)
			{
				Text = SR.Config.OptionsUserForm.NewUser;
				lblNewUserMessage.Text = ApplicationInfo.ApplicationName + SR.Config.OptionsUserForm.FirstRunMessage;

				var janusDir = EnvironmentHelper.GetJanusRootDir();
				txtPathToDb.Text =
					"{0}\\{1}"
					.FormatStr(new FileInfo(janusDir).DirectoryName, Environment.UserName);
				txtUserName.Text = Environment.UserName;
				LocalUser.CreateUser(_serviceProvider, txtPathToDb.Text, txtUserName.Text, txtUserPsw.Text);
			}
			else
			{
				Text = SR.Config.OptionsUserForm.FormOptions;
				lblNewUserMessage.Visible = false;

				txtPathToDb.Text = LocalUser.DatabasePath;
				txtUserName.Text = Config.Instance.Login;
				txtUserPsw.Text = LocalUser.UserPassword;

				txtConstr.Text = Config.Instance.ConnectionString;
			}
		}

		#endregion

		#region Управляющие методы

		/// <summary>
		/// сохраняет или создает настройки для пользователя
		/// </summary>
		private void SaveUserInfo()
		{
			if (_isFirstRun)
			{
				Config.Instance.DbDriver		 = _dbDriver;
				Config.Instance.ConnectionString = _connStr;
				LocalUser.CreateUser(_serviceProvider, txtPathToDb.Text, txtUserName.Text,
					txtUserPsw.Text);
			}
			else
			{
				var odbp = LocalUser.DatabasePath;
				// Если меняем каталог с базой, то перед сменой каталога сохраняем конфигурацию
				if (txtPathToDb.Text != odbp)
					Config.Save();

				if (!LocalUser.IsDbAndCfgExists(txtPathToDb.Text))
				{
					LocalUser.CreateUser(_serviceProvider, txtPathToDb.Text, txtUserName.Text,
						txtUserPsw.Text);
				}
				else
				{
					LocalUser.DatabasePath = txtPathToDb.Text;
					// Если каталог изменен, то загружаем существующую конфигурацию
					if (LocalUser.DatabasePath != odbp)
						Config.Reload();
					Config.Instance.Login = txtUserName.Text;
					LocalUser.UserPassword = txtUserPsw.Text;
					Config.Instance.DbDriver		 = _dbDriver;
					Config.Instance.ConnectionString = _connStr;
					Config.Save();
				}

			}
		}

		#endregion

		#region Обработка событий GUI

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			using (var fbd = new FolderBrowserDialog())
			{
			  fbd.Description = SR.Config.OptionsUserForm.UserFolder;
			  if (Directory.Exists(txtPathToDb.Text))
				fbd.SelectedPath = txtPathToDb.Text;
			  if (fbd.ShowDialog(this) == DialogResult.OK)
				txtPathToDb.Text = fbd.SelectedPath;
			}
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			SaveUserInfo();
		}

		#endregion

		private void btnSelectDb_Click(object sender, EventArgs e)
		{
			using (var seldb = new SelectDB(_serviceProvider))
			{
				if (seldb.ShowDialog(this) != DialogResult.OK || seldb.ConnectionString == "")
					return;
				_dbDriver = seldb.DbDriver;
				_connStr   = seldb.ConnectionString;
				txtConstr.Text = _connStr;
			}
		}
	}
}
