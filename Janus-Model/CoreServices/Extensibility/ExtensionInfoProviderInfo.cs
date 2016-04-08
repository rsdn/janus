using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Информация о провайдере информации о расширении.
	/// </summary>
	public class ExtensionInfoProviderInfo
	{
		public ExtensionInfoProviderInfo(Type providerType)
		{
			ProviderType = providerType;
		}

		public Type ProviderType { get; }
	}
}