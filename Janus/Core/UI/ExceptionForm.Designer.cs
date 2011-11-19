namespace Rsdn.Janus
{
	partial class ExceptionForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExceptionForm));
			this._errorPictureBox = new System.Windows.Forms.PictureBox();
			this._additionalInfoTextBox = new System.Windows.Forms.TextBox();
			this._bugReportButton = new System.Windows.Forms.Button();
			this._continueButton = new System.Windows.Forms.Button();
			this._exitButton = new System.Windows.Forms.Button();
			this._titleLabel = new System.Windows.Forms.Label();
			this._errorMessageTextBox = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this._errorPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// _errorPictureBox
			// 
			resources.ApplyResources(this._errorPictureBox, "_errorPictureBox");
			this._errorPictureBox.Name = "_errorPictureBox";
			this._errorPictureBox.TabStop = false;
			// 
			// _additionalInfoTextBox
			// 
			resources.ApplyResources(this._additionalInfoTextBox, "_additionalInfoTextBox");
			this._additionalInfoTextBox.Name = "_additionalInfoTextBox";
			this._additionalInfoTextBox.ReadOnly = true;
			// 
			// _bugReportButton
			// 
			resources.ApplyResources(this._bugReportButton, "_bugReportButton");
			this._bugReportButton.Name = "_bugReportButton";
			_bugReportButton.Enabled = false;
			this._bugReportButton.UseVisualStyleBackColor = true;
			this._bugReportButton.Click += new System.EventHandler(this.BugReportButtonClick);
			// 
			// _continueButton
			// 
			resources.ApplyResources(this._continueButton, "_continueButton");
			this._continueButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._continueButton.Name = "_continueButton";
			this._continueButton.UseVisualStyleBackColor = true;
			// 
			// _exitButton
			// 
			resources.ApplyResources(this._exitButton, "_exitButton");
			this._exitButton.Name = "_exitButton";
			this._exitButton.UseVisualStyleBackColor = true;
			this._exitButton.Click += new System.EventHandler(this.ExitButtonClick);
			// 
			// _titleLabel
			// 
			resources.ApplyResources(this._titleLabel, "_titleLabel");
			this._titleLabel.Name = "_titleLabel";
			// 
			// _errorMessageTextBox
			// 
			resources.ApplyResources(this._errorMessageTextBox, "_errorMessageTextBox");
			this._errorMessageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._errorMessageTextBox.Name = "_errorMessageTextBox";
			this._errorMessageTextBox.ReadOnly = true;
			// 
			// ExceptionForm
			// 
			this.AcceptButton = this._continueButton;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._continueButton;
			this.Controls.Add(this._errorMessageTextBox);
			this.Controls.Add(this._titleLabel);
			this.Controls.Add(this._exitButton);
			this.Controls.Add(this._continueButton);
			this.Controls.Add(this._bugReportButton);
			this.Controls.Add(this._additionalInfoTextBox);
			this.Controls.Add(this._errorPictureBox);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ExceptionForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			((System.ComponentModel.ISupportInitialize)(this._errorPictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox _errorPictureBox;
		private System.Windows.Forms.TextBox _additionalInfoTextBox;
		private System.Windows.Forms.Button _bugReportButton;
		private System.Windows.Forms.Button _continueButton;
		private System.Windows.Forms.Button _exitButton;
		private System.Windows.Forms.Label _titleLabel;
		private System.Windows.Forms.TextBox _errorMessageTextBox;
	}
}