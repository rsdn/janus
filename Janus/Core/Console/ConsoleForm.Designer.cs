namespace Rsdn.Janus
{
	partial class ConsoleForm
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
			if (disposing)
			{
				if (_toolbarGenerator != null)
					_toolbarGenerator.Dispose();
				if ((components != null))
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsoleForm));
			this._toolStrip = new System.Windows.Forms.ToolStrip();
			this._consoleEditor = new Rsdn.Janus.JanusScintilla();
			this.SuspendLayout();
			// 
			// _toolStrip
			// 
			this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			resources.ApplyResources(this._toolStrip, "_toolStrip");
			this._toolStrip.Name = "_toolStrip";
			// 
			// _consoleEditor
			// 
			this._consoleEditor.AutocompleteIgnoreCase = true;
			this._consoleEditor.CaretColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this._consoleEditor.CaretLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
			this._consoleEditor.CaretPosition = 0;
			resources.ApplyResources(this._consoleEditor, "_consoleEditor");
			this._consoleEditor.EdgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this._consoleEditor.Lexer = Rsdn.Scintilla.SciLexer.Container;
			this._consoleEditor.Name = "_consoleEditor";
			this._consoleEditor.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			this._consoleEditor.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			this._consoleEditor.WrapLines = true;
			this._consoleEditor.Modified += new Rsdn.Scintilla.ModifiedEventHandler(this.ConsoleEditorModified);
			this._consoleEditor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ConsoleEditorKeyDown);
			this._consoleEditor.CharAdded += new System.Windows.Forms.KeyPressEventHandler(this.ConsoleEditorCharAdded);
			this._consoleEditor.StyleNeeded += new Rsdn.Scintilla.StyleNeededEventHandler(this.ConsoleEditorStyleNeeded);
			// 
			// ConsoleForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._consoleEditor);
			this.Controls.Add(this._toolStrip);
			this.Name = "ConsoleForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip _toolStrip;
		private JanusScintilla _consoleEditor;
	}
}