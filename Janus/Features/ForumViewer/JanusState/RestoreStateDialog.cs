using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Rsdn.Janus
{
	public partial class RestoreStateDialog : Form
	{
		#region Constructor
		[DebuggerStepThrough]
		public RestoreStateDialog()
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
		public RestoreStateOptions Options
		{
			[DebuggerStepThrough]
			get
			{
				var options = RestoreStateOptions.None;

				if (_markedMessages.Checked)
				{
					options |= RestoreStateOptions.Markers;

					if (_removeMarkers.Checked)
						options |= RestoreStateOptions.ClearMarkers;
				}

				if (_readedMessages.Checked)
					options |= RestoreStateOptions.ReadedMessages;


				if (_favoriteMessages.Checked)
				{
					options |= RestoreStateOptions.Favorites;

					if (_removeFavoriteMessages.Checked)
						options |= RestoreStateOptions.ClearFavorites;
				}

				return options;
			}
			private set
			{
				_markedMessages.Checked =
					IsSet(value, RestoreStateOptions.Markers);

				_removeMarkers.Checked =
					IsSet(value, RestoreStateOptions.ClearMarkers);

				_readedMessages.Checked =
					IsSet(value, RestoreStateOptions.ReadedMessages);

				_favoriteMessages.Checked =
					IsSet(value, RestoreStateOptions.Favorites);

				_removeFavoriteMessages.Checked =
					IsSet(value, RestoreStateOptions.ClearFavorites);
			}
		}
		#endregion

		#region Private Methods
		private void CustomInitializeComponent()
		{
			Options = Config.Instance.JanusStateConfig.RestoreOptions;
			_fileNameBox.Text = Config.Instance.JanusStateConfig.LastFileName;

			UpdateItemsState();
		}

		private static bool IsSet(RestoreStateOptions options,
			RestoreStateOptions value)
		{
			return (options & value) != RestoreStateOptions.None;
		}

		private void UpdateItemsState()
		{
			try
			{
				_okButton.Enabled = Directory.Exists(
					Path.GetDirectoryName(FileName));

				_okButton.Enabled &= Options != RestoreStateOptions.None;
			}
			catch
			{
				_okButton.Enabled = false;
			}

			_removeMarkers.Enabled = _markedMessages.Checked;
			_removeFavoriteMessages.Enabled = _favoriteMessages.Checked;
		}
		#endregion

		#region Events
		private void _browseFileButton_Click(object sender, EventArgs e)
		{
			_openFileDialog.FileName = FileName;

			if (_openFileDialog.ShowDialog(this) == DialogResult.OK)
				_fileNameBox.Text = _openFileDialog.FileName;
		}

		private void _fileNameBox_TextChanged(object sender, EventArgs e)
		{
			UpdateItemsState();
		}

		private void Options_CheckedChanged(object sender, EventArgs e)
		{
			UpdateItemsState();
		}

		private void RestoreStateDialog_FormClosing(object sender,
			FormClosingEventArgs e)
		{
			Config.Instance.JanusStateConfig.LastFileName = FileName;
			Config.Instance.JanusStateConfig.RestoreOptions = Options;
		}
		#endregion
	}
}