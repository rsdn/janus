using System.ComponentModel;

namespace Rsdn.Janus
{
	/// <summary>
	/// Настройки подтверждений.
	/// </summary>
	public class ConfirmationConfig : SubConfigBase
	{
		private const bool _defaultConfirmClosing = false;

		[JanusDisplayName(SR.Config.Confirmation.Exit.DisplayNameResourceName)]
		[JanusDescription(SR.Config.Confirmation.Exit.DescriptionResourceName)]
		[DefaultValue(_defaultConfirmClosing)]
		[SortIndex(10)]
		public bool ConfirmClosing { get; set; }

		private const bool _defaultConfirmMarkAll = true;
		private bool _confirmMarkAll = _defaultConfirmMarkAll;

		[JanusDisplayName(SR.Config.Confirmation.MarkAll.DisplayNameResourceName)]
		[JanusDescription(SR.Config.Confirmation.MarkAll.DescriptionResourceName)]
		[ChangeProperty(ChangeActionType.Refresh)]
		[DefaultValue(_defaultConfirmMarkAll)]
		[SortIndex(20)]
		public bool ConfirmMarkAll
		{
			get { return _confirmMarkAll; }
			set { _confirmMarkAll = value; }
		}

		private const bool _defaultConfirmJump = false;

		[JanusDisplayName(SR.Config.Confirmation.SearchJump.DisplayNameResourceName)]
		[JanusDescription(SR.Config.Confirmation.SearchJump.DescriptionResourceName)]
		[DefaultValue(_defaultConfirmJump)]
		[SortIndex(30)]
		public bool ConfirmJump { get; set; }
	}
}
