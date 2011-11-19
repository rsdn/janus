using System.ComponentModel;

namespace Rsdn.Janus
{
	/// <summary>
	/// Настройка отображения конверта сообщения.
	/// </summary>
	[TypeConverter(typeof (ExpandableObjectConverter))]
	public class EnvelopeConfig
	{
		public override string ToString()
		{
			return string.Empty;
		}

		private const bool _defaultShowHeader = true;
		private bool _showHeader = _defaultShowHeader;

		[SortIndex(0)]
		[DefaultValue(_defaultShowHeader)]
		[JanusDisplayName(SR.Config.ForumDisplay.MessageEnvelope.ShowHeader.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ForumDisplay.MessageEnvelope.ShowHeader.DescriptionResourceName)]
		
		public bool ShowHeader
		{
			get { return _showHeader; }
			set { _showHeader = value; }
		}

		private const bool _defaultShowRateFrame = false;

		[SortIndex(1)]
		[DefaultValue(_defaultShowRateFrame)]
		[JanusDisplayName(SR.Config.ForumDisplay.MessageEnvelope.ShowRateFrame.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ForumDisplay.MessageEnvelope.ShowRateFrame.DescriptionResourceName)]
		public bool ShowRateFrame { get; set; }

		private const int _defaultRateWidth = 180; // в пикселях
		private int _rateWidth = _defaultRateWidth;

		[SortIndex(2)]
		[DefaultValue(_defaultRateWidth)]
		[JanusDisplayName(SR.Config.ForumDisplay.MessageEnvelope.RateWidth.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ForumDisplay.MessageEnvelope.RateWidth.DescriptionResourceName)]
		
		public int RateWidth
		{
			get { return _rateWidth; }
			set { _rateWidth = value; }
		}

		[JanusDisplayName(
			SR.Config.ForumDisplay.MessageEnvelope.UseFriendlyCopyCode.DisplayNameResourceName)]
		[JanusDescription(
			SR.Config.ForumDisplay.MessageEnvelope.UseFriendlyCopyCode.DescriptionResourceName)]
		[DefaultValue(false)]
		[SortIndex(3)]
		public bool UseFriendlyCopyCode { get; set; }
	}
}
