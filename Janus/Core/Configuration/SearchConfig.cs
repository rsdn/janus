using System.ComponentModel;

namespace Rsdn.Janus
{
	/// <summary>
	/// Настройки поиска.
	/// </summary>
	public class SearchConfig : SubConfigBase
	{
		private const int _defaultMaxResultInSelect = 500;
		private int _maxResultInSelect = _defaultMaxResultInSelect;

		[JanusDisplayName(SR.Config.Search.MaxResultInSelect.DisplayNameResourceName)]
		[JanusDescription(SR.Config.Search.MaxResultInSelect.DescriptionResourceName)]
		[ChangeProperty(ChangeActionType.Refresh)]
		[DefaultValue(_defaultMaxResultInSelect)]
		[SortIndex(20)]
		public int MaxResultInSelect
		{
			get { return _maxResultInSelect; }
			set { _maxResultInSelect = value; }
		}
	}
}
