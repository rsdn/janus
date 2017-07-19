namespace Rsdn.Janus.Mssql {
	partial class MssqlConfigControl {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
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
			this.chIsSqlExpress = new System.Windows.Forms.CheckBox();
			this.notExpressControls = new System.Windows.Forms.Panel();
			this.expressControls = new System.Windows.Forms.Panel();
			this.label5 = new System.Windows.Forms.Label();
			this.cmbExpressInstances = new System.Windows.Forms.ComboBox();
			this.txConnectionString = new System.Windows.Forms.TextBox();
			this.btBrowseDatabaseFile = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.txDbFilePath = new System.Windows.Forms.TextBox();
			this.notExpressControls.SuspendLayout();
			this.expressControls.SuspendLayout();
			this.SuspendLayout();
			// 
			// _selectDatabaseDialog
			// 
			this._selectDatabaseDialog.CheckFileExists = false;
			this._selectDatabaseDialog.Filter = "mdf files (*.mdf)|*.mdf|All files|*.*";
			this._selectDatabaseDialog.RestoreDirectory = true;
			// 
			// lblMsSqlSelectBase
			// 
			this.lblMsSqlSelectBase.AutoSize = true;
			this.lblMsSqlSelectBase.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lblMsSqlSelectBase.Location = new System.Drawing.Point(3, 130);
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
			this.lsbMsSqlBasesExist.Size = new System.Drawing.Size(336, 108);
			this.lsbMsSqlBasesExist.TabIndex = 18;
			this.lsbMsSqlBasesExist.SelectedIndexChanged += new System.EventHandler(this.LsbMsSqlBasesExistSelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label3.Location = new System.Drawing.Point(3, 111);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(53, 13);
			this.label3.TabIndex = 20;
			this.label3.Text = "Password";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label2.Location = new System.Drawing.Point(3, 87);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(33, 13);
			this.label2.TabIndex = 19;
			this.label2.Text = "Login";
			// 
			// txtMsSqlPassword
			// 
			this.txtMsSqlPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtMsSqlPassword.Location = new System.Drawing.Point(79, 107);
			this.txtMsSqlPassword.Name = "txtMsSqlPassword";
			this.txtMsSqlPassword.PasswordChar = '*';
			this.txtMsSqlPassword.Size = new System.Drawing.Size(260, 20);
			this.txtMsSqlPassword.TabIndex = 17;
			// 
			// txtMsSqlLogin
			// 
			this.txtMsSqlLogin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtMsSqlLogin.Location = new System.Drawing.Point(79, 83);
			this.txtMsSqlLogin.Name = "txtMsSqlLogin";
			this.txtMsSqlLogin.Size = new System.Drawing.Size(260, 20);
			this.txtMsSqlLogin.TabIndex = 16;
			// 
			// rbtMsSqlServerAuth
			// 
			this.rbtMsSqlServerAuth.AutoSize = true;
			this.rbtMsSqlServerAuth.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.rbtMsSqlServerAuth.Location = new System.Drawing.Point(3, 55);
			this.rbtMsSqlServerAuth.Name = "rbtMsSqlServerAuth";
			this.rbtMsSqlServerAuth.Size = new System.Drawing.Size(151, 17);
			this.rbtMsSqlServerAuth.TabIndex = 15;
			this.rbtMsSqlServerAuth.Text = "SQL Server Authentication";
			// 
			// rbtMsSqlWinAuth
			// 
			this.rbtMsSqlWinAuth.AutoSize = true;
			this.rbtMsSqlWinAuth.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.rbtMsSqlWinAuth.Location = new System.Drawing.Point(3, 31);
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
			this.label1.Location = new System.Drawing.Point(3, 7);
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
			this.cmbMsSqlServersExist.Location = new System.Drawing.Point(79, 3);
			this.cmbMsSqlServersExist.Name = "cmbMsSqlServersExist";
			this.cmbMsSqlServersExist.Size = new System.Drawing.Size(260, 21);
			this.cmbMsSqlServersExist.TabIndex = 12;
			// 
			// chIsSqlExpress
			// 
			this.chIsSqlExpress.Location = new System.Drawing.Point(3, 3);
			this.chIsSqlExpress.Name = "chIsSqlExpress";
			this.chIsSqlExpress.Size = new System.Drawing.Size(84, 18);
			this.chIsSqlExpress.TabIndex = 22;
			this.chIsSqlExpress.Text = "Sql LocalDb";
			this.chIsSqlExpress.UseVisualStyleBackColor = true;
			this.chIsSqlExpress.CheckedChanged += new System.EventHandler(this.chIsSqlExpress_CheckedChanged);
			// 
			// notExpressControls
			// 
			this.notExpressControls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.notExpressControls.Controls.Add(this.lblMsSqlSelectBase);
			this.notExpressControls.Controls.Add(this.lsbMsSqlBasesExist);
			this.notExpressControls.Controls.Add(this.label3);
			this.notExpressControls.Controls.Add(this.label2);
			this.notExpressControls.Controls.Add(this.txtMsSqlPassword);
			this.notExpressControls.Controls.Add(this.txtMsSqlLogin);
			this.notExpressControls.Controls.Add(this.rbtMsSqlServerAuth);
			this.notExpressControls.Controls.Add(this.rbtMsSqlWinAuth);
			this.notExpressControls.Controls.Add(this.label1);
			this.notExpressControls.Controls.Add(this.cmbMsSqlServersExist);
			this.notExpressControls.Location = new System.Drawing.Point(0, 24);
			this.notExpressControls.Name = "notExpressControls";
			this.notExpressControls.Size = new System.Drawing.Size(342, 262);
			this.notExpressControls.TabIndex = 24;
			// 
			// expressControls
			// 
			this.expressControls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.expressControls.Controls.Add(this.cmbExpressInstances);
			this.expressControls.Controls.Add(this.txConnectionString);
			this.expressControls.Controls.Add(this.btBrowseDatabaseFile);
			this.expressControls.Controls.Add(this.label4);
			this.expressControls.Controls.Add(this.txDbFilePath);
			this.expressControls.Controls.Add(this.label5);
			this.expressControls.Enabled = false;
			this.expressControls.Location = new System.Drawing.Point(0, 24);
			this.expressControls.Name = "expressControls";
			this.expressControls.Size = new System.Drawing.Size(342, 262);
			this.expressControls.TabIndex = 23;
			this.expressControls.Visible = false;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(3, 7);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(42, 13);
			this.label5.TabIndex = 1;
			this.label5.Text = "Version";
			// 
			// cmbExpressInstances
			// 
			this.cmbExpressInstances.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbExpressInstances.FormattingEnabled = true;
			this.cmbExpressInstances.Location = new System.Drawing.Point(79, 3);
			this.cmbExpressInstances.Name = "cmbExpressInstances";
			this.cmbExpressInstances.Size = new System.Drawing.Size(260, 21);
			this.cmbExpressInstances.TabIndex = 2;
			// 
			// txConnectionString
			// 
			this.txConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txConnectionString.Location = new System.Drawing.Point(3, 83);
			this.txConnectionString.Multiline = true;
			this.txConnectionString.Name = "txConnectionString";
			this.txConnectionString.ReadOnly = true;
			this.txConnectionString.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txConnectionString.Size = new System.Drawing.Size(336, 173);
			this.txConnectionString.TabIndex = 6;
			// 
			// btBrowseDatabaseFile
			// 
			this.btBrowseDatabaseFile.Location = new System.Drawing.Point(246, 54);
			this.btBrowseDatabaseFile.Name = "btBrowseDatabaseFile";
			this.btBrowseDatabaseFile.Size = new System.Drawing.Size(93, 23);
			this.btBrowseDatabaseFile.TabIndex = 5;
			this.btBrowseDatabaseFile.Text = "Browse…";
			this.btBrowseDatabaseFile.UseVisualStyleBackColor = true;
			this.btBrowseDatabaseFile.Click += new System.EventHandler(this.btBrowseDatabaseFile_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 33);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(37, 13);
			this.label4.TabIndex = 3;
			this.label4.Text = "&Db file";
			// 
			// txDbFilePath
			// 
			this.txDbFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txDbFilePath.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txDbFilePath.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
			this.txDbFilePath.BackColor = System.Drawing.SystemColors.Window;
			this.txDbFilePath.Location = new System.Drawing.Point(79, 28);
			this.txDbFilePath.Name = "txDbFilePath";
			this.txDbFilePath.ReadOnly = true;
			this.txDbFilePath.Size = new System.Drawing.Size(260, 20);
			this.txDbFilePath.TabIndex = 4;
			// 
			// MssqlConfigControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.chIsSqlExpress);
			this.Controls.Add(this.expressControls);
			this.Controls.Add(this.notExpressControls);
			this.Name = "MssqlConfigControl";
			this.Size = new System.Drawing.Size(342, 286);
			this.notExpressControls.ResumeLayout(false);
			this.notExpressControls.PerformLayout();
			this.expressControls.ResumeLayout(false);
			this.expressControls.PerformLayout();
			this.ResumeLayout(false);

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
		private System.Windows.Forms.Panel notExpressControls;
		private System.Windows.Forms.Panel expressControls;
		private System.Windows.Forms.CheckBox chIsSqlExpress;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txDbFilePath;
		private System.Windows.Forms.Button btBrowseDatabaseFile;
		private System.Windows.Forms.TextBox txConnectionString;
		private System.Windows.Forms.ComboBox cmbExpressInstances;
		private System.Windows.Forms.Label label5;
	}
}


