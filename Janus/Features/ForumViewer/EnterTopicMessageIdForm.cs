using System;
using System.Windows.Forms;

namespace Rsdn.Janus
{
	/// <summary>
	/// Окно ввода ID топика или ссылки.
	/// </summary>
	internal partial class EnterTopicMessageIdForm : Form
	{
		internal EnterTopicMessageIdForm()
		{
			InitializeComponent();

			// Попытаться извлечь дефолтный номер сообщения из буфера обмена.
			var dto = Clipboard.GetDataObject();
			if (dto != null)
				if (dto.GetDataPresent(DataFormats.Text))
				{
					var data = (string) dto.GetData(DataFormats.Text);
					var info = JanusProtocolInfo.Parse(data);
					if (info != null && info.ResourceType == JanusProtocolResourceType.Message && info.IsId)
						_idBox.Text = data;
				}
		}

		public int MessageId { get; private set; }

		private void IdBoxTextChanged(object sender, EventArgs e)
		{
			var isValid = true;

			try
			{
				var input = _idBox.Text;

				if (!string.IsNullOrEmpty(input))
				{
					var janusProtocolInfo = JanusProtocolInfo.Parse(input);
					if (null != janusProtocolInfo && janusProtocolInfo.IsId)
						MessageId = janusProtocolInfo.Id;
					else
						MessageId = int.Parse(input);
				}
				else
				{
					isValid = false;
				}
			}
			catch
			{
				isValid = false;
			}

			_labelMessageIdIsText.Text = string.Format(SR.EnterTopicMessageIdForm.MessageIdIsTextFormat, MessageId);

			_labelMessageIdIsText.Visible = isValid;
			_okButton.Enabled = isValid;
		}
	}
}