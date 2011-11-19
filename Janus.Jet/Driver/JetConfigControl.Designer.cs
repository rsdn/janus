namespace Rsdn.Janus
{
	partial class JetConfigControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._selectDatabasePathComboBox = new System.Windows.Forms.ComboBox();
			this._selectDatabasePathLabel = new System.Windows.Forms.Label();
			this._selectDatabasePathButton = new System.Windows.Forms.Button();
			this._selectDatabaseDialog = new System.Windows.Forms.OpenFileDialog();
			this.SuspendLayout();
			// 
			// _selectDatabasePathComboBox
			// 
			this._selectDatabasePathComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._selectDatabasePathComboBox.FormattingEnabled = true;
			this._selectDatabasePathComboBox.Location = new System.Drawing.Point(3, 24);
			this._selectDatabasePathComboBox.Name = "_selectDatabasePathComboBox";
			this._selectDatabasePathComboBox.Size = new System.Drawing.Size(306, 21);
			this._selectDatabasePathComboBox.TabIndex = 8;
			// 
			// _selectDatabasePathLabel
			// 
			this._selectDatabasePathLabel.AutoSize = true;
			this._selectDatabasePathLabel.Location = new System.Drawing.Point(3, 7);
			this._selectDatabasePathLabel.Name = "_selectDatabasePathLabel";
			this._selectDatabasePathLabel.Size = new System.Drawing.Size(80, 13);
			this._selectDatabasePathLabel.TabIndex = 7;
			this._selectDatabasePathLabel.Text = "Database path:";
			// 
			// _selectDatabasePathButton
			// 
			this._selectDatabasePathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._selectDatabasePathButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this._selectDatabasePathButton.Location = new System.Drawing.Point(315, 24);
			this._selectDatabasePathButton.Name = "_selectDatabasePathButton";
			this._selectDatabasePathButton.Size = new System.Drawing.Size(24, 20);
			this._selectDatabasePathButton.TabIndex = 6;
			this._selectDatabasePathButton.Text = "...";
			this._selectDatabasePathButton.Click += new System.EventHandler(this.SelectDatabasePathButton_Click);
			// 
			// _selectDatabaseDialog
			// 
			this._selectDatabaseDialog.CheckFileExists = false;
			this._selectDatabaseDialog.Filter = "Access files (*.mdb)|*.mdb|All files (*.*)|*.*";
			this._selectDatabaseDialog.RestoreDirectory = true;
			// 
			// JetConfigControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._selectDatabasePathComboBox);
			this.Controls.Add(this._selectDatabasePathLabel);
			this.Controls.Add(this._selectDatabasePathButton);
			this.Name = "JetConfigControl";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label _selectDatabasePathLabel;
		private System.Windows.Forms.ComboBox _selectDatabasePathComboBox;
		private System.Windows.Forms.Button _selectDatabasePathButton;
		private System.Windows.Forms.OpenFileDialog _selectDatabaseDialog;
	}
}
