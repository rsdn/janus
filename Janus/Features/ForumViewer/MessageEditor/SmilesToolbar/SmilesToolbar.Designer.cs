namespace Rsdn.Janus
{
	public sealed partial class SmilesToolbar
	{
		private System.ComponentModel.IContainer components;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this._toolTip = new System.Windows.Forms.ToolTip(this.components);
			// 
			// SmilesToolbar
			// 
			this.Name = "SmilesToolbar";
			this.Size = new System.Drawing.Size(480, 104);
		}

		#endregion

		private System.Windows.Forms.ToolTip _toolTip;
	}
}