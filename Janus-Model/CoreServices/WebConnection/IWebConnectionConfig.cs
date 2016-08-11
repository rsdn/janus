namespace Rsdn.Janus
{
	public interface IWebConnectionConfig
	{
		string SiteUrl { get; }

		bool UseCompression { get; }
		int HttpTimeout { get; }
		int RetriesCount { get; }
		IProxyConfig ProxyConfig { get; }
	}
}