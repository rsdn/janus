namespace Rsdn.Janus
{
	partial class RestoreStateDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RestoreStateDialog));
			this._cancelButton = new System.Windows.Forms.Button();
			this._okButton = new System.Windows.Forms.Button();
			this._fileNameBox = new System.Windows.Forms.TextBox();
			this._labelFileName = new System.Windows.Forms.Label();
			this._exportOptionsGroup = new System.Windows.Forms.GroupBox();
			this._removeFavoriteMessages = new System.Windows.Forms.CheckBox();
			this._favoriteMessages = new System.Windows.Forms.CheckBox();
			this._removeMarkers = new System.Windows.Forms.CheckBox();
			this._markedMessages = new System.Windows.Forms.CheckBox();
			this._readedMessages = new System.Windows.Forms.CheckBox();
			this._browseFileButton = new System.Windows.Forms.Button();
			this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this._exportOptionsGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// _cancelButton
			// 
			resources.ApplyResources(this._cancelButton, "_cancelButton");
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Name = "_cancelButton";
			// 
			// _okButton
			// 
			resources.ApplyResources(this._okButton, "_okButton");
			this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._okButton.Name = "_okButton";
			// 
			// _fileNameBox
			// 
			resources.ApplyResources(this._fileNameBox, "_fileNameBox");
			this._fileNameBox.Name = "_fileNameBox";
			this._fileNameBox.TextChanged += new System.EventHandler(this._fileNameBox_TextChanged);
			// 
			// _labelFileName
			// 
			resources.ApplyResources(this._labelFileName, "_labelFileName");
			this._labelFileName.Name = "_labelFileName";
			// 
			// _exportOptionsGroup
			// 
			this._exportOptionsGroup.Controls.Add(this._removeFavoriteMessages);
			this._exportOptionsGroup.Controls.Add(this._favoriteMessages);
			this._exportOptionsGroup.Controls.Add(this._removeMarkers);
			this._exportOptionsGroup.Controls.Add(this._markedMessages);
			this._exportOptionsGroup.Controls.Add(this._readedMessages);
			resources.ApplyResources(this._exportOptionsGroup, "_exportOptionsGroup");
			this._exportOptionsGroup.Name = "_exportOptionsGroup";
			this._exportOptionsGroup.TabStop = false;
			// 
			// _removeFavoriteMessages
			// 
			resources.ApplyResources(this._removeFavoriteMessages, "_removeFavoriteMessages");
			this._removeFavoriteMessages.Name = "_removeFavoriteMessages";
			this._removeFavoriteMessages.UseVisualStyleBackColor = true;
			// 
			// _favoriteMessages
			// 
			resources.ApplyResources(this._favoriteMessages, "_favoriteMessages");
			this._favoriteMessages.Checked = true;
			this._favoriteMessages.CheckState = System.Windows.Forms.CheckState.Checked;
			this._favoriteMessages.Name = "_favoriteMessages";
			this._favoriteMessages.UseVisualStyleBackColor = true;
			this._favoriteMessages.CheckedChanged += new System.EventHandler(this.Options_CheckedChanged);
			// 
			// _removeMarkers
			// 
			resources.ApplyResources(this._removeMarkers, "_removeMarkers");
			this._removeMarkers.Name = "_removeMarkers";
			this._removeMarkers.UseVisualStyleBackColor = true;
			// 
			// _markedMessages
			// 
			resources.ApplyResources(this._markedMessages, "_markedMessages");
			this._markedMessages.Checked = true;
			this._markedMessages.CheckState = System.Windows.Forms.CheckState.Checked;
			this._markedMessages.Name = "_markedMessages";
			this._markedMessages.UseVisualStyleBackColor = true;
			this._markedMessages.CheckedChanged += new System.EventHandler(this.Options_CheckedChanged);
			// 
			// _readedMessages
			// 
			resources.ApplyResources(this._readedMessages, "_readedMessages");
			this._readedMessages.Checked = true;
			this._readedMessages.CheckState = System.Windows.Forms.CheckState.Checked;
			this._readedMessages.Name = "_readedMessages";
			this._readedMessages.UseVisualStyleBackColor = true;
			this._readedMessages.CheckedChanged += new System.EventHandler(this.Options_CheckedChanged);
			// 
			// _browseFileButton
			// 
			resources.ApplyResources(this._browseFileButton, "_browseFileButton");
			this._browseFileButton.Name = "_browseFileButton";
			this._browseFileButton.UseVisualStyleBackColor = true;
			this._browseFileButton.Click += new System.EventHandler(this._browseFileButton_Click);
			// 
			// _openFileDialog
			// 
			this._openFileDialog.DefaultExt = "xml";
			resources.ApplyResources(this._openFileDialog, "_openFileDialog");
			this._openFileDialog.RestoreDirectory = true;
			// 
			// RestoreStateDialog
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
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RestoreStateDialog";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RestoreStateDialog_FormClosing);
			this._exportOptionsGroup.ResumeLayout(false);
			this._exportOptionsGroup.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.Button _okButton;
		private System.Windows.Forms.TextBox _fileNameBox;
		private System.Windows.Forms.Label _labelFileName;
		private System.Windows.Forms.GroupBox _exportOptionsGroup;
		private System.Windows.Forms.Button _browseFileButton;
		private System.Windows.Forms.CheckBox _removeFavoriteMessages;
		private System.Windows.Forms.CheckBox _favoriteMessages;
		private System.Windows.Forms.CheckBox _removeMarkers;
		private System.Windows.Forms.CheckBox _markedMessages;
		private System.Windows.Forms.CheckBox _readedMessages;
		private System.Windows.Forms.OpenFileDialog _openFileDialog;
	}
}