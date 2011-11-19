namespace ImageUtil
{
	partial class ImageNameForm
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
			this._nameBox = new System.Windows.Forms.TextBox();
			this._descriptionLabel = new System.Windows.Forms.Label();
			this._cancelButton = new System.Windows.Forms.Button();
			this._okButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// _nameBox
			// 
			this._nameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._nameBox.Location = new System.Drawing.Point(12, 25);
			this._nameBox.Name = "_nameBox";
			this._nameBox.Size = new System.Drawing.Size(250, 20);
			this._nameBox.TabIndex = 0;
			this._nameBox.TextChanged += new System.EventHandler(this.NameBoxTextChanged);
			// 
			// _descriptionLabel
			// 
			this._descriptionLabel.AutoSize = true;
			this._descriptionLabel.Location = new System.Drawing.Point(12, 9);
			this._descriptionLabel.Name = "_descriptionLabel";
			this._descriptionLabel.Size = new System.Drawing.Size(68, 13);
			this._descriptionLabel.TabIndex = 1;
			this._descriptionLabel.Text = "Image name:";
			// 
			// _cancelButton
			// 
			this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Location = new System.Drawing.Point(187, 51);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new System.Drawing.Size(75, 23);
			this._cancelButton.TabIndex = 2;
			this._cancelButton.Text = "Cancel";
			this._cancelButton.UseVisualStyleBackColor = true;
			// 
			// _okButton
			// 
			this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._okButton.Enabled = false;
			this._okButton.Location = new System.Drawing.Point(106, 51);
			this._okButton.Name = "_okButton";
			this._okButton.Size = new System.Drawing.Size(75, 23);
			this._okButton.TabIndex = 3;
			this._okButton.Text = "OK";
			this._okButton.UseVisualStyleBackColor = true;
			// 
			// ImageNameForm
			// 
			this.AcceptButton = this._okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._cancelButton;
			this.ClientSize = new System.Drawing.Size(274, 86);
			this.Controls.Add(this._okButton);
			this.Controls.Add(this._cancelButton);
			this.Controls.Add(this._descriptionLabel);
			this.Controls.Add(this._nameBox);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(1000, 122);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(200, 122);
			this.Name = "ImageNameForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Enter image name";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox _nameBox;
		private System.Windows.Forms.Label _descriptionLabel;
		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.Button _okButton;
	}
}