namespace Rsdn.Janus
{
	internal partial class TagLineEditorForm
	{
		private System.ComponentModel.IContainer components;

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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.Button _resetButton;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TagLineEditorForm));
			this._okButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			this._forumListGroup = new System.Windows.Forms.GroupBox();
			this._rightPanel = new System.Windows.Forms.Panel();
			this._forumsTree = new System.Windows.Forms.TreeView();
			this._forumsImages = new System.Windows.Forms.ImageList(this.components);
			this._allForumsCheck = new System.Windows.Forms.CheckBox();
			this._contentPanel = new System.Windows.Forms.Panel();
			this._leftPanel = new System.Windows.Forms.Panel();
			this._nameLabel = new System.Windows.Forms.Label();
			this._nameBox = new System.Windows.Forms.TextBox();
			this._taglineFormatGroup = new System.Windows.Forms.GroupBox();
			this._formatEditor = new Rsdn.Scintilla.ScintillaEditor();
			this._macroStyle = new Rsdn.Scintilla.TextStyle("Default", 1, new System.Drawing.Font("Arial", 10F), System.Drawing.Color.Red, System.Drawing.Color.MistyRose, false, Rsdn.Scintilla.CaseMode.Mixed, true, false, Rsdn.Scintilla.PredefinedStyle.None);
			this._defaultStyle = new Rsdn.Scintilla.TextStyle("PredefinedDefault", 0, new System.Drawing.Font("Arial", 10F), System.Drawing.Color.Brown, System.Drawing.Color.White, false, Rsdn.Scintilla.CaseMode.Mixed, true, false, Rsdn.Scintilla.PredefinedStyle.None);
			this._vertSplitter = new System.Windows.Forms.Splitter();
			this._macrosButton = new System.Windows.Forms.Button();
			this._macrosContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			_resetButton = new System.Windows.Forms.Button();
			this._forumListGroup.SuspendLayout();
			this._rightPanel.SuspendLayout();
			this._contentPanel.SuspendLayout();
			this._leftPanel.SuspendLayout();
			this._taglineFormatGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// _resetButton
			// 
			resources.ApplyResources(_resetButton, "_resetButton");
			_resetButton.Name = "_resetButton";
			_resetButton.UseVisualStyleBackColor = true;
			_resetButton.Click += new System.EventHandler(this.MakeDefaultButtonClick);
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
			// _forumListGroup
			// 
			this._forumListGroup.Controls.Add(this._rightPanel);
			resources.ApplyResources(this._forumListGroup, "_forumListGroup");
			this._forumListGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._forumListGroup.Name = "_forumListGroup";
			this._forumListGroup.TabStop = false;
			// 
			// _rightPanel
			// 
			this._rightPanel.Controls.Add(this._forumsTree);
			this._rightPanel.Controls.Add(this._allForumsCheck);
			resources.ApplyResources(this._rightPanel, "_rightPanel");
			this._rightPanel.Name = "_rightPanel";
			// 
			// _forumsTree
			// 
			this._forumsTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._forumsTree.CheckBoxes = true;
			resources.ApplyResources(this._forumsTree, "_forumsTree");
			this._forumsTree.ImageList = this._forumsImages;
			this._forumsTree.ItemHeight = 16;
			this._forumsTree.Name = "_forumsTree";
			// 
			// _forumsImages
			// 
			this._forumsImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			resources.ApplyResources(this._forumsImages, "_forumsImages");
			this._forumsImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// _allForumsCheck
			// 
			this._allForumsCheck.Checked = true;
			this._allForumsCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			resources.ApplyResources(this._allForumsCheck, "_allForumsCheck");
			this._allForumsCheck.Name = "_allForumsCheck";
			this._allForumsCheck.CheckedChanged += new System.EventHandler(this.AllForumsCheckCheckedChanged);
			// 
			// _contentPanel
			// 
			resources.ApplyResources(this._contentPanel, "_contentPanel");
			this._contentPanel.Controls.Add(this._leftPanel);
			this._contentPanel.Controls.Add(this._vertSplitter);
			this._contentPanel.Controls.Add(this._forumListGroup);
			this._contentPanel.Name = "_contentPanel";
			// 
			// _leftPanel
			// 
			this._leftPanel.Controls.Add(this._nameLabel);
			this._leftPanel.Controls.Add(this._nameBox);
			this._leftPanel.Controls.Add(this._taglineFormatGroup);
			resources.ApplyResources(this._leftPanel, "_leftPanel");
			this._leftPanel.Name = "_leftPanel";
			// 
			// _nameLabel
			// 
			resources.ApplyResources(this._nameLabel, "_nameLabel");
			this._nameLabel.Name = "_nameLabel";
			// 
			// _nameBox
			// 
			resources.ApplyResources(this._nameBox, "_nameBox");
			this._nameBox.Name = "_nameBox";
			// 
			// _taglineFormatGroup
			// 
			resources.ApplyResources(this._taglineFormatGroup, "_taglineFormatGroup");
			this._taglineFormatGroup.Controls.Add(this._formatEditor);
			this._taglineFormatGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._taglineFormatGroup.Name = "_taglineFormatGroup";
			this._taglineFormatGroup.TabStop = false;
			// 
			// _formatEditor
			// 
			this._formatEditor.CaretColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this._formatEditor.CaretLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
			this._formatEditor.CaretPosition = 0;
			resources.ApplyResources(this._formatEditor, "_formatEditor");
			this._formatEditor.EdgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this._formatEditor.EdgeColumn = 0;
			this._formatEditor.Lexer = Rsdn.Scintilla.SciLexer.Container;
			this._formatEditor.Name = "_formatEditor";
			this._formatEditor.TextStyles.AddRange(new Rsdn.Scintilla.TextStyle[] {
			this._macroStyle,
			this._defaultStyle});
			this._formatEditor.StyleNeeded += new Rsdn.Scintilla.StyleNeededEventHandler(this.FormatEditorStyleNeeded);
			this._formatEditor.CharAdded += new System.Windows.Forms.KeyPressEventHandler(this._formatEditor_CharAdded);
			// 
			// _macroStyle
			// 
			this._macroStyle.BackColor = System.Drawing.Color.MistyRose;
			this._macroStyle.Font = new System.Drawing.Font("Arial", 10F);
			this._macroStyle.ForeColor = System.Drawing.Color.Red;
			this._macroStyle.Number = 1;
			// 
			// _defaultStyle
			// 
			this._defaultStyle.Font = new System.Drawing.Font("Arial", 10F);
			this._defaultStyle.ForeColor = System.Drawing.Color.Brown;
			this._defaultStyle.StyleName = "PredefinedDefault";
			// 
			// _vertSplitter
			// 
			resources.ApplyResources(this._vertSplitter, "_vertSplitter");
			this._vertSplitter.Name = "_vertSplitter";
			this._vertSplitter.TabStop = false;
			// 
			// _macrosButton
			// 
			resources.ApplyResources(this._macrosButton, "_macrosButton");
			this._macrosButton.Name = "_macrosButton";
			this._macrosButton.UseVisualStyleBackColor = true;
			this._macrosButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this._macrosButton_MouseDown);
			// 
			// _macrosContextMenuStrip
			// 
			this._macrosContextMenuStrip.Name = "_macrosContextMenuStrip";
			resources.ApplyResources(this._macrosContextMenuStrip, "_macrosContextMenuStrip");
			// 
			// TagLineEditorForm
			// 
			this.AcceptButton = this._okButton;
			this.CancelButton = this._cancelButton;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this._macrosButton);
			this.Controls.Add(this._contentPanel);
			this.Controls.Add(this._cancelButton);
			this.Controls.Add(_resetButton);
			this.Controls.Add(this._okButton);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TagLineEditorForm";
			this.ShowInTaskbar = false;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TagLineEditorForm_FormClosing);
			this._forumListGroup.ResumeLayout(false);
			this._rightPanel.ResumeLayout(false);
			this._contentPanel.ResumeLayout(false);
			this._leftPanel.ResumeLayout(false);
			this._leftPanel.PerformLayout();
			this._taglineFormatGroup.ResumeLayout(false);
			this.ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.Button _macrosButton;
		private System.Windows.Forms.Button _okButton;
		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.Panel _contentPanel;
		private System.Windows.Forms.Splitter _vertSplitter;
		private System.Windows.Forms.GroupBox _forumListGroup;
		private System.Windows.Forms.GroupBox _taglineFormatGroup;
		private System.Windows.Forms.CheckBox _allForumsCheck;
		private System.Windows.Forms.TextBox _nameBox;
		private System.Windows.Forms.Label _nameLabel;
		private System.Windows.Forms.Panel _leftPanel;
		private System.Windows.Forms.Panel _rightPanel;
		private System.Windows.Forms.ImageList _forumsImages;
		private System.Windows.Forms.ContextMenuStrip _macrosContextMenuStrip;
		private System.Windows.Forms.TreeView _forumsTree;
		private Rsdn.Scintilla.ScintillaEditor _formatEditor;
		private Rsdn.Scintilla.TextStyle _macroStyle;
		private Rsdn.Scintilla.TextStyle _defaultStyle;
	}
}