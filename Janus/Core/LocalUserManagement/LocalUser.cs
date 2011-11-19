using System;
using System.IO;

using Microsoft.Win32;

using Rsdn.SmartApp.CommandLine;

using System.Linq;

namespace Rsdn.Janus
{
	/// <summary>
	/// Класс реализующий многопользовательские возможности
	/// </summary>
	public class LocalUser
	{
		private const string _keyPath = @"Software\Rsdn\Janus\LocalUser";
		private const string _keyName = "DBPath";

		private string _databasePath = string.Empty;

		private LocalUser()
		{
			var dbFromArgs = GetDbPathFromCommandLine();
			if (dbFromArgs == null)
			{
				using (var luk = Registry.CurrentUser.OpenSubKey(_keyPath)) 
				{
					if (luk == null)
						throw new LocalUserNotFoundException();

					var dbp = luk.GetValue(_keyName);
					if (dbp == null)
						throw new LocalUserNotFoundException();

					_databasePath = dbp.ToString();
				}
			}
			else
			{
				if (!IsDbAndCfgExists(dbFromArgs))
					throw new LocalUserNotFoundException();

				_databasePath = dbFromArgs;
			}
		}

		private const string _dbPathKey = "dbp";

		private static string GetDbPathFromCommandLine()
		{
			var ast = CommandLineHelper.ParseCommandLine(Environment.CommandLine);
			CommandLineHelper.Check(
				ast,
				new CmdLineRules(new OptionRule(_dbPathKey, OptionType.Value, false)));
			var opt = ast.Options.FirstOrDefault(o => o.Text == _dbPathKey);
			return opt == null ? null : opt.Value.Text;
		}

		private static LocalUser _instance;

		private static LocalUser Instance
		{
			get { return _instance ?? (_instance = new LocalUser()); }
		}

		public static bool IsDbAndCfgExists(string dbpath)
		{
			return /*File.Exists(dbpath + @"\janus.mdb") &&*/ File.Exists(dbpath + @"\config.xml");
		}

		public static bool UserExists()
		{
			using (var luk = Registry.CurrentUser.OpenSubKey(_keyPath))
			{
				if (luk != null)
				{
					var dbp = luk.GetValue(_keyName);
					if (dbp != null && IsDbAndCfgExists(dbp.ToString()))
						return true;
				}
			}
			return false;
		}

		#region Новый пользователь

		public static void CreateUser(
			IServiceProvider provider,
			string dbPath,
			string userName,
			string pwd)
		{
			if (!Directory.Exists(dbPath))
				Directory.CreateDirectory(dbPath);

			var key = Registry.CurrentUser.CreateSubKey(_keyPath);
			if (key == null)
				throw new ApplicationException("Could not create registry key");
			using(key)
				key.SetValue(_keyName, dbPath);

			Config.Instance.Login = userName;
			Config.Instance.EncodedPassword = pwd.EncryptPassword();
			//При изменении критических данных сохраняем сразу
			Config.Save();
			_instance = null;
		}

		#endregion

		#region Доступ к данным пользователя

		public static string DatabasePath
		{
			get { return Instance._databasePath; }
			set
			{
				var key = Registry.CurrentUser.CreateSubKey(_keyPath);
				if (key == null)
					throw new ApplicationException("Could not create registry key");
				using (key)
					key.SetValue(_keyName, value);
				Instance._databasePath = value;
			}
		}

		public static string UserPassword
		{
			get { return Config.Instance.EncodedPassword.DecryptPassword(); }
			set { Config.Instance.EncodedPassword = value.EncryptPassword(); }
		}

		public static string LuceneIndexPath
		{
			get { return Path.Combine(DatabasePath, ".index"); }
		}
		#endregion
	}
}