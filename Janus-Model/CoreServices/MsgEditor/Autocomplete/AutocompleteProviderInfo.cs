using System;

namespace Rsdn.Janus
{
	public class AutocompleteProviderInfo
	{
		public AutocompleteProviderInfo(Type providerType)
		{
			ProviderType = providerType;
		}

		public Type ProviderType { get; }
	}
}