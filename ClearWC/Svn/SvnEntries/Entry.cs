using System;
using System.Xml.Serialization;

namespace Rsdn.Utils.SvnEntries
{
	public class Entry
	{
		private int _commitedRevision;
		private string _name;
		private string _textTime;
		private string _commitedDate;
		private string _checksum;
		private string _url;
		private string _lastAuthor;
		private EntryKind _kind;
		private Guid _uuid;
		private string _propTime;
		private int _revision;

		[XmlAttribute("commited-revision")]
		public int CommitedRevision
		{
			get { return _commitedRevision; }
			set { _commitedRevision = value; }
		}

		[XmlAttribute("name")]
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		[XmlAttribute("text-time")]
		public string TextTime
		{
			get { return _textTime; }
			set { _textTime = value; }
		}

		[XmlAttribute("commited-date")]
		public string CommitedDate
		{
			get { return _commitedDate; }
			set { _commitedDate = value; }
		}

		[XmlAttribute("checksum")]
		public string Checksum
		{
			get { return _checksum; }
			set { _checksum = value; }
		}

		[XmlAttribute("url")]
		public string Url
		{
			get { return _url; }
			set { _url = value; }
		}

		[XmlAttribute("last-author")]
		public string LastAuthor
		{
			get { return _lastAuthor; }
			set { _lastAuthor = value; }
		}

		[XmlAttribute("kind")]
		public EntryKind Kind
		{
			get { return _kind; }
			set { _kind = value; }
		}

		[XmlAttribute("uuid")]
		public Guid Uuid
		{
			get { return _uuid; }
			set { _uuid = value; }
		}

		[XmlAttribute("prop-time")]
		public string PropTime
		{
			get { return _propTime; }
			set { _propTime = value; }
		}

		[XmlAttribute("revision")]
		public int Revision
		{
			get { return _revision; }
			set { _revision = value; }
		}
	}
}
