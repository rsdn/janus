namespace Rsdn.Janus
{
	internal partial class ProxyAuthForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProxyAuthForm));
			System.Windows.Forms.Label label2;
			System.Windows.Forms.Label label3;
			this._infoLabel = new System.Windows.Forms.Label();
			this._proxyLoginTextBox = new System.Windows.Forms.TextBox();
			this._proxyPassTextBox = new System.Windows.Forms.TextBox();
			this._savePassCheckBox = new System.Windows.Forms.CheckBox();
			this._okButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			label2 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// _infoLabel
			// 
			resources.ApplyResources(this._infoLabel, "_infoLabel");
			this._infoLabel.Name = "_infoLabel";
			// 
			// label2
			// 
			resources.ApplyResources(label2, "label2");
			label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			label2.Name = "label2";
			// 
			// label3
			// 
			resources.ApplyResources(label3, "label3");
			label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			label3.Name = "label3";
			// 
			// _proxyLoginTextBox
			// 
			resources.ApplyResources(this._proxyLoginTextBox, "_proxyLoginTextBox");
			this._proxyLoginTextBox.Name = "_proxyLoginTextBox";
			// 
			// _proxyPassTextBox
			// 
			resources.ApplyResources(this._proxyPassTextBox, "_proxyPassTextBox");
			this._proxyPassTextBox.Name = "_proxyPassTextBox";
			// 
			// _savePassCheckBox
			// 
			resources.ApplyResources(this._savePassCheckBox, "_savePassCheckBox");
			this._savePassCheckBox.Name = "_savePassCheckBox";
			// 
			// _okButton
			// 
			resources.ApplyResources(this._okButton, "_okButton");
			this._okButton.Name = "_okButton";
			this._okButton.Click += new System.EventHandler(this.OkButtonClick);
			// 
			// _cancelButton
			// 
			resources.ApplyResources(this._cancelButton, "_cancelButton");
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Name = "_cancelButton";
			// 
			// ProxyAuthForm
			// 
			this.AcceptButton = this._okButton;
			resources.ApplyResources(this, "$this");
			this.CancelButton = this._cancelButton;
			this.Controls.Add(this._cancelButton);
			this.Controls.Add(this._okButton);
			this.Controls.Add(this._savePassCheckBox);
			this.Controls.Add(this._proxyLoginTextBox);
			this.Controls.Add(this._proxyPassTextBox);
			this.Controls.Add(this._infoLabel);
			this.Controls.Add(label2);
			this.Controls.Add(label3);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ProxyAuthForm";
			this.ShowInTaskbar = false;
			this.Activated += new System.EventHandler(this.ProxyAuthForm_Activated);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.Label _infoLabel;
		private System.Windows.Forms.Button _okButton;
		private System.Windows.Forms.TextBox _proxyLoginTextBox;
		private System.Windows.Forms.TextBox _proxyPassTextBox;
		private System.Windows.Forms.CheckBox _savePassCheckBox;
	}
}