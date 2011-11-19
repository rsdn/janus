namespace Rsdn.Shortcuts
{
	public partial class SelectPresetForm
	{
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{ }

			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectPresetForm));
			this._lvPresetList = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._okButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			this._removeButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// _lvPresetList
			// 
			this._lvPresetList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
			this._lvPresetList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this._lvPresetList.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("_lvPresetList.Items")))});
			resources.ApplyResources(this._lvPresetList, "_lvPresetList");
			this._lvPresetList.Name = "_lvPresetList";
			this._lvPresetList.UseCompatibleStateImageBehavior = false;
			this._lvPresetList.View = System.Windows.Forms.View.Details;
			this._lvPresetList.SelectedIndexChanged += new System.EventHandler(this._lvPresetList_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			resources.ApplyResources(this.columnHeader1, "columnHeader1");
			// 
			// _okButton
			// 
			this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			resources.ApplyResources(this._okButton, "_okButton");
			this._okButton.Name = "_okButton";
			// 
			// _cancelButton
			// 
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this._cancelButton, "_cancelButton");
			this._cancelButton.Name = "_cancelButton";
			// 
			// _removeButton
			// 
			resources.ApplyResources(this._removeButton, "_removeButton");
			this._removeButton.Name = "_removeButton";
			this._removeButton.Click += new System.EventHandler(this._removeButton_Click);
			// 
			// SelectPresetForm
			// 
			this.AcceptButton = this._okButton;
			this.CancelButton = this._cancelButton;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this._removeButton);
			this.Controls.Add(this._okButton);
			this.Controls.Add(this._lvPresetList);
			this.Controls.Add(this._cancelButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SelectPresetForm";
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.ListView _lvPresetList;
		private System.Windows.Forms.Button _okButton;
		private System.Windows.Forms.Button _removeButton;
		private System.Windows.Forms.ColumnHeader columnHeader1;
	}
}