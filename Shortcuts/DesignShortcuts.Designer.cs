namespace Rsdn.Shortcuts
{
	partial class DesignShortcuts
	{
		private System.ComponentModel.IContainer components;

		#region Designer generated code
		protected override void Dispose(bool disposing)
		{
			if (disposing)
				if (components != null)
					components.Dispose();

			base.Dispose(disposing);
		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DesignShortcuts));
			this._treeView = new System.Windows.Forms.TreeView();
			this._imageList = new System.Windows.Forms.ImageList(this.components);
			this._okButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			this._refreshButton = new System.Windows.Forms.Button();
			this._applyButton = new System.Windows.Forms.Button();
			this._panel2 = new System.Windows.Forms.Panel();
			this._panel3 = new System.Windows.Forms.Panel();
			this._panel1 = new System.Windows.Forms.Panel();
			this._listBox = new Rsdn.Shortcuts.ListBoxShortcuts();
			this._splitter2 = new System.Windows.Forms.Splitter();
			this._panel4 = new System.Windows.Forms.Panel();
			this._panel5 = new System.Windows.Forms.Panel();
			this._longNameLabel = new System.Windows.Forms.Label();
			this._shortNameLabel = new System.Windows.Forms.Label();
			this._containerPanel = new System.Windows.Forms.Panel();
			this._splitter1 = new System.Windows.Forms.Splitter();
			this._toolbar = new System.Windows.Forms.ToolBar();
			this._toolBarButton1 = new System.Windows.Forms.ToolBarButton();
			this._toolBarButton2 = new System.Windows.Forms.ToolBarButton();
			this._panel2.SuspendLayout();
			this._panel3.SuspendLayout();
			this._panel1.SuspendLayout();
			this._panel4.SuspendLayout();
			this._panel5.SuspendLayout();
			this._containerPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// _treeView
			// 
			this._treeView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(255)))), ((int)(((byte)(224)))));
			this._treeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._treeView.Cursor = System.Windows.Forms.Cursors.Default;
			resources.ApplyResources(this._treeView, "_treeView");
			this._treeView.ImageList = this._imageList;
			this._treeView.Name = "_treeView";
			this._treeView.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeSelect);
			this._treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			// 
			// _imageList
			// 
			this._imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_imageList.ImageStream")));
			this._imageList.TransparentColor = System.Drawing.Color.Transparent;
			this._imageList.Images.SetKeyName(0, "");
			this._imageList.Images.SetKeyName(1, "");
			// 
			// _okButton
			// 
			resources.ApplyResources(this._okButton, "_okButton");
			this._okButton.Name = "_okButton";
			this._okButton.Click += new System.EventHandler(this._okButton_Click);
			// 
			// _cancelButton
			// 
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this._cancelButton, "_cancelButton");
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Click += new System.EventHandler(this._cancelButton_Click);
			// 
			// _refreshButton
			// 
			resources.ApplyResources(this._refreshButton, "_refreshButton");
			this._refreshButton.Name = "_refreshButton";
			this._refreshButton.Click += new System.EventHandler(this._refreshButton_Click);
			// 
			// _applyButton
			// 
			resources.ApplyResources(this._applyButton, "_applyButton");
			this._applyButton.Name = "_applyButton";
			this._applyButton.Click += new System.EventHandler(this._applyButton_Click);
			// 
			// _panel2
			// 
			this._panel2.Controls.Add(this._treeView);
			resources.ApplyResources(this._panel2, "_panel2");
			this._panel2.Name = "_panel2";
			// 
			// _panel3
			// 
			this._panel3.Controls.Add(this._okButton);
			this._panel3.Controls.Add(this._cancelButton);
			this._panel3.Controls.Add(this._applyButton);
			this._panel3.Controls.Add(this._refreshButton);
			resources.ApplyResources(this._panel3, "_panel3");
			this._panel3.Name = "_panel3";
			// 
			// _panel1
			// 
			this._panel1.Controls.Add(this._listBox);
			this._panel1.Controls.Add(this._splitter2);
			this._panel1.Controls.Add(this._panel4);
			resources.ApplyResources(this._panel1, "_panel1");
			this._panel1.Name = "_panel1";
			// 
			// _listBox
			// 
			this._listBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
			this._listBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			resources.ApplyResources(this._listBox, "_listBox");
			this._listBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this._listBox.Name = "_listBox";
			this._listBox.SelectedValueChanged += new System.EventHandler(this.listBox1_SelectedValueChanged);
			// 
			// _splitter2
			// 
			resources.ApplyResources(this._splitter2, "_splitter2");
			this._splitter2.Name = "_splitter2";
			this._splitter2.TabStop = false;
			// 
			// _panel4
			// 
			this._panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panel4.Controls.Add(this._panel5);
			resources.ApplyResources(this._panel4, "_panel4");
			this._panel4.Name = "_panel4";
			// 
			// _panel5
			// 
			this._panel5.Controls.Add(this._longNameLabel);
			this._panel5.Controls.Add(this._shortNameLabel);
			resources.ApplyResources(this._panel5, "_panel5");
			this._panel5.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this._panel5.Name = "_panel5";
			// 
			// _longNameLabel
			// 
			resources.ApplyResources(this._longNameLabel, "_longNameLabel");
			this._longNameLabel.Name = "_longNameLabel";
			// 
			// _shortNameLabel
			// 
			resources.ApplyResources(this._shortNameLabel, "_shortNameLabel");
			this._shortNameLabel.Name = "_shortNameLabel";
			// 
			// _containerPanel
			// 
			this._containerPanel.Controls.Add(this._panel1);
			this._containerPanel.Controls.Add(this._splitter1);
			this._containerPanel.Controls.Add(this._panel2);
			this._containerPanel.Controls.Add(this._toolbar);
			resources.ApplyResources(this._containerPanel, "_containerPanel");
			this._containerPanel.Name = "_containerPanel";
			// 
			// _splitter1
			// 
			resources.ApplyResources(this._splitter1, "_splitter1");
			this._splitter1.Name = "_splitter1";
			this._splitter1.TabStop = false;
			// 
			// _toolbar
			// 
			this._toolbar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this._toolBarButton1,
            this._toolBarButton2});
			resources.ApplyResources(this._toolbar, "_toolbar");
			this._toolbar.Name = "_toolbar";
			this._toolbar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this._toolbar_ButtonClick);
			// 
			// _toolBarButton1
			// 
			this._toolBarButton1.Name = "_toolBarButton1";
			this._toolBarButton1.Tag = "SaveAsPreset";
			resources.ApplyResources(this._toolBarButton1, "_toolBarButton1");
			// 
			// _toolBarButton2
			// 
			this._toolBarButton2.Name = "_toolBarButton2";
			this._toolBarButton2.Tag = "LoadPreset";
			resources.ApplyResources(this._toolBarButton2, "_toolBarButton2");
			// 
			// DesignShortcuts
			// 
			this.AcceptButton = this._okButton;
			this.CancelButton = this._cancelButton;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this._containerPanel);
			this.Controls.Add(this._panel3);
			this.MaximizeBox = false;
			this.Name = "DesignShortcuts";
			this._panel2.ResumeLayout(false);
			this._panel3.ResumeLayout(false);
			this._panel1.ResumeLayout(false);
			this._panel4.ResumeLayout(false);
			this._panel5.ResumeLayout(false);
			this._containerPanel.ResumeLayout(false);
			this._containerPanel.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private Rsdn.Shortcuts.ListBoxShortcuts _listBox;
		private System.Windows.Forms.Button _applyButton;
		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.Panel _containerPanel;
		private System.Windows.Forms.ImageList _imageList;
		private System.Windows.Forms.Label _longNameLabel;
		private System.Windows.Forms.Button _okButton;
		private System.Windows.Forms.Panel _panel1;
		private System.Windows.Forms.Panel _panel2;
		private System.Windows.Forms.Panel _panel3;
		private System.Windows.Forms.Panel _panel4;
		private System.Windows.Forms.Panel _panel5;
		private System.Windows.Forms.Button _refreshButton;
		private System.Windows.Forms.Label _shortNameLabel;
		private System.Windows.Forms.Splitter _splitter1;
		private System.Windows.Forms.Splitter _splitter2;
		private System.Windows.Forms.ToolBar _toolbar;
		private System.Windows.Forms.ToolBarButton _toolBarButton1;
		private System.Windows.Forms.ToolBarButton _toolBarButton2;
		private System.Windows.Forms.TreeView _treeView;
	}
}