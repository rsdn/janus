using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;


namespace Rsdn.Janus
{
	public class ConfigSelDb
	{
		private static ConfigSelDb _instance;
		private static string _configPath;
		private static XmlSerializer _serializer;

		[XmlIgnore]
		public static ConfigSelDb Instance
		{
			get
			{
				if(_instance == null)
				{
					_configPath = LocalUser.DatabasePath + "\\configSelDb.xml";
					try
					{
						using (FileStream fs = new FileStream(_configPath, FileMode.Open))
							_instance = (ConfigSelDb)Serializer.Deserialize(fs);

						for (int i = _instance._jetServers.Count - 1; i > -1; i--)
						{
							if (!File.Exists(_instance._jetServers[i].ToString()))
								_instance._jetServers.RemoveAt(i);
						}
						for (int i = _instance._fbDatabases.Count - 1; i > -1; i--)
						{
							if (!File.Exists(_instance._fbDatabases[i].ToString()))
								_instance._fbDatabases.RemoveAt(i);
						}
					}
					catch(Exception)
					{
						_instance = new ConfigSelDb();
					}
				}
				return _instance;
			}
		}

		private static XmlSerializer Serializer
		{
			get
			{
				if(_serializer == null)
					_serializer = new XmlSerializer(typeof(ConfigSelDb));
				return _serializer;
			}
		}

		public void Save()
		{
			try
			{
				using (TextWriter writer = new StreamWriter(_configPath))
					Serializer.Serialize(writer, _instance);
			}
			catch
			{ }
		}

		private ArrayList _mssqlServers = new ArrayList();

		public ArrayList MssqlServers
		{
			get{ return _mssqlServers; }
			set{ _mssqlServers = value; }
		}

		private List<string> _jetServers = new List<string>();
		public List<string> JetServers
		{
			get{ return _jetServers; }
			set{ _jetServers = value; }
		}

		private ArrayList _fbServerTypes = new ArrayList();

		public ArrayList FbServerTypes
		{
			get{ return _fbServerTypes; }
			set{ _fbServerTypes = value; }
		}

		private ArrayList _fbServers = new ArrayList();

		public ArrayList FbServers
		{
			get{ return _fbServers; }
			set{ _fbServers = value; }
		}

		private ArrayList _fbDatabases = new ArrayList();

		public ArrayList FbDatabases
		{
			get{ return _fbDatabases; }
			set{ _fbDatabases = value; }
		}

		private ArrayList _fbDialects = new ArrayList();

		public ArrayList FbDialects
		{
			get{ return _fbDialects; }
			set{ _fbDialects = value; }
		}

		private List<string> _sqliteDatabasePathList = new List<string>();
		public List<string> SqliteDatabasePathList
		{
			get { return _sqliteDatabasePathList; }
			set { _sqliteDatabasePathList = value; }
		}
	}
}
