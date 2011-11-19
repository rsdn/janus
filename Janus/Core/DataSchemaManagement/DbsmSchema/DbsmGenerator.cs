using System.Xml.Serialization;

namespace Rsdn.Janus
{

	public class DbsmGenerator : DbsmNamedElement
	{
		[XmlAttribute("start-value")]
		public int StartValue { get; set; }
	}

}
