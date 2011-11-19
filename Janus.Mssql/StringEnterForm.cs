using System;
using System.Windows.Forms;

namespace Rsdn.Janus.Mssql
{
	public partial class StringEnterForm : Form
	{
		public StringEnterForm(string description)
		{
			InitializeComponent();

			_description.Text = description;

			StringTextChanged(this, EventArgs.Empty);
		}

		public string String
		{
			get { return _string.Text; }
		}

		private void StringTextChanged(object sender, EventArgs e)
		{
			_okButton.Enabled = !string.IsNullOrEmpty(_string.Text);
		}
	}
}