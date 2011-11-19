using System;

namespace Rsdn.Janus
{
	public interface IProxySettings
	{
		Uri ProxyUri { get; }
		string Login { get; set; }
		string EncodedPassword { get; set; }
	}
}