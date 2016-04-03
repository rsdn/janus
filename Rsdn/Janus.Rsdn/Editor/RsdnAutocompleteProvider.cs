using System.Collections.Generic;
using System.Linq;

namespace Rsdn.Janus.Autocomplete
{
	[AutocompleteProvider]
	public class RsdnAutocompleteProvider : IAutocompleteProvider
	{
		public IEnumerable<AutocompleteItem> GetAutocompleteList(char charPressed)
		{
			switch (charPressed)
			{
				case '[':
					return
						RsdnSyntaxInfo
							.KnownTags
							.OrderBy(pair => pair.Key)
							.Select(
								pair =>
									pair.Value
										? new AutocompleteItem($"[{pair.Key}][/{pair.Key}]", GetTagShift)
										: new AutocompleteItem("[" + pair.Key + "]"));
				case ':':
					return
						RsdnSyntaxInfo
							.KnownSmiles
							.OrderBy(smile => smile)
							.Select(sm => new AutocompleteItem(sm, null));
			}
			return null;
		}

		private static int GetTagShift(string text)
		{
			return text.IndexOf(']') + 1;
		}
	}
}