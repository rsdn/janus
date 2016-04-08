using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Rsdn.Janus
{
	[XmlRoot("DbSchema", Namespace = "", IsNullable = false)]
	public class DBSchema : SchemaNamedElement
	{
		[XmlAttribute("version")]
		public int Version { get; set; }

		[XmlElement("table", IsNullable = false)]
		public List<TableSchema> Tables { get; set; } = new List<TableSchema>();

		[XmlElement("generator", IsNullable = false)]
		public List<DBGenerator> Generators { get; set; } = new List<DBGenerator>();

		#region Methods
		public DBSchema Copy()
		{
			var serializer = new XmlSerializer(typeof (DBSchema));
			var ms = new MemoryStream();
			serializer.Serialize(ms, this);
			ms.Position = 0;
			var dbsc = (DBSchema)serializer.Deserialize(ms);
			dbsc.Normalize();
			return dbsc;
		}

		public void Normalize()
		{
			foreach (var table in Tables)
			{
				if (table.Columns == null)
					table.Columns = new TableColumnSchema[0];
				if (table.Indexes == null)
					table.Indexes = new IndexSchema[0];
				if (table.Keys == null)
					table.Keys = new KeySchema[0];
			}
		}
		#endregion
	}
}