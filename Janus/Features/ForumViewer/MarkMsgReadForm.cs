using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Rsdn.Janus
{
	/// <summary>
	/// Основная форма пометки сообщений прочтенным
	/// </summary>
	public partial class MarkMsgReadForm : Form
	{
		private readonly IServiceProvider _provider;

		#region Constructor

		public MarkMsgReadForm(IServiceProvider provider)
		{
			_provider = provider;
			InitializeComponent();

			CustomInitializeComponent();
		}

		#endregion

		#region Private methods

		private void CustomInitializeComponent()
		{
			_toolTip.SetToolTip(_checkInvertButton, SR.Forum.MarkMsg.TipCheckInvert);
			_toolTip.SetToolTip(_checkAllButton, SR.Forum.MarkMsg.TipCheckAll);
			_toolTip.SetToolTip(_unCheckAllButton, SR.Forum.MarkMsg.TipUnCheckAll);

			using (var mgr = _provider.CreateDBContext())
			{
				using (_forumsList.UpdateScope())
					_forumsList
						.Items
						.AddRange(
							mgr
								.SubscribedForums()
								.Select(
									f =>
										new ListViewItem(new[] {"", f.Name, f.Descript})
										{
											Checked = true,
											ImageIndex = 0,
											Tag = f.ID
										})
								.ToArray());
			}

			RadioButtonsCheckedChanged(_markBeforeRadio, EventArgs.Empty);
			_markAsCombo.SelectedIndex = 0;

			if (Forums.Instance.ActiveForum == null)
				_currentForumButton.Enabled = false;
		}

		#endregion

		#region Properties

		public DateTime? BeforeDate
		{
			get
			{
				if (_markBeforeRadio.Checked)
					return _dateStartPicker.Value;
				if (_markBetweenRadio.Checked)
					return _dateEndPicker.Value;

				return null;
			}
		}

		public DateTime? AfterDate
		{
			get
			{
				if (_markBetweenRadio.Checked)
					return _dateStartPicker.Value;
				if (_markAfterRadio.Checked)
					return _dateEndPicker.Value;

				return null;
			}
		}

		public bool MarkAsRead
		{
			get { return _markAsCombo.SelectedIndex == 0; }
		}

		public bool ExceptAnswersMe
		{
			get { return _exceptAnswersMeCheck.Checked; }
		}

		public bool MarkAllForums
		{
			get { return _forumsList.Items.Count == _forumsList.CheckedItems.Count; }
		}

		public IEnumerable<int> ForumsIdsForMark
		{
			get
			{
				var forumsIds = new int[_forumsList.CheckedItems.Count];
				for (var i = 0; i < _forumsList.CheckedItems.Count; i++)
					forumsIds[i] = (int)_forumsList.CheckedItems[i].Tag;
				return forumsIds;
			}
		}

		#endregion

		#region Control Events

		private void BtnOkClick(object sender, EventArgs e)
		{
			if (_forumsList.CheckedItems.Count == 0)
			{
				MessageBox.Show(this, SR.Forum.MarkMsg.ReadNeedSelectForum,
					ApplicationInfo.ApplicationName,
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			DialogResult = DialogResult.OK;
		}

		private void BtnCheckInvertClick(object sender, EventArgs e)
		{
			if (_forumsList.CheckedItems.Count == 0)
				return;
			foreach (var t in _forumsList.Items.Cast<ListViewItem>())
				t.Checked = !t.Checked;
		}

		private void BtnCheckAllClick(object sender, EventArgs e)
		{
			foreach (var t in _forumsList.Items.Cast<ListViewItem>())
				t.Checked = true;
		}

		private void BtnUnCheckAllClick(object sender, EventArgs e)
		{
			if (_forumsList.CheckedItems.Count == 0)
				return;
			foreach (var t in _forumsList.Items.Cast<ListViewItem>())
				t.Checked = false;
		}

		private void RadioButtonsCheckedChanged(object sender, EventArgs e)
		{
			_dateStartPicker.Enabled = _markBeforeRadio.Checked || _markBetweenRadio.Checked;
			_dateEndPicker.Enabled = _markBetweenRadio.Checked || _markAfterRadio.Checked;
		}

		private void ButtonCurrentForumClick(object sender, EventArgs e)
		{
			var activeId = (Forums.Instance.ActiveForum == null) ? "" : Forums.Instance.ActiveForum.ID.ToString();
			foreach (ListViewItem lvi in _forumsList.Items)
			{
				lvi.Checked = (lvi.Tag.ToString() == activeId);
				lvi.Selected = (lvi.Tag.ToString() == activeId);
				if (lvi.Checked)
					_forumsList.EnsureVisible(lvi.Index);
			}
		}

		#endregion

	}
}