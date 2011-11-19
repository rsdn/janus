namespace Rsdn.Janus
{
	public sealed partial class OutboxDummyForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components;

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutboxDummyForm));
			this._grid = new Rsdn.TreeGrid.TreeGrid();
			this._forumCol = new System.Windows.Forms.ColumnHeader();
			this._subjectCol = new System.Windows.Forms.ColumnHeader();
			this._addInfoCol = new System.Windows.Forms.ColumnHeader();
			this._pnlFeatureView = new System.Windows.Forms.Panel();
			this._contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this._pnlFeatureView.SuspendLayout();
			this.SuspendLayout();
			// 
			// _grid
			// 
			this._grid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(255)))), ((int)(((byte)(244)))));
			this._grid.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this._forumCol,
			this._subjectCol,
			this._addInfoCol});
			resources.ApplyResources(this._grid, "_grid");
			this._grid.FullRowSelect = true;
			this._grid.GridLines = true;
			this._grid.HideSelection = false;
			this._grid.Indent = 5;
			this._grid.Name = "_grid";
			this._grid.OwnerDraw = true;
			this._grid.UseCompatibleStateImageBehavior = false;
			this._grid.View = System.Windows.Forms.View.Details;
			this._grid.VirtualMode = true;
			this._grid.DoubleClick += new System.EventHandler(this.GridDoubleClick);
			this._grid.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GridMouseUp);
			this._grid.SelectedIndexChanged += new System.EventHandler(this.GridSelectedIndexChanged);
			// 
			// _forumCol
			// 
			resources.ApplyResources(this._forumCol, "_forumCol");
			// 
			// _subjectCol
			// 
			resources.ApplyResources(this._subjectCol, "_subjectCol");
			// 
			// _addInfoCol
			// 
			resources.ApplyResources(this._addInfoCol, "_addInfoCol");
			// 
			// _pnlFeatureView
			// 
			this._pnlFeatureView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(240)))), ((int)(((byte)(224)))));
			this._pnlFeatureView.Controls.Add(this._grid);
			resources.ApplyResources(this._pnlFeatureView, "_pnlFeatureView");
			this._pnlFeatureView.Name = "_pnlFeatureView";
			// 
			// _contextMenuStrip
			// 
			this._contextMenuStrip.Name = "_contextMenuStrip";
			resources.ApplyResources(this._contextMenuStrip, "_contextMenuStrip");
			// 
			// OutboxDummyForm
			// 
			this.Controls.Add(this._pnlFeatureView);
			this.Name = "OutboxDummyForm";
			resources.ApplyResources(this, "$this");
			this._pnlFeatureView.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
		
		private System.Windows.Forms.ColumnHeader _forumCol;
		private System.Windows.Forms.ColumnHeader _subjectCol;
		private System.Windows.Forms.ColumnHeader _addInfoCol;
		private System.Windows.Forms.ContextMenuStrip _contextMenuStrip;
		private System.Windows.Forms.Panel _pnlFeatureView;

		private Rsdn.TreeGrid.TreeGrid _grid;
	}
}