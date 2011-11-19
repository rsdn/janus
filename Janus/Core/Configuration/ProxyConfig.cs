using System;
using System.ComponentModel;
using System.Net;
using Rsdn.Janus.Core.Synchronization;

namespace Rsdn.Janus
{
	/// <summary>
	/// Конфигурация прокси.
	/// </summary>
	[TypeConverter(typeof (ExpandableObjectConverter))]
	public class ProxyConfig : IProxyConfig
	{
		private const bool _defaultCustomUseAuthProxy = false;
		private const UseProxyType _defaultUseProxyType = UseProxyType.UseIESettings;
		private static readonly Uri _testProxyUri = new Uri("http://rsdn.ru/");
		private ProxySettings _proxySettings = new ProxySettings();
		private UseProxyType _useProxyType = _defaultUseProxyType;

		[JanusDisplayName(SR.Config.Proxy.UseType.DisplayNameResourceName)]
		[JanusDescription(SR.Config.Proxy.UseType.DescriptionResourceName)]
		[SortIndex(0)]
		[DefaultValue(_defaultUseProxyType)]
		public UseProxyType UseProxyType
		{
			get { return _useProxyType; }
			set { _useProxyType = value; }
		}

		[JanusDisplayName(SR.Config.Proxy.UseAuthorization.DisplayNameResourceName)]
		[JanusDescription(SR.Config.Proxy.UseAuthorization.DescriptionResourceName)]
		[SortIndex(1)]
		[DefaultValue(_defaultCustomUseAuthProxy)]
		public bool UseCustomAuthProxy { get; set; }

		[JanusDisplayName(SR.Config.Proxy.Settings.DisplayNameResourceName)]
		[JanusDescription(SR.Config.Proxy.Settings.DescriptionResourceName)]
		[SortIndex(2)]
		public ProxySettings ProxySettings
		{
			get { return _proxySettings; }
			set { _proxySettings = value; }
		}

		IProxySettings IProxyConfig.ProxySettings
		{
			get { return ProxySettings; }
		}

		public override string ToString()
		{
			switch (_useProxyType)
			{
				case UseProxyType.NoUse:
					break;

				case UseProxyType.UseIESettings:
					var proxy = WebRequest.GetSystemWebProxy();
					if (!proxy.IsBypassed(_testProxyUri))
						return proxy.GetProxy(_testProxyUri).ToString();
					break;

				case UseProxyType.UseCustomSettings:
					return ProxySettings.ToString();
			}
			return SyncResources.ProxyDirectConnection;
		}
	}
}