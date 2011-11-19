using System.ComponentModel;

namespace Rsdn.Janus
{
	/// <summary>
	/// Конфигурация тег-лайна.
	/// </summary>
	[TypeConverter(typeof (ExpandableObjectConverter))]
	public class TagLineConfig
	{
		private const bool _defaultUseTagline = true;
		private bool _useTagline = _defaultUseTagline;
		[SortIndex(10)]
		[DefaultValue(_defaultUseTagline)]
		[JanusDisplayName(SR.Config.TagLine.Use.DisplayNameResourceName)]
		[JanusDescription(SR.Config.TagLine.Use.DescriptionResourceName)]
		public bool UseTagLine
		{
			get { return _useTagline; }
			set { _useTagline = value; }
		}

		/*private const string _defaultFormat = "... << @@appname @@version @@release rev. @@revision>>";
		private string _format = _defaultFormat;
		[SortIndex(20)]
		[DefaultValue(_defaultFormat)]
		[JanusDisplayName(SR.Config.TagLine.Format.DisplayNameResourceName)]
		[JanusDescription(SR.Config.TagLine.Format.DescriptionResourceName)]
		public string Format
		{
			get { return _format; }
			set { _format = value; }
		}*/

		private TagLineInfos _tagLineInfos = TagLineInfos.DefaultInfos;
		[SortIndex(20)]
		[JanusDisplayName(SR.Config.TagLine.Format.DisplayNameResourceName)]
		[JanusDescription(SR.Config.TagLine.Format.DescriptionResourceName)]
		public TagLineInfos TagLineInfos
		{
			get { return _tagLineInfos; }
			set { _tagLineInfos = value; }
		}

		private const string _defaultLocalTimeFormat = "hh:mm";
		private string _localTimeFormat = _defaultLocalTimeFormat;
		[SortIndex(30)]
		[JanusDisplayName(SR.Config.TagLine.TimeFormat.DisplayNameResourceName)]
		[JanusDescription(SR.Config.TagLine.TimeFormat.DescriptionResourceName)]
		[DefaultValue(_defaultLocalTimeFormat)]
		public string LocalTimeFormat
		{
			get { return _localTimeFormat; }
			set { _localTimeFormat = value; }
		}

		private MediaPlayerTagConfig _mediaPlayerTagConfig = new MediaPlayerTagConfig();
		[SortIndex(40)]
		[JanusDisplayName(SR.Config.TagLine.MediaPlayer.DisplayNameResourceName)]
		[JanusDescription(SR.Config.TagLine.MediaPlayer.DescriptionResourceName)]
		public MediaPlayerTagConfig MediaPlayerTagConfig
		{
			get { return _mediaPlayerTagConfig; }
			set { _mediaPlayerTagConfig = value; }
		}

		private CitationTagConfig _citationTagConfig = new CitationTagConfig();
		[SortIndex(50)]
		[JanusDisplayName(SR.Config.TagLine.Citation.DisplayNameResourceName)]
		[JanusDescription(SR.Config.TagLine.Citation.DescriptionResourceName)]
		public CitationTagConfig CitationTagConfig
		{
			get { return _citationTagConfig; }
			set { _citationTagConfig = value; }
		}

		public override string ToString()
		{
			return string.Empty;
		}
	}
}
