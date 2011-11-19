namespace Rsdn.Janus
{
	public partial class SelectDB
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.FlowLayoutPanel lytControlButtons;
			this.btnConnect = new System.Windows.Forms.Button();
			this.btnCreateDatabase = new System.Windows.Forms.Button();
			this.tbcEngine = new System.Windows.Forms.TabControl();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			lytControlButtons = new System.Windows.Forms.FlowLayoutPanel();
			lytControlButtons.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lytControlButtons
			// 
			lytControlButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			lytControlButtons.AutoSize = true;
			lytControlButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			lytControlButtons.Controls.Add(this.btnConnect);
			lytControlButtons.Controls.Add(this.btnCreateDatabase);
			lytControlButtons.Location = new System.Drawing.Point(7, 248);
			lytControlButtons.Name = "lytControlButtons";
			lytControlButtons.Size = new System.Drawing.Size(140, 30);
			lytControlButtons.TabIndex = 11;
			lytControlButtons.WrapContents = false;
			// 
			// btnConnect
			// 
			this.btnConnect.AutoSize = true;
			this.btnConnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.btnConnect.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btnConnect.Location = new System.Drawing.Point(3, 3);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(64, 24);
			this.btnConnect.TabIndex = 9;
			this.btnConnect.Text = "connect";
			this.btnConnect.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btnConnect.UseVisualStyleBackColor = false;
			this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
			// 
			// btnCreateDatabase
			// 
			this.btnCreateDatabase.AutoSize = true;
			this.btnCreateDatabase.BackColor = System.Drawing.SystemColors.Control;
			this.btnCreateDatabase.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btnCreateDatabase.Location = new System.Drawing.Point(73, 3);
			this.btnCreateDatabase.Name = "btnCreateDatabase";
			this.btnCreateDatabase.Size = new System.Drawing.Size(64, 24);
			this.btnCreateDatabase.TabIndex = 10;
			this.btnCreateDatabase.Text = "create";
			this.btnCreateDatabase.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btnCreateDatabase.UseVisualStyleBackColor = false;
			this.btnCreateDatabase.Click += new System.EventHandler(this.btnCreateDatabase_Click);
			// 
			// tbcEngine
			// 
			this.tbcEngine.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.tbcEngine.ItemSize = new System.Drawing.Size(47, 0);
			this.tbcEngine.Location = new System.Drawing.Point(7, 2);
			this.tbcEngine.Name = "tbcEngine";
			this.tbcEngine.SelectedIndex = 0;
			this.tbcEngine.Size = new System.Drawing.Size(349, 242);
			this.tbcEngine.TabIndex = 1;
			this.tbcEngine.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.TearoffDbDriver);
			this.tbcEngine.SelectedIndexChanged += new System.EventHandler(this.tbcEngine_SelectedIndexChanged);
			// 
			// btnOK
			// 
			this.btnOK.AutoSize = true;
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btnOK.Location = new System.Drawing.Point(3, 3);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(64, 24);
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.AutoSize = true;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btnCancel.Location = new System.Drawing.Point(73, 3);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(64, 24);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.btnOK);
			this.flowLayoutPanel1.Controls.Add(this.btnCancel);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(207, 248);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(140, 30);
			this.flowLayoutPanel1.TabIndex = 12;
			this.flowLayoutPanel1.WrapContents = false;
			// 
			// SelectDB
			// 
			this.AcceptButton = this.btnOK;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(364, 279);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(lytControlButtons);
			this.Controls.Add(this.tbcEngine);
			this.MaximizeBox = false;
			this.Name = "SelectDB";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Select DB Engine";
			lytControlButtons.ResumeLayout(false);
			lytControlButtons.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TabControl tbcEngine;
		private System.Windows.Forms.Button btnConnect;
		private System.Windows.Forms.Button btnCreateDatabase;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
	}
}
