using System.Windows.Forms;

namespace Rsdn.LocUtil.Model.Design
{
	/// <summary>
	/// Summary description for ResourceEditorForm.
	/// </summary>
	internal partial class ResourceEditorForm : Form
	{
		public ResourceEditorForm(string localeName)
		{
			InitializeComponent();

			_localeNameLabel.Text = localeName;
		}

		public string ResourceText
		{
			get { return _editor.Model.Text; }
			set { _editor.Model.Text = value; }
		}
	}
}
