namespace Rsdn.Janus
{
	partial class GoToForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GoToForm));
			this._idTextLabel = new System.Windows.Forms.Label();
			this._idTextBox = new System.Windows.Forms.TextBox();
			this._cancelButton = new System.Windows.Forms.Button();
			this._okButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// _idTextLabel
			// 
			this._idTextLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			resources.ApplyResources(this._idTextLabel, "_idTextLabel");
			this._idTextLabel.Name = "_idTextLabel";
			// 
			// _idTextBox
			// 
			resources.ApplyResources(this._idTextBox, "_idTextBox");
			this._idTextBox.Name = "_idTextBox";
			this._idTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this._idTextBox_KeyPress);
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
			// GoToForm
			// 
			this.AcceptButton = this._okButton;
			this.CancelButton = this._cancelButton;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this._okButton);
			this.Controls.Add(this._cancelButton);
			this.Controls.Add(this._idTextBox);
			this.Controls.Add(this._idTextLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GoToForm";
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label _idTextLabel;
		private System.Windows.Forms.TextBox _idTextBox;
		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.Button _okButton;
	}
}