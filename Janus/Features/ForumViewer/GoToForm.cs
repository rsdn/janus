using System;
using System.Windows.Forms;

namespace Rsdn.Janus
{
	/// <summary>
	/// Переход на сообщение по ИД сообщения
	/// </summary>
	public partial class GoToForm : Form
	{
		private const int _maxMid = 10000000;

		public GoToForm()
		{
			InitializeComponent();
		}

		public GoToForm(int msgId)
			: this()
		{
			MessageId = msgId;
		}

		#region Fields & Properties
		private int _msgId;

		public int MessageId
		{
			get { return _msgId; }
			private set
			{
				_msgId = value;
				_idTextBox.Text = _msgId.ToString();
			}
		}
		#endregion

		#region Private methods
		private void NotifyAboutError(string msg)
		{
			MessageBox.Show(this, msg,
				SR.Forum.GoToMessage.ValidationError,
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
		#endregion

		#region Control events
		private void _okButton_Click(object sender, EventArgs e)
		{
			try
			{
				_msgId = int.Parse(_idTextBox.Text);
				if (_msgId > _maxMid || _msgId < 0)
					throw new OverflowException();
			}
			catch (FormatException)
			{
				NotifyAboutError(
					SR.Forum.GoToMessage.BadNumber);
				return;
			}
			catch (OverflowException)
			{
				NotifyAboutError(
					SR.Forum.GoToMessage.IncorrectNumber);
				return;
			}
			catch (Exception ex)
			{
				NotifyAboutError(ex.Message);
				return;
			}
			DialogResult = DialogResult.OK;
		}

		private void _idTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar))
				e.Handled = false;
			else
				e.Handled = true;
		}
		#endregion
	}
}