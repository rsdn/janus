using System.Collections.Generic;

namespace Rsdn.Janus.Admin
{
	[AutocompleteProvider]
	internal class ModeratorAutocompleteProvider : IAutocompleteProvider
	{
		public IEnumerable<AutocompleteItem> GetAutocompleteList(char charPressed)
		{
			if (charPressed == '[')
				yield return new AutocompleteItem("[moderator][/moderator]", text => text.IndexOf("]") + 1);
		}
	}
}