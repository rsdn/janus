using System;
using System.Collections.Generic;

using Rsdn.Janus.Core.TextMacros.CitationMacros;

namespace Rsdn.Janus
{
	/// <summary>
	/// Макрос для вставки цитаты.
	/// </summary>
	[TextMacrosProvider]
	internal sealed class CitationMacrosProvider : ITextMacrosProvider
	{
		private static string GetValue(IServiceProvider serviceProvider)
		{
			var citations = Config.Instance.TagLine.CitationTagConfig.Citations;
			if (citations == null || citations.Length < 1)
				return "<no any citation>";

			if (Config.Instance.TagLine.CitationTagConfig.QueryType == CitationQueryType.Random)
			{
				var rnd = new Random();
				var cp = rnd.Next(citations.Length);
				return citations[cp];
			}

			if (Config.Instance.TagLine.CitationTagConfig.LastPosition >= citations.Length)
				Config.Instance.TagLine.CitationTagConfig.LastPosition = 0;
			return citations[Config.Instance.TagLine.CitationTagConfig.LastPosition];
		}

		public IEnumerable<ITextMacros> CreateTextMacroses()
		{
			return new[] { new TextMacros("citation", CitationMacrosResources.Citation, GetValue) };
		}
	}
}
