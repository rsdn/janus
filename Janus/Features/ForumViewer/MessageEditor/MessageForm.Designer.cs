namespace Rsdn.Janus
{
	internal partial class MessageForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer _components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				StyleConfig.StyleChange -= OnStyleChanged;

				_menuGenerator.Dispose();
				_toolbarGenerator.Dispose();
				_tagsbarGenerator.Dispose();

				if (_components != null)
					_components.Dispose();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageForm));
			this._contentPanel = new System.Windows.Forms.Panel();
			this._editorContainer = new Rsdn.Janus.Framework.FramePanel();
			this._messageEditor = new Rsdn.Janus.JanusScintilla();
			this._bottomSplitter = new Rsdn.Janus.Framework.CollapsibleSplitter();
			this._tagsBar = new Rsdn.Janus.SmilesToolbar();
			this._headerPanel = new System.Windows.Forms.Panel();
			this._subjectTextBox = new System.Windows.Forms.TextBox();
			this._forumsComboBox = new Rsdn.Janus.ForumsBox(_serviceManager);
			this._subjectLabel = new System.Windows.Forms.Label();
			this._fromLabel = new System.Windows.Forms.Label();
			this._forumLabel = new System.Windows.Forms.Label();
			this._fromUserLabel = new System.Windows.Forms.Label();
			this._menuStrip = new System.Windows.Forms.MenuStrip();
			this._toolStrip = new System.Windows.Forms.ToolStrip();
			this._contentPanel.SuspendLayout();
			this._editorContainer.SuspendLayout();
			this._headerPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// _contentPanel
			// 
			this._contentPanel.Controls.Add(this._editorContainer);
			this._contentPanel.Controls.Add(this._bottomSplitter);
			this._contentPanel.Controls.Add(this._headerPanel);
			this._contentPanel.Controls.Add(this._tagsBar);
			resources.ApplyResources(this._contentPanel, "_contentPanel");
			this._contentPanel.Name = "_contentPanel";
			// 
			// _editorContainer
			// 
			this._editorContainer.Controls.Add(this._messageEditor);
			resources.ApplyResources(this._editorContainer, "_editorContainer");
			this._editorContainer.Name = "_editorContainer";
			this._editorContainer.PanelBorderStyle = Rsdn.Janus.Framework.FramePanelBorderStyle.System;
			// 
			// _messageEditor
			// 
			this._messageEditor.CaretColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this._messageEditor.CaretLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
			this._messageEditor.CaretPosition = 0;
			resources.ApplyResources(this._messageEditor, "_messageEditor");
			this._messageEditor.EdgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this._messageEditor.Lexer = Rsdn.Scintilla.SciLexer.Container;
			this._messageEditor.MatchBraces = true;
			this._messageEditor.Name = "_messageEditor";
			this._messageEditor.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			this._messageEditor.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			this._messageEditor.ShowIndentationGuides = true;
			this._messageEditor.UseDisplayLines = true;
			this._messageEditor.WrapLines = true;
			this._messageEditor.Modified += new Rsdn.Scintilla.ModifiedEventHandler(this.MessageEditorModified);
			this._messageEditor.IsModifiedChanged += new System.EventHandler(this.MessageEditorIsModifiedChanged);
			this._messageEditor.CharAdded += new System.Windows.Forms.KeyPressEventHandler(this.MessageEditorCharAdded);
			this._messageEditor.AutocompleteSelected += new System.EventHandler<Rsdn.Scintilla.AutocompleteSelectedEventArgs>(this.MessageEditorAutocompleteSelected);
			this._messageEditor.StyleNeeded += new Rsdn.Scintilla.StyleNeededEventHandler(this.MessageEditorStyleNeeded);
			// 
			// _bottomSplitter
			// 
			this._bottomSplitter.AnimationDelay = 20;
			this._bottomSplitter.AnimationStep = 20;
			this._bottomSplitter.BorderStyle3D = System.Windows.Forms.Border3DStyle.Flat;
			this._bottomSplitter.ControlToHide = this._tagsBar;
			resources.ApplyResources(this._bottomSplitter, "_bottomSplitter");
			this._bottomSplitter.ExpandParentForm = false;
			this._bottomSplitter.Name = "bottomSplitter";
			this._bottomSplitter.TabStop = false;
			this._bottomSplitter.UseAnimations = false;
			this._bottomSplitter.VisualStyle = Rsdn.Janus.Framework.VisualStyle.Mozilla;
			// 
			// _tagsBar
			// 
			resources.ApplyResources(this._tagsBar, "_tagsBar");
			this._tagsBar.Name = "_tagsBar";
			// 
			// _headerPanel
			// 
			this._headerPanel.Controls.Add(this._subjectTextBox);
			this._headerPanel.Controls.Add(this._forumsComboBox);
			this._headerPanel.Controls.Add(this._subjectLabel);
			this._headerPanel.Controls.Add(this._fromLabel);
			this._headerPanel.Controls.Add(this._forumLabel);
			this._headerPanel.Controls.Add(this._fromUserLabel);
			resources.ApplyResources(this._headerPanel, "_headerPanel");
			this._headerPanel.Name = "_headerPanel";
			// 
			// _subjectTextBox
			// 
			resources.ApplyResources(this._subjectTextBox, "_subjectTextBox");
			this._subjectTextBox.Name = "_subjectTextBox";
			this._subjectTextBox.TextChanged += new System.EventHandler(this.SubjectTextBoxTextChanged);
			this._subjectTextBox.ModifiedChanged += new System.EventHandler(this.SubjectTextBoxModifiedChanged);
			// 
			// _forumsComboBox
			// 
			resources.ApplyResources(this._forumsComboBox, "_forumsComboBox");
			this._forumsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._forumsComboBox.FormattingEnabled = true;
			this._forumsComboBox.Name = "_forumsComboBox";
			// 
			// _subjectLabel
			// 
			resources.ApplyResources(this._subjectLabel, "_subjectLabel");
			this._subjectLabel.Name = "_subjectLabel";
			// 
			// _fromLabel
			// 
			resources.ApplyResources(this._fromLabel, "_fromLabel");
			this._fromLabel.Name = "_fromLabel";
			// 
			// _forumLabel
			// 
			resources.ApplyResources(this._forumLabel, "_forumLabel");
			this._forumLabel.Name = "_forumLabel";
			// 
			// _fromUserLabel
			// 
			this._fromUserLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			resources.ApplyResources(this._fromUserLabel, "_fromUserLabel");
			this._fromUserLabel.Name = "_fromUserLabel";
			// 
			// _menuStrip
			// 
			resources.ApplyResources(this._menuStrip, "_menuStrip");
			this._menuStrip.Name = "_menuStrip";
			// 
			// _toolStrip
			// 
			this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this._toolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
			resources.ApplyResources(this._toolStrip, "_toolStrip");
			this._toolStrip.Name = "_toolStrip";
			// 
			// MessageForm
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this._contentPanel);
			this.Controls.Add(this._toolStrip);
			this.Controls.Add(this._menuStrip);
			this.MainMenuStrip = this._menuStrip;
			this.Name = "MessageForm";
			this._contentPanel.ResumeLayout(false);
			this._editorContainer.ResumeLayout(false);
			this._headerPanel.ResumeLayout(false);
			this._headerPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private JanusScintilla _messageEditor;
		private System.Windows.Forms.TextBox _subjectTextBox;
		private Rsdn.Janus.ForumsBox _forumsComboBox;
		private Rsdn.Janus.Framework.FramePanel _editorContainer;
		private System.Windows.Forms.Panel _contentPanel;
		private System.Windows.Forms.Panel _headerPanel;
		private System.Windows.Forms.Label _fromUserLabel;
		private System.Windows.Forms.Label _subjectLabel;
		private System.Windows.Forms.Label _fromLabel;
		private System.Windows.Forms.Label _forumLabel;
		private Rsdn.Janus.Framework.CollapsibleSplitter _bottomSplitter;
		private System.Windows.Forms.ToolStrip _toolStrip;
		private System.Windows.Forms.MenuStrip _menuStrip;
	}
}