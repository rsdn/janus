using System.ComponentModel;
using System.Windows.Forms;

using Rsdn.Scintilla;

namespace Rsdn.LocUtil
{
	/// <summary>
	/// Summary description for ShowResultFormForm.
	/// </summary>
	internal partial class ShowResultForm : Form
	{
		public ShowResultForm()
		{
			InitializeComponent();
		}

		public string Result
		{
			get { return _editor.Model.Text; }
			set { _editor.Model.Text = value; }
		}
	}
}
