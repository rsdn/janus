namespace Rsdn.LocUtil
{
	public partial class Pane
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components;

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
			this._caption = new PaneCaption();
			this.SuspendLayout();
			// 
			// caption
			// 
			this._caption.Dock = System.Windows.Forms.DockStyle.Top;
			this._caption.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
			this._caption.Location = new System.Drawing.Point(1, 1);
			this._caption.Name = "_caption";
			this._caption.Size = new System.Drawing.Size(248, 18);
			this._caption.TabIndex = 0;
			this._caption.Text = "Panel 1";
			// 
			// BasePane
			// 
			//this.Controls.Add(this._caption);
			this.Name = "BasePane";
			this.Size = new System.Drawing.Size(250, 230);
			this.ResumeLayout(false);

		}

		#endregion
	}
}