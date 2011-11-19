using System.Collections;
using System.IO;
using System.Xml.Serialization;

namespace Rsdn.Utils.SvnEntries
{
	[XmlRoot("wc-entries", Namespace = "svn:")]
	public class WCEntries
	{
		private static XmlSerializer _serializer;

		private ArrayList _entries = new ArrayList();

		[XmlElement("entry", typeof (Entry))]
		public ArrayList Entries
		{
			get { return _entries; }
			set { _entries = value; }
		}

		public static WCEntries FromFile(string fileName)
		{
			if (_serializer == null)
				_serializer = new XmlSerializer(typeof (WCEntries));
			using (StreamReader sr = new StreamReader(fileName))
				return (WCEntries)_serializer.Deserialize(sr);
		}
	}
}
