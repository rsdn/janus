namespace Rsdn.Janus
{
	/// <summary>
	/// Менеджер расширений.
	/// </summary>
	public interface IJanusExtensionManager
	{
		IExtensionInfoProvider[] GetInfoProviders();
	}
}