using System.Xml.Serialization;

namespace Rsdn.Janus
{
	public class TableSchema : SchemaNamedElement
	{
		[XmlElement("column")]
		public TableColumnSchema[] Columns { get; set; }

		[XmlElement("index")]
		public IndexSchema[] Indexes { get; set; }

		[XmlElement("key")]
		public KeySchema[] Keys { get; set; }

		public TableSchema Clone()
		{
			return
				new TableSchema
					{
						Name = Name,
						Columns = Columns,
						Indexes = Indexes,
						Keys = Keys
					};
		}
	}
}