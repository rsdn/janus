namespace Rsdn.Janus
{
	public interface IWebConnectionConfig
	{
		bool UseCompression { get; }
		int HttpTimeout { get; }
		int RetriesCount { get; }
		IProxyConfig ProxyConfig { get; }
	}
}