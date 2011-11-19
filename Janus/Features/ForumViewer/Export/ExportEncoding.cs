namespace Rsdn.Janus
{
	public enum ExportPlainTextEncoding
	{
		[JanusDisplayName("PTEncDefaultDisplayName")]
		Default,
		[JanusDisplayName("PTEncUnicodeDisplayName")]
		Unicode,
		[JanusDisplayName("PTEncUTF8DisplayName")]
		UTF8,
		[JanusDisplayName("PTEncDOSCP866DisplayName")]
		DOSCP866,
		[JanusDisplayName("PTEncWin1251DisplayName")]
		WinCP1251
	}
}