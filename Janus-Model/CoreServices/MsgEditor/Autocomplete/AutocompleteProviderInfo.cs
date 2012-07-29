using System;

namespace Rsdn.Janus
{
	public class AutocompleteProviderInfo
	{
		private readonly Type _providerType;

		public AutocompleteProviderInfo(Type providerType)
		{
			_providerType = providerType;
		}

		public Type ProviderType
		{
			get { return _providerType; }
		}
	}
}