using System.ComponentModel;
using System.Drawing;

namespace Rsdn.Janus
{
	/// <summary>
	/// Натсройки тикера.
	/// </summary>
	public class TickerConfig : SubConfigBase
	{
		public const int DefTickerValue = -1346325;
		public Point TickerPosition = new Point(DefTickerValue, DefTickerValue);

		private const bool _defaultShowTicker = true;
		private bool _showTicker = _defaultShowTicker;

		[JanusDisplayName(SR.Config.Ticker.Show.DisplayNameResourceName)]
		[JanusDescription(SR.Config.Ticker.Show.DescriptionResourceName)]
		[DefaultValue(_defaultShowTicker)]
		[ChangeProperty(ChangeActionType.Refresh)]
		[SortIndex(10)]
		public bool ShowTicker
		{
			get { return _showTicker; }
			set { _showTicker = value; }
		}

		private double _tickerOpacity = 0.5;

		[JanusDisplayName(SR.Config.Ticker.Opacity.DisplayNameResourceName)]
		[JanusDescription(SR.Config.Ticker.Opacity.DescriptionResourceName)]
		[ChangeProperty(ChangeActionType.Refresh)]
		[SortIndex(30)]
		public double TickerOpacity
		{
			get { return _tickerOpacity; }
			set { _tickerOpacity = value; }
		}

		private const bool _defaultStickyTicker = true;
		private bool _stickyTicker = _defaultStickyTicker;

		[JanusDisplayName(SR.Config.Ticker.Sticky.DisplayNameResourceName)]
		[JanusDescription(SR.Config.Ticker.Sticky.DescriptionResourceName)]
		[DefaultValue(_defaultStickyTicker)]
		[SortIndex(40)]
		public bool StickyTicker
		{
			get { return _stickyTicker; }
			set { _stickyTicker = value; }
		}
	}
}
