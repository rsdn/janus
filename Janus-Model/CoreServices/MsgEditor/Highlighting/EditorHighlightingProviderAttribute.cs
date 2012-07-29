using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	[MeansImplicitUse]
	[BaseTypeRequired(typeof(IHighlightingProvider))]
	public class EditorHighlightingProviderAttribute : Attribute
	{}
}