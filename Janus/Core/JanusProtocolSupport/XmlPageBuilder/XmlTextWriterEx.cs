using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace Rsdn.Janus
{
	internal class XmlTextWriterEx : XmlTextWriter
	{
		public XmlTextWriterEx(Stream stream, System.Text.Encoding encoding)
			: base(stream, encoding)
		{ }
		
		// диапазон 0x00-0x1f исключая 0x09, 0x0a, 0x0d
		private static readonly Regex _reInvalidXmlCharacters = new Regex(
			"[\u0000\u0001\u0002\u0003\u0004\u0005\u0006\u0007\u0008"
			+ "\u000b\u000c\u000e\u000f"
			+ "\u0010\u0011\u0012\u0013\u0014\u0015\u0016\u0017\u0018\u0019"
			+ "\u001a\u001b\u001c\u001d\u001e\u001f]",
			RegexOptions.Compiled
			);
		
		public override void WriteString(string text)
		{
			base.WriteString(
				_reInvalidXmlCharacters.Replace(text, "\x25a1")
				);
		}
	}
}
