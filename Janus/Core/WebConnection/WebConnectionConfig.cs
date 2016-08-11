namespace Rsdn.Janus
{
	internal class WebConnectionConfig : IWebConnectionConfig
	{
		public WebConnectionConfig(
			string webServiceUrl,
			bool useCompression,
			int httpTimeout,
			int retriesCount,
			IProxyConfig proxyConfig)
		{
			WebServiceUrl = webServiceUrl;
			UseCompression = useCompression;
			HttpTimeout = httpTimeout;
			RetriesCount = retriesCount;
			ProxyConfig = proxyConfig;
		}

		public string WebServiceUrl { get; }

		public bool UseCompression { get; }

		public int HttpTimeout { get; }

		public int RetriesCount { get; }

		public IProxyConfig ProxyConfig { get; }
	}
}