using System;
using System.Windows.Forms;

namespace Rsdn.Janus
{
	public partial class StringEnterForm: Form
	{
		public StringEnterForm()
		{
			InitializeComponent();

			string_TextChanged(this, EventArgs.Empty);
		}

		public string String
		{
			get { return _string.Text; }
			set { _string.Text = value; }
		}

		public string Description
		{
			get { return _description.Text; }
			set { _description.Text = value; }
		}

		private void string_TextChanged(object sender, EventArgs e)
		{
			_okButton.Enabled = !string.IsNullOrEmpty(_string.Text);
		}
	}
}