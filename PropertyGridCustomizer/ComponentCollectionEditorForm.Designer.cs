namespace Rsdn.Janus
{
	internal partial class ComponentCollectionEditorForm
	{
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
				if (components != null)
					components.Dispose();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComponentCollectionEditorForm));
			this._ItemsList = new System.Windows.Forms.ListView();
			this._columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._ItemListImages = new System.Windows.Forms.ImageList(this.components);
			this._OkButton = new System.Windows.Forms.Button();
			this._CancelButton = new System.Windows.Forms.Button();
			this._AddItemButton = new System.Windows.Forms.Button();
			this._DelItemButton = new System.Windows.Forms.Button();
			this._ItemPropertyGrid = new System.Windows.Forms.PropertyGrid();
			this._panel1 = new System.Windows.Forms.Panel();
			this._splitter1 = new System.Windows.Forms.Splitter();
			this._panel2 = new System.Windows.Forms.Panel();
			this._panel1.SuspendLayout();
			this._panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// _ItemsList
			// 
			resources.ApplyResources(this._ItemsList, "_ItemsList");
			this._ItemsList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(255)))), ((int)(((byte)(224)))));
			this._ItemsList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._ItemsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this._columnHeader1});
			this._ItemsList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this._ItemsList.Name = "_ItemsList";
			this._ItemsList.SmallImageList = this._ItemListImages;
			this._ItemsList.UseCompatibleStateImageBehavior = false;
			this._ItemsList.View = System.Windows.Forms.View.Details;
			this._ItemsList.SelectedIndexChanged += new System.EventHandler(this._ItemsList_SelectedIndexChanged);
			// 
			// _columnHeader1
			// 
			resources.ApplyResources(this._columnHeader1, "_columnHeader1");
			// 
			// _ItemListImages
			// 
			this._ItemListImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_ItemListImages.ImageStream")));
			this._ItemListImages.TransparentColor = System.Drawing.Color.Transparent;
			this._ItemListImages.Images.SetKeyName(0, "");
			// 
			// _OkButton
			// 
			resources.ApplyResources(this._OkButton, "_OkButton");
			this._OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._OkButton.Name = "_OkButton";
			this._OkButton.Click += new System.EventHandler(this._OkButton_Click);
			// 
			// _CancelButton
			// 
			resources.ApplyResources(this._CancelButton, "_CancelButton");
			this._CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._CancelButton.Name = "_CancelButton";
			// 
			// _AddItemButton
			// 
			resources.ApplyResources(this._AddItemButton, "_AddItemButton");
			this._AddItemButton.Name = "_AddItemButton";
			this._AddItemButton.Click += new System.EventHandler(this._AddItemButton_Click);
			// 
			// _DelItemButton
			// 
			resources.ApplyResources(this._DelItemButton, "_DelItemButton");
			this._DelItemButton.Name = "_DelItemButton";
			this._DelItemButton.Click += new System.EventHandler(this._DelItemButton_Click);
			// 
			// _ItemPropertyGrid
			// 
			this._ItemPropertyGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(244)))));
			this._ItemPropertyGrid.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(244)))));
			resources.ApplyResources(this._ItemPropertyGrid, "_ItemPropertyGrid");
			this._ItemPropertyGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(244)))));
			this._ItemPropertyGrid.Name = "_ItemPropertyGrid";
			this._ItemPropertyGrid.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(255)))));
			this._ItemPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this._ItemPropertyGrid_PropertyValueChanged);
			// 
			// _panel1
			// 
			resources.ApplyResources(this._panel1, "_panel1");
			this._panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panel1.Controls.Add(this._ItemPropertyGrid);
			this._panel1.Controls.Add(this._splitter1);
			this._panel1.Controls.Add(this._panel2);
			this._panel1.Name = "_panel1";
			// 
			// _splitter1
			// 
			this._splitter1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			resources.ApplyResources(this._splitter1, "_splitter1");
			this._splitter1.Name = "_splitter1";
			this._splitter1.TabStop = false;
			// 
			// _panel2
			// 
			this._panel2.Controls.Add(this._ItemsList);
			this._panel2.Controls.Add(this._DelItemButton);
			this._panel2.Controls.Add(this._AddItemButton);
			resources.ApplyResources(this._panel2, "_panel2");
			this._panel2.Name = "_panel2";
			// 
			// ComponentCollectionEditorForm
			// 
			resources.ApplyResources(this, "$this");
			this.CancelButton = this._CancelButton;
			this.Controls.Add(this._panel1);
			this.Controls.Add(this._CancelButton);
			this.Controls.Add(this._OkButton);
			this.Name = "ComponentCollectionEditorForm";
			this._panel1.ResumeLayout(false);
			this._panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.Button _AddItemButton;
		private System.Windows.Forms.Button _CancelButton;
		private System.Windows.Forms.Button _OkButton;
		private System.Windows.Forms.Button _DelItemButton;
		private System.Windows.Forms.ImageList _ItemListImages;
		private System.Windows.Forms.PropertyGrid _ItemPropertyGrid;
		private System.Windows.Forms.ListView _ItemsList;
		private System.Windows.Forms.ColumnHeader _columnHeader1;
		private System.Windows.Forms.Panel _panel1;
		private System.Windows.Forms.Panel _panel2;
		private System.Windows.Forms.Splitter _splitter1;
	}
}