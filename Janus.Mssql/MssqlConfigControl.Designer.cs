namespace Rsdn.Janus.Mssql
{
	partial class MssqlConfigControl
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
			this.lblMsSqlSelectBase = new System.Windows.Forms.Label();
			this.lsbMsSqlBasesExist = new System.Windows.Forms.ListBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtMsSqlPassword = new System.Windows.Forms.TextBox();
			this.txtMsSqlLogin = new System.Windows.Forms.TextBox();
			this.rbtMsSqlServerAuth = new System.Windows.Forms.RadioButton();
			this.rbtMsSqlWinAuth = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.cmbMsSqlServersExist = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// _selectDatabaseDialog
			// 
			this._selectDatabaseDialog.CheckFileExists = false;
			this._selectDatabaseDialog.Filter = "mdb files (*.mdb)|*.mdb|All files|*.*";
			this._selectDatabaseDialog.RestoreDirectory = true;
			// 
			// lblMsSqlSelectBase
			// 
			this.lblMsSqlSelectBase.AutoSize = true;
			this.lblMsSqlSelectBase.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lblMsSqlSelectBase.Location = new System.Drawing.Point(3, 131);
			this.lblMsSqlSelectBase.Name = "lblMsSqlSelectBase";
			this.lblMsSqlSelectBase.Size = new System.Drawing.Size(66, 13);
			this.lblMsSqlSelectBase.TabIndex = 21;
			this.lblMsSqlSelectBase.Text = "Select base:";
			// 
			// lsbMsSqlBasesExist
			// 
			this.lsbMsSqlBasesExist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
																					| System.Windows.Forms.AnchorStyles.Left)
																				   | System.Windows.Forms.AnchorStyles.Right)));
			this.lsbMsSqlBasesExist.FormattingEnabled = true;
			this.lsbMsSqlBasesExist.Location = new System.Drawing.Point(3, 148);
			this.lsbMsSqlBasesExist.Name = "lsbMsSqlBasesExist";
			this.lsbMsSqlBasesExist.Size = new System.Drawing.Size(336, 56);
			this.lsbMsSqlBasesExist.TabIndex = 18;
			this.lsbMsSqlBasesExist.SelectedIndexChanged += new System.EventHandler(this.LsbMsSqlBasesExistSelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label3.Location = new System.Drawing.Point(3, 112);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(53, 13);
			this.label3.TabIndex = 20;
			this.label3.Text = "Password";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label2.Location = new System.Drawing.Point(3, 88);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(33, 13);
			this.label2.TabIndex = 19;
			this.label2.Text = "Login";
			// 
			// txtMsSqlPassword
			// 
			this.txtMsSqlPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
																				 | System.Windows.Forms.AnchorStyles.Right)));
			this.txtMsSqlPassword.Location = new System.Drawing.Point(79, 108);
			this.txtMsSqlPassword.Name = "txtMsSqlPassword";
			this.txtMsSqlPassword.PasswordChar = '*';
			this.txtMsSqlPassword.Size = new System.Drawing.Size(260, 20);
			this.txtMsSqlPassword.TabIndex = 17;
			// 
			// txtMsSqlLogin
			// 
			this.txtMsSqlLogin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
																			  | System.Windows.Forms.AnchorStyles.Right)));
			this.txtMsSqlLogin.Location = new System.Drawing.Point(79, 84);
			this.txtMsSqlLogin.Name = "txtMsSqlLogin";
			this.txtMsSqlLogin.Size = new System.Drawing.Size(260, 20);
			this.txtMsSqlLogin.TabIndex = 16;
			// 
			// rbtMsSqlServerAuth
			// 
			this.rbtMsSqlServerAuth.AutoSize = true;
			this.rbtMsSqlServerAuth.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.rbtMsSqlServerAuth.Location = new System.Drawing.Point(3, 56);
			this.rbtMsSqlServerAuth.Name = "rbtMsSqlServerAuth";
			this.rbtMsSqlServerAuth.Size = new System.Drawing.Size(151, 17);
			this.rbtMsSqlServerAuth.TabIndex = 15;
			this.rbtMsSqlServerAuth.Text = "SQL Server Authentication";
			// 
			// rbtMsSqlWinAuth
			// 
			this.rbtMsSqlWinAuth.AutoSize = true;
			this.rbtMsSqlWinAuth.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.rbtMsSqlWinAuth.Location = new System.Drawing.Point(3, 32);
			this.rbtMsSqlWinAuth.Name = "rbtMsSqlWinAuth";
			this.rbtMsSqlWinAuth.Size = new System.Drawing.Size(140, 17);
			this.rbtMsSqlWinAuth.TabIndex = 13;
			this.rbtMsSqlWinAuth.Text = "Windows Authentication";
			this.rbtMsSqlWinAuth.CheckedChanged += new System.EventHandler(this.RbtMsSqlWinAuthCheckedChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label1.Location = new System.Drawing.Point(3, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(68, 13);
			this.label1.TabIndex = 14;
			this.label1.Text = "SQL Server :";
			// 
			// cmbMsSqlServersExist
			// 
			this.cmbMsSqlServersExist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
																					 | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbMsSqlServersExist.FormattingEnabled = true;
			this.cmbMsSqlServersExist.Location = new System.Drawing.Point(79, 4);
			this.cmbMsSqlServersExist.Name = "cmbMsSqlServersExist";
			this.cmbMsSqlServersExist.Size = new System.Drawing.Size(260, 21);
			this.cmbMsSqlServersExist.TabIndex = 12;
			// 
			// MssqlConfigControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblMsSqlSelectBase);
			this.Controls.Add(this.lsbMsSqlBasesExist);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtMsSqlPassword);
			this.Controls.Add(this.txtMsSqlLogin);
			this.Controls.Add(this.rbtMsSqlServerAuth);
			this.Controls.Add(this.rbtMsSqlWinAuth);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmbMsSqlServersExist);
			this.Name = "MssqlConfigControl";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.OpenFileDialog _selectDatabaseDialog;
		private System.Windows.Forms.Label lblMsSqlSelectBase;
		private System.Windows.Forms.ListBox lsbMsSqlBasesExist;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtMsSqlPassword;
		private System.Windows.Forms.TextBox txtMsSqlLogin;
		private System.Windows.Forms.RadioButton rbtMsSqlServerAuth;
		private System.Windows.Forms.RadioButton rbtMsSqlWinAuth;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbMsSqlServersExist;
	}
}


