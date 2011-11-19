using System.Xml.Serialization;

namespace Rsdn.Janus
{
	/// <summary>
	/// Элемент схемы с именем.
	/// </summary>
	public class DbsmNamedElement
	{
		[XmlAttribute("name")]
		public string Name { get; set; }
	}
}
