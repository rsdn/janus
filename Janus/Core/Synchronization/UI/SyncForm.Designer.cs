namespace Rsdn.Janus
{
	public partial class SyncForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components;

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
			System.Windows.Forms.GroupBox _syncTasksGroup;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SyncForm));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this._syncTaskList = new System.Windows.Forms.ListView();
			this._nameCol = new System.Windows.Forms.ColumnHeader();
			this._startTimeCol = new System.Windows.Forms.ColumnHeader();
			this._durationCol = new System.Windows.Forms.ColumnHeader();
			this._statusCol = new System.Windows.Forms.ColumnHeader();
			this._splitContainer = new System.Windows.Forms.SplitContainer();
			this._compressPicture = new System.Windows.Forms.PictureBox();
			this._progressLabel = new System.Windows.Forms.Label();
			this._progressPie = new Rsdn.Janus.Framework.ProgressPieControl();
			this._errorsGroup = new System.Windows.Forms.GroupBox();
			this._errorsGrid = new System.Windows.Forms.DataGridView();
			this._errTypeCol = new System.Windows.Forms.DataGridViewImageColumn();
			this._errTaskCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._errTextCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._iconsImages = new System.Windows.Forms.ImageList(this.components);
			this._pinCheck = new System.Windows.Forms.CheckBox();
			this._cancelButton = new System.Windows.Forms.Button();
			this._refreshDurationTimer = new System.Windows.Forms.Timer(this.components);
			this._copyButton = new System.Windows.Forms.Button();
			_syncTasksGroup = new System.Windows.Forms.GroupBox();
			_syncTasksGroup.SuspendLayout();
			this._splitContainer.Panel1.SuspendLayout();
			this._splitContainer.Panel2.SuspendLayout();
			this._splitContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._compressPicture)).BeginInit();
			this._errorsGroup.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._errorsGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// _syncTasksGroup
			// 
			resources.ApplyResources(_syncTasksGroup, "_syncTasksGroup");
			_syncTasksGroup.Controls.Add(this._syncTaskList);
			_syncTasksGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
			_syncTasksGroup.Name = "_syncTasksGroup";
			_syncTasksGroup.TabStop = false;
			// 
			// _syncTaskList
			// 
			this._syncTaskList.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._syncTaskList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this._nameCol,
			this._startTimeCol,
			this._durationCol,
			this._statusCol});
			resources.ApplyResources(this._syncTaskList, "_syncTaskList");
			this._syncTaskList.FullRowSelect = true;
			this._syncTaskList.Name = "_syncTaskList";
			this._syncTaskList.UseCompatibleStateImageBehavior = false;
			this._syncTaskList.View = System.Windows.Forms.View.Details;
			this._syncTaskList.VirtualMode = true;
			this._syncTaskList.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.SyncTaskListRetrieveVirtualItem);
			// 
			// _nameCol
			// 
			resources.ApplyResources(this._nameCol, "_nameCol");
			// 
			// _startTimeCol
			// 
			resources.ApplyResources(this._startTimeCol, "_startTimeCol");
			// 
			// _durationCol
			// 
			resources.ApplyResources(this._durationCol, "_durationCol");
			// 
			// _statusCol
			// 
			resources.ApplyResources(this._statusCol, "_statusCol");
			// 
			// _splitContainer
			// 
			resources.ApplyResources(this._splitContainer, "_splitContainer");
			this._splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this._splitContainer.Name = "_splitContainer";
			// 
			// _splitContainer.Panel1
			// 
			this._splitContainer.Panel1.Controls.Add(this._compressPicture);
			this._splitContainer.Panel1.Controls.Add(this._progressLabel);
			this._splitContainer.Panel1.Controls.Add(_syncTasksGroup);
			this._splitContainer.Panel1.Controls.Add(this._progressPie);
			// 
			// _splitContainer.Panel2
			// 
			this._splitContainer.Panel2.Controls.Add(this._errorsGroup);
			// 
			// _compressPicture
			// 
			resources.ApplyResources(this._compressPicture, "_compressPicture");
			this._compressPicture.Name = "_compressPicture";
			this._compressPicture.TabStop = false;
			// 
			// _progressLabel
			// 
			resources.ApplyResources(this._progressLabel, "_progressLabel");
			this._progressLabel.Name = "_progressLabel";
			// 
			// _progressPie
			// 
			resources.ApplyResources(this._progressPie, "_progressPie");
			this._progressPie.Name = "_progressPie";
			this._progressPie.PieBackColor = System.Drawing.Color.White;
			this._progressPie.SectorCount = 24;
			// 
			// _errorsGroup
			// 
			this._errorsGroup.Controls.Add(this._errorsGrid);
			resources.ApplyResources(this._errorsGroup, "_errorsGroup");
			this._errorsGroup.Name = "_errorsGroup";
			this._errorsGroup.TabStop = false;
			// 
			// _errorsGrid
			// 
			this._errorsGrid.AllowUserToAddRows = false;
			this._errorsGrid.AllowUserToDeleteRows = false;
			this._errorsGrid.AllowUserToOrderColumns = true;
			this._errorsGrid.AllowUserToResizeColumns = false;
			this._errorsGrid.AllowUserToResizeRows = false;
			this._errorsGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this._errorsGrid.BackgroundColor = System.Drawing.SystemColors.Window;
			this._errorsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._errorsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this._errorsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
			this._errTypeCol,
			this._errTaskCol,
			this._errTextCol});
			resources.ApplyResources(this._errorsGrid, "_errorsGrid");
			this._errorsGrid.Name = "_errorsGrid";
			this._errorsGrid.RowHeadersVisible = false;
			this._errorsGrid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this._errorsGrid_CellFormatting);
			// 
			// _errTypeCol
			// 
			this._errTypeCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this._errTypeCol.DataPropertyName = "Type";
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
			dataGridViewCellStyle1.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle1.NullValue")));
			this._errTypeCol.DefaultCellStyle = dataGridViewCellStyle1;
			this._errTypeCol.Frozen = true;
			resources.ApplyResources(this._errTypeCol, "_errTypeCol");
			this._errTypeCol.Name = "_errTypeCol";
			this._errTypeCol.ReadOnly = true;
			this._errTypeCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			// 
			// _errTaskCol
			// 
			this._errTaskCol.DataPropertyName = "TaskName";
			resources.ApplyResources(this._errTaskCol, "_errTaskCol");
			this._errTaskCol.Name = "_errTaskCol";
			this._errTaskCol.ReadOnly = true;
			// 
			// _errTextCol
			// 
			this._errTextCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this._errTextCol.DataPropertyName = "Text";
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this._errTextCol.DefaultCellStyle = dataGridViewCellStyle2;
			resources.ApplyResources(this._errTextCol, "_errTextCol");
			this._errTextCol.Name = "_errTextCol";
			this._errTextCol.ReadOnly = true;
			this._errTextCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// _iconsImages
			// 
			this._iconsImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_iconsImages.ImageStream")));
			this._iconsImages.TransparentColor = System.Drawing.Color.Transparent;
			this._iconsImages.Images.SetKeyName(0, "");
			this._iconsImages.Images.SetKeyName(1, "");
			this._iconsImages.Images.SetKeyName(2, "");
			// 
			// _pinCheck
			// 
			resources.ApplyResources(this._pinCheck, "_pinCheck");
			this._pinCheck.Name = "_pinCheck";
			this._pinCheck.UseVisualStyleBackColor = true;
			this._pinCheck.CheckedChanged += new System.EventHandler(this.PinCheckCheckedChanged);
			// 
			// _cancelButton
			// 
			resources.ApplyResources(this._cancelButton, "_cancelButton");
			this._cancelButton.BackColor = System.Drawing.SystemColors.Control;
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Tag = "";
			this._cancelButton.UseVisualStyleBackColor = false;
			this._cancelButton.Click += new System.EventHandler(this._cancelButton_Click);
			// 
			// _refreshDurationTimer
			// 
			this._refreshDurationTimer.Interval = 1000;
			this._refreshDurationTimer.Tick += new System.EventHandler(this.RefreshDurationTimer_Tick);
			// 
			// _copyButton
			// 
			resources.ApplyResources(this._copyButton, "_copyButton");
			this._copyButton.Name = "_copyButton";
			this._copyButton.UseVisualStyleBackColor = true;
			this._copyButton.Click += new System.EventHandler(this._copyButton_Click);
			// 
			// SyncForm
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this._copyButton);
			this.Controls.Add(this._pinCheck);
			this.Controls.Add(this._splitContainer);
			this.Controls.Add(this._cancelButton);
			this.DoubleBuffered = true;
			this.MaximizeBox = false;
			this.Name = "SyncForm";
			this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
			_syncTasksGroup.ResumeLayout(false);
			this._splitContainer.Panel1.ResumeLayout(false);
			this._splitContainer.Panel2.ResumeLayout(false);
			this._splitContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._compressPicture)).EndInit();
			this._errorsGroup.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._errorsGrid)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.ImageList _iconsImages;
		private Rsdn.Janus.Framework.ProgressPieControl _progressPie;
		private System.Windows.Forms.ListView _syncTaskList;
		private System.Windows.Forms.ColumnHeader _nameCol;
		private System.Windows.Forms.ColumnHeader _startTimeCol;
		private System.Windows.Forms.ColumnHeader _durationCol;
		private System.Windows.Forms.ColumnHeader _statusCol;
		private System.Windows.Forms.Timer _refreshDurationTimer;
		private System.Windows.Forms.CheckBox _pinCheck;
		private System.Windows.Forms.DataGridView _errorsGrid;
		private System.Windows.Forms.GroupBox _errorsGroup;
		private System.Windows.Forms.SplitContainer _splitContainer;
		private System.Windows.Forms.Label _progressLabel;
		private System.Windows.Forms.Button _copyButton;
		private System.Windows.Forms.DataGridViewImageColumn _errTypeCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn _errTaskCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn _errTextCol;
		private System.Windows.Forms.PictureBox _compressPicture;
	}
}
