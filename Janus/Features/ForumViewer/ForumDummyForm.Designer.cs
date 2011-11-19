namespace Rsdn.Janus
{
	sealed partial class ForumDummyForm
	{
		private System.ComponentModel.IContainer components;

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ForumDummyForm));
			this._toolTip = new System.Windows.Forms.ToolTip(this.components);
			this._btnResetFilter = new System.Windows.Forms.Button();
			this._contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this._saveMessageDialog = new System.Windows.Forms.SaveFileDialog();
			this._markTimer = new System.Timers.Timer();
			this._tgMsgs = new Rsdn.Janus.JanusGrid();
			this._idCol = new System.Windows.Forms.ColumnHeader();
			this._markedCol = new System.Windows.Forms.ColumnHeader();
			this._subjectCol = new System.Windows.Forms.ColumnHeader();
			this._userNickCol = new System.Windows.Forms.ColumnHeader();
			this._ratesCol = new System.Windows.Forms.ColumnHeader();
			this._ratesThisCol = new System.Windows.Forms.ColumnHeader();
			this._repliesCountCol = new System.Windows.Forms.ColumnHeader();
			this._msgDateCol = new System.Windows.Forms.ColumnHeader();
			this._filterPanel = new Rsdn.Janus.Framework.FramePanel();
			this._btnHideFilterPanel = new System.Windows.Forms.Button();
			this._tbxFilter = new System.Windows.Forms.TextBox();
			this._topPanel = new System.Windows.Forms.Panel();
			this._bottomPanel = new System.Windows.Forms.Panel();
			this._splitter = new System.Windows.Forms.Splitter();
			((System.ComponentModel.ISupportInitialize)(this._markTimer)).BeginInit();
			this._filterPanel.SuspendLayout();
			this._topPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// _toolTip
			// 
			this._toolTip.ShowAlways = true;
			// 
			// _btnResetFilter
			// 
			resources.ApplyResources(this._btnResetFilter, "_btnResetFilter");
			this._btnResetFilter.Name = "_btnResetFilter";
			this._toolTip.SetToolTip(this._btnResetFilter, resources.GetString("_btnResetFilter.ToolTip"));
			this._btnResetFilter.UseVisualStyleBackColor = true;
			this._btnResetFilter.Click += new System.EventHandler(this._btnResetFilter_Click);
			// 
			// _contextMenuStrip
			// 
			this._contextMenuStrip.Name = "_contextMenuStrip";
			resources.ApplyResources(this._contextMenuStrip, "_contextMenuStrip");
			// 
			// _saveMessageDialog
			// 
			this._saveMessageDialog.RestoreDirectory = true;
			this._saveMessageDialog.ShowHelp = true;
			resources.ApplyResources(this._saveMessageDialog, "_saveMessageDialog");
			// 
			// _markTimer
			// 
			this._markTimer.Interval = 2000;
			this._markTimer.SynchronizingObject = this;
			this._markTimer.Elapsed += new System.Timers.ElapsedEventHandler(this._markTimer_Elapsed);
			// 
			// _tgMsgs
			// 
			this._tgMsgs.AllowColumnReorder = true;
			this._tgMsgs.AllowDrop = true;
			this._tgMsgs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(255)))), ((int)(((byte)(244)))));
			this._tgMsgs.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._tgMsgs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this._idCol,
			this._markedCol,
			this._subjectCol,
			this._userNickCol,
			this._ratesCol,
			this._ratesThisCol,
			this._repliesCountCol,
			this._msgDateCol});
			this._tgMsgs.ColumnsOrder = new int[] {
		0,
		1,
		2,
		3,
		4,
		5,
		6,
		7};
			this._tgMsgs.ColumnsWidth = new int[] {
		40,
		26,
		200,
		60,
		60,
		60,
		60,
		60};
			this._tgMsgs.DefaultTextFormat = System.Windows.Forms.TextFormatFlags.GlyphOverhangPadding;
			resources.ApplyResources(this._tgMsgs, "_tgMsgs");
			this._tgMsgs.FullRowSelect = true;
			this._tgMsgs.GridLines = true;
			this._tgMsgs.HideSelection = false;
			this._tgMsgs.Indent = 4;
			this._tgMsgs.Name = "_tgMsgs";
			this._tgMsgs.OwnerDraw = true;
			this._tgMsgs.TreeColumnIndex = 2;
			this._tgMsgs.UseCompatibleStateImageBehavior = false;
			this._tgMsgs.View = System.Windows.Forms.View.Details;
			this._tgMsgs.VirtualMode = true;
			this._tgMsgs.ColumnReordered += new System.Windows.Forms.ColumnReorderedEventHandler(this._tgMsgs_ColumnReordered);
			this._tgMsgs.BeforeMouseDown += new System.EventHandler<Rsdn.Janus.BeforeMouseDownEventArgs>(this._tgMsgs_BeforeMouseDown);
			this._tgMsgs.MouseDown += new System.Windows.Forms.MouseEventHandler(this._tgMsgs_MouseDown);
			this._tgMsgs.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this._tgMsgs_MouseDoubleClick);
			this._tgMsgs.MouseUp += new System.Windows.Forms.MouseEventHandler(this._tgMsgs_MouseUp);
			this._tgMsgs.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this._tgMsgs_ColumnClick);
			this._tgMsgs.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this._tgMsgs_ColumnWidthChanged);
			this._tgMsgs.AfterActivateNode += new Rsdn.TreeGrid.AfterActivateNodeHandler(this._tgMsgs_AfterActivateNode);
			this._tgMsgs.MouseMove += new System.Windows.Forms.MouseEventHandler(this._tgMsgs_MouseMove);
			// 
			// _idCol
			// 
			resources.ApplyResources(this._idCol, "_idCol");
			// 
			// _markedCol
			// 
			resources.ApplyResources(this._markedCol, "_markedCol");
			// 
			// _subjectCol
			// 
			resources.ApplyResources(this._subjectCol, "_subjectCol");
			// 
			// _userNickCol
			// 
			resources.ApplyResources(this._userNickCol, "_userNickCol");
			// 
			// _ratesCol
			// 
			resources.ApplyResources(this._ratesCol, "_ratesCol");
			// 
			// _ratesThisCol
			// 
			resources.ApplyResources(this._ratesThisCol, "_ratesThisCol");
			// 
			// _repliesCountCol
			// 
			resources.ApplyResources(this._repliesCountCol, "_repliesCountCol");
			// 
			// _msgDateCol
			// 
			resources.ApplyResources(this._msgDateCol, "_msgDateCol");
			// 
			// _filterPanel
			// 
			this._filterPanel.BackColor = System.Drawing.SystemColors.Control;
			this._filterPanel.Controls.Add(this._btnResetFilter);
			this._filterPanel.Controls.Add(this._btnHideFilterPanel);
			this._filterPanel.Controls.Add(this._tbxFilter);
			resources.ApplyResources(this._filterPanel, "_filterPanel");
			this._filterPanel.Name = "_filterPanel";
			// 
			// _btnHideFilterPanel
			// 
			resources.ApplyResources(this._btnHideFilterPanel, "_btnHideFilterPanel");
			this._btnHideFilterPanel.ForeColor = System.Drawing.SystemColors.ControlText;
			this._btnHideFilterPanel.Name = "_btnHideFilterPanel";
			this._btnHideFilterPanel.UseVisualStyleBackColor = true;
			this._btnHideFilterPanel.Click += new System.EventHandler(this._btnHideFilterPanel_Click);
			// 
			// _tbxFilter
			// 
			resources.ApplyResources(this._tbxFilter, "_tbxFilter");
			this._tbxFilter.Name = "_tbxFilter";
			this._tbxFilter.KeyUp += new System.Windows.Forms.KeyEventHandler(this._tbxFilter_KeyUp);
			// 
			// _topPanel
			// 
			this._topPanel.Controls.Add(this._tgMsgs);
			this._topPanel.Controls.Add(this._filterPanel);
			resources.ApplyResources(this._topPanel, "_topPanel");
			this._topPanel.Name = "_topPanel";
			// 
			// _bottomPanel
			// 
			resources.ApplyResources(this._bottomPanel, "_bottomPanel");
			this._bottomPanel.Name = "_bottomPanel";
			// 
			// _splitter
			// 
			resources.ApplyResources(this._splitter, "_splitter");
			this._splitter.Name = "_splitter";
			this._splitter.TabStop = false;
			this._splitter.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this._splitter_SplitterMoved);
			// 
			// ForumDummyForm
			// 
			this.Controls.Add(this._bottomPanel);
			this.Controls.Add(this._splitter);
			this.Controls.Add(this._topPanel);
			this.Name = "ForumDummyForm";
			resources.ApplyResources(this, "$this");
			((System.ComponentModel.ISupportInitialize)(this._markTimer)).EndInit();
			this._filterPanel.ResumeLayout(false);
			this._filterPanel.PerformLayout();
			this._topPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.ToolTip _toolTip;
		private System.Windows.Forms.ContextMenuStrip _contextMenuStrip;
		private System.Windows.Forms.SaveFileDialog _saveMessageDialog;
		private System.Timers.Timer _markTimer;
		private Rsdn.Janus.JanusGrid _tgMsgs;
		private System.Windows.Forms.ColumnHeader _idCol;
		private System.Windows.Forms.ColumnHeader _markedCol;
		private System.Windows.Forms.ColumnHeader _subjectCol;
		private System.Windows.Forms.ColumnHeader _userNickCol;
		private System.Windows.Forms.ColumnHeader _ratesCol;
		private System.Windows.Forms.ColumnHeader _ratesThisCol;
		private System.Windows.Forms.ColumnHeader _repliesCountCol;
		private System.Windows.Forms.ColumnHeader _msgDateCol;
		private Rsdn.Janus.Framework.FramePanel _filterPanel;
		private System.Windows.Forms.Button _btnResetFilter;
		private System.Windows.Forms.Button _btnHideFilterPanel;
		private System.Windows.Forms.TextBox _tbxFilter;
		private System.Windows.Forms.Splitter _splitter;
		private System.Windows.Forms.Panel _bottomPanel;
		private System.Windows.Forms.Panel _topPanel;
	}
}