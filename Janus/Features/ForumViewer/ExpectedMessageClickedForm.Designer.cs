namespace Rsdn.Janus
{
	internal partial class ExpectedMessageClickedForm
	{
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExpectedMessageClickedForm));
			this._messageLabel = new System.Windows.Forms.Label();
			this._openInBrowserButton = new System.Windows.Forms.Button();
			this._downloadButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			this._imageLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// _messageLabel
			// 
			resources.ApplyResources(this._messageLabel, "_messageLabel");
			this._messageLabel.Name = "_messageLabel";
			// 
			// _openInBrowserButton
			// 
			resources.ApplyResources(this._openInBrowserButton, "_openInBrowserButton");
			this._openInBrowserButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._openInBrowserButton.Name = "_openInBrowserButton";
			// 
			// _downloadButton
			// 
			resources.ApplyResources(this._downloadButton, "_downloadButton");
			this._downloadButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._downloadButton.Name = "_downloadButton";
			this._downloadButton.Click += new System.EventHandler(this._downloadButton_Click);
			// 
			// _cancelButton
			// 
			resources.ApplyResources(this._cancelButton, "_cancelButton");
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Name = "_cancelButton";
			// 
			// _imageLabel
			// 
			resources.ApplyResources(this._imageLabel, "_imageLabel");
			this._imageLabel.Name = "_imageLabel";
			// 
			// ExpectedMessageClickedForm
			// 
			this.AcceptButton = this._openInBrowserButton;
			this.CancelButton = this._cancelButton;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this._imageLabel);
			this.Controls.Add(this._cancelButton);
			this.Controls.Add(this._downloadButton);
			this.Controls.Add(this._openInBrowserButton);
			this.Controls.Add(this._messageLabel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ExpectedMessageClickedForm";
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.Button _downloadButton;
		private System.Windows.Forms.Label _imageLabel;
		private System.Windows.Forms.Label _messageLabel;
		private System.Windows.Forms.Button _openInBrowserButton;
	}
}