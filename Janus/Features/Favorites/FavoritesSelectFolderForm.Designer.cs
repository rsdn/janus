namespace Rsdn.Janus
{
	public partial class FavoritesSelectFolderForm
	{
		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FavoritesSelectFolderForm));
			this._folderView = new Rsdn.TreeGrid.TreeGrid();
			this._folderCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._commentCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._okBtn = new System.Windows.Forms.Button();
			this._cancelBtn = new System.Windows.Forms.Button();
			this._commentLabel = new System.Windows.Forms.Label();
			this._comment = new System.Windows.Forms.TextBox();
			this._createFolderButton = new System.Windows.Forms.Button();
			this._folderListGroup = new System.Windows.Forms.GroupBox();
			this._folderListGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// _folderView
			// 
			resources.ApplyResources(this._folderView, "_folderView");
			this._folderView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(255)))), ((int)(((byte)(244)))));
			this._folderView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._folderView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this._folderCol,
			this._commentCol});
			this._folderView.FullRowSelect = true;
			this._folderView.GridLines = true;
			this._folderView.Indent = 5;
			this._folderView.Name = "_folderView";
			this._folderView.OwnerDraw = true;
			this._folderView.UseCompatibleStateImageBehavior = false;
			this._folderView.View = System.Windows.Forms.View.Details;
			this._folderView.VirtualMode = true;
			// 
			// _folderCol
			// 
			resources.ApplyResources(this._folderCol, "_folderCol");
			// 
			// _commentCol
			// 
			resources.ApplyResources(this._commentCol, "_commentCol");
			// 
			// _okBtn
			// 
			resources.ApplyResources(this._okBtn, "_okBtn");
			this._okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._okBtn.Name = "_okBtn";
			// 
			// _cancelBtn
			// 
			resources.ApplyResources(this._cancelBtn, "_cancelBtn");
			this._cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelBtn.Name = "_cancelBtn";
			// 
			// _commentLabel
			// 
			resources.ApplyResources(this._commentLabel, "_commentLabel");
			this._commentLabel.Name = "_commentLabel";
			// 
			// _comment
			// 
			resources.ApplyResources(this._comment, "_comment");
			this._comment.Name = "_comment";
			// 
			// _createFolderButton
			// 
			resources.ApplyResources(this._createFolderButton, "_createFolderButton");
			this._createFolderButton.Name = "_createFolderButton";
			this._createFolderButton.Click += new System.EventHandler(this.CreateFolderButtonClick);
			// 
			// _folderListGroup
			// 
			resources.ApplyResources(this._folderListGroup, "_folderListGroup");
			this._folderListGroup.Controls.Add(this._folderView);
			this._folderListGroup.Name = "_folderListGroup";
			this._folderListGroup.TabStop = false;
			// 
			// FavoritesSelectFolderForm
			// 
			this.AcceptButton = this._okBtn;
			resources.ApplyResources(this, "$this");
			this.CancelButton = this._cancelBtn;
			this.Controls.Add(this._folderListGroup);
			this.Controls.Add(this._createFolderButton);
			this.Controls.Add(this._comment);
			this.Controls.Add(this._commentLabel);
			this.Controls.Add(this._cancelBtn);
			this.Controls.Add(this._okBtn);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FavoritesSelectFolderForm";
			this._folderListGroup.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Button _okBtn;
		private System.Windows.Forms.Button _cancelBtn;
		private System.Windows.Forms.Button _createFolderButton;
		private System.Windows.Forms.TextBox _comment;
		private System.Windows.Forms.Label _commentLabel;
		private System.Windows.Forms.ColumnHeader _commentCol;
		private System.Windows.Forms.ColumnHeader _folderCol;
		private System.Windows.Forms.GroupBox _folderListGroup;
		
		private Rsdn.TreeGrid.TreeGrid _folderView;
	}
}