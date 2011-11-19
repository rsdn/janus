namespace ImageUtil
{
	partial class MainForm
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
				DisposeLoadedImages();
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
			this.components = new System.ComponentModel.Container();
			this._toolStripContainer = new System.Windows.Forms.ToolStripContainer();
			this._treeSplitter = new System.Windows.Forms.SplitContainer();
			this._foldersTree = new Rsdn.TreeGrid.TreeGrid();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this._imagesList = new System.Windows.Forms.ListView();
			this._menuStrip = new System.Windows.Forms.MenuStrip();
			this._fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._saveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this._exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._imageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._addMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._removeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._saveToFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._renameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this._importFolderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._toolStrip = new System.Windows.Forms.ToolStrip();
			this._openButton = new System.Windows.Forms.ToolStripButton();
			this._saveButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this._addButton = new System.Windows.Forms.ToolStripButton();
			this._removeButton = new System.Windows.Forms.ToolStripButton();
			this._renameButton = new System.Windows.Forms.ToolStripButton();
			this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this._contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this._addContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._removeContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._renameContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._renameFolderContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._saveToFileContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._separatorContextMenuItem = new System.Windows.Forms.ToolStripSeparator();
			this._importFolderContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._openImageDialog = new System.Windows.Forms.OpenFileDialog();
			this._toolStripContainer.ContentPanel.SuspendLayout();
			this._toolStripContainer.TopToolStripPanel.SuspendLayout();
			this._toolStripContainer.SuspendLayout();
			this._treeSplitter.Panel1.SuspendLayout();
			this._treeSplitter.Panel2.SuspendLayout();
			this._treeSplitter.SuspendLayout();
			this._menuStrip.SuspendLayout();
			this._toolStrip.SuspendLayout();
			this._contextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// _toolStripContainer
			// 
			// 
			// _toolStripContainer.ContentPanel
			// 
			this._toolStripContainer.ContentPanel.Controls.Add(this._treeSplitter);
			this._toolStripContainer.ContentPanel.Size = new System.Drawing.Size(561, 302);
			this._toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this._toolStripContainer.Location = new System.Drawing.Point(0, 0);
			this._toolStripContainer.Name = "_toolStripContainer";
			this._toolStripContainer.Size = new System.Drawing.Size(561, 351);
			this._toolStripContainer.TabIndex = 0;
			this._toolStripContainer.Text = "toolStripContainer1";
			// 
			// _toolStripContainer.TopToolStripPanel
			// 
			this._toolStripContainer.TopToolStripPanel.Controls.Add(this._menuStrip);
			this._toolStripContainer.TopToolStripPanel.Controls.Add(this._toolStrip);
			// 
			// _treeSplitter
			// 
			this._treeSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this._treeSplitter.Location = new System.Drawing.Point(0, 0);
			this._treeSplitter.Name = "_treeSplitter";
			// 
			// _treeSplitter.Panel1
			// 
			this._treeSplitter.Panel1.Controls.Add(this._foldersTree);
			// 
			// _treeSplitter.Panel2
			// 
			this._treeSplitter.Panel2.Controls.Add(this._imagesList);
			this._treeSplitter.Size = new System.Drawing.Size(561, 302);
			this._treeSplitter.SplitterDistance = 246;
			this._treeSplitter.TabIndex = 0;
			// 
			// _foldersTree
			// 
			this._foldersTree.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.columnHeader1,
			this.columnHeader2});
			this._foldersTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this._foldersTree.FullRowSelect = true;
			this._foldersTree.HideSelection = false;
			this._foldersTree.Indent = 5;
			this._foldersTree.Location = new System.Drawing.Point(0, 0);
			this._foldersTree.Name = "_foldersTree";
			this._foldersTree.OwnerDraw = true;
			this._foldersTree.Size = new System.Drawing.Size(246, 302);
			this._foldersTree.TabIndex = 0;
			this._foldersTree.UseCompatibleStateImageBehavior = false;
			this._foldersTree.View = System.Windows.Forms.View.Details;
			this._foldersTree.VirtualMode = true;
			this._foldersTree.GetData += new System.EventHandler<Rsdn.TreeGrid.GetDataEventArgs>(this.FoldersTreeGetData);
			this._foldersTree.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FoldersTreeMouseUp);
			this._foldersTree.AfterActivateNode += new Rsdn.TreeGrid.AfterActivateNodeHandler(this.FoldersTreeAfterActivateNode);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Name";
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Count";
			// 
			// _imagesList
			// 
			this._imagesList.AllowDrop = true;
			this._imagesList.Dock = System.Windows.Forms.DockStyle.Fill;
			this._imagesList.LabelEdit = true;
			this._imagesList.Location = new System.Drawing.Point(0, 0);
			this._imagesList.Name = "_imagesList";
			this._imagesList.ShowItemToolTips = true;
			this._imagesList.Size = new System.Drawing.Size(311, 302);
			this._imagesList.TabIndex = 0;
			this._imagesList.UseCompatibleStateImageBehavior = false;
			this._imagesList.DragEnter += new System.Windows.Forms.DragEventHandler(this.ImagesListDragEnter);
			this._imagesList.DragDrop += new System.Windows.Forms.DragEventHandler(this.ImagesListDragDrop);
			this._imagesList.SelectedIndexChanged += new System.EventHandler(this.ImagesListSelectedIndexChanged);
			this._imagesList.DragLeave += new System.EventHandler(this.ImagesListDragLeave);
			this._imagesList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ImagesListMouseUp);
			this._imagesList.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.ImagesListAfterLabelEdit);
			// 
			// _menuStrip
			// 
			this._menuStrip.Dock = System.Windows.Forms.DockStyle.None;
			this._menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this._fileMenuItem,
			this._imageMenuItem});
			this._menuStrip.Location = new System.Drawing.Point(0, 0);
			this._menuStrip.Name = "_menuStrip";
			this._menuStrip.Size = new System.Drawing.Size(561, 24);
			this._menuStrip.TabIndex = 0;
			this._menuStrip.Text = "menuStrip1";
			// 
			// _fileMenuItem
			// 
			this._fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this._openMenuItem,
			this._saveAsMenuItem,
			this._saveMenuItem,
			this.toolStripMenuItem1,
			this._exitMenuItem});
			this._fileMenuItem.Name = "_fileMenuItem";
			this._fileMenuItem.Size = new System.Drawing.Size(37, 20);
			this._fileMenuItem.Text = "&File";
			// 
			// _openMenuItem
			// 
			this._openMenuItem.Image = global::ImageUtil.Properties.Resources.Open;
			this._openMenuItem.Name = "_openMenuItem";
			this._openMenuItem.Size = new System.Drawing.Size(123, 22);
			this._openMenuItem.Text = "&Open...";
			this._openMenuItem.Click += new System.EventHandler(this.OpenMenuItemClick);
			// 
			// _saveAsMenuItem
			// 
			this._saveAsMenuItem.Name = "_saveAsMenuItem";
			this._saveAsMenuItem.Size = new System.Drawing.Size(123, 22);
			this._saveAsMenuItem.Text = "Save As...";
			this._saveAsMenuItem.Click += new System.EventHandler(this.SaveAsMenuItemClick);
			// 
			// _saveMenuItem
			// 
			this._saveMenuItem.Image = global::ImageUtil.Properties.Resources.Save;
			this._saveMenuItem.Name = "_saveMenuItem";
			this._saveMenuItem.Size = new System.Drawing.Size(123, 22);
			this._saveMenuItem.Text = "&Save";
			this._saveMenuItem.Click += new System.EventHandler(this.SaveMenuItemClick);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(120, 6);
			// 
			// _exitMenuItem
			// 
			this._exitMenuItem.Name = "_exitMenuItem";
			this._exitMenuItem.Size = new System.Drawing.Size(123, 22);
			this._exitMenuItem.Text = "E&xit";
			this._exitMenuItem.Click += new System.EventHandler(this.ExitMenuItemClick);
			// 
			// _imageMenuItem
			// 
			this._imageMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this._addMenuItem,
			this._removeMenuItem,
			this._saveToFileMenuItem,
			this._renameMenuItem,
			this.toolStripMenuItem2,
			this._importFolderMenuItem});
			this._imageMenuItem.Name = "_imageMenuItem";
			this._imageMenuItem.Size = new System.Drawing.Size(52, 20);
			this._imageMenuItem.Text = "&Image";
			// 
			// _addMenuItem
			// 
			this._addMenuItem.Image = global::ImageUtil.Properties.Resources.Add;
			this._addMenuItem.Name = "_addMenuItem";
			this._addMenuItem.Size = new System.Drawing.Size(153, 22);
			this._addMenuItem.Text = "&Add...";
			this._addMenuItem.Click += new System.EventHandler(this.AddMenuItemClick);
			// 
			// _removeMenuItem
			// 
			this._removeMenuItem.Image = global::ImageUtil.Properties.Resources.Remove;
			this._removeMenuItem.Name = "_removeMenuItem";
			this._removeMenuItem.Size = new System.Drawing.Size(153, 22);
			this._removeMenuItem.Text = "&Remove";
			this._removeMenuItem.Click += new System.EventHandler(this.RemoveMenuItemClick);
			// 
			// _saveToFileMenuItem
			// 
			this._saveToFileMenuItem.Name = "_saveToFileMenuItem";
			this._saveToFileMenuItem.Size = new System.Drawing.Size(153, 22);
			this._saveToFileMenuItem.Text = "&Save to file...";
			this._saveToFileMenuItem.Click += new System.EventHandler(this.SaveToFileMenuItemClick);
			// 
			// _renameMenuItem
			// 
			this._renameMenuItem.Image = global::ImageUtil.Properties.Resources.Rename;
			this._renameMenuItem.Name = "_renameMenuItem";
			this._renameMenuItem.Size = new System.Drawing.Size(153, 22);
			this._renameMenuItem.Text = "Re&name...";
			this._renameMenuItem.Click += new System.EventHandler(this.RenameMenuItemClick);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(150, 6);
			// 
			// _importFolderMenuItem
			// 
			this._importFolderMenuItem.Name = "_importFolderMenuItem";
			this._importFolderMenuItem.Size = new System.Drawing.Size(153, 22);
			this._importFolderMenuItem.Text = "Import &folder...";
			this._importFolderMenuItem.Click += new System.EventHandler(this.ImportFolderMenuItemClick);
			// 
			// _toolStrip
			// 
			this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
			this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this._openButton,
			this._saveButton,
			this.toolStripSeparator1,
			this._addButton,
			this._removeButton,
			this._renameButton});
			this._toolStrip.Location = new System.Drawing.Point(3, 24);
			this._toolStrip.Name = "_toolStrip";
			this._toolStrip.Size = new System.Drawing.Size(133, 25);
			this._toolStrip.TabIndex = 0;
			this._toolStrip.Text = "toolStrip1";
			// 
			// _openButton
			// 
			this._openButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._openButton.Image = global::ImageUtil.Properties.Resources.Open;
			this._openButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._openButton.Name = "_openButton";
			this._openButton.Size = new System.Drawing.Size(23, 22);
			this._openButton.Text = "Open resource file";
			this._openButton.Click += new System.EventHandler(this.OpenButtonClick);
			// 
			// _saveButton
			// 
			this._saveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._saveButton.Image = global::ImageUtil.Properties.Resources.Save;
			this._saveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._saveButton.Name = "_saveButton";
			this._saveButton.Size = new System.Drawing.Size(23, 22);
			this._saveButton.Text = "Save resource file";
			this._saveButton.Click += new System.EventHandler(this.SaveButtonClick);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// _addButton
			// 
			this._addButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._addButton.Image = global::ImageUtil.Properties.Resources.Add;
			this._addButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._addButton.Name = "_addButton";
			this._addButton.Size = new System.Drawing.Size(23, 22);
			this._addButton.Text = "Add image";
			this._addButton.Click += new System.EventHandler(this.AddButtonClick);
			// 
			// _removeButton
			// 
			this._removeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._removeButton.Image = global::ImageUtil.Properties.Resources.Remove;
			this._removeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._removeButton.Name = "_removeButton";
			this._removeButton.Size = new System.Drawing.Size(23, 22);
			this._removeButton.Text = "Remove";
			this._removeButton.Click += new System.EventHandler(this.RemoveButtonClick);
			// 
			// _renameButton
			// 
			this._renameButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._renameButton.Image = global::ImageUtil.Properties.Resources.Rename;
			this._renameButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._renameButton.Name = "_renameButton";
			this._renameButton.Size = new System.Drawing.Size(23, 22);
			this._renameButton.Text = "Rename";
			this._renameButton.Click += new System.EventHandler(this.RenameButtonClick);
			// 
			// _openFileDialog
			// 
			this._openFileDialog.Filter = "ResX files|*.resx|Resources files|*.resources|All files|*.*";
			// 
			// _saveFileDialog
			// 
			this._saveFileDialog.Filter = "ResX files|*.resx|Resources files|*.resources|All files|*.*";
			// 
			// _contextMenu
			// 
			this._contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this._addContextMenuItem,
			this._removeContextMenuItem,
			this._renameContextMenuItem,
			this._renameFolderContextMenuItem,
			this._saveToFileContextMenuItem,
			this._separatorContextMenuItem,
			this._importFolderContextMenuItem});
			this._contextMenu.Name = "_contextMenu";
			this._contextMenu.Size = new System.Drawing.Size(161, 142);
			// 
			// _addContextMenuItem
			// 
			this._addContextMenuItem.Image = global::ImageUtil.Properties.Resources.Add;
			this._addContextMenuItem.Name = "_addContextMenuItem";
			this._addContextMenuItem.Size = new System.Drawing.Size(160, 22);
			this._addContextMenuItem.Text = "&Add...";
			this._addContextMenuItem.Click += new System.EventHandler(this.AddContextMenuItemClick);
			// 
			// _removeContextMenuItem
			// 
			this._removeContextMenuItem.Image = global::ImageUtil.Properties.Resources.Remove;
			this._removeContextMenuItem.Name = "_removeContextMenuItem";
			this._removeContextMenuItem.Size = new System.Drawing.Size(160, 22);
			this._removeContextMenuItem.Text = "&Remove";
			this._removeContextMenuItem.Click += new System.EventHandler(this.RemoveContextMenuItemClick);
			// 
			// _renameContextMenuItem
			// 
			this._renameContextMenuItem.Image = global::ImageUtil.Properties.Resources.Rename;
			this._renameContextMenuItem.Name = "_renameContextMenuItem";
			this._renameContextMenuItem.Size = new System.Drawing.Size(160, 22);
			this._renameContextMenuItem.Text = "Re&name...";
			this._renameContextMenuItem.Click += new System.EventHandler(this.RenameContextMenuItemClick);
			// 
			// _renameFolderContextMenuItem
			// 
			this._renameFolderContextMenuItem.Enabled = false;
			this._renameFolderContextMenuItem.Image = global::ImageUtil.Properties.Resources.Rename;
			this._renameFolderContextMenuItem.Name = "_renameFolderContextMenuItem";
			this._renameFolderContextMenuItem.Size = new System.Drawing.Size(160, 22);
			this._renameFolderContextMenuItem.Text = "Re&name folder...";
			// 
			// _saveToFileContextMenuItem
			// 
			this._saveToFileContextMenuItem.Name = "_saveToFileContextMenuItem";
			this._saveToFileContextMenuItem.Size = new System.Drawing.Size(160, 22);
			this._saveToFileContextMenuItem.Text = "&Save to file...";
			this._saveToFileContextMenuItem.Click += new System.EventHandler(this.SaveToFileContextMenuItemClick);
			// 
			// _separatorContextMenuItem
			// 
			this._separatorContextMenuItem.Name = "_separatorContextMenuItem";
			this._separatorContextMenuItem.Size = new System.Drawing.Size(157, 6);
			// 
			// _importFolderContextMenuItem
			// 
			this._importFolderContextMenuItem.Name = "_importFolderContextMenuItem";
			this._importFolderContextMenuItem.Size = new System.Drawing.Size(160, 22);
			this._importFolderContextMenuItem.Text = "Import &folder...";
			this._importFolderContextMenuItem.Click += new System.EventHandler(this.ImportFolderContextMenuItemClick);
			// 
			// _openImageDialog
			// 
			this._openImageDialog.Filter = "All files|*.*";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(561, 351);
			this.Controls.Add(this._toolStripContainer);
			this.MainMenuStrip = this._menuStrip;
			this.Name = "MainForm";
			this.Text = "Images management utility";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this._toolStripContainer.ContentPanel.ResumeLayout(false);
			this._toolStripContainer.TopToolStripPanel.ResumeLayout(false);
			this._toolStripContainer.TopToolStripPanel.PerformLayout();
			this._toolStripContainer.ResumeLayout(false);
			this._toolStripContainer.PerformLayout();
			this._treeSplitter.Panel1.ResumeLayout(false);
			this._treeSplitter.Panel2.ResumeLayout(false);
			this._treeSplitter.ResumeLayout(false);
			this._menuStrip.ResumeLayout(false);
			this._menuStrip.PerformLayout();
			this._toolStrip.ResumeLayout(false);
			this._toolStrip.PerformLayout();
			this._contextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolStripContainer _toolStripContainer;
		private System.Windows.Forms.MenuStrip _menuStrip;
		private System.Windows.Forms.ToolStripMenuItem _fileMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _imageMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _addMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _removeMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _renameMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _openMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _saveMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem _exitMenuItem;
		private System.Windows.Forms.ToolStrip _toolStrip;
		private System.Windows.Forms.ToolStripButton _openButton;
		private System.Windows.Forms.ToolStripButton _saveButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton _addButton;
		private System.Windows.Forms.ToolStripButton _removeButton;
		private System.Windows.Forms.ToolStripButton _renameButton;
		private System.Windows.Forms.SplitContainer _treeSplitter;
		private System.Windows.Forms.OpenFileDialog _openFileDialog;
		private System.Windows.Forms.SaveFileDialog _saveFileDialog;
		private System.Windows.Forms.ListView _imagesList;
		private Rsdn.TreeGrid.TreeGrid _foldersTree;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ToolStripMenuItem _saveAsMenuItem;
		private System.Windows.Forms.ContextMenuStrip _contextMenu;
		private System.Windows.Forms.ToolStripMenuItem _addContextMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _removeContextMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _renameContextMenuItem;
		private System.Windows.Forms.OpenFileDialog _openImageDialog;
		private System.Windows.Forms.ToolStripMenuItem _importFolderContextMenuItem;
		private System.Windows.Forms.ToolStripSeparator _separatorContextMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _saveToFileContextMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _saveToFileMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem _importFolderMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _renameFolderContextMenuItem;
	}
}

