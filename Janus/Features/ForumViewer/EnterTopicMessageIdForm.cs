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
		}

		public int MessageId { get; private set; }

		private void _idBox_TextChanged(object sender, EventArgs e)
		{
			bool isValid = true;

			try
			{
				string input = _idBox.Text;

				if (!string.IsNullOrEmpty(input))
				{
					JanusProtocolInfo janusProtocolInfo = JanusProtocolInfo.Parse(input);
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