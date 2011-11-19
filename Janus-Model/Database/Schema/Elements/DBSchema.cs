using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Rsdn.Janus
{
	[XmlRoot("DbSchema", Namespace = "", IsNullable = false)]
	public class DBSchema : SchemaNamedElement
	{
		private List<DBGenerator> _generators = new List<DBGenerator>();
		private List<TableSchema> _tables = new List<TableSchema>();

		[XmlAttribute("version")]
		public int Version { get; set; }

		[XmlElement("table", IsNullable = false)]
		public List<TableSchema> Tables
		{
			get { return _tables; }
			set { _tables = value; }
		}

		[XmlElement("generator", IsNullable = false)]
		public List<DBGenerator> Generators
		{
			get { return _generators; }
			set { _generators = value; }
		}

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