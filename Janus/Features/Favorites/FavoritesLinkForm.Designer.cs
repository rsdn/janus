namespace Rsdn.Janus
{
	public partial class FavoritesLinkForm
	{
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FavoritesLinkForm));
			this._cancelButton = new System.Windows.Forms.Button();
			this._okButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this._urlTextBox = new System.Windows.Forms.TextBox();
			this._commentTextBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// _cancelButton
			// 
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this._cancelButton, "_cancelButton");
			this._cancelButton.Name = "_cancelButton";
			// 
			// _okButton
			// 
			resources.ApplyResources(this._okButton, "_okButton");
			this._okButton.Name = "_okButton";
			this._okButton.Click += new System.EventHandler(this._okButton_Click);
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// label2
			// 
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// _urlTextBox
			// 
			resources.ApplyResources(this._urlTextBox, "_urlTextBox");
			this._urlTextBox.Name = "_urlTextBox";
			// 
			// _commentTextBox
			// 
			resources.ApplyResources(this._commentTextBox, "_commentTextBox");
			this._commentTextBox.Name = "_commentTextBox";
			// 
			// FavoritesLinkForm
			// 
			this.AcceptButton = this._okButton;
			this.CancelButton = this._cancelButton;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this._urlTextBox);
			this.Controls.Add(this._commentTextBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this._cancelButton);
			this.Controls.Add(this._okButton);
			this.Controls.Add(this.label2);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FavoritesLinkForm";
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.Button _okButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox _urlTextBox;
		private System.Windows.Forms.TextBox _commentTextBox;
	}
}