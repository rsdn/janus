namespace Rsdn.Janus.Mssql
{
	partial class StringEnterForm
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
			this._string = new System.Windows.Forms.TextBox();
			this._description = new System.Windows.Forms.Label();
			this._cancelButton = new System.Windows.Forms.Button();
			this._okButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// _string
			// 
			this._string.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
																		| System.Windows.Forms.AnchorStyles.Right)));
			this._string.Location = new System.Drawing.Point(12, 26);
			this._string.Name = "_string";
			this._string.Size = new System.Drawing.Size(223, 20);
			this._string.TabIndex = 1;
			this._string.TextChanged += new System.EventHandler(this.StringTextChanged);
			// 
			// _description
			// 
			this._description.AutoSize = true;
			this._description.Location = new System.Drawing.Point(12, 9);
			this._description.Name = "_description";
			this._description.Size = new System.Drawing.Size(63, 13);
			this._description.TabIndex = 0;
			this._description.Text = "Enter string:";
			// 
			// _cancelButton
			// 
			this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Location = new System.Drawing.Point(160, 53);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new System.Drawing.Size(75, 23);
			this._cancelButton.TabIndex = 3;
			this._cancelButton.Text = "Cancel";
			this._cancelButton.UseVisualStyleBackColor = true;
			// 
			// _okButton
			// 
			this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._okButton.Location = new System.Drawing.Point(12, 53);
			this._okButton.Name = "_okButton";
			this._okButton.Size = new System.Drawing.Size(75, 23);
			this._okButton.TabIndex = 2;
			this._okButton.Text = "OK";
			this._okButton.UseVisualStyleBackColor = true;
			// 
			// StringEnterForm
			// 
			this.AcceptButton = this._okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._cancelButton;
			this.ClientSize = new System.Drawing.Size(247, 87);
			this.Controls.Add(this._okButton);
			this.Controls.Add(this._cancelButton);
			this.Controls.Add(this._description);
			this.Controls.Add(this._string);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(2000, 113);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(185, 113);
			this.Name = "StringEnterForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "StringEnterForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox _string;
		private System.Windows.Forms.Label _description;
		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.Button _okButton;
	}
}