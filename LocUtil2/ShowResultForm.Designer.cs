namespace Rsdn.LocUtil
{
	internal partial class ShowResultForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowResultForm));
			this._closeButton = new System.Windows.Forms.Button();
			this._editor = new Rsdn.Scintilla.ScintillaEditor();
			this._resultGroup = new System.Windows.Forms.GroupBox();
			this._resultGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// _closeButton
			// 
			this._closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._closeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._closeButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._closeButton.Location = new System.Drawing.Point(397, 406);
			this._closeButton.Name = "_closeButton";
			this._closeButton.Size = new System.Drawing.Size(75, 23);
			this._closeButton.TabIndex = 0;
			this._closeButton.Text = "Close";
			// 
			// _editor
			// 
			this._editor.CaretColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this._editor.CaretLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
			this._editor.CaretPosition = 0;
			this._editor.Dock = System.Windows.Forms.DockStyle.Fill;
			this._editor.EdgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this._editor.EdgeColumn = 0;
			this._editor.Lexer = Rsdn.Scintilla.SciLexer.CPlusPlus;
			this._editor.Location = new System.Drawing.Point(3, 16);
			this._editor.Name = "_editor";
			this._editor.Size = new System.Drawing.Size(458, 373);
			this._editor.TabIndex = 1;
			// 
			// _resultGroup
			// 
			this._resultGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this._resultGroup.Controls.Add(this._editor);
			this._resultGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._resultGroup.Location = new System.Drawing.Point(8, 8);
			this._resultGroup.Name = "_resultGroup";
			this._resultGroup.Size = new System.Drawing.Size(464, 392);
			this._resultGroup.TabIndex = 2;
			this._resultGroup.TabStop = false;
			this._resultGroup.Text = "Generation result";
			// 
			// ShowResultForm
			// 
			this.AcceptButton = this._closeButton;
			this.CancelButton = this._closeButton;
			this.ClientSize = new System.Drawing.Size(480, 438);
			this.Controls.Add(this._resultGroup);
			this.Controls.Add(this._closeButton);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ShowResultForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Generation result";
			this._resultGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.Button _closeButton;
		private System.Windows.Forms.GroupBox _resultGroup;

		private Rsdn.Scintilla.ScintillaEditor _editor;
	}
}