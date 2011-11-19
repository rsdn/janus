using System;
using System.Collections.Generic;

using Rsdn.Janus.Core.TextMacros.LocalTimeMacros;

namespace Rsdn.Janus
{
	/// <summary>
	/// Макрос локального времени.
	/// </summary>
	[TextMacrosProvider]
	internal sealed class LocalTimeMacrosProvider : ITextMacrosProvider
	{
		public IEnumerable<ITextMacros> CreateTextMacroses()
		{
			return new[]
				{
					new TextMacros(
						"datetime",
						LocalTimeMacrosResources.LocalTime,
						serviceProvider => DateTime.Now.ToString(Config.Instance.TagLine.LocalTimeFormat)),
				};
		}
	}
}