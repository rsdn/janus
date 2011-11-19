using System.Windows.Forms;

namespace Rsdn.Shortcuts
{
	/// <summary>
	/// Summary description for SavePresetForm.
	/// </summary>
	public partial class SavePresetForm : Form
	{
		public SavePresetForm()
		{
			InitializeComponent();
		}

		public string PresetName
		{
			get { return tbPresetName.Text; }
			set { tbPresetName.Text = value; }
		}


	}
}