using System.Windows.Forms;

namespace Rsdn.Janus
{
	partial class OptionsUserForm
	{
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsUserForm));
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.txtPathToDb = new System.Windows.Forms.TextBox();
			this.txtUserName = new System.Windows.Forms.TextBox();
			this.txtUserPsw = new System.Windows.Forms.TextBox();
			this.lblNewUserMessage = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtConstr = new System.Windows.Forms.TextBox();
			this.btnSelectDb = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnOk
			// 
			resources.ApplyResources(this.btnOk, "btnOk");
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Name = "btnOk";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			// 
			// btnBrowse
			// 
			resources.ApplyResources(this.btnBrowse, "btnBrowse");
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// txtPathToDb
			// 
			resources.ApplyResources(this.txtPathToDb, "txtPathToDb");
			this.txtPathToDb.Name = "txtPathToDb";
			// 
			// txtUserName
			// 
			resources.ApplyResources(this.txtUserName, "txtUserName");
			this.txtUserName.Name = "txtUserName";
			// 
			// txtUserPsw
			// 
			resources.ApplyResources(this.txtUserPsw, "txtUserPsw");
			this.txtUserPsw.Name = "txtUserPsw";
			// 
			// lblNewUserMessage
			// 
			resources.ApplyResources(this.lblNewUserMessage, "lblNewUserMessage");
			this.lblNewUserMessage.ForeColor = System.Drawing.SystemColors.Highlight;
			this.lblNewUserMessage.Name = "lblNewUserMessage";
			// 
			// label2
			// 
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// label3
			// 
			this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// label4
			// 
			this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// txtConstr
			// 
			resources.ApplyResources(this.txtConstr, "txtConstr");
			this.txtConstr.Name = "txtConstr";
			this.txtConstr.ReadOnly = true;
			// 
			// btnSelectDb
			// 
			resources.ApplyResources(this.btnSelectDb, "btnSelectDb");
			this.btnSelectDb.Name = "btnSelectDb";
			this.btnSelectDb.Click += new System.EventHandler(this.btnSelectDb_Click);
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// OptionsUserForm
			// 
			this.AcceptButton = this.btnOk;
			this.CancelButton = this.btnCancel;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnSelectDb);
			this.Controls.Add(this.txtConstr);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lblNewUserMessage);
			this.Controls.Add(this.txtUserPsw);
			this.Controls.Add(this.txtUserName);
			this.Controls.Add(this.txtPathToDb);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptionsUserForm";
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Label label2;
		private Label label3;
		private Label label4;
		private Button btnOk;
		private Button btnCancel;
		private Button btnBrowse;
		private TextBox txtPathToDb;
		private TextBox txtUserName;
		private TextBox txtUserPsw;
		private Label lblNewUserMessage;
		private Label label1;
		private TextBox txtConstr;
		private Button btnSelectDb;
	}
}