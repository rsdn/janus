namespace Rsdn.Janus
{
	public enum UrlBehavior
	{
		/// <summary>
		/// Открывать ссылку во внутреннем браузере.
		/// </summary>
		[JanusDisplayName(SR.Config.ExternalUrlBehavior.OpenJBrowserResourceName)]
		InternalBrowser,

		/// <summary>
		/// Открывать ссылку во внешнем браузере по умолчанию.
		/// </summary>
		[JanusDisplayName(SR.Config.ExternalUrlBehavior.OpenDefaultBrowserResourceName)]
		ExternalBrowser
	}
}