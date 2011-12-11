using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	/// <summary>
	/// Помечает форматтеры сообщения.
	/// </summary>
	[MeansImplicitUse]
	[BaseTypeRequired(typeof (IMessageFormatter))]
	public class MessageFormatterAttribute : Attribute
	{
		public MessageFormatterAttribute()
		{
			FormatSource = true;
			FormatHtml = true;
		}

		public bool FormatSource { get; set; }
		public bool FormatHtml { get; set; }
	}
}