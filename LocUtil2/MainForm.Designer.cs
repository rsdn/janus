using System.Windows.Forms;

namespace Rsdn.LocUtil
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
			System.Windows.Forms.ToolStrip toolStrip1;
			System.Windows.Forms.ToolStripButton _openButton;
			System.Windows.Forms.ToolStripMenuItem _fileMenuItem;
			System.Windows.Forms.ToolStripMenuItem _openMenuItem;
			System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
			System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
			System.Windows.Forms.ToolStripMenuItem _exitMenuItem;
			System.Windows.Forms.ToolStrip toolStrip2;
			System.Windows.Forms.ToolStripContainer _toolStripContainer;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			System.Windows.Forms.ToolStripMenuItem _addTreeContextMenuIten;
			System.Windows.Forms.ToolStripMenuItem _removeListContextMenuItem;
			System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
			System.Windows.Forms.ToolStripMenuItem _renameListContextMenuItem;
			this._saveButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this._generateButton = new System.Windows.Forms.ToolStripButton();
			this._saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._manageCulturesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._generateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._addResourceButton = new System.Windows.Forms.ToolStripButton();
			this._removeResourceButton = new System.Windows.Forms.ToolStripButton();
			this._renameResourceButton = new System.Windows.Forms.ToolStripButton();
			this._treeSplitter = new System.Windows.Forms.SplitContainer();
			this._paneTree = new Rsdn.LocUtil.Pane();
			this._categoryTree = new Rsdn.TreeGrid.TreeGrid();
			this._nameCol = new System.Windows.Forms.ColumnHeader();
			this._countCol = new System.Windows.Forms.ColumnHeader();
			this._treeImages = new System.Windows.Forms.ImageList(this.components);
			this._resourceSplitter = new System.Windows.Forms.SplitContainer();
			this._paneItems = new Rsdn.LocUtil.Pane();
			this._itemList = new System.Windows.Forms.ListView();
			this._itemNameCol = new System.Windows.Forms.ColumnHeader();
			this._listImages = new System.Windows.Forms.ImageList(this.components);
			this._paneProperties = new Rsdn.LocUtil.Pane();
			this._propertyGrid = new System.Windows.Forms.PropertyGrid();
			this._menuStrip = new System.Windows.Forms.MenuStrip();
			this._resourceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._addResourceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._removeResourceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._renameResourceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._treeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this._listContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
			this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
			this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
			this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
			this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
			toolStrip1 = new System.Windows.Forms.ToolStrip();
			_openButton = new System.Windows.Forms.ToolStripButton();
			_fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			_openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			_exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			toolStrip2 = new System.Windows.Forms.ToolStrip();
			_toolStripContainer = new System.Windows.Forms.ToolStripContainer();
			_addTreeContextMenuIten = new System.Windows.Forms.ToolStripMenuItem();
			_removeListContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			_renameListContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			toolStrip1.SuspendLayout();
			toolStrip2.SuspendLayout();
			_toolStripContainer.ContentPanel.SuspendLayout();
			_toolStripContainer.TopToolStripPanel.SuspendLayout();
			_toolStripContainer.SuspendLayout();
			this._treeSplitter.Panel1.SuspendLayout();
			this._treeSplitter.Panel2.SuspendLayout();
			this._treeSplitter.SuspendLayout();
			this._paneTree.SuspendLayout();
			this._resourceSplitter.Panel1.SuspendLayout();
			this._resourceSplitter.Panel2.SuspendLayout();
			this._resourceSplitter.SuspendLayout();
			this._paneItems.SuspendLayout();
			this._paneProperties.SuspendLayout();
			this._menuStrip.SuspendLayout();
			this._treeContextMenu.SuspendLayout();
			this._listContextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
			toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			_openButton,
			this._saveButton,
			this.toolStripSeparator3,
			this._generateButton});
			toolStrip1.Location = new System.Drawing.Point(3, 24);
			toolStrip1.Name = "toolStrip1";
			toolStrip1.Size = new System.Drawing.Size(87, 25);
			toolStrip1.TabIndex = 1;
			toolStrip1.Text = "toolStrip1";
			// 
			// _openButton
			// 
			_openButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			_openButton.Image = global::Rsdn.LocUtil.Properties.Resources.Open;
			_openButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			_openButton.Name = "_openButton";
			_openButton.Size = new System.Drawing.Size(23, 22);
			_openButton.Text = "Open Resource File";
			_openButton.Click += new System.EventHandler(this._openMenuItem_Click);
			// 
			// _saveButton
			// 
			this._saveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._saveButton.Enabled = false;
			this._saveButton.Image = global::Rsdn.LocUtil.Properties.Resources.Save;
			this._saveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._saveButton.Name = "_saveButton";
			this._saveButton.Size = new System.Drawing.Size(23, 22);
			this._saveButton.Text = "Save Resource File";
			this._saveButton.Click += new System.EventHandler(this._saveMenuItem_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// _generateButton
			// 
			this._generateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._generateButton.Enabled = false;
			this._generateButton.Image = global::Rsdn.LocUtil.Properties.Resources.Generate;
			this._generateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._generateButton.Name = "_generateButton";
			this._generateButton.Size = new System.Drawing.Size(23, 22);
			this._generateButton.Text = "Generate Resource Helper";
			this._generateButton.Click += new System.EventHandler(this._generateMenuItem_Click);
			// 
			// _fileMenuItem
			// 
			_fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			_openMenuItem,
			this._saveMenuItem,
			toolStripSeparator1,
			this._manageCulturesMenuItem,
			this._generateMenuItem,
			toolStripSeparator2,
			_exitMenuItem});
			_fileMenuItem.Name = "_fileMenuItem";
			_fileMenuItem.Size = new System.Drawing.Size(35, 20);
			_fileMenuItem.Text = "&File";
			// 
			// _openMenuItem
			// 
			_openMenuItem.Image = global::Rsdn.LocUtil.Properties.Resources.Open;
			_openMenuItem.Name = "_openMenuItem";
			_openMenuItem.Size = new System.Drawing.Size(166, 22);
			_openMenuItem.Text = "&Open";
			_openMenuItem.Click += new System.EventHandler(this._openMenuItem_Click);
			// 
			// _saveMenuItem
			// 
			this._saveMenuItem.Enabled = false;
			this._saveMenuItem.Image = global::Rsdn.LocUtil.Properties.Resources.Save;
			this._saveMenuItem.Name = "_saveMenuItem";
			this._saveMenuItem.Size = new System.Drawing.Size(166, 22);
			this._saveMenuItem.Text = "&Save";
			this._saveMenuItem.Click += new System.EventHandler(this._saveMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new System.Drawing.Size(163, 6);
			// 
			// _manageCulturesMenuItem
			// 
			this._manageCulturesMenuItem.Enabled = false;
			this._manageCulturesMenuItem.Name = "_manageCulturesMenuItem";
			this._manageCulturesMenuItem.Size = new System.Drawing.Size(166, 22);
			this._manageCulturesMenuItem.Text = "Manage &Cultures";
			this._manageCulturesMenuItem.Click += new System.EventHandler(this._manageCulturesMenuItem_Click);
			// 
			// _generateMenuItem
			// 
			this._generateMenuItem.Enabled = false;
			this._generateMenuItem.Image = global::Rsdn.LocUtil.Properties.Resources.Generate;
			this._generateMenuItem.Name = "_generateMenuItem";
			this._generateMenuItem.Size = new System.Drawing.Size(166, 22);
			this._generateMenuItem.Text = "&Generate helper";
			this._generateMenuItem.Click += new System.EventHandler(this._generateMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			toolStripSeparator2.Name = "toolStripSeparator2";
			toolStripSeparator2.Size = new System.Drawing.Size(163, 6);
			// 
			// _exitMenuItem
			// 
			_exitMenuItem.Name = "_exitMenuItem";
			_exitMenuItem.Size = new System.Drawing.Size(166, 22);
			_exitMenuItem.Text = "&Exit";
			_exitMenuItem.Click += new System.EventHandler(this._exitMenuItem_Click);
			// 
			// toolStrip2
			// 
			toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
			toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this._addResourceButton,
			this._removeResourceButton,
			this._renameResourceButton});
			toolStrip2.Location = new System.Drawing.Point(90, 24);
			toolStrip2.Name = "toolStrip2";
			toolStrip2.Size = new System.Drawing.Size(81, 25);
			toolStrip2.TabIndex = 2;
			toolStrip2.Text = "toolStrip2";
			// 
			// _addResourceButton
			// 
			this._addResourceButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._addResourceButton.Enabled = false;
			this._addResourceButton.Image = global::Rsdn.LocUtil.Properties.Resources.Add;
			this._addResourceButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._addResourceButton.Name = "_addResourceButton";
			this._addResourceButton.Size = new System.Drawing.Size(23, 22);
			this._addResourceButton.Text = "Add Resource";
			this._addResourceButton.Click += new System.EventHandler(this._addResourceMenuItem_Click);
			// 
			// _removeResourceButton
			// 
			this._removeResourceButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._removeResourceButton.Enabled = false;
			this._removeResourceButton.Image = global::Rsdn.LocUtil.Properties.Resources.Remove;
			this._removeResourceButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._removeResourceButton.Name = "_removeResourceButton";
			this._removeResourceButton.Size = new System.Drawing.Size(23, 22);
			this._removeResourceButton.Text = "Remove Resource";
			this._removeResourceButton.Click += new System.EventHandler(this._removeResourceMenuItem_Click);
			// 
			// _renameResourceButton
			// 
			this._renameResourceButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._renameResourceButton.Enabled = false;
			this._renameResourceButton.Image = global::Rsdn.LocUtil.Properties.Resources.Rename;
			this._renameResourceButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._renameResourceButton.Name = "_renameResourceButton";
			this._renameResourceButton.Size = new System.Drawing.Size(23, 22);
			this._renameResourceButton.Text = "Rename Resource";
			this._renameResourceButton.Click += new System.EventHandler(this._renameResourceMenuItem_Click);
			// 
			// _toolStripContainer
			// 
			// 
			// _toolStripContainer.ContentPanel
			// 
			_toolStripContainer.ContentPanel.Controls.Add(this._treeSplitter);
			_toolStripContainer.ContentPanel.Size = new System.Drawing.Size(792, 517);
			_toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			_toolStripContainer.Location = new System.Drawing.Point(0, 0);
			_toolStripContainer.Name = "_toolStripContainer";
			_toolStripContainer.Size = new System.Drawing.Size(792, 566);
			_toolStripContainer.TabIndex = 12;
			_toolStripContainer.Text = "toolStripContainer1";
			// 
			// _toolStripContainer.TopToolStripPanel
			// 
			_toolStripContainer.TopToolStripPanel.Controls.Add(this._menuStrip);
			_toolStripContainer.TopToolStripPanel.Controls.Add(toolStrip1);
			_toolStripContainer.TopToolStripPanel.Controls.Add(toolStrip2);
			// 
			// _treeSplitter
			// 
			this._treeSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this._treeSplitter.Location = new System.Drawing.Point(0, 0);
			this._treeSplitter.Name = "_treeSplitter";
			// 
			// _treeSplitter.Panel1
			// 
			this._treeSplitter.Panel1.Controls.Add(this._paneTree);
			// 
			// _treeSplitter.Panel2
			// 
			this._treeSplitter.Panel2.Controls.Add(this._resourceSplitter);
			this._treeSplitter.Size = new System.Drawing.Size(792, 517);
			this._treeSplitter.SplitterDistance = 232;
			this._treeSplitter.TabIndex = 0;
			this._treeSplitter.Text = "splitContainer1";
			// 
			// _paneTree
			// 
			this._paneTree.Controls.Add(this._categoryTree);
			this._paneTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this._paneTree.Location = new System.Drawing.Point(0, 0);
			this._paneTree.Name = "_paneTree";
			this._paneTree.Padding = new System.Windows.Forms.Padding(1, 20, 1, 1);
			// 
			// 
			// 
			this._paneTree.PaneCaptionContol.Dock = System.Windows.Forms.DockStyle.Top;
			this._paneTree.PaneCaptionContol.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
			this._paneTree.PaneCaptionContol.Location = new System.Drawing.Point(1, 1);
			this._paneTree.PaneCaptionContol.Name = "_caption";
			this._paneTree.PaneCaptionContol.Size = new System.Drawing.Size(230, 18);
			this._paneTree.PaneCaptionContol.TabIndex = 0;
			this._paneTree.PaneCaptionContol.Text = "Panel 1";
			this._paneTree.Size = new System.Drawing.Size(232, 517);
			this._paneTree.TabIndex = 7;
			// 
			// _categoryTree
			// 
			this._categoryTree.BackColor = System.Drawing.SystemColors.Window;
			this._categoryTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._categoryTree.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this._nameCol,
			this._countCol});
			this._categoryTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this._categoryTree.FullRowSelect = true;
			this._categoryTree.HideSelection = false;
			this._categoryTree.Indent = 5;
			this._categoryTree.LargeImageList = this._treeImages;
			this._categoryTree.Location = new System.Drawing.Point(1, 20);
			this._categoryTree.Name = "_categoryTree";
			this._categoryTree.OwnerDraw = true;
			this._categoryTree.Size = new System.Drawing.Size(230, 496);
			this._categoryTree.SmallImageList = this._treeImages;
			this._categoryTree.TabIndex = 0;
			this._categoryTree.UseCompatibleStateImageBehavior = false;
			this._categoryTree.View = System.Windows.Forms.View.Details;
			this._categoryTree.VirtualMode = true;
			this._categoryTree.GetData += new System.EventHandler<Rsdn.TreeGrid.GetDataEventArgs>(this._categoryTree_GetData);
			this._categoryTree.MouseUp += new System.Windows.Forms.MouseEventHandler(this._categoryTree_MouseUp);
			this._categoryTree.AfterActivateNode += new Rsdn.TreeGrid.AfterActivateNodeHandler(this._categoryTree_AfterActivateNode);
			// 
			// _nameCol
			// 
			this._nameCol.Text = "Name";
			this._nameCol.Width = 159;
			// 
			// _countCol
			// 
			this._countCol.Text = "Count";
			// 
			// _treeImages
			// 
			this._treeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_treeImages.ImageStream")));
			this._treeImages.TransparentColor = System.Drawing.Color.Transparent;
			this._treeImages.Images.SetKeyName(0, "root");
			this._treeImages.Images.SetKeyName(1, "category");
			// 
			// _resourceSplitter
			// 
			this._resourceSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this._resourceSplitter.Location = new System.Drawing.Point(0, 0);
			this._resourceSplitter.Name = "_resourceSplitter";
			this._resourceSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// _resourceSplitter.Panel1
			// 
			this._resourceSplitter.Panel1.Controls.Add(this._paneItems);
			// 
			// _resourceSplitter.Panel2
			// 
			this._resourceSplitter.Panel2.Controls.Add(this._paneProperties);
			this._resourceSplitter.Size = new System.Drawing.Size(556, 517);
			this._resourceSplitter.SplitterDistance = 336;
			this._resourceSplitter.TabIndex = 0;
			this._resourceSplitter.Text = "splitContainer1";
			// 
			// _paneItems
			// 
			this._paneItems.Controls.Add(this._itemList);
			this._paneItems.Dock = System.Windows.Forms.DockStyle.Fill;
			this._paneItems.Location = new System.Drawing.Point(0, 0);
			this._paneItems.Name = "_paneItems";
			this._paneItems.Padding = new System.Windows.Forms.Padding(1, 20, 1, 1);
			// 
			// 
			// 
			this._paneItems.PaneCaptionContol.Dock = System.Windows.Forms.DockStyle.Top;
			this._paneItems.PaneCaptionContol.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
			this._paneItems.PaneCaptionContol.Location = new System.Drawing.Point(1, 1);
			this._paneItems.PaneCaptionContol.Name = "_caption";
			this._paneItems.PaneCaptionContol.Size = new System.Drawing.Size(554, 18);
			this._paneItems.PaneCaptionContol.TabIndex = 0;
			this._paneItems.PaneCaptionContol.Text = "Panel 1";
			this._paneItems.Size = new System.Drawing.Size(556, 336);
			this._paneItems.TabIndex = 9;
			// 
			// _itemList
			// 
			this._itemList.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._itemList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this._itemNameCol});
			this._itemList.Dock = System.Windows.Forms.DockStyle.Fill;
			this._itemList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this._itemList.HideSelection = false;
			this._itemList.LabelEdit = true;
			this._itemList.Location = new System.Drawing.Point(1, 20);
			this._itemList.Name = "_itemList";
			this._itemList.Size = new System.Drawing.Size(554, 315);
			this._itemList.SmallImageList = this._listImages;
			this._itemList.TabIndex = 4;
			this._itemList.UseCompatibleStateImageBehavior = false;
			this._itemList.View = System.Windows.Forms.View.Details;
			this._itemList.SelectedIndexChanged += new System.EventHandler(this._itemList_SelectedIndexChanged);
			this._itemList.MouseUp += new System.Windows.Forms.MouseEventHandler(this._itemList_MouseUp);
			this._itemList.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this._itemList_AfterLabelEdit);
			// 
			// _listImages
			// 
			this._listImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_listImages.ImageStream")));
			this._listImages.TransparentColor = System.Drawing.Color.Transparent;
			this._listImages.Images.SetKeyName(0, "item");
			// 
			// _paneProperties
			// 
			this._paneProperties.Controls.Add(this._propertyGrid);
			this._paneProperties.Dock = System.Windows.Forms.DockStyle.Fill;
			this._paneProperties.Location = new System.Drawing.Point(0, 0);
			this._paneProperties.Name = "_paneProperties";
			this._paneProperties.Padding = new System.Windows.Forms.Padding(1, 20, 1, 1);
			// 
			// 
			// 
			this._paneProperties.PaneCaptionContol.Dock = System.Windows.Forms.DockStyle.Top;
			this._paneProperties.PaneCaptionContol.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
			this._paneProperties.PaneCaptionContol.Location = new System.Drawing.Point(1, 1);
			this._paneProperties.PaneCaptionContol.Name = "_caption";
			this._paneProperties.PaneCaptionContol.Size = new System.Drawing.Size(554, 18);
			this._paneProperties.PaneCaptionContol.TabIndex = 0;
			this._paneProperties.PaneCaptionContol.Text = "Panel 1";
			this._paneProperties.Size = new System.Drawing.Size(556, 177);
			this._paneProperties.TabIndex = 8;
			// 
			// _propertyGrid
			// 
			this._propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this._propertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
			this._propertyGrid.Location = new System.Drawing.Point(1, 20);
			this._propertyGrid.Name = "_propertyGrid";
			this._propertyGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
			this._propertyGrid.Size = new System.Drawing.Size(554, 156);
			this._propertyGrid.TabIndex = 0;
			this._propertyGrid.ToolbarVisible = false;
			// 
			// _menuStrip
			// 
			this._menuStrip.Dock = System.Windows.Forms.DockStyle.None;
			this._menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			_fileMenuItem,
			this._resourceMenuItem});
			this._menuStrip.Location = new System.Drawing.Point(0, 0);
			this._menuStrip.Name = "_menuStrip";
			this._menuStrip.Size = new System.Drawing.Size(792, 24);
			this._menuStrip.TabIndex = 0;
			this._menuStrip.Text = "menuStrip1";
			// 
			// _resourceMenuItem
			// 
			this._resourceMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this._addResourceMenuItem,
			this._removeResourceMenuItem,
			this._renameResourceMenuItem});
			this._resourceMenuItem.Enabled = false;
			this._resourceMenuItem.Name = "_resourceMenuItem";
			this._resourceMenuItem.Size = new System.Drawing.Size(64, 20);
			this._resourceMenuItem.Text = "&Resource";
			// 
			// _addResourceMenuItem
			// 
			this._addResourceMenuItem.Enabled = false;
			this._addResourceMenuItem.Image = global::Rsdn.LocUtil.Properties.Resources.Add;
			this._addResourceMenuItem.Name = "_addResourceMenuItem";
			this._addResourceMenuItem.Size = new System.Drawing.Size(124, 22);
			this._addResourceMenuItem.Text = "&Add";
			this._addResourceMenuItem.Click += new System.EventHandler(this._addResourceMenuItem_Click);
			// 
			// _removeResourceMenuItem
			// 
			this._removeResourceMenuItem.Enabled = false;
			this._removeResourceMenuItem.Image = global::Rsdn.LocUtil.Properties.Resources.Remove;
			this._removeResourceMenuItem.Name = "_removeResourceMenuItem";
			this._removeResourceMenuItem.Size = new System.Drawing.Size(124, 22);
			this._removeResourceMenuItem.Text = "&Remove";
			this._removeResourceMenuItem.Click += new System.EventHandler(this._removeResourceMenuItem_Click);
			// 
			// _renameResourceMenuItem
			// 
			this._renameResourceMenuItem.Enabled = false;
			this._renameResourceMenuItem.Image = global::Rsdn.LocUtil.Properties.Resources.Rename;
			this._renameResourceMenuItem.Name = "_renameResourceMenuItem";
			this._renameResourceMenuItem.Size = new System.Drawing.Size(124, 22);
			this._renameResourceMenuItem.Text = "Re&name";
			this._renameResourceMenuItem.Click += new System.EventHandler(this._renameResourceMenuItem_Click);
			// 
			// _addTreeContextMenuIten
			// 
			_addTreeContextMenuIten.Image = global::Rsdn.LocUtil.Properties.Resources.Add;
			_addTreeContextMenuIten.Name = "_addTreeContextMenuIten";
			_addTreeContextMenuIten.Size = new System.Drawing.Size(104, 22);
			_addTreeContextMenuIten.Text = "&Add";
			// 
			// _removeListContextMenuItem
			// 
			_removeListContextMenuItem.Image = global::Rsdn.LocUtil.Properties.Resources.Add;
			_removeListContextMenuItem.Name = "_removeListContextMenuItem";
			_removeListContextMenuItem.Size = new System.Drawing.Size(124, 22);
			_removeListContextMenuItem.Text = "&Add";
			_removeListContextMenuItem.Click += new System.EventHandler(this._addResourceMenuItem_Click);
			// 
			// removeToolStripMenuItem
			// 
			removeToolStripMenuItem.Image = global::Rsdn.LocUtil.Properties.Resources.Remove;
			removeToolStripMenuItem.Name = "removeToolStripMenuItem";
			removeToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
			removeToolStripMenuItem.Text = "&Remove";
			removeToolStripMenuItem.Click += new System.EventHandler(this._removeResourceMenuItem_Click);
			// 
			// _renameListContextMenuItem
			// 
			_renameListContextMenuItem.Image = global::Rsdn.LocUtil.Properties.Resources.Rename;
			_renameListContextMenuItem.Name = "_renameListContextMenuItem";
			_renameListContextMenuItem.Size = new System.Drawing.Size(124, 22);
			_renameListContextMenuItem.Text = "Re&name";
			_renameListContextMenuItem.Click += new System.EventHandler(this._renameResourceMenuItem_Click);
			// 
			// _treeContextMenu
			// 
			this._treeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			_addTreeContextMenuIten});
			this._treeContextMenu.Name = "_treeContextMenu";
			this._treeContextMenu.Size = new System.Drawing.Size(105, 26);
			this._treeContextMenu.Click += new System.EventHandler(this._addResourceMenuItem_Click);
			// 
			// _listContextMenu
			// 
			this._listContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			_removeListContextMenuItem,
			removeToolStripMenuItem,
			_renameListContextMenuItem});
			this._listContextMenu.Name = "_listContextMenu";
			this._listContextMenu.Size = new System.Drawing.Size(125, 70);
			// 
			// _openFileDialog
			// 
			this._openFileDialog.DefaultExt = "resx";
			this._openFileDialog.Filter = "ResX files|*.resx";
			// 
			// BottomToolStripPanel
			// 
			this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
			this.BottomToolStripPanel.Name = "BottomToolStripPanel";
			this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(0);
			this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
			// 
			// TopToolStripPanel
			// 
			this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
			this.TopToolStripPanel.Name = "TopToolStripPanel";
			this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(0);
			this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
			// 
			// RightToolStripPanel
			// 
			this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
			this.RightToolStripPanel.Name = "RightToolStripPanel";
			this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(0);
			this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
			// 
			// LeftToolStripPanel
			// 
			this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
			this.LeftToolStripPanel.Name = "LeftToolStripPanel";
			this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(0);
			this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
			// 
			// ContentPanel
			// 
			this.ContentPanel.Size = new System.Drawing.Size(200, 100);
			// 
			// MainForm
			// 
			this.ClientSize = new System.Drawing.Size(792, 566);
			this.Controls.Add(_toolStripContainer);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this._menuStrip;
			this.Name = "MainForm";
			this.Text = "Localization utility";
			toolStrip1.ResumeLayout(false);
			toolStrip1.PerformLayout();
			toolStrip2.ResumeLayout(false);
			toolStrip2.PerformLayout();
			_toolStripContainer.ContentPanel.ResumeLayout(false);
			_toolStripContainer.TopToolStripPanel.ResumeLayout(false);
			_toolStripContainer.TopToolStripPanel.PerformLayout();
			_toolStripContainer.ResumeLayout(false);
			_toolStripContainer.PerformLayout();
			this._treeSplitter.Panel1.ResumeLayout(false);
			this._treeSplitter.Panel2.ResumeLayout(false);
			this._treeSplitter.ResumeLayout(false);
			this._paneTree.ResumeLayout(false);
			this._resourceSplitter.Panel1.ResumeLayout(false);
			this._resourceSplitter.Panel2.ResumeLayout(false);
			this._resourceSplitter.ResumeLayout(false);
			this._paneItems.ResumeLayout(false);
			this._paneProperties.ResumeLayout(false);
			this._menuStrip.ResumeLayout(false);
			this._menuStrip.PerformLayout();
			this._treeContextMenu.ResumeLayout(false);
			this._listContextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenFileDialog _openFileDialog;
		private ImageList _treeImages;
		private ImageList _listImages;
		private ToolStripPanel BottomToolStripPanel;
		private ToolStripPanel TopToolStripPanel;
		private MenuStrip _menuStrip;
		private ToolStripPanel RightToolStripPanel;
		private ToolStripPanel LeftToolStripPanel;
		private ToolStripContentPanel ContentPanel;
		private SplitContainer _treeSplitter;
		private SplitContainer _resourceSplitter;
		private ContextMenuStrip _treeContextMenu;
		private Pane _paneTree;
		private Rsdn.TreeGrid.TreeGrid _categoryTree;
		private ColumnHeader _nameCol;
		private ColumnHeader _countCol;
		private Pane _paneItems;
		private ListView _itemList;
		private ColumnHeader _itemNameCol;
		private Pane _paneProperties;
		private PropertyGrid _propertyGrid;
		private ContextMenuStrip _listContextMenu;
		private ToolStripSeparator toolStripSeparator3;
		private ToolStripButton _saveButton;
		private ToolStripButton _generateButton;
		private ToolStripMenuItem _saveMenuItem;
		private ToolStripMenuItem _generateMenuItem;
		private ToolStripMenuItem _addResourceMenuItem;
		private ToolStripMenuItem _removeResourceMenuItem;
		private ToolStripMenuItem _renameResourceMenuItem;
		private ToolStripButton _addResourceButton;
		private ToolStripButton _removeResourceButton;
		private ToolStripButton _renameResourceButton;
		private ToolStripMenuItem _resourceMenuItem;
		private ToolStripMenuItem _manageCulturesMenuItem;

	}
}