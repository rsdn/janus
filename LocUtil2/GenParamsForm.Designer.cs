namespace Rsdn.LocUtil
{
	internal partial class GenParamsForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenParamsForm));
			this._nsBox = new System.Windows.Forms.TextBox();
			this._nsLabel = new System.Windows.Forms.Label();
			this._okButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			this._showResultBox = new System.Windows.Forms.CheckBox();
			this._internalModifierBox = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// _nsBox
			// 
			this._nsBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this._nsBox.Location = new System.Drawing.Point(8, 24);
			this._nsBox.Name = "_nsBox";
			this._nsBox.Size = new System.Drawing.Size(248, 20);
			this._nsBox.TabIndex = 0;
			// 
			// _nsLabel
			// 
			this._nsLabel.Location = new System.Drawing.Point(8, 8);
			this._nsLabel.Name = "_nsLabel";
			this._nsLabel.Size = new System.Drawing.Size(72, 16);
			this._nsLabel.TabIndex = 1;
			this._nsLabel.Text = "Namespace";
			// 
			// _okButton
			// 
			this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._okButton.Location = new System.Drawing.Point(100, 120);
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
			this._cancelButton.Location = new System.Drawing.Point(181, 120);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new System.Drawing.Size(75, 23);
			this._cancelButton.TabIndex = 3;
			this._cancelButton.Text = "Cancel";
			// 
			// _showResultBox
			// 
			this._showResultBox.Checked = true;
			this._showResultBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this._showResultBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._showResultBox.Location = new System.Drawing.Point(8, 88);
			this._showResultBox.Name = "_showResultBox";
			this._showResultBox.Size = new System.Drawing.Size(200, 24);
			this._showResultBox.TabIndex = 6;
			this._showResultBox.Text = "Show result after generation";
			// 
			// _internalModifierBox
			// 
			this._internalModifierBox.Checked = true;
			this._internalModifierBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this._internalModifierBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._internalModifierBox.Location = new System.Drawing.Point(8, 56);
			this._internalModifierBox.Name = "_internalModifierBox";
			this._internalModifierBox.Size = new System.Drawing.Size(192, 24);
			this._internalModifierBox.TabIndex = 7;
			this._internalModifierBox.Text = "Generate internal class";
			// 
			// GenParamsForm
			// 
			this.AcceptButton = this._okButton;
			this.CancelButton = this._cancelButton;
			this.ClientSize = new System.Drawing.Size(264, 150);
			this.Controls.Add(this._internalModifierBox);
			this.Controls.Add(this._showResultBox);
			this.Controls.Add(this._cancelButton);
			this.Controls.Add(this._okButton);
			this.Controls.Add(this._nsLabel);
			this.Controls.Add(this._nsBox);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GenParamsForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Generation parameters";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private System.Windows.Forms.TextBox _nsBox;
		private System.Windows.Forms.Label _nsLabel;
		private System.Windows.Forms.Button _okButton;
		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.CheckBox _showResultBox;
		private System.Windows.Forms.CheckBox _internalModifierBox;
	}
}