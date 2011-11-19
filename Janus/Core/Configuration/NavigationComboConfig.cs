using System.ComponentModel;

namespace Rsdn.Janus
{
	/// <summary>
	/// Настройки панели навигации.
	/// </summary>
	public class NavigationComboConfig : SubConfigBase
	{
		private const bool _defaultShowNavigationCombo = true;
		private bool _show = _defaultShowNavigationCombo;

		[JanusDisplayName(SR.Config.NavigationCombo.Show.DisplayNameResourceName)]
		[JanusDescription(SR.Config.NavigationCombo.Show.DescriptionResourceName)]
		[ChangeProperty(ChangeActionType.Refresh)]
		[DefaultValue(_defaultShowNavigationCombo)]
		[SortIndex(10)]
		public bool Show
		{
			get { return _show; }
			set { _show = value; }
		}

		private const int _defaultMaxDropDownItems = 20;
		private int _maxDropDownItems = _defaultMaxDropDownItems;

		[JanusDisplayName(SR.Config.NavigationCombo.MaxDropDownItems.DisplayNameResourceName)]
		[JanusDescription(SR.Config.NavigationCombo.MaxDropDownItems.DescriptionResourceName)]
		[ChangeProperty(ChangeActionType.Refresh)]
		[DefaultValue(_defaultMaxDropDownItems)]
		[SortIndex(20)]
		public int MaxDropDownItems
		{
			get { return _maxDropDownItems; }
			set { _maxDropDownItems = value; }
		}
	}
}
