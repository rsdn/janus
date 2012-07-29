using System.Collections.Generic;

namespace Rsdn.Janus
{
	public interface IEditorSyntaxExtensibilityService
	{
		IEnumerable<AutocompleteItem> GetAutocompleteList(char charPressed);
		IEnumerable<Highlighting> GetHighlightings(string line);
	}
}