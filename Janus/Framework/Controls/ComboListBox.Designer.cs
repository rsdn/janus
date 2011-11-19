namespace Rsdn.Janus.Framework
{
	partial class ComboListBox
	{
		#region IDisposable support

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
			}
			base.Dispose(disposing);
		}

		#endregion

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.cbxLog = new System.Windows.Forms.ComboBox();
			this.lbxLog = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// cbxLog
			// 
			this.cbxLog.Dock = System.Windows.Forms.DockStyle.Top;
			this.cbxLog.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.cbxLog.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxLog.MaxDropDownItems = 16;
			this.cbxLog.Name = "cbxLog";
			this.cbxLog.Size = new System.Drawing.Size(384, 21);
			this.cbxLog.TabIndex = 1;
			this.cbxLog.SelectedIndexChanged += new System.EventHandler(this.Log_SelectedIndexChanged);
			this.cbxLog.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.Log_DrawItem);
			// 
			// lbxLog
			// 
			this.lbxLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbxLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbxLog.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.lbxLog.IntegralHeight = false;
			this.lbxLog.Name = "lbxLog";
			this.lbxLog.Size = new System.Drawing.Size(384, 168);
			this.lbxLog.TabIndex = 2;
			this.lbxLog.MouseClick += new System.Windows.Forms.MouseEventHandler(lbxLog_MouseClick);
			this.lbxLog.MouseUp += new System.Windows.Forms.MouseEventHandler(lbxLog_MouseUp);
			this.lbxLog.SelectedIndexChanged += new System.EventHandler(lbxLog_SelectedIndexChanged);
			this.lbxLog.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.Log_DrawItem);
			// 
			// ComboListBox
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[]
				{
					this.cbxLog,
					this.lbxLog
				});
			this.Size = new System.Drawing.Size(384, 168);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox cbxLog;
		private System.Windows.Forms.ListBox lbxLog;
	}
}
