using System.Xml.Serialization;

namespace Rsdn.Janus
{
	public class DBGenerator : SchemaNamedElement
	{
		[XmlAttribute("start-value")]
		public int StartValue { get; set; }
	}
}