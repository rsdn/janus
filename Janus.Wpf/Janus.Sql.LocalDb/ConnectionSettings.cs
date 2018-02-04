using Janus.Model;
using Janus.Model.Gui;
using Janus.Model.Perist;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Janus.Sql.LocalDb {
	public class ConnectionSettings : ModelBase, IConnectionSettings {
		private string _ConnectionString;
		private bool? _IsValid;
		private string _DbFilePath;

		private static HashSet<string> _LocalDbVersions;
		private string _LocalDbServerName = "(localdb)\\mssqllocaldb";

		public bool Build() {
			try {
				if (LocalDbVersions.Count == 0) {
					_IsValid = false;
					return false;
				}
				else if (string.IsNullOrEmpty(DbFilePath)) {
					_IsValid = false;
					return false;
				}
				else if (Path.GetInvalidPathChars().Any(ch => DbFilePath.Contains(ch))) {
					UserInteraction.OnErrorMessage(this, $@"Db file path 
	{DbFilePath}
contains some invalid characters");
					_IsValid = false;
					return false;
				}
				else if (!(LocalDbVersions.Contains(LocalDbServerName)
					|| LocalDbServerName.Equals("(localdb)\\mssqllocaldb", StringComparison.OrdinalIgnoreCase))) {
					_IsValid = false;
					return false;
				}
				else {
					if (!File.Exists(DbFilePath)) {
						var args = new CreateFileConfirmEventArgs(DbFilePath);
						if (CreateFileConfirm == null) {
							args.Cancel = (UserInteraction.OnConfirmationMessage($@"Db file located at
	{DbFilePath}
cannot be found!
Create?",
								"Confirmation") != ConfirmationResult.Yes);
						}
						else {
							CreateFileConfirm(this, args);
						}
						if (args.Cancel) {
							IsValid = false;
							return false;
						}
						else {
							DbFilePath = args.DbFilePath;
							try {
								using (var dbCreateConnection = new SqlConnection($@"Data Source={LocalDbServerName};")) {
									dbCreateConnection.Open();
									var plainName = Path.GetFileNameWithoutExtension(DbFilePath);
									var lofFileName = Path.ChangeExtension(DbFilePath, "log");
									using (var dbCreateCommand = new SqlCommand(
			$@"CREATE DATABASE [{plainName}] ON 
PRIMARY ( NAME={plainName}_data, FILENAME = '{DbFilePath}' ) 
LOG ON ( NAME={plainName}_log, FILENAME = '{lofFileName}' )",
											dbCreateConnection)) {
										dbCreateCommand.ExecuteNonQuery();
									}
								}
							}
							catch (SqlException err) {
								UserInteraction.OnErrorMessage(this, err.ToString());
								_IsValid = false;
								return false;
							}
						}
					}

					using (var checkSqlConnection = new SqlConnection($@"Data Source={LocalDbServerName};AttachDbFilename={DbFilePath};Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30;")) {
						checkSqlConnection.Open();
					}
					_IsValid = true;
					return true;
				}
			}
			finally {
				OnPropertyChanged(nameof(IsValid));
			}
		}

		public string ConnectionString {
			get {
				return _ConnectionString;
			}
			set {
				if (string.Equals(_ConnectionString, value)) {
					return;
				}
				_ConnectionString = value;
				OnPropertyChanged();
			}
		}

		public bool IsValid {
			get { return _IsValid ?? Build(); }
			protected set {
				_IsValid = value;
				OnPropertyChanged();
			}
		}

		public string DbFilePath {
			get {
				return _DbFilePath;
			}
			set {
				_DbFilePath = value;
				OnPropertyChanged();
				_IsValid = null;
				OnPropertyChanged(nameof(IsValid));
			}
		}

		public string LocalDbServerName {
			get { return _LocalDbServerName; }
			set {
				if (string.Equals(_LocalDbServerName, value)) {
					return;
				}
				_LocalDbServerName = value;
				OnPropertyChanged();
				_IsValid = null;
				OnPropertyChanged(nameof(IsValid));
			}
		}

		public static HashSet<string> LocalDbVersions {
			get {
				if (_LocalDbVersions == null) {
					_LocalDbVersions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
					using (var regRoot = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server Local DB\Installed Versions")) {
						foreach (var subKey in regRoot.GetSubKeyNames()) {
							_LocalDbVersions.Add(subKey);
						}
					}
					if (Environment.Is64BitOperatingSystem) {
						using (var regRoot = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Microsoft SQL Server Local DB\Installed Versions")) {
							if (regRoot != null) {
								foreach (var subKey in regRoot.GetSubKeyNames()) {
									_LocalDbVersions.Add(subKey);
								}
							}
						}
					}
				}
				return _LocalDbVersions;
			}
		}

		public event EventHandler<CreateFileConfirmEventArgs> CreateFileConfirm;
	}
}
