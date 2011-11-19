using System;
using System.Windows.Forms;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Авторизация на прокси-сервере
	/// </summary>
	internal partial class ProxyAuthForm : Form
	{
		private readonly ProxyConfig _proxyConfig;

		#region Constructor
		[Obsolete]
		public ProxyAuthForm()
		{}

		public ProxyAuthForm(ProxyConfig proxyConfig)
		{
			_proxyConfig = proxyConfig;
			InitializeComponent();

			_infoLabel.Text =
				(_infoLabel.Text + " {0}")
					.FormatStr(_proxyConfig.ProxySettings);
			_proxyLoginTextBox.Text = _proxyConfig.ProxySettings.Login;
			_proxyPassTextBox.PasswordChar = (char)0x25CF;
		}
		#endregion

		#region Fields & Properties
		public string Login
		{
			get { return _proxyLoginTextBox.Text; }
		}

		public string Password
		{
			get { return _proxyPassTextBox.Text; }
		}
		#endregion

		#region Control Events
		private void OkButtonClick(object sender, EventArgs e)
		{
			if (_savePassCheckBox.Checked)
				_proxyConfig.ProxySettings.SaveAuth = true;
			DialogResult = DialogResult.OK;
		}

		private void ProxyAuthForm_Activated(object sender, EventArgs e)
		{
			if (_proxyLoginTextBox.Text.Length == 0)
				_proxyLoginTextBox.Focus();
			else
				_proxyPassTextBox.Focus();
		}
		#endregion
	}
}