namespace Rsdn.LocUtil
{
	partial class CultureManagerForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CultureManagerForm));
			this._okButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			this._cultureListGroup = new System.Windows.Forms.GroupBox();
			this._culturesList = new System.Windows.Forms.ListView();
			this._nameCol = new System.Windows.Forms.ColumnHeader();
			this._fullNameCol = new System.Windows.Forms.ColumnHeader();
			this._images = new System.Windows.Forms.ImageList(this.components);
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this._addButton = new System.Windows.Forms.ToolStripButton();
			this._removeButton = new System.Windows.Forms.ToolStripButton();
			this._cultureListGroup.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _okButton
			// 
			this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._okButton.Location = new System.Drawing.Point(170, 316);
			this._okButton.Name = "_okButton";
			this._okButton.Size = new System.Drawing.Size(75, 23);
			this._okButton.TabIndex = 0;
			this._okButton.Text = "OK";
			this._okButton.UseVisualStyleBackColor = true;
			// 
			// _cancelButton
			// 
			this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Location = new System.Drawing.Point(251, 316);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new System.Drawing.Size(75, 23);
			this._cancelButton.TabIndex = 1;
			this._cancelButton.Text = "Cancel";
			this._cancelButton.UseVisualStyleBackColor = true;
			// 
			// _cultureListGroup
			// 
			this._cultureListGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this._cultureListGroup.Controls.Add(this._culturesList);
			this._cultureListGroup.Controls.Add(this.toolStrip1);
			this._cultureListGroup.Location = new System.Drawing.Point(12, 12);
			this._cultureListGroup.Name = "_cultureListGroup";
			this._cultureListGroup.Size = new System.Drawing.Size(314, 298);
			this._cultureListGroup.TabIndex = 2;
			this._cultureListGroup.TabStop = false;
			this._cultureListGroup.Text = "Cultures";
			// 
			// _culturesList
			// 
			this._culturesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this._nameCol,
			this._fullNameCol});
			this._culturesList.Dock = System.Windows.Forms.DockStyle.Fill;
			this._culturesList.FullRowSelect = true;
			this._culturesList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this._culturesList.HideSelection = false;
			this._culturesList.LargeImageList = this._images;
			this._culturesList.Location = new System.Drawing.Point(3, 41);
			this._culturesList.Name = "_culturesList";
			this._culturesList.Size = new System.Drawing.Size(308, 254);
			this._culturesList.SmallImageList = this._images;
			this._culturesList.TabIndex = 1;
			this._culturesList.UseCompatibleStateImageBehavior = false;
			this._culturesList.View = System.Windows.Forms.View.Details;
			this._culturesList.VirtualMode = true;
			this._culturesList.VirtualItemsSelectionRangeChanged += new System.Windows.Forms.ListViewVirtualItemsSelectionRangeChangedEventHandler(this._culturesList_VirtualItemsSelectionRangeChanged);
			this._culturesList.SelectedIndexChanged += new System.EventHandler(this._culturesList_SelectedIndexChanged);
			this._culturesList.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this._culturesList_RetrieveVirtualItem);
			// 
			// _nameCol
			// 
			this._nameCol.Text = "Name";
			this._nameCol.Width = 76;
			// 
			// _fullNameCol
			// 
			this._fullNameCol.Text = "FullName";
			this._fullNameCol.Width = 202;
			// 
			// _images
			// 
			this._images.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_images.ImageStream")));
			this._images.TransparentColor = System.Drawing.Color.Transparent;
			this._images.Images.SetKeyName(0, "locale");
			this._images.Images.SetKeyName(1, "SubLocale.png");
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this._addButton,
			this._removeButton});
			this.toolStrip1.Location = new System.Drawing.Point(3, 16);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(308, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// _addButton
			// 
			this._addButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._addButton.Image = global::Rsdn.LocUtil.Properties.Resources.Add;
			this._addButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._addButton.Name = "_addButton";
			this._addButton.Size = new System.Drawing.Size(23, 22);
			this._addButton.Text = "Add Culture";
			this._addButton.Click += new System.EventHandler(this._addButton_Click);
			// 
			// _removeButton
			// 
			this._removeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._removeButton.Enabled = false;
			this._removeButton.Image = global::Rsdn.LocUtil.Properties.Resources.Remove;
			this._removeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._removeButton.Name = "_removeButton";
			this._removeButton.Size = new System.Drawing.Size(23, 22);
			this._removeButton.Text = "Remove Culture";
			this._removeButton.Click += new System.EventHandler(this._removeButton_Click);
			// 
			// CultureManagerForm
			// 
			this.AcceptButton = this._okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._cancelButton;
			this.ClientSize = new System.Drawing.Size(338, 351);
			this.Controls.Add(this._cultureListGroup);
			this.Controls.Add(this._cancelButton);
			this.Controls.Add(this._okButton);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CultureManagerForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Manage Cultures";
			this._cultureListGroup.ResumeLayout(false);
			this._cultureListGroup.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.Button _okButton;
		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.GroupBox _cultureListGroup;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton _addButton;
		private System.Windows.Forms.ToolStripButton _removeButton;
		private System.Windows.Forms.ListView _culturesList;
		private System.Windows.Forms.ColumnHeader _nameCol;
		private System.Windows.Forms.ColumnHeader _fullNameCol;
		private System.Windows.Forms.ImageList _images;
	}
}