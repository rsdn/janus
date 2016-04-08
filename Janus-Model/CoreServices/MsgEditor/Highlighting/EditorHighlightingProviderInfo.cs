using System;

namespace Rsdn.Janus
{
	public class EditorHighlightingProviderInfo
	{
		public EditorHighlightingProviderInfo(Type providerType)
		{
			ProviderType = providerType;
		}

		public Type ProviderType { get; }
	}
}