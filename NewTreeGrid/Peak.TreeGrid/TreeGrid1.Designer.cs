namespace Peak.TreeGrid
{
	partial class TreeGrid1
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

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.realGrid = new System.Windows.Forms.DataGridView();
			((System.ComponentModel.ISupportInitialize)(this.realGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// realGrid
			// 
			this.realGrid.AllowUserToAddRows = false;
			this.realGrid.AllowUserToDeleteRows = false;
			this.realGrid.AllowUserToOrderColumns = true;
			this.realGrid.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
			this.realGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this.realGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.realGrid.CausesValidation = false;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.realGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.realGrid.ColumnHeadersHeight = 25;
			this.realGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.realGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.realGrid.Location = new System.Drawing.Point(0, 0);
			this.realGrid.Name = "realGrid";
			this.realGrid.ReadOnly = true;
			this.realGrid.RowHeadersVisible = false;
			this.realGrid.RowHeadersWidth = 4;
			this.realGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.realGrid.Size = new System.Drawing.Size(444, 255);
			this.realGrid.StandardTab = true;
			this.realGrid.TabIndex = 0;
			this.realGrid.VirtualMode = true;
			this.realGrid.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.realGrid_CellMouseClick);
			this.realGrid.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.realGrid_CellPainting);
			// 
			// TreeGrid1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.realGrid);
			this.Name = "TreeGrid1";
			this.Size = new System.Drawing.Size(444, 255);
			((System.ComponentModel.ISupportInitialize)(this.realGrid)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		protected internal System.Windows.Forms.DataGridView realGrid;

	}
}
