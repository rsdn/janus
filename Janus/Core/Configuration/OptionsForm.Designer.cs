using System.ComponentModel;
using System.Windows.Forms;

namespace Rsdn.Janus
{
	partial class OptionsForm
	{
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				StyleConfig.StyleChange -= OnStyleChange;

				if (components != null)
					components.Dispose();
			}

			_dialogContainer.RejectChanges();
			_dialogContainer.Dispose();

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
			System.Windows.Forms.FlowLayoutPanel _lytButtons;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
			this._saveButton = new System.Windows.Forms.Button();
			this._okButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			this._pluginsTreeImages = new System.Windows.Forms.ImageList(this.components);
			this._tabControl = new System.Windows.Forms.TabControl();
			this._applicationTab = new System.Windows.Forms.TabPage();
			this._appPropertyGrid = new System.Windows.Forms.PropertyGrid();
			this._hotKeysTab = new System.Windows.Forms.TabPage();
			this._styleTab = new System.Windows.Forms.TabPage();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this._saveSchemeButton = new System.Windows.Forms.Button();
			this._loadSchemeButton = new System.Windows.Forms.Button();
			this._stylePropertyGrid = new System.Windows.Forms.PropertyGrid();
			_lytButtons = new System.Windows.Forms.FlowLayoutPanel();
			_lytButtons.SuspendLayout();
			this._tabControl.SuspendLayout();
			this._applicationTab.SuspendLayout();
			this._styleTab.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _lytButtons
			// 
			resources.ApplyResources(_lytButtons, "_lytButtons");
			_lytButtons.Controls.Add(this._saveButton);
			_lytButtons.Controls.Add(this._okButton);
			_lytButtons.Controls.Add(this._cancelButton);
			_lytButtons.Name = "_lytButtons";
			// 
			// _saveButton
			// 
			resources.ApplyResources(this._saveButton, "_saveButton");
			this._saveButton.Name = "_saveButton";
			this._saveButton.Click += new System.EventHandler(this._saveButton_Click);
			// 
			// _okButton
			// 
			resources.ApplyResources(this._okButton, "_okButton");
			this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._okButton.Name = "_okButton";
			this._okButton.Click += new System.EventHandler(this._okButton_Click);
			// 
			// _cancelButton
			// 
			resources.ApplyResources(this._cancelButton, "_cancelButton");
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Click += new System.EventHandler(this._cancelButton_Click);
			// 
			// _pluginsTreeImages
			// 
			this._pluginsTreeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_pluginsTreeImages.ImageStream")));
			this._pluginsTreeImages.TransparentColor = System.Drawing.Color.Transparent;
			this._pluginsTreeImages.Images.SetKeyName(0, "");
			this._pluginsTreeImages.Images.SetKeyName(1, "");
			// 
			// _tabControl
			// 
			resources.ApplyResources(this._tabControl, "_tabControl");
			this._tabControl.Controls.Add(this._applicationTab);
			this._tabControl.Controls.Add(this._hotKeysTab);
			this._tabControl.Controls.Add(this._styleTab);
			this._tabControl.HotTrack = true;
			this._tabControl.Name = "_tabControl";
			this._tabControl.SelectedIndex = 0;
			// 
			// _applicationTab
			// 
			this._applicationTab.Controls.Add(this._appPropertyGrid);
			resources.ApplyResources(this._applicationTab, "_applicationTab");
			this._applicationTab.Name = "_applicationTab";
			// 
			// _appPropertyGrid
			// 
			this._appPropertyGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(244)))));
			this._appPropertyGrid.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(244)))));
			resources.ApplyResources(this._appPropertyGrid, "_appPropertyGrid");
			this._appPropertyGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(244)))));
			this._appPropertyGrid.Name = "_appPropertyGrid";
			this._appPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
			this._appPropertyGrid.ToolbarVisible = false;
			this._appPropertyGrid.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(255)))));
			this._appPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this._appPropertyGrid_PropertyValueChanged);
			// 
			// _hotKeysTab
			// 
			resources.ApplyResources(this._hotKeysTab, "_hotKeysTab");
			this._hotKeysTab.Name = "_hotKeysTab";
			// 
			// _styleTab
			// 
			this._styleTab.Controls.Add(this.flowLayoutPanel1);
			this._styleTab.Controls.Add(this._stylePropertyGrid);
			resources.ApplyResources(this._styleTab, "_styleTab");
			this._styleTab.Name = "_styleTab";
			// 
			// flowLayoutPanel1
			// 
			resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
			this.flowLayoutPanel1.Controls.Add(this._saveSchemeButton);
			this.flowLayoutPanel1.Controls.Add(this._loadSchemeButton);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			// 
			// _saveSchemeButton
			// 
			resources.ApplyResources(this._saveSchemeButton, "_saveSchemeButton");
			this._saveSchemeButton.Name = "_saveSchemeButton";
			this._saveSchemeButton.Click += new System.EventHandler(this._saveSchemeButton_Click);
			// 
			// _loadSchemeButton
			// 
			resources.ApplyResources(this._loadSchemeButton, "_loadSchemeButton");
			this._loadSchemeButton.Name = "_loadSchemeButton";
			this._loadSchemeButton.Click += new System.EventHandler(this._loadSchemeButton_Click);
			// 
			// _stylePropertyGrid
			// 
			resources.ApplyResources(this._stylePropertyGrid, "_stylePropertyGrid");
			this._stylePropertyGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(244)))));
			this._stylePropertyGrid.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(244)))));
			this._stylePropertyGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(244)))));
			this._stylePropertyGrid.Name = "_stylePropertyGrid";
			this._stylePropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
			this._stylePropertyGrid.ToolbarVisible = false;
			this._stylePropertyGrid.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(255)))));
			// 
			// OptionsForm
			// 
			this.AcceptButton = this._okButton;
			this.CancelButton = this._cancelButton;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(_lytButtons);
			this.Controls.Add(this._tabControl);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptionsForm";
			this.ShowInTaskbar = false;
			_lytButtons.ResumeLayout(false);
			_lytButtons.PerformLayout();
			this._tabControl.ResumeLayout(false);
			this._applicationTab.ResumeLayout(false);
			this._styleTab.ResumeLayout(false);
			this._styleTab.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private IContainer components;
		private Button _cancelButton;
		private Button _loadSchemeButton;
		private Button _okButton;
		private Button _saveButton;
		private Button _saveSchemeButton;
		private TabControl _tabControl;
		private TabPage _applicationTab;
		private TabPage _hotKeysTab;
		private TabPage _styleTab;
		private PropertyGrid _appPropertyGrid;
		private PropertyGrid _stylePropertyGrid;
		private ImageList _pluginsTreeImages;
		private StyleConfig _savedConfig;
		private FlowLayoutPanel flowLayoutPanel1;
	}
}