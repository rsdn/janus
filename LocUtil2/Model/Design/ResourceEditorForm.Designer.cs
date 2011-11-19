namespace Rsdn.LocUtil.Model.Design
{
	internal partial class ResourceEditorForm
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
			this._okButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			this._editor = new Rsdn.Scintilla.ScintillaEditor();
			this._localeNameLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// _okButton
			// 
			this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._okButton.Location = new System.Drawing.Point(366, 336);
			this._okButton.Name = "_okButton";
			this._okButton.Size = new System.Drawing.Size(75, 23);
			this._okButton.TabIndex = 0;
			this._okButton.Text = "OK";
			// 
			// _cancelButton
			// 
			this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._cancelButton.Location = new System.Drawing.Point(447, 336);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new System.Drawing.Size(75, 23);
			this._cancelButton.TabIndex = 1;
			this._cancelButton.Text = "Cancel";
			// 
			// _editor
			// 
			this._editor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this._editor.CaretColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this._editor.CaretLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
			this._editor.CaretPosition = 0;
			this._editor.EdgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this._editor.Lexer = Rsdn.Scintilla.SciLexer.Container;
			this._editor.Location = new System.Drawing.Point(10, 32);
			this._editor.Name = "_editor";
			this._editor.Size = new System.Drawing.Size(512, 296);
			this._editor.TabIndex = 2;
			this._editor.Text = "fdfdf";
			// 
			// _localeNameLabel
			// 
			this._localeNameLabel.AutoSize = true;
			this._localeNameLabel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._localeNameLabel.Location = new System.Drawing.Point(8, 8);
			this._localeNameLabel.Name = "_localeNameLabel";
			this._localeNameLabel.Size = new System.Drawing.Size(0, 16);
			this._localeNameLabel.TabIndex = 3;
			// 
			// ResourceEditorForm
			// 
			this.AcceptButton = this._okButton;
			this.CancelButton = this._cancelButton;
			this.ClientSize = new System.Drawing.Size(532, 366);
			this.Controls.Add(this._localeNameLabel);
			this.Controls.Add(this._editor);
			this.Controls.Add(this._cancelButton);
			this.Controls.Add(this._okButton);
			this.MinimizeBox = false;
			this.Name = "ResourceEditorForm";
			this.ShowInTaskbar = false;
			this.Text = "Enter resource text:";
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Button _okButton;
		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.Label _localeNameLabel;

		private Rsdn.Scintilla.ScintillaEditor _editor;
	}
}
