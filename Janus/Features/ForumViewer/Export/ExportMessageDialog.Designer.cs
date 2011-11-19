namespace Rsdn.Janus
{
	partial class ExportMessageDialog
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportMessageDialog));
			this._fileNameBox = new System.Windows.Forms.TextBox();
			this._fileNameLabel = new System.Windows.Forms.Label();
			this._browseFileButton = new System.Windows.Forms.Button();
			this._exportModeGroup = new System.Windows.Forms.GroupBox();
			this._unreadOnlyCheckBox = new System.Windows.Forms.CheckBox();
			this._forumRadioButton = new System.Windows.Forms.RadioButton();
			this._topicRadioButton = new System.Windows.Forms.RadioButton();
			this._selectedMessagesRadioButton = new System.Windows.Forms.RadioButton();
			this._formatGroup = new System.Windows.Forms.GroupBox();
			this._mhtFormatRadioButton = new System.Windows.Forms.RadioButton();
			this._htmlFormatRadioButton = new System.Windows.Forms.RadioButton();
			this._textFormatRadioButton = new System.Windows.Forms.RadioButton();
			this._okButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this._exportModeGroup.SuspendLayout();
			this._formatGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// _fileNameBox
			// 
			resources.ApplyResources(this._fileNameBox, "_fileNameBox");
			this._fileNameBox.Name = "_fileNameBox";
			this._fileNameBox.TextChanged += new System.EventHandler(this._fileNameBox_TextChanged);
			// 
			// _fileNameLabel
			// 
			resources.ApplyResources(this._fileNameLabel, "_fileNameLabel");
			this._fileNameLabel.Name = "_fileNameLabel";
			// 
			// _browseFileButton
			// 
			resources.ApplyResources(this._browseFileButton, "_browseFileButton");
			this._browseFileButton.Name = "_browseFileButton";
			this._browseFileButton.Click += new System.EventHandler(this._browseFileButton_Click);
			// 
			// _exportModeGroup
			// 
			resources.ApplyResources(this._exportModeGroup, "_exportModeGroup");
			this._exportModeGroup.Controls.Add(this._unreadOnlyCheckBox);
			this._exportModeGroup.Controls.Add(this._forumRadioButton);
			this._exportModeGroup.Controls.Add(this._topicRadioButton);
			this._exportModeGroup.Controls.Add(this._selectedMessagesRadioButton);
			this._exportModeGroup.Name = "_exportModeGroup";
			this._exportModeGroup.TabStop = false;
			// 
			// _unreadOnlyCheckBox
			// 
			resources.ApplyResources(this._unreadOnlyCheckBox, "_unreadOnlyCheckBox");
			this._unreadOnlyCheckBox.Name = "_unreadOnlyCheckBox";
			this._unreadOnlyCheckBox.UseVisualStyleBackColor = true;
			// 
			// _forumRadioButton
			// 
			resources.ApplyResources(this._forumRadioButton, "_forumRadioButton");
			this._forumRadioButton.Name = "_forumRadioButton";
			// 
			// _topicRadioButton
			// 
			resources.ApplyResources(this._topicRadioButton, "_topicRadioButton");
			this._topicRadioButton.Name = "_topicRadioButton";
			// 
			// _selectedMessagesRadioButton
			// 
			resources.ApplyResources(this._selectedMessagesRadioButton, "_selectedMessagesRadioButton");
			this._selectedMessagesRadioButton.Checked = true;
			this._selectedMessagesRadioButton.Name = "_selectedMessagesRadioButton";
			this._selectedMessagesRadioButton.TabStop = true;
			// 
			// _formatGroup
			// 
			resources.ApplyResources(this._formatGroup, "_formatGroup");
			this._formatGroup.Controls.Add(this._mhtFormatRadioButton);
			this._formatGroup.Controls.Add(this._htmlFormatRadioButton);
			this._formatGroup.Controls.Add(this._textFormatRadioButton);
			this._formatGroup.Name = "_formatGroup";
			this._formatGroup.TabStop = false;
			// 
			// _mhtFormatRadioButton
			// 
			resources.ApplyResources(this._mhtFormatRadioButton, "_mhtFormatRadioButton");
			this._mhtFormatRadioButton.Name = "_mhtFormatRadioButton";
			this._mhtFormatRadioButton.Tag = "mht";
			this._mhtFormatRadioButton.Click += new System.EventHandler(this._formatBttClick);
			// 
			// _htmlFormatRadioButton
			// 
			resources.ApplyResources(this._htmlFormatRadioButton, "_htmlFormatRadioButton");
			this._htmlFormatRadioButton.Name = "_htmlFormatRadioButton";
			this._htmlFormatRadioButton.Tag = "html";
			this._htmlFormatRadioButton.Click += new System.EventHandler(this._formatBttClick);
			// 
			// _textFormatRadioButton
			// 
			resources.ApplyResources(this._textFormatRadioButton, "_textFormatRadioButton");
			this._textFormatRadioButton.Checked = true;
			this._textFormatRadioButton.Name = "_textFormatRadioButton";
			this._textFormatRadioButton.TabStop = true;
			this._textFormatRadioButton.Tag = "txt";
			this._textFormatRadioButton.Click += new System.EventHandler(this._formatBttClick);
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
			// _saveFileDialog
			// 
			this._saveFileDialog.DefaultExt = "txt";
			resources.ApplyResources(this._saveFileDialog, "_saveFileDialog");
			this._saveFileDialog.RestoreDirectory = true;
			// 
			// ExportMessageDialog
			// 
			this.AcceptButton = this._okButton;
			this.CancelButton = this._cancelButton;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this._cancelButton);
			this.Controls.Add(this._okButton);
			this.Controls.Add(this._formatGroup);
			this.Controls.Add(this._exportModeGroup);
			this.Controls.Add(this._browseFileButton);
			this.Controls.Add(this._fileNameLabel);
			this.Controls.Add(this._fileNameBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ExportMessageDialog";
			this.ShowInTaskbar = false;
			this._exportModeGroup.ResumeLayout(false);
			this._exportModeGroup.PerformLayout();
			this._formatGroup.ResumeLayout(false);
			this._formatGroup.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox _fileNameBox;
		private System.Windows.Forms.Label _fileNameLabel;
		private System.Windows.Forms.Button _browseFileButton;
		private System.Windows.Forms.RadioButton _selectedMessagesRadioButton;
		private System.Windows.Forms.RadioButton _topicRadioButton;
		private System.Windows.Forms.GroupBox _formatGroup;
		private System.Windows.Forms.RadioButton _textFormatRadioButton;
		private System.Windows.Forms.RadioButton _htmlFormatRadioButton;
		private System.Windows.Forms.Button _okButton;
		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.RadioButton _forumRadioButton;
		private System.Windows.Forms.SaveFileDialog _saveFileDialog;
		private System.Windows.Forms.GroupBox _exportModeGroup;
		private System.Windows.Forms.RadioButton _mhtFormatRadioButton;
		private System.Windows.Forms.CheckBox _unreadOnlyCheckBox;
	}
}