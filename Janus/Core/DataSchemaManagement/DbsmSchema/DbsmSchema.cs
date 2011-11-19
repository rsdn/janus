using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;

namespace Rsdn.Janus
{
	[XmlRootAttribute("DbSchema", Namespace="", IsNullable=false)]
	public class DbsmSchema : DbsmNamedElement
	{
		[XmlAttribute]
		public DbEngineType DbEngine { get; set; }

		[XmlAttribute("version")]
		public int Version { get; set; }

		private List<DbsmTable> _tables = new List<DbsmTable>();
		[XmlElement("table", IsNullable = false)]
		public List<DbsmTable> Tables
		{
			get { return _tables; }
			set { _tables = value; }
		}

		private List<DbsmGenerator> _generators = new List<DbsmGenerator>();
		[XmlElement("generator", IsNullable = false)]
		public List<DbsmGenerator> Generators
		{
			get { return _generators; }
			set { _generators = value; }
		}

		#region Methods
		public DbsmSchema Copy()
		{
			var serializer = new XmlSerializer(typeof(DbsmSchema));
			var ms = new MemoryStream();
			serializer.Serialize(ms,this);
			ms.Position = 0;
			var dbsc = (DbsmSchema)serializer.Deserialize(ms);
			dbsc.Normalize();
			return dbsc;
		}

		public void Normalize()
		{
			foreach (var table in Tables)
			{
				if (table.Columns == null)
					table.Columns = new DbsmColumn[0];
				if (table.Indexes == null)
					table.Indexes = new DbsmIndex[0];
				if (table.Keys == null)
					table.Keys = new DbsmKey[0];
			}
		}
		#endregion
	}
}
