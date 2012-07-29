using System.Collections.Generic;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public interface IAutocompleteProvider
	{
		[CanBeNull]
		IEnumerable<AutocompleteItem> GetAutocompleteList(char charPressed);
	}
}