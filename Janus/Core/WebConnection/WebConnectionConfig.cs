namespace Rsdn.Janus
{
	internal class WebConnectionConfig : IWebConnectionConfig
	{
		public WebConnectionConfig(
			string siteUrl,
			bool useCompression,
			int httpTimeout,
			int retriesCount,
			IProxyConfig proxyConfig)
		{
			SiteUrl = siteUrl;
			UseCompression = useCompression;
			HttpTimeout = httpTimeout;
			RetriesCount = retriesCount;
			ProxyConfig = proxyConfig;
		}

		public string SiteUrl { get; }

		public bool UseCompression { get; }

		public int HttpTimeout { get; }

		public int RetriesCount { get; }

		public IProxyConfig ProxyConfig { get; }
	}
}