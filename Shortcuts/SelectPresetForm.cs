using System;
using System.Collections;
using System.Windows.Forms;

namespace Rsdn.Shortcuts
{
	/// <summary>
	/// Summary description for SelectPresetForm.
	/// </summary>
	public partial class SelectPresetForm : Form
	{
		private readonly ShortcutManager _manager;

		public SelectPresetForm(ShortcutManager manager)
		{
			_manager = manager;

			InitializeComponent();

			SetButtonsState();
		}

		private void SetButtonsState()
		{
			this._okButton.Enabled = this._lvPresetList.SelectedItems.Count > 0;
			this._removeButton.Enabled = this._lvPresetList.SelectedItems.Count > 0;
		}

		public string PresetName
		{
			get
			{
				if (_lvPresetList.SelectedItems.Count == 0)
					throw new ApplicationException("Не выбран пресет.");

				return _lvPresetList.SelectedItems[0].Text;
			}
		}

		public void LoadPresets(PresetCollection presets)
		{
			try
			{
				_lvPresetList.BeginUpdate();
				_lvPresetList.Items.Clear();

				foreach (DictionaryEntry de in presets)
					_lvPresetList.Items.Add(
						new ListViewItem((string)de.Key));
			}
			finally
			{
				_lvPresetList.EndUpdate();
			}
		}

		private void _removeButton_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem it in _lvPresetList.SelectedItems)
			{
				_lvPresetList.Items.Remove(it);
				_manager.Presets.RemovePreset(it.Text);
			}
		}

		private void _lvPresetList_SelectedIndexChanged(object sender, EventArgs e)
		{
			SetButtonsState();
		}
	}
}