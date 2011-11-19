using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

using Microsoft.Win32;

namespace Rsdn.Janus
{
	public class ConfigSelDb
	{
		private const string _keyName = "DBPath";
		private const string _keyPath = @"Software\Rsdn\Janus\LocalUser";
		private static string _configPath;
		private static ConfigSelDb _instance;
		private static XmlSerializer _serializer;
		private ArrayList _fbDatabases = new ArrayList();
		private ArrayList _fbDialects = new ArrayList();
		private ArrayList _fbServers = new ArrayList();
		private ArrayList _fbServerTypes = new ArrayList();
		private List<string> _jetServers = new List<string>();
		private ArrayList _mssqlServers = new ArrayList();
		private List<string> _sqliteDatabasePathList = new List<string>();

		[XmlIgnore]
		public static ConfigSelDb Instance
		{
			get
			{
				if (_instance == null)
				{
					// Временное решение до перехода на профили
					using (var key = Registry.CurrentUser.OpenSubKey(_keyPath))
						if (key != null)
							_configPath = key.GetValue(_keyName) + "\\configSelDb.xml";
					try
					{
						using (var fs = new FileStream(_configPath, FileMode.Open))
							_instance = (ConfigSelDb)Serializer.Deserialize(fs);

						for (var i = _instance._jetServers.Count - 1; i > -1; i--)
							if (!File.Exists(_instance._jetServers[i]))
								_instance._jetServers.RemoveAt(i);
						for (var i = _instance._fbDatabases.Count - 1; i > -1; i--)
							if (!File.Exists(_instance._fbDatabases[i].ToString()))
								_instance._fbDatabases.RemoveAt(i);
					}
					catch (Exception)
					{
						_instance = new ConfigSelDb();
					}
				}
				return _instance;
			}
		}

		private static XmlSerializer Serializer
		{
			get { return _serializer ?? (_serializer = new XmlSerializer(typeof (ConfigSelDb))); }
		}

		public ArrayList MssqlServers
		{
			get { return _mssqlServers; }
			set { _mssqlServers = value; }
		}

		public List<string> JetServers
		{
			get { return _jetServers; }
			set { _jetServers = value; }
		}

		public ArrayList FbServerTypes
		{
			get { return _fbServerTypes; }
			set { _fbServerTypes = value; }
		}

		public ArrayList FbServers
		{
			get { return _fbServers; }
			set { _fbServers = value; }
		}

		public ArrayList FbDatabases
		{
			get { return _fbDatabases; }
			set { _fbDatabases = value; }
		}

		public ArrayList FbDialects
		{
			get { return _fbDialects; }
			set { _fbDialects = value; }
		}

		public List<string> SqliteDatabasePathList
		{
			get { return _sqliteDatabasePathList; }
			set { _sqliteDatabasePathList = value; }
		}

		public void Save()
		{
			try
			{
				using (TextWriter writer = new StreamWriter(_configPath))
					Serializer.Serialize(writer, _instance);
			}
// ReSharper disable EmptyGeneralCatchClause
			catch
// ReSharper restore EmptyGeneralCatchClause
			{}
		}
	}
}