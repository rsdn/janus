using System.ComponentModel;

namespace Rsdn.Janus
{
	/// <summary>
	/// Конфигурация тега винампа.
	/// </summary>
	[TypeConverter(typeof (ExpandableObjectConverter))]
	public class MediaPlayerTagConfig
	{
		private const MediaPlayerType _defaultMediaPlayer = MediaPlayerType.Winamp2x;
		private MediaPlayerType _mediaPlayer = _defaultMediaPlayer;
		
		[SortIndex(10)]
		[JanusDisplayName(SR.Config.TagLine.MediaPlayer.Type.DisplayNameResourceName)]
		[JanusDescription(SR.Config.TagLine.MediaPlayer.Type.DescriptionResourceName)]
		[DefaultValue(_defaultMediaPlayer)]
		public MediaPlayerType MediaPlayer
		{
			get {return _mediaPlayer;}
			set {_mediaPlayer = value;}
		}

		private const string _defaultSilentName = "silent";
		private string silentName = _defaultSilentName;
		
		[SortIndex(20)]
		[JanusDisplayName(SR.Config.TagLine.MediaPlayer.Silent.DisplayNameResourceName)]
		[JanusDescription(SR.Config.TagLine.MediaPlayer.Silent.DescriptionResourceName)]
		[DefaultValue(_defaultSilentName)]
		public string SilentString
		{
			get {return silentName;}
			set {silentName = value;}
		}

		public override string ToString()
		{
			return string.Empty;
		}
	}
}
