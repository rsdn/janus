namespace Rsdn.Janus
{
	/// <summary>
	/// Версия винампа.
	/// </summary>
	public enum MediaPlayerType
	{
		[JanusDisplayName("DisplayNameWinamp3x")]
		Winamp3x,

		[JanusDisplayName("DisplayNameWinamp2x")]
		Winamp2x,

		[JanusDisplayName("DisplayNameWindowsMedia")]
		WindowsMedia,

		Foobar2000,

		Ultra,
	}
}