using System.ComponentModel;

namespace Rsdn.Janus
{
	/// <summary>
	/// Настройки нотификации в трее.
	/// </summary>
	public class NotificationConfig : SubConfigBase
	{
		private const bool _defaultLateralNotifier = false;

		[JanusDisplayName(SR.Config.Notification.Tray.DisplayNameResourceName)]
		[JanusDescription(SR.Config.Notification.Tray.DescriptionResourceName)]
		[DefaultValue(_defaultLateralNotifier)]
		[SortIndex(10)]
		public bool LateralNotifier { get; set; }

		private const int _defaultLateralTimeout = 4000;
		private int _lateralTimeout = _defaultLateralTimeout;

		[JanusDisplayName(SR.Config.Notification.Time.DisplayNameResourceName)]
		[JanusDescription(SR.Config.Notification.Time.DescriptionResourceName)]
		[DefaultValue(_defaultLateralTimeout)]
		[SortIndex(20)]
		public int LateralTimeout
		{
			get { return _lateralTimeout; }
			set { _lateralTimeout = value; }
		}
	}
}
