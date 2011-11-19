using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Информация о провайдере информации о расширении.
	/// </summary>
	public class ExtensionInfoProviderInfo
	{
		private readonly Type _providerType;

		public ExtensionInfoProviderInfo(Type providerType)
		{
			_providerType = providerType;
		}

		public Type ProviderType
		{
			get { return _providerType; }
		}
	}
}