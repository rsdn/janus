namespace Rsdn.Janus
{
	internal class WebConnectionConfig : IWebConnectionConfig
	{
		private readonly bool _useCompression;
		private readonly int _httpTimeout;
		private readonly int _retriesCount;
		private readonly IProxyConfig _proxyConfig;

		public WebConnectionConfig(bool useCompression, int httpTimeout, int retriesCount, IProxyConfig proxyConfig)
		{
			_useCompression = useCompression;
			_httpTimeout = httpTimeout;
			_retriesCount = retriesCount;
			_proxyConfig = proxyConfig;
		}

		public bool UseCompression
		{
			get { return _useCompression; }
		}

		public int HttpTimeout
		{
			get { return _httpTimeout; }
		}

		public int RetriesCount
		{
			get { return _retriesCount; }
		}

		public IProxyConfig ProxyConfig
		{
			get { return _proxyConfig; }
		}
	}
}