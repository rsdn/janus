using System;

namespace Rsdn.Janus
{
	public class MessageFormatterInfo
	{
		private readonly Type _formatterType;
		private readonly bool _formatSource;
		private readonly bool _formatHtml;

		public MessageFormatterInfo(Type formatterType, bool formatSource, bool formatHtml)
		{
			_formatterType = formatterType;
			_formatSource = formatSource;
			_formatHtml = formatHtml;
		}

		public Type FormatterType
		{
			get { return _formatterType; }
		}

		public bool FormatSource
		{
			get { return _formatSource; }
		}

		public bool FormatHtml
		{
			get { return _formatHtml; }
		}
	}
}