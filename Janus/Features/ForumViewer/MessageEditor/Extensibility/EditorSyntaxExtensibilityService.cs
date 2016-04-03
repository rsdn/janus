using System;
using System.Collections.Generic;
using System.Linq;

using CodeJam.Collections;
using CodeJam.Extensibility;
using CodeJam.Extensibility.Instancing;

namespace Rsdn.Janus
{
	[Service(typeof(IEditorSyntaxExtensibilityService))]
	public class EditorSyntaxExtensibilityService : IEditorSyntaxExtensibilityService
	{
		private readonly Lazy<IAutocompleteProvider[]> _acProviders;
		private readonly Lazy<IHighlightingProvider[]> _hlProviders;

		public EditorSyntaxExtensibilityService(IServiceProvider provider)
		{
			_acProviders =
				new Lazy<IAutocompleteProvider[]>(
					() =>
						provider
							.GetRegisteredElements<AutocompleteProviderInfo>()
							.Select(info => info.ProviderType.CreateInstance<IAutocompleteProvider>(provider))
							.ToArray());
			_hlProviders =
				new Lazy<IHighlightingProvider[]>(
					() =>
						provider
							.GetRegisteredElements<EditorHighlightingProviderInfo>()
							.Select(info => info.ProviderType.CreateInstance<IHighlightingProvider>(provider))
							.ToArray());
		}

		public IEnumerable<AutocompleteItem> GetAutocompleteList(char charPressed)
		{
			return
				_acProviders
					.Value
					.SelectMany(prov => prov.GetAutocompleteList(charPressed) ?? Array<AutocompleteItem>.Empty);
		}

		public IEnumerable<Highlighting> GetHighlightings(string line)
		{
			return _hlProviders.Value.SelectMany(prov => prov.GetHighlightings(line));
		}
	}
}