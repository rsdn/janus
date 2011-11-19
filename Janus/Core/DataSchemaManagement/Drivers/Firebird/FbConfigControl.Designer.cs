namespace Rsdn.Janus
{
	partial class FbConfigControl
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
			this._selectDatabaseDialog = new System.Windows.Forms.OpenFileDialog();
			this.label11 = new System.Windows.Forms.Label();
			this.cmbFbServerType = new System.Windows.Forms.ComboBox();
			this.label10 = new System.Windows.Forms.Label();
			this.cmbFbServerName = new System.Windows.Forms.ComboBox();
			this.cmbFbSelectPath = new System.Windows.Forms.ComboBox();
			this.label9 = new System.Windows.Forms.Label();
			this.cmbFbDialect = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.txtFbPassword = new System.Windows.Forms.TextBox();
			this.txtFbUserID = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.btnFbSelectPath = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// _selectDatabaseDialog
			// 
			this._selectDatabaseDialog.CheckFileExists = false;
			this._selectDatabaseDialog.Filter = "Interbase database (*.gdb)|*.gdb|Firebird database (*.fdb)|*.fdb|All files|*.*";
			this._selectDatabaseDialog.RestoreDirectory = true;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label11.Location = new System.Drawing.Point(3, 7);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(67, 13);
			this.label11.TabIndex = 32;
			this.label11.Text = "Server type :";
			// 
			// cmbFbServerType
			// 
			this.cmbFbServerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbFbServerType.FormattingEnabled = true;
			this.cmbFbServerType.Location = new System.Drawing.Point(3, 24);
			this.cmbFbServerType.Name = "cmbFbServerType";
			this.cmbFbServerType.Size = new System.Drawing.Size(110, 21);
			this.cmbFbServerType.TabIndex = 31;
			this.cmbFbServerType.SelectedIndexChanged += new System.EventHandler(this.cmbFbServerType_SelectedIndexChanged);
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label10.Location = new System.Drawing.Point(119, 7);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(73, 13);
			this.label10.TabIndex = 30;
			this.label10.Text = "Server name :";
			// 
			// cmbFbServerName
			// 
			this.cmbFbServerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cmbFbServerName.FormattingEnabled = true;
			this.cmbFbServerName.Location = new System.Drawing.Point(119, 24);
			this.cmbFbServerName.Name = "cmbFbServerName";
			this.cmbFbServerName.Size = new System.Drawing.Size(220, 21);
			this.cmbFbServerName.TabIndex = 29;
			// 
			// cmbFbSelectPath
			// 
			this.cmbFbSelectPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cmbFbSelectPath.FormattingEnabled = true;
			this.cmbFbSelectPath.Location = new System.Drawing.Point(3, 69);
			this.cmbFbSelectPath.Name = "cmbFbSelectPath";
			this.cmbFbSelectPath.Size = new System.Drawing.Size(306, 21);
			this.cmbFbSelectPath.TabIndex = 20;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label9.Location = new System.Drawing.Point(35, 111);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(43, 13);
			this.label9.TabIndex = 28;
			this.label9.Text = "Dialect:";
			// 
			// cmbFbDialect
			// 
			this.cmbFbDialect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cmbFbDialect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbFbDialect.FormattingEnabled = true;
			this.cmbFbDialect.Items.AddRange(new object[] {
			"Dialect2",
			"Dialect3"});
			this.cmbFbDialect.Location = new System.Drawing.Point(119, 108);
			this.cmbFbDialect.Name = "cmbFbDialect";
			this.cmbFbDialect.Size = new System.Drawing.Size(220, 21);
			this.cmbFbDialect.TabIndex = 22;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label7.Location = new System.Drawing.Point(35, 168);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(53, 13);
			this.label7.TabIndex = 27;
			this.label7.Text = "Password";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label8.Location = new System.Drawing.Point(35, 144);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(40, 13);
			this.label8.TabIndex = 26;
			this.label8.Text = "UserID";
			// 
			// txtFbPassword
			// 
			this.txtFbPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFbPassword.Location = new System.Drawing.Point(119, 164);
			this.txtFbPassword.Name = "txtFbPassword";
			this.txtFbPassword.PasswordChar = '*';
			this.txtFbPassword.Size = new System.Drawing.Size(220, 20);
			this.txtFbPassword.TabIndex = 24;
			// 
			// txtFbUserID
			// 
			this.txtFbUserID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFbUserID.Location = new System.Drawing.Point(119, 140);
			this.txtFbUserID.Name = "txtFbUserID";
			this.txtFbUserID.Size = new System.Drawing.Size(220, 20);
			this.txtFbUserID.TabIndex = 23;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label5.Location = new System.Drawing.Point(3, 52);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(177, 13);
			this.label5.TabIndex = 25;
			this.label5.Text = "Database path (or database name) :";
			// 
			// btnFbSelectPath
			// 
			this.btnFbSelectPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFbSelectPath.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnFbSelectPath.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btnFbSelectPath.Location = new System.Drawing.Point(315, 69);
			this.btnFbSelectPath.Name = "btnFbSelectPath";
			this.btnFbSelectPath.Size = new System.Drawing.Size(24, 20);
			this.btnFbSelectPath.TabIndex = 21;
			this.btnFbSelectPath.Text = "...";
			this.btnFbSelectPath.Click += new System.EventHandler(this.btnFbSelectPath_Click);
			// 
			// FbConfigControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label11);
			this.Controls.Add(this.cmbFbServerType);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.cmbFbServerName);
			this.Controls.Add(this.cmbFbSelectPath);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.cmbFbDialect);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.txtFbPassword);
			this.Controls.Add(this.txtFbUserID);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.btnFbSelectPath);
			this.Name = "FbConfigControl";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.OpenFileDialog _selectDatabaseDialog;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.ComboBox cmbFbServerType;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.ComboBox cmbFbServerName;
		private System.Windows.Forms.ComboBox cmbFbSelectPath;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox cmbFbDialect;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtFbPassword;
		private System.Windows.Forms.TextBox txtFbUserID;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button btnFbSelectPath;
	}
}
