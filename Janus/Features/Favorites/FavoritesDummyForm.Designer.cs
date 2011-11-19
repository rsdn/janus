namespace Rsdn.Janus
{
	sealed partial class FavoritesDummyForm
	{
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_contextMenuGenerator.Dispose();
				_favManager.FavoritesReloaded -= FavoritesReloaded;
				StyleConfig.StyleChange -= OnStyleChanged;

				if (components != null)
					components.Dispose();
			}

			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private System.ComponentModel.IContainer components;

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FavoritesDummyForm));
			this._contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this._splitContainer = new System.Windows.Forms.SplitContainer();
			this._grid = new Rsdn.Janus.JanusGrid();
			this._linkCol = new System.Windows.Forms.ColumnHeader();
			this._commentCol = new System.Windows.Forms.ColumnHeader();
			this._splitContainer.Panel1.SuspendLayout();
			this._splitContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// _contextMenuStrip
			// 
			this._contextMenuStrip.Name = "_contextMenuStrip";
			resources.ApplyResources(this._contextMenuStrip, "_contextMenuStrip");
			// 
			// _splitContainer
			// 
			resources.ApplyResources(this._splitContainer, "_splitContainer");
			this._splitContainer.Name = "_splitContainer";
			// 
			// _splitContainer.Panel1
			// 
			this._splitContainer.Panel1.Controls.Add(this._grid);
			// 
			// _grid
			// 
			this._grid.AllowDrop = true;
			this._grid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(255)))), ((int)(((byte)(244)))));
			this._grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._grid.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this._linkCol,
			this._commentCol});
			this._grid.ColumnsOrder = new int[] {
		0,
		1};
			this._grid.ColumnsWidth = new int[] {
		200,
		200};
			resources.ApplyResources(this._grid, "_grid");
			this._grid.FullRowSelect = true;
			this._grid.GridLines = true;
			this._grid.Indent = 5;
			this._grid.Name = "_grid";
			this._grid.OwnerDraw = true;
			this._grid.UseCompatibleStateImageBehavior = false;
			this._grid.View = System.Windows.Forms.View.Details;
			this._grid.VirtualMode = true;
			this._grid.ColumnReordered += new System.Windows.Forms.ColumnReorderedEventHandler(this.GridColumnReordered);
			this._grid.DoubleClick += new System.EventHandler(this.GridDoubleClick);
			this._grid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
			this._grid.DragDrop += new System.Windows.Forms.DragEventHandler(this.GridDragDrop);
			this._grid.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GridMouseUp);
			this._grid.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.GridColumnClick);
			this._grid.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.GridColumnWidthChanged);
			this._grid.DragOver += new System.Windows.Forms.DragEventHandler(this.GridDragOver);
			this._grid.DragEnter += new System.Windows.Forms.DragEventHandler(this.GridDragEnter);
			this._grid.AfterActivateNode += new Rsdn.TreeGrid.AfterActivateNodeHandler(this.GridAfterActivateNode);
			this._grid.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GridMouseMove);
			// 
			// _linkCol
			// 
			resources.ApplyResources(this._linkCol, "_linkCol");
			// 
			// _commentCol
			// 
			resources.ApplyResources(this._commentCol, "_commentCol");
			// 
			// FavoritesDummyForm
			// 
			this.Controls.Add(this._splitContainer);
			this.Name = "FavoritesDummyForm";
			resources.ApplyResources(this, "$this");
			this._splitContainer.Panel1.ResumeLayout(false);
			this._splitContainer.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Rsdn.Janus.JanusGrid _grid;
		private System.Windows.Forms.SplitContainer _splitContainer;
		private System.Windows.Forms.ColumnHeader _commentCol;
		private System.Windows.Forms.ColumnHeader _linkCol;
		private System.Windows.Forms.ContextMenuStrip _contextMenuStrip;
	}
}