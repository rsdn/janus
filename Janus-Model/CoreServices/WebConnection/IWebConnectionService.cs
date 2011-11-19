using System;
using System.Net;

namespace Rsdn.Janus
{
	/// <summary>
	/// Some service functionality around web connection
	/// </summary>
	public interface IWebConnectionService
	{
		IWebConnectionConfig GetConfig();
		ICredentials ProxyDemandAuth(Action check, Action abort);
		void AuthOnProxy(Action check, Action abort);
	}
}