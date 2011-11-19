using System.Xml.Schema;

namespace Rsdn.Janus
{
	public static class CommonTypesSchema
	{
		public static readonly XmlSchema Schema =
			XmlSchema.Read(
				typeof(CommonTypesSchema)
					.Assembly
					.GetRequiredResourceStream("Rsdn.Janus.Utils.CommonTypes.xsd"),
				null);
	}
}