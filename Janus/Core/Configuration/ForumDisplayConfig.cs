using System.ComponentModel;

namespace Rsdn.Janus
{
	/// <summary>
	/// Конфигурация отображения форума.
	/// </summary>
	public class ForumDisplayConfig : SubConfigBase
	{
		private const bool _defaultMsgListGridLines = true;
		private bool _msgListGridLines = _defaultMsgListGridLines;

		[JanusDisplayName(SR.Config.ForumDisplay.GridLines.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ForumDisplay.GridLines.DescriptionResourceName)]
		[ChangeProperty(ChangeActionType.Refresh)]
		[DefaultValue(_defaultMsgListGridLines)]
		[SortIndex(10)]
		public bool MsgListGridLines
		{
			get { return _msgListGridLines; }
			set { _msgListGridLines = value; }
		}

		private const int _defaultTreeSpacing = 5;
		private int _treeSpacing = _defaultTreeSpacing;

		[JanusDisplayName(SR.Config.ForumDisplay.TreeSpacing.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ForumDisplay.TreeSpacing.DescriptionResourceName)]
		[DefaultValue(_defaultTreeSpacing)]
		[SortIndex(20)]
		public int GridIndent
		{
			get { return _treeSpacing; }
			set { _treeSpacing = value; }
		}

		private const bool _defaultShowFullForumNames = true;
		private bool _showFullForumNames = _defaultShowFullForumNames;

		[JanusDisplayName(SR.Config.ForumDisplay.FullForumNames.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ForumDisplay.FullForumNames.DescriptionResourceName)]
		[ChangeProperty(ChangeActionType.Refresh)]
		[DefaultValue(_defaultShowFullForumNames)]
		[SortIndex(30)]
		public bool ShowFullForumNames
		{
			get { return _showFullForumNames; }
			set { _showFullForumNames = value; }
		}

		private const bool _defaultShowTotalMessages = true;
		private bool _showTotalMessages = _defaultShowTotalMessages;

		[JanusDisplayName(SR.Config.ForumDisplay.ShowTotalMessages.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ForumDisplay.ShowTotalMessages.DescriptionResourceName)]
		[SortIndex(40)]
		[ChangeProperty(ChangeActionType.Refresh)]
		[DefaultValue(_defaultShowTotalMessages)]
		public bool ShowTotalMessages
		{
			get { return _showTotalMessages; }
			set { _showTotalMessages = value; }
		}

		private const bool _defaultShowUserRateLastMonthOnly = true;
		private bool _showUserRateLastMonthOnly = _defaultShowUserRateLastMonthOnly;

		[JanusDisplayName(SR.Config.ForumDisplay.ShowUserRateLastMonthOnly.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ForumDisplay.ShowUserRateLastMonthOnly.DescriptionResourceName)]
		[ChangeProperty(ChangeActionType.Refresh)]
		[DefaultValue(_defaultShowUserRateLastMonthOnly)]
		[SortIndex(50)]
		public bool ShowUserRateLastMonthOnly
		{
			get { return _showUserRateLastMonthOnly; }
			set { _showUserRateLastMonthOnly = value; }
		}

		private const bool _defaultShowMessageFormCustomBarOnNewLine = false;

		[JanusDisplayName(SR.Config.ForumDisplay.ShowMessageFormCustomBarOnNewLine.DisplayNameResourceName
			)]
		[JanusDescription(SR.Config.ForumDisplay.ShowMessageFormCustomBarOnNewLine.DescriptionResourceName
			)]
		[ChangeProperty(ChangeActionType.Refresh)]
		[DefaultValue(_defaultShowMessageFormCustomBarOnNewLine)]
		[SortIndex(80)]
		public bool ShowMessageFormCustomBarOnNewLine { get; set; }

		private const int _defaultDaysToOutdate = 7;
		private int _daysToOutdate = _defaultDaysToOutdate;

		[JanusDisplayName(SR.Config.ForumDisplay.DaysToOutdate.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ForumDisplay.DaysToOutdate.DescriptionResourceName)]
		[ChangeProperty(ChangeActionType.Refresh)]
		[DefaultValue(_defaultDaysToOutdate)]
		[SortIndex(90)]
		public int DaysToOutdate
		{
			get { return _daysToOutdate; }
			set { _daysToOutdate = value; }
		}

		private const bool _defaultShowImagesAtImgTag = true;
		private bool _showImagesAtImgTag = _defaultShowImagesAtImgTag;

		[JanusDisplayName(SR.Config.ForumDisplay.ShowImagesAtImgTag.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ForumDisplay.ShowImagesAtImgTag.DescriptionResourceName)]
		[ChangeProperty(ChangeActionType.Refresh)]
		[DefaultValue(_defaultShowImagesAtImgTag)]
		[SortIndex(100)]

		public bool ShowImagesAtImgTag
		{
			get { return _showImagesAtImgTag; }
			set { _showImagesAtImgTag = value; }
		}

		private const bool _defaultShowUnreadThreadsOnly = false;

		[JanusDisplayName(SR.Config.ForumDisplay.ShowUnreadThreadsOnly.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ForumDisplay.ShowUnreadThreadsOnly.DescriptionResourceName)]
		[DefaultValue(_defaultShowUnreadThreadsOnly)]
		[ChangeProperty(ChangeActionType.Refresh)]
		[SortIndex(110)]
		public bool ShowUnreadThreadsOnly { get; set; }

		private const int _defaultMaxTopicsPerForum = -1;
		private int _maxTopicsPerForum = _defaultMaxTopicsPerForum;

		[JanusDisplayName(SR.Config.ForumDisplay.MaxTopicsPerForum.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ForumDisplay.MaxTopicsPerForum.DescriptionResourceName)]
		[ChangeProperty(ChangeActionType.Refresh)]
		[DefaultValue(_defaultMaxTopicsPerForum)]
		[SortIndex(120)]
		public int MaxTopicsPerForum
		{
			get { return _maxTopicsPerForum; }
			set { _maxTopicsPerForum = value; }
		}

		private const string _defaultDateFormat = "dd.MM.yy HH:mm";
		private string _dateFormat = _defaultDateFormat;

		[JanusDisplayName(SR.Config.ForumDisplay.DateFormat.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ForumDisplay.DateFormat.DescriptionResourceName)]
		[DefaultValue(_defaultDateFormat)]
		[ChangeProperty(ChangeActionType.Refresh)]
		[SortIndex(130)]
		public string DateFormat
		{
			get { return _dateFormat; }
			set { _dateFormat = value; }
		}

		private const bool _defaultShowMessageId = false;

		[JanusDisplayName(SR.Config.ForumDisplay.ShowMessageId.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ForumDisplay.ShowMessageId.DescriptionResourceName)]
		[SortIndex(140)]
		[DefaultValue(_defaultShowMessageId)]
		public bool ShowMessageId { get; set; }

		private const bool _defaultNewMessagesOnTop = false;

		[JanusDisplayName(SR.Config.ForumDisplay.NewMessagesOnTop.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ForumDisplay.NewMessagesOnTop.DescriptionResourceName)]
		[SortIndex(150)]
		[ChangeProperty(ChangeActionType.Refresh)]
		[DefaultValue(_defaultNewMessagesOnTop)]
		public bool NewMessagesOnTop { get; set; }

		[JanusDisplayName(SR.Config.ForumDisplay.HighlightParentMessage.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ForumDisplay.HighlightParentMessage.DescriptionResourceName)]
		[ChangeProperty(ChangeActionType.Refresh)]
		[DefaultValue(false)]
		[SortIndex(160)]
		public bool HighlightParentMessage { get; set; }

		[JanusDisplayName(SR.Config.ForumDisplay.HighlightChildMessages.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ForumDisplay.HighlightChildMessages.DescriptionResourceName)]
		[ChangeProperty(ChangeActionType.Refresh)]
		[DefaultValue(false)]
		[SortIndex(161)]
		public bool HighlightChildMessages { get; set; }


		[JanusDisplayName(SR.Config.ForumDisplay.DetectMediaContentLinks.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ForumDisplay.DetectMediaContentLinks.DescriptionResourceName)]
		[DefaultValue(false)]
		[SortIndex(162)]
		public bool DetectMediaContentLinks { get; set; }

		#region Конверт сообщения
		private EnvelopeConfig _envelope = new EnvelopeConfig();

		[JanusDisplayName(SR.Config.ForumDisplay.MessageEnvelope.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ForumDisplay.MessageEnvelope.DescriptionResourceName)]
		[ChangeProperty(ChangeActionType.Refresh)]
		[SortIndex(170)]
		public EnvelopeConfig Envelope
		{
			get { return _envelope; }
			set { _envelope = value; }
		}
		#endregion
	}
}
