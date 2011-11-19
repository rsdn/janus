using System.ComponentModel;

namespace Rsdn.Janus
{
	/// <summary>
	/// Настройки экспорта форумов.
	/// </summary>
	public class ForumExportConfig : SubConfigBase
	{
		private const int _defaultCharsPerLine = 80;
		private int _charsPerLine = _defaultCharsPerLine;

		[JanusDisplayName(SR.Config.ForumExport.CharsPerLine.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ForumExport.CharsPerLine.DescriptionResourceName)]
		[DefaultValue(_defaultCharsPerLine)]
		[SortIndex(10)]
		public int CharsPerLine
		{
			get { return _charsPerLine; }
			set { _charsPerLine = value; }
		}

		private ExportPlainTextEncoding _plainTextEncoding = ExportPlainTextEncoding.Default;

		[JanusDisplayName(SR.Config.ForumExport.PlainTextEncoding.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ForumExport.PlainTextEncoding.DescriptionResourceName)]
		[DefaultValue(ExportPlainTextEncoding.Default)]
		[SortIndex(20)]
		public ExportPlainTextEncoding ExportPlainTextEncoding
		{
			get { return _plainTextEncoding; }
			set { _plainTextEncoding = value; }
		}

		private const bool _defaultExportRemoveTags = true;
		private bool _removeTags = _defaultExportRemoveTags;

		[JanusDisplayName(SR.Config.ForumExport.RemoveTags.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ForumExport.RemoveTags.DescriptionResourceName)]
		[DefaultValue(_defaultExportRemoveTags)]
		[SortIndex(30)]
		public bool ExportRemoveTags
		{
			get { return _removeTags; }
			set { _removeTags = value; }
		}

		#region Невидимые в настройках

		private const string _defaultValueLastFileName = "c:\\";
		private string _lastFileName = _defaultValueLastFileName;

		[DefaultValue(_defaultValueLastFileName)]
		[Browsable(false)]
		public string LastFileName
		{
			get { return _lastFileName; }
			set { _lastFileName = value; }
		}

		private const bool _defaultValueUnreadMessagesOnly = false;
		private bool _lastUnreadMessagesOnly = _defaultValueUnreadMessagesOnly;

		[DefaultValue(_defaultValueUnreadMessagesOnly)]
		[Browsable(false)]
		public bool LastUnreadMessagesOnly
		{
			get { return _lastUnreadMessagesOnly; }
			set { _lastUnreadMessagesOnly = value; }
		}

		private const int _defaultValueExportMode = 0;

		[DefaultValue(_defaultValueExportMode)]
		[Browsable(false)]
		public int ExportMode { get; set; }

		private const int _defaultValueExportFormat = 0;

		[DefaultValue(_defaultValueExportFormat)]
		[Browsable(false)]
		public int ExportFormat { get; set; }
		#endregion
	}
}
