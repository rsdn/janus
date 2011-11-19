namespace Rsdn.LocUtil
{
	partial class NewCultureForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewCultureForm));
			this._okButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			this._cultureBox = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// _okButton
			// 
			this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._okButton.Enabled = false;
			this._okButton.Location = new System.Drawing.Point(92, 40);
			this._okButton.Name = "_okButton";
			this._okButton.Size = new System.Drawing.Size(75, 23);
			this._okButton.TabIndex = 1;
			this._okButton.Text = "OK";
			this._okButton.UseVisualStyleBackColor = true;
			// 
			// _cancelButton
			// 
			this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Location = new System.Drawing.Point(173, 40);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new System.Drawing.Size(75, 23);
			this._cancelButton.TabIndex = 2;
			this._cancelButton.Text = "Cancel";
			this._cancelButton.UseVisualStyleBackColor = true;
			// 
			// _cultureBox
			// 
			this._cultureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this._cultureBox.DisplayMember = "DisplayName";
			this._cultureBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._cultureBox.FormattingEnabled = true;
			this._cultureBox.Location = new System.Drawing.Point(12, 13);
			this._cultureBox.Name = "_cultureBox";
			this._cultureBox.Size = new System.Drawing.Size(236, 21);
			this._cultureBox.Sorted = true;
			this._cultureBox.TabIndex = 0;
			this._cultureBox.SelectedIndexChanged += new System.EventHandler(this._cultureBox_SelectedIndexChanged);
			// 
			// NewCultureForm
			// 
			this.AcceptButton = this._okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._cancelButton;
			this.ClientSize = new System.Drawing.Size(260, 75);
			this.Controls.Add(this._cultureBox);
			this.Controls.Add(this._cancelButton);
			this.Controls.Add(this._okButton);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(10000, 109);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(70, 109);
			this.Name = "NewCultureForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select Culture";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button _okButton;
		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.ComboBox _cultureBox;
	}
}