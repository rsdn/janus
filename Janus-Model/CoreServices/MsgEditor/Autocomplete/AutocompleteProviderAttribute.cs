using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	[BaseTypeRequired(typeof(IAutocompleteProvider))]
	[MeansImplicitUse]
	public class AutocompleteProviderAttribute : Attribute
	{}
}