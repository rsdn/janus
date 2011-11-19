using System;
using System.Windows.Forms;

namespace Rsdn.Janus
{
	/// <summary>
	/// Форма, появляющаяся если при клике на ссылку на сообщение, 
	/// отсутствующее в локальной БД.
	/// </summary>
	internal partial class ExpectedMessageClickedForm : Form
	{
		private bool _openInBrowser = true;

		public ExpectedMessageClickedForm()
		{
			InitializeComponent();
		}

		public bool OpenInBrowser
		{
			get { return _openInBrowser; }
		}

		private void _downloadButton_Click(object sender, EventArgs e)
		{
			_openInBrowser = false;
		}
	}
}