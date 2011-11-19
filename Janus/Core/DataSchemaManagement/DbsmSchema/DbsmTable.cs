using System.ComponentModel;
using System.Xml.Serialization;

namespace Rsdn.Janus
{
	public class DbsmTable : DbsmNamedElement
	{
		private const DbsmTableType _defaultType = DbsmTableType.Table;
		private DbsmTableType _type = _defaultType;

		[XmlAttribute("type")]
		[DefaultValue(_defaultType)]
		public DbsmTableType Type
		{
			get { return _type; }
			set { _type = value; }
		}

		[XmlElement("column")]
		public DbsmColumn[] Columns { get; set; }

		[XmlElement("index")]
		public DbsmIndex[] Indexes { get; set; }

		[XmlElement("key")]
		public DbsmKey[] Keys { get; set; }

		public DbsmTable Clone()
		{
			return new DbsmTable
				{
					Name = Name,
					Type = Type,
					Columns = Columns,
					Indexes = Indexes,
					Keys = Keys
				};
		}
	}
}
