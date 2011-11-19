namespace Rsdn.Janus
{
	partial class ModeratingForm
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
			System.Windows.Forms.Button okButton;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModeratingForm));
			System.Windows.Forms.GroupBox modsGroup;
			this._moderatorialsList = new System.Windows.Forms.ListView();
			this._userCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._actionCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._createCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			okButton = new System.Windows.Forms.Button();
			modsGroup = new System.Windows.Forms.GroupBox();
			modsGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// okButton
			// 
			resources.ApplyResources(okButton, "okButton");
			okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			okButton.Name = "okButton";
			okButton.UseVisualStyleBackColor = true;
			// 
			// modsGroup
			// 
			resources.ApplyResources(modsGroup, "modsGroup");
			modsGroup.Controls.Add(this._moderatorialsList);
			modsGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
			modsGroup.Name = "modsGroup";
			modsGroup.TabStop = false;
			// 
			// _moderatorialsList
			// 
			this._moderatorialsList.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._moderatorialsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this._userCol,
			this._actionCol,
			this._createCol});
			resources.ApplyResources(this._moderatorialsList, "_moderatorialsList");
			this._moderatorialsList.FullRowSelect = true;
			this._moderatorialsList.Name = "_moderatorialsList";
			this._moderatorialsList.UseCompatibleStateImageBehavior = false;
			this._moderatorialsList.View = System.Windows.Forms.View.Details;
			this._moderatorialsList.VirtualMode = true;
			this._moderatorialsList.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.ModeratorialsList_RetrieveVirtualItem);
			// 
			// _userCol
			// 
			resources.ApplyResources(this._userCol, "_userCol");
			// 
			// _actionCol
			// 
			resources.ApplyResources(this._actionCol, "_actionCol");
			// 
			// _createCol
			// 
			resources.ApplyResources(this._createCol, "_createCol");
			// 
			// ModeratingForm
			// 
			this.AcceptButton = okButton;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = okButton;
			this.Controls.Add(modsGroup);
			this.Controls.Add(okButton);
			this.Name = "ModeratingForm";
			this.ShowInTaskbar = false;
			modsGroup.ResumeLayout(false);
			this.ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.ListView _moderatorialsList;
		private System.Windows.Forms.ColumnHeader _userCol;
		private System.Windows.Forms.ColumnHeader _actionCol;
		private System.Windows.Forms.ColumnHeader _createCol;
	}
}