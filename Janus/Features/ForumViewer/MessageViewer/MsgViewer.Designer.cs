namespace Rsdn.Janus
{
	internal sealed partial class MsgViewer
	{
		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._toolStrip = new System.Windows.Forms.ToolStrip();
			this._messagePanel = new Rsdn.Janus.Framework.FramePanel();
			this._messageBrowser = new System.Windows.Forms.WebBrowser();
			this._bottomPanel = new System.Windows.Forms.Panel();
			this._statusLabel = new System.Windows.Forms.Label();
			this._messagePanel.SuspendLayout();
			this._bottomPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// _toolStrip
			// 
			this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this._toolStrip.Location = new System.Drawing.Point(0, 0);
			this._toolStrip.Name = "_toolStrip";
			this._toolStrip.Size = new System.Drawing.Size(656, 25);
			this._toolStrip.TabIndex = 0;
			// 
			// _messagePanel
			// 
			this._messagePanel.Controls.Add(this._messageBrowser);
			this._messagePanel.Controls.Add(this._bottomPanel);
			this._messagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._messagePanel.Location = new System.Drawing.Point(0, 25);
			this._messagePanel.Name = "_messagePanel";
			this._messagePanel.Padding = new System.Windows.Forms.Padding(1, 1, 1, 1);
			this._messagePanel.Size = new System.Drawing.Size(656, 311);
			this._messagePanel.TabIndex = 6;
			// 
			// _messageBrowser
			// 
			this._messageBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this._messageBrowser.Location = new System.Drawing.Point(1, 1);
			this._messageBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			this._messageBrowser.Name = "_messageBrowser";
			this._messageBrowser.Size = new System.Drawing.Size(654, 293);
			this._messageBrowser.TabIndex = 3;
			// 
			// _bottomPanel
			// 
			this._bottomPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(240)))), ((int)(((byte)(224)))));
			this._bottomPanel.Controls.Add(this._statusLabel);
			this._bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._bottomPanel.Location = new System.Drawing.Point(1, 294);
			this._bottomPanel.Name = "_bottomPanel";
			this._bottomPanel.Size = new System.Drawing.Size(654, 16);
			this._bottomPanel.TabIndex = 2;
			// 
			// _statusLabel
			// 
			this._statusLabel.AutoSize = true;
			this._statusLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._statusLabel.ForeColor = System.Drawing.Color.DimGray;
			this._statusLabel.Location = new System.Drawing.Point(0, 0);
			this._statusLabel.Name = "_statusLabel";
			this._statusLabel.Size = new System.Drawing.Size(0, 14);
			this._statusLabel.TabIndex = 0;
			this._statusLabel.UseMnemonic = false;
			// 
			// MsgViewer
			// 
			this.Controls.Add(this._messagePanel);
			this.Controls.Add(this._toolStrip);
			this.Name = "MsgViewer";
			this.Size = new System.Drawing.Size(656, 336);
			this._messagePanel.ResumeLayout(false);
			this._bottomPanel.ResumeLayout(false);
			this._bottomPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Panel _bottomPanel;
		private Rsdn.Janus.Framework.FramePanel _messagePanel;
		private System.Windows.Forms.Label _statusLabel;
		private System.Windows.Forms.WebBrowser _messageBrowser;
		private System.Windows.Forms.ToolStrip _toolStrip;
	}
}