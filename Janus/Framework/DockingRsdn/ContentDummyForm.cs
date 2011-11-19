using System.ComponentModel;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

namespace Rsdn.Janus.Framework
{
	public partial class ContentDummyForm : DockContent
	{
		public ContentDummyForm()
		{
			InitializeComponent();

			this.DockAreas = DockAreas.Document;
			this.ShowHint = DockState.Document;
		}

		/// <summary>
		/// Связанный с формой элемент управления.
		/// </summary>
		public Control AssociatedControl
		{
			get { return Controls.Count > 0 ? Controls[0] : null; }
			set
			{
				SuspendLayout();
				Controls.Clear();
				if (value != null)
				{
					value.Dock = DockStyle.Fill;
					Controls.Add(value);
				}
				ResumeLayout();
			}
		}

		private void ContentDummyForm_Closing(object sender, CancelEventArgs e)
		{
			Controls.Clear();
		}
	}
}