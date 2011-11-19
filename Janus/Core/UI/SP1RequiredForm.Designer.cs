namespace Rsdn.Janus
{
	partial class SP1RequiredForm
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
			System.Windows.Forms.Button _okButton;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SP1RequiredForm));
			System.Windows.Forms.LinkLabel _linkLabel;
			this._messageLabel = new System.Windows.Forms.Label();
			_okButton = new System.Windows.Forms.Button();
			_linkLabel = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// _okButton
			// 
			_okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			resources.ApplyResources(_okButton, "_okButton");
			_okButton.Name = "_okButton";
			_okButton.UseVisualStyleBackColor = true;
			// 
			// _linkLabel
			// 
			resources.ApplyResources(_linkLabel, "_linkLabel");
			_linkLabel.Name = "_linkLabel";
			_linkLabel.TabStop = true;
			_linkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
			// 
			// _messageLabel
			// 
			resources.ApplyResources(this._messageLabel, "_messageLabel");
			this._messageLabel.Name = "_messageLabel";
			// 
			// SP1RequiredForm
			// 
			this.AcceptButton = _okButton;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = _okButton;
			this.Controls.Add(_linkLabel);
			this.Controls.Add(this._messageLabel);
			this.Controls.Add(_okButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SP1RequiredForm";
			this.ShowIcon = false;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label _messageLabel;
	}
}