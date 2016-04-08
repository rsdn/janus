using System;

namespace Rsdn.Janus
{
	public class MessageFormatterInfo
	{
		public MessageFormatterInfo(Type formatterType, bool formatSource, bool formatHtml)
		{
			FormatterType = formatterType;
			FormatSource = formatSource;
			FormatHtml = formatHtml;
		}

		public Type FormatterType { get; }

		public bool FormatSource { get; }

		public bool FormatHtml { get; }
	}
}