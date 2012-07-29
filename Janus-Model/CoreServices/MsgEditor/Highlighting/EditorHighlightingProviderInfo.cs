using System;

namespace Rsdn.Janus
{
	public class EditorHighlightingProviderInfo
	{
		private readonly Type _providerType;

		public EditorHighlightingProviderInfo(Type providerType)
		{
			_providerType = providerType;
		}

		public Type ProviderType
		{
			get { return _providerType; }
		}
	}
}