using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Rsdn.Janus
{
	public partial class CurrentData
	{
		private const string _folderName = "Janus";
		private const string _fileName = "CurrentData.xml";

		private static XmlSerializer _serializer = new XmlSerializer(typeof (CurrentData));

		private static CurrentData _instance;

		public static CurrentData Instance
		{
			get
			{
				if (_instance == null)
					_instance = LoadInstance();
				return _instance;
			}
		}

		private void Save()
		{
			using (FileStream fs = new FileStream(GetFileName(), FileMode.Create))
				_serializer.Serialize(fs, this);
		}

		private CurrentData()
		{ }

		private static CurrentData LoadInstance()
		{
			string fn = GetFileName();
			if (!File.Exists(fn))
				return new CurrentData();
			using (FileStream fs = new FileStream(GetFileName(), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				return (CurrentData)_serializer.Deserialize(fs);
		}

		private static string GetFileName()
		{
			string path = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
					_folderName);
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
			return path + @"\" + _fileName;
		}
	}
}
