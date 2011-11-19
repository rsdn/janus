namespace Rsdn.LocUtil
{
	internal partial class ResourceNameForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResourceNameForm));
			this._nameBox = new System.Windows.Forms.TextBox();
			this._label = new System.Windows.Forms.Label();
			this._okButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// _nameBox
			// 
			this._nameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this._nameBox.Location = new System.Drawing.Point(8, 24);
			this._nameBox.Name = "_nameBox";
			this._nameBox.Size = new System.Drawing.Size(400, 20);
			this._nameBox.TabIndex = 0;
			this._nameBox.TextChanged += new System.EventHandler(this._nameBox_TextChanged);
			// 
			// _label
			// 
			this._label.Location = new System.Drawing.Point(8, 8);
			this._label.Name = "_label";
			this._label.Size = new System.Drawing.Size(100, 16);
			this._label.TabIndex = 1;
			this._label.Text = "Resource name:";
			// 
			// _okButton
			// 
			this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._okButton.Enabled = false;
			this._okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._okButton.Location = new System.Drawing.Point(252, 56);
			this._okButton.Name = "_okButton";
			this._okButton.Size = new System.Drawing.Size(75, 23);
			this._okButton.TabIndex = 2;
			this._okButton.Text = "OK";
			// 
			// _cancelButton
			// 
			this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._cancelButton.Location = new System.Drawing.Point(333, 56);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new System.Drawing.Size(75, 23);
			this._cancelButton.TabIndex = 2;
			this._cancelButton.Text = "Cancel";
			// 
			// ResourceNameForm
			// 
			this.AcceptButton = this._okButton;
			this.CancelButton = this._cancelButton;
			this.ClientSize = new System.Drawing.Size(416, 82);
			this.Controls.Add(this._okButton);
			this.Controls.Add(this._label);
			this.Controls.Add(this._nameBox);
			this.Controls.Add(this._cancelButton);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(800, 120);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(200, 120);
			this.Name = "ResourceNameForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Enter resource name";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private System.Windows.Forms.TextBox _nameBox;
		private System.Windows.Forms.Label _label;
		private System.Windows.Forms.Button _okButton;
		private System.Windows.Forms.Button _cancelButton;
	}
}