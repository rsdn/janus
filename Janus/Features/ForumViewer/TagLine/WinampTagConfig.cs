using System;
using System.ComponentModel;

namespace Rsdn.Janus
{
	/// <summary>
	/// Конфигурация тега винампа.
	/// </summary>
	[TypeConverter(typeof (ExpandableObjectConverter))]
	public class WinampTagConfig
	{
		private const WinampVersion _defaultPlayerVersion = WinampVersion.Winamp2x;
		private WinampVersion playerVersion = _defaultPlayerVersion;
		[SortIndex(1)]
		[JanusDisplayName("ConfigDisplayNameWinampVersion")]
		[JanusDescription("ConfigDescriptionWinampVersion")]
		[DefaultValue(_defaultPlayerVersion)]
		public WinampVersion PlayerVersion
		{
			get {return playerVersion;}
			set {playerVersion = value;}
		}

		private const string _defaultSilentName = "silent";
		private string silentName = _defaultSilentName;
		[SortIndex(1)]
		[JanusDisplayName("ConfigDisplayNameWinampSilent")]
		[JanusDescription("ConfigDescriptionWinampSilent")]
		[DefaultValue(_defaultSilentName)]
		public string SilentName
		{
			get {return silentName;}
			set {silentName = value;}
		}

		public WinampTagConfig()
		{
		}

		public override string ToString()
		{
			return string.Empty;
		}

	}
}
