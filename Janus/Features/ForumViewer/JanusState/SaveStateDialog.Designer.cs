namespace Rsdn.Janus
{
	partial class SaveStateDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveStateDialog));
			this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this._fileNameBox = new System.Windows.Forms.TextBox();
			this._browseFileButton = new System.Windows.Forms.Button();
			this._exportOptionsGroup = new System.Windows.Forms.GroupBox();
			this._favoriteMessages = new System.Windows.Forms.CheckBox();
			this._markedMessages = new System.Windows.Forms.CheckBox();
			this._readedMessages = new System.Windows.Forms.CheckBox();
			this._labelFileName = new System.Windows.Forms.Label();
			this._okButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			this._exportOptionsGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// _saveFileDialog
			// 
			this._saveFileDialog.DefaultExt = "xml";
			resources.ApplyResources(this._saveFileDialog, "_saveFileDialog");
			this._saveFileDialog.RestoreDirectory = true;
			// 
			// _fileNameBox
			// 
			resources.ApplyResources(this._fileNameBox, "_fileNameBox");
			this._fileNameBox.Name = "_fileNameBox";
			this._fileNameBox.TextChanged += new System.EventHandler(this._fileNameBox_TextChanged);
			// 
			// _browseFileButton
			// 
			resources.ApplyResources(this._browseFileButton, "_browseFileButton");
			this._browseFileButton.Name = "_browseFileButton";
			this._browseFileButton.UseVisualStyleBackColor = true;
			this._browseFileButton.Click += new System.EventHandler(this._browseFileButton_Click);
			// 
			// _exportOptionsGroup
			// 
			this._exportOptionsGroup.Controls.Add(this._favoriteMessages);
			this._exportOptionsGroup.Controls.Add(this._markedMessages);
			this._exportOptionsGroup.Controls.Add(this._readedMessages);
			resources.ApplyResources(this._exportOptionsGroup, "_exportOptionsGroup");
			this._exportOptionsGroup.Name = "_exportOptionsGroup";
			this._exportOptionsGroup.TabStop = false;
			// 
			// _favoriteMessages
			// 
			resources.ApplyResources(this._favoriteMessages, "_favoriteMessages");
			this._favoriteMessages.Checked = true;
			this._favoriteMessages.CheckState = System.Windows.Forms.CheckState.Checked;
			this._favoriteMessages.Name = "_favoriteMessages";
			this._favoriteMessages.UseVisualStyleBackColor = true;
			this._favoriteMessages.CheckedChanged += new System.EventHandler(this.OptionsChanged);
			// 
			// _markedMessages
			// 
			resources.ApplyResources(this._markedMessages, "_markedMessages");
			this._markedMessages.Checked = true;
			this._markedMessages.CheckState = System.Windows.Forms.CheckState.Checked;
			this._markedMessages.Name = "_markedMessages";
			this._markedMessages.UseVisualStyleBackColor = true;
			this._markedMessages.CheckedChanged += new System.EventHandler(this.OptionsChanged);
			// 
			// _readedMessages
			// 
			resources.ApplyResources(this._readedMessages, "_readedMessages");
			this._readedMessages.Checked = true;
			this._readedMessages.CheckState = System.Windows.Forms.CheckState.Checked;
			this._readedMessages.Name = "_readedMessages";
			this._readedMessages.UseVisualStyleBackColor = true;
			this._readedMessages.CheckedChanged += new System.EventHandler(this.OptionsChanged);
			// 
			// _labelFileName
			// 
			resources.ApplyResources(this._labelFileName, "_labelFileName");
			this._labelFileName.Name = "_labelFileName";
			// 
			// _okButton
			// 
			resources.ApplyResources(this._okButton, "_okButton");
			this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._okButton.Name = "_okButton";
			// 
			// _cancelButton
			// 
			resources.ApplyResources(this._cancelButton, "_cancelButton");
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Name = "_cancelButton";
			// 
			// SaveStateDialog
			// 
			this.AcceptButton = this._okButton;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._cancelButton;
			this.Controls.Add(this._cancelButton);
			this.Controls.Add(this._okButton);
			this.Controls.Add(this._fileNameBox);
			this.Controls.Add(this._labelFileName);
			this.Controls.Add(this._exportOptionsGroup);
			this.Controls.Add(this._browseFileButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SaveStateDialog";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SaveStateDialog_FormClosing);
			this._exportOptionsGroup.ResumeLayout(false);
			this._exportOptionsGroup.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SaveFileDialog _saveFileDialog;
		private System.Windows.Forms.TextBox _fileNameBox;
		private System.Windows.Forms.Button _browseFileButton;
		private System.Windows.Forms.GroupBox _exportOptionsGroup;
		private System.Windows.Forms.CheckBox _favoriteMessages;
		private System.Windows.Forms.CheckBox _markedMessages;
		private System.Windows.Forms.CheckBox _readedMessages;
		private System.Windows.Forms.Label _labelFileName;
		private System.Windows.Forms.Button _okButton;
		private System.Windows.Forms.Button _cancelButton;
	}
}