using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Rsdn.Janus
{
	/// <summary>
	/// Настройки для прокси при синхронизации
	/// </summary>
	[TypeConverter(typeof (ExpandableObjectConverter))]
	public class ProxySettings : IProxySettings
	{
		private string _address = String.Empty;
		private string _encodedPassword = string.Empty;
		private string _login = string.Empty;

		private int _port;

		[JanusDisplayName(SR.Config.Proxy.Settings.Address.DisplayNameResourceName)]
		[JanusDescription(SR.Config.Proxy.Settings.Address.DescriptionResourceName)]
		[SortIndex(0)]
		public string Address
		{
			get { return _address; }
			set { _address = value; }
		}

		[JanusDisplayName(SR.Config.Proxy.Settings.Port.DisplayNameResourceName)]
		[JanusDescription(SR.Config.Proxy.Settings.Port.DescriptionResourceName)]
		[SortIndex(1)]
		public int Port
		{
			get { return _port; }
			set { _port = value; }
		}

		[XmlIgnore]
		[Browsable(false)]
		public Uri ProxyUri
		{
			get { return new Uri(String.Format("http://{0}:{1}/", _address, _port)); }
		}

		[Browsable(false)]
		public string Login
		{
			get { return _login; }
			set { _login = value; }
		}

		[XmlIgnore]
		[Browsable(false)]
		public bool SaveAuth { get; set; }

		[Browsable(false)]
		public bool IsPasswordAlreadySaved { get; set; }

		[Browsable(false)]
		public string EncodedPassword
		{
			get { return _encodedPassword; }
			set { _encodedPassword = value; }
		}

		public override string ToString()
		{
			return String.IsNullOrEmpty(_address)
				? string.Empty
				: ProxyUri.ToString();
		}
	}
}