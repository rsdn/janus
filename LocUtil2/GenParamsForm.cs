using System.ComponentModel;
using System.Windows.Forms;

namespace Rsdn.LocUtil
{
	/// <summary>
	/// Summary description for GenParamsForm.
	/// </summary>
	internal partial class GenParamsForm : Form
	{

		public GenParamsForm()
		{
			InitializeComponent();
		}

		public string Namespace
		{
			get { return _nsBox.Text; }
			set { _nsBox.Text = value; }
		}

		public bool IsInternal
		{
			get { return _internalModifierBox.Checked; }
			set { _internalModifierBox.Checked = value; }
		}

		public bool ShowResult
		{
			get { return _showResultBox.Checked; }
			set { _showResultBox.Checked = value; }
		}
	}
}
