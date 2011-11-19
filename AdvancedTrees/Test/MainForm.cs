using System.Windows.Forms;

namespace Test
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, System.EventArgs e)
		{
			treeView1.Model = new FileSystemTreeModel();
		}
	}
}