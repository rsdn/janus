namespace Rsdn.Janus
{
	public interface IProxyConfig
	{
		UseProxyType UseProxyType { get; }
		bool UseCustomAuthProxy { get; }
		IProxySettings ProxySettings { get; }
	}
}