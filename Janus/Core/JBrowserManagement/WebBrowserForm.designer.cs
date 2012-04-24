namespace Rsdn.Janus
{
	sealed partial class WebBrowserForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebBrowserForm));
			this._statusStrip = new System.Windows.Forms.StatusStrip();
			this._stateStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this._statusProgressBar = new System.Windows.Forms.ToolStripProgressBar();
			this._menuStrip = new System.Windows.Forms.MenuStrip();
			this._toolStrip = new System.Windows.Forms.ToolStrip();
			this._locationComboBox = new System.Windows.Forms.ComboBox();
			this._webBrowser = new System.Windows.Forms.WebBrowser();
			this._statusStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// _statusStrip
			// 
			this._statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._stateStatusLabel,
            this._statusProgressBar});
			resources.ApplyResources(this._statusStrip, "_statusStrip");
			this._statusStrip.Name = "_statusStrip";
			// 
			// _stateStatusLabel
			// 
			this._stateStatusLabel.Name = "_stateStatusLabel";
			resources.ApplyResources(this._stateStatusLabel, "_stateStatusLabel");
			this._stateStatusLabel.Spring = true;
			// 
			// _statusProgressBar
			// 
			this._statusProgressBar.Name = "_statusProgressBar";
			resources.ApplyResources(this._statusProgressBar, "_statusProgressBar");
			// 
			// _menuStrip
			// 
			resources.ApplyResources(this._menuStrip, "_menuStrip");
			this._menuStrip.Name = "_menuStrip";
			// 
			// _toolStrip
			// 
			this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			resources.ApplyResources(this._toolStrip, "_toolStrip");
			this._toolStrip.Name = "_toolStrip";
			// 
			// _locationComboBox
			// 
			this._locationComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._locationComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			resources.ApplyResources(this._locationComboBox, "_locationComboBox");
			this._locationComboBox.Name = "_locationComboBox";
			this._locationComboBox.SelectionChangeCommitted += new System.EventHandler(this._locationComboBox_SelectionChangeCommitted);
			this._locationComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this._locationComboBox_KeyDown);
			// 
			// _webBrowser
			// 
			this._webBrowser.CausesValidation = false;
			resources.ApplyResources(this._webBrowser, "_webBrowser");
			this._webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			this._webBrowser.Name = "_webBrowser";
			this._webBrowser.ScriptErrorsSuppressed = true;
			// 
			// WebBrowserForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._webBrowser);
			this.Controls.Add(this._locationComboBox);
			this.Controls.Add(this._toolStrip);
			this.Controls.Add(this._menuStrip);
			this.Controls.Add(this._statusStrip);
			this.Name = "WebBrowserForm";
			this._statusStrip.ResumeLayout(false);
			this._statusStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip _statusStrip;
		private System.Windows.Forms.ToolStripStatusLabel _stateStatusLabel;
		private System.Windows.Forms.ToolStripProgressBar _statusProgressBar;
		private System.Windows.Forms.MenuStrip _menuStrip;
		private System.Windows.Forms.ToolStrip _toolStrip;
		private System.Windows.Forms.ComboBox _locationComboBox;
		private System.Windows.Forms.WebBrowser _webBrowser;
	}
}