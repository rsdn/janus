namespace Rsdn.Janus
{
	sealed partial class TagLineListForm
	{
		private System.ComponentModel.IContainer components;

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TagLineListForm));
			this._okButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			this._listGroup = new System.Windows.Forms.GroupBox();
			this._tagsList = new System.Windows.Forms.ListView();
			this._nameCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._formatCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this._listImages = new System.Windows.Forms.ImageList(this.components);
			this._toolStrip = new System.Windows.Forms.ToolStrip();
			this._listGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// _okButton
			// 
			resources.ApplyResources(this._okButton, "_okButton");
			this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._okButton.Name = "_okButton";
			// 
			// _cancelButton
			// 
			resources.ApplyResources(this._cancelButton, "_cancelButton");
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Name = "_cancelButton";
			// 
			// _listGroup
			// 
			resources.ApplyResources(this._listGroup, "_listGroup");
			this._listGroup.Controls.Add(this._tagsList);
			this._listGroup.Controls.Add(this._toolStrip);
			this._listGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._listGroup.Name = "_listGroup";
			this._listGroup.TabStop = false;
			// 
			// _tagsList
			// 
			this._tagsList.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._tagsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this._nameCol,
			this._formatCol});
			this._tagsList.ContextMenuStrip = this._contextMenuStrip;
			resources.ApplyResources(this._tagsList, "_tagsList");
			this._tagsList.FullRowSelect = true;
			this._tagsList.HideSelection = false;
			this._tagsList.Name = "_tagsList";
			this._tagsList.SmallImageList = this._listImages;
			this._tagsList.UseCompatibleStateImageBehavior = false;
			this._tagsList.View = System.Windows.Forms.View.Details;
			this._tagsList.SelectedIndexChanged += new System.EventHandler(this._tagsList_SelectedIndexChanged);
			this._tagsList.DoubleClick += new System.EventHandler(this._tagsList_DoubleClick);
			// 
			// _nameCol
			// 
			resources.ApplyResources(this._nameCol, "_nameCol");
			// 
			// _formatCol
			// 
			resources.ApplyResources(this._formatCol, "_formatCol");
			// 
			// _contextMenuStrip
			// 
			this._contextMenuStrip.Name = "_contextMenuStrip";
			resources.ApplyResources(this._contextMenuStrip, "_contextMenuStrip");
			// 
			// _listImages
			// 
			this._listImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			resources.ApplyResources(this._listImages, "_listImages");
			this._listImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// _toolStrip
			// 
			this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			resources.ApplyResources(this._toolStrip, "_toolStrip");
			this._toolStrip.Name = "_toolStrip";
			// 
			// TagLineListForm
			// 
			this.AcceptButton = this._okButton;
			this.CancelButton = this._cancelButton;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this._listGroup);
			this.Controls.Add(this._cancelButton);
			this.Controls.Add(this._okButton);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TagLineListForm";
			this.ShowInTaskbar = false;
			this._listGroup.ResumeLayout(false);
			this._listGroup.PerformLayout();
			this.ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.Button _okButton;
		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.GroupBox _listGroup;
		private System.Windows.Forms.ListView _tagsList;
		private System.Windows.Forms.ColumnHeader _nameCol;
		private System.Windows.Forms.ColumnHeader _formatCol;
		private System.Windows.Forms.ImageList _listImages;
		private System.Windows.Forms.ToolStrip _toolStrip;
		private System.Windows.Forms.ContextMenuStrip _contextMenuStrip;
	}
}
