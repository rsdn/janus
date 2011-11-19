using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Rsdn.Janus
{
	public partial class SaveStateDialog : Form
	{
		#region Constructor
		[DebuggerStepThrough]
		public SaveStateDialog()
		{
			InitializeComponent();
			CustomInitializeComponent();
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Имя файла, в который будут сохраняться данные.
		/// </summary>
		public string FileName
		{
			[DebuggerStepThrough]
			get { return _fileNameBox.Text; }
		}

		/// <summary>
		/// Опции экспорта.
		/// </summary>
		public SaveStateOptions Options
		{
			[DebuggerStepThrough]
			get
			{
				var options = SaveStateOptions.None;

				if (_markedMessages.Checked)
					options |= SaveStateOptions.Markers;

				if (_readedMessages.Checked)
					options |= SaveStateOptions.ReadedMessages;

				if (_favoriteMessages.Checked)
					options |= SaveStateOptions.Favorites;

				return options;
			}
			private set
			{
				_markedMessages.Checked = IsSet(value, SaveStateOptions.Markers);
				_readedMessages.Checked = IsSet(value, SaveStateOptions.ReadedMessages);
				_favoriteMessages.Checked = IsSet(value, SaveStateOptions.Favorites);
			}
		}
		#endregion

		#region Private Methods
		private static bool IsSet(SaveStateOptions options,
			SaveStateOptions value)
		{
			return (options & value) != SaveStateOptions.None;
		}

		private void CustomInitializeComponent()
		{
			_fileNameBox.Text = Config.Instance.JanusStateConfig.LastFileName;
			Options = Config.Instance.JanusStateConfig.SaveOptions;

			UpdateButtonState();
		}

		private void UpdateButtonState()
		{
			try
			{
				_okButton.Enabled = Directory.Exists(
					Path.GetDirectoryName(FileName));

				_okButton.Enabled &= Options != SaveStateOptions.None;
			}
			catch
			{
				_okButton.Enabled = false;
			}
		}
		#endregion

		#region Events
		private void _browseFileButton_Click(object sender, EventArgs e)
		{
			if (_saveFileDialog.ShowDialog(this) == DialogResult.OK)
				_fileNameBox.Text = _saveFileDialog.FileName;
		}

		private void _fileNameBox_TextChanged(object sender, EventArgs e)
		{
			UpdateButtonState();
		}

		private void OptionsChanged(object sender, EventArgs e)
		{
			UpdateButtonState();
		}

		private void SaveStateDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			Config.Instance.JanusStateConfig.LastFileName = FileName;
			Config.Instance.JanusStateConfig.SaveOptions = Options;
		}
		#endregion
	}
}