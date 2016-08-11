namespace Rsdn.Janus
{
	public interface IWebConnectionConfig
	{
		string WebServiceUrl { get; }

		bool UseCompression { get; }
		int HttpTimeout { get; }
		int RetriesCount { get; }
		IProxyConfig ProxyConfig { get; }
	}
}