using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace Rsdn.Janus
{
	internal partial class ExportMessageDialog : Form
	{
		private readonly bool _canExportMessages;
		private readonly bool _canExportForum;


		public ExportMessageDialog(bool canExportMessages, bool canExportForum)
		{
			if(!canExportMessages && !canExportForum)
				throw new ArgumentException(
					"Не указано ни одного источника сообщений для экспорта.");

			_canExportMessages = canExportMessages;
			_canExportForum = canExportForum;

			InitializeComponent();

			_selectedMessagesRadioButton.Enabled = _canExportMessages;
			_topicRadioButton.Enabled = _canExportMessages;
			_forumRadioButton.Enabled = _canExportForum;

			FileName = Config.Instance.ForumExportConfig.LastFileName;
			ExportMode = (ExportMode)Config.Instance.ForumExportConfig.ExportMode;
			UnreadMessagesOnly = Config.Instance.ForumExportConfig.LastUnreadMessagesOnly;
			ExportFormat = (ExportFormat)Config.Instance.ForumExportConfig.ExportFormat;
		}


		public ExportMode ExportMode
		{
			get
			{
				if (_selectedMessagesRadioButton.Checked)
					return ExportMode.Messages;

				if (_topicRadioButton.Checked)
					return ExportMode.Topics;

				if (_forumRadioButton.Checked)
					return ExportMode.Forum;

				throw new ApplicationException();
			}
			private set
			{
				switch (value)
				{
					case ExportMode.Messages:
						if (!_canExportMessages)
							goto case ExportMode.Forum;
						_selectedMessagesRadioButton.Checked = true;
						break;

					case ExportMode.Topics:
						if (!_canExportMessages)
							goto case ExportMode.Forum;
						_topicRadioButton.Checked = true;
						break;

					case ExportMode.Forum:
						if (!_canExportForum)
							return;
						_forumRadioButton.Checked = true;
						break;
				}
			}
		}

		public bool UnreadMessagesOnly
		{
			get { return _unreadOnlyCheckBox.Checked; }
			private set { _unreadOnlyCheckBox.Checked = value; }
		}

		public ExportFormat ExportFormat
		{
			get
			{
				if (_textFormatRadioButton.Checked)
					return ExportFormat.Text;

				if (_htmlFormatRadioButton.Checked)
					return ExportFormat.HTML;

				if (_mhtFormatRadioButton.Checked)
					return ExportFormat.MHT;

				throw new ApplicationException();
			}
			private set
			{
				switch (value)
				{
					case ExportFormat.Text:
						_textFormatRadioButton.Checked = true;
						break;
					case ExportFormat.HTML:
						_htmlFormatRadioButton.Checked = true;
						break;
					case ExportFormat.MHT:
						_mhtFormatRadioButton.Checked = true;
						break;
				}
			}
		}

		public string FileName
		{
			get { return _fileNameBox.Text; }
			private set
			{
				_fileNameBox.Text = value;
			}
		}


		protected override void OnClosing(CancelEventArgs e)
		{
			Config.Instance.ForumExportConfig.LastFileName = _fileNameBox.Text;
			Config.Instance.ForumExportConfig.ExportMode = (int)ExportMode;
			Config.Instance.ForumExportConfig.LastUnreadMessagesOnly = UnreadMessagesOnly;
			Config.Instance.ForumExportConfig.ExportFormat = (int)ExportFormat;
			base.OnClosing(e);
		}


		private void _browseFileButton_Click(object sender, EventArgs e)
		{
			switch (ExportFormat)
			{
				case ExportFormat.Text:
					_saveFileDialog.FilterIndex = 1;
					break;
				case ExportFormat.HTML:
					_saveFileDialog.FilterIndex = 2;
					break;
				case ExportFormat.MHT:
					_saveFileDialog.FilterIndex = 3;
					break;
			}

			if (_saveFileDialog.ShowDialog(this) == DialogResult.OK)
				_fileNameBox.Text = _saveFileDialog.FileName;

			switch (_saveFileDialog.FilterIndex)
			{
				case 1:
					_textFormatRadioButton.Checked = true;
					break;
				case 2:
					_htmlFormatRadioButton.Checked = true;
					break;
				case 3:
					_mhtFormatRadioButton.Checked = true;
					break;
			}
		}

		private void _fileNameBox_TextChanged(object sender, EventArgs e)
		{
			bool valid;
			try
			{
				valid = Directory.Exists(Path.GetDirectoryName(_fileNameBox.Text));
				_saveFileDialog.FileName = Path.GetFileName(_fileNameBox.Text);
			}
			catch
			{
				valid = false;
			}

			_okButton.Enabled = valid;
		}

		private void _formatBttClick(object sender, EventArgs e)
		{
			_fileNameBox.Text = Path.ChangeExtension(
				_fileNameBox.Text, (string)((Control)sender).Tag);
		}
	}
}