namespace Rsdn.Janus
{
	public enum UseProxyType
	{
		[ConfigDisplayName("UseProxyType_NoUse")]
		NoUse,

		[ConfigDisplayName("UseProxyType_UseIESettings")]
		UseIESettings,

		[ConfigDisplayName("UseProxyType_UseCustomSettings")]
		UseCustomSettings
	}
}