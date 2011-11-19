namespace Rsdn.Shortcuts
{
	internal partial class EditorNodes
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorNodes));
			this._treeView = new System.Windows.Forms.TreeView();
			this._addButton = new System.Windows.Forms.Button();
			this._deleteButton = new System.Windows.Forms.Button();
			this._groupBox = new System.Windows.Forms.GroupBox();
			this._descriptionBox = new System.Windows.Forms.TextBox();
			this._nameBox = new System.Windows.Forms.TextBox();
			this._label2 = new System.Windows.Forms.Label();
			this._label1 = new System.Windows.Forms.Label();
			this._okButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			this._groupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// _treeView
			// 
			resources.ApplyResources(this._treeView, "_treeView");
			this._treeView.HideSelection = false;
			this._treeView.LabelEdit = true;
			this._treeView.Name = "_treeView";
			this._treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._treeView_AfterSelect);
			// 
			// _addButton
			// 
			resources.ApplyResources(this._addButton, "_addButton");
			this._addButton.Name = "_addButton";
			this._addButton.Click += new System.EventHandler(this._addButton_Click);
			// 
			// _deleteButton
			// 
			resources.ApplyResources(this._deleteButton, "_deleteButton");
			this._deleteButton.Name = "_deleteButton";
			this._deleteButton.Click += new System.EventHandler(this._deleteButton_Click);
			// 
			// _groupBox
			// 
			resources.ApplyResources(this._groupBox, "_groupBox");
			this._groupBox.Controls.Add(this._descriptionBox);
			this._groupBox.Controls.Add(this._nameBox);
			this._groupBox.Controls.Add(this._treeView);
			this._groupBox.Controls.Add(this._deleteButton);
			this._groupBox.Controls.Add(this._label2);
			this._groupBox.Controls.Add(this._addButton);
			this._groupBox.Controls.Add(this._label1);
			this._groupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._groupBox.Name = "_groupBox";
			this._groupBox.TabStop = false;
			// 
			// _descriptionBox
			// 
			resources.ApplyResources(this._descriptionBox, "_descriptionBox");
			this._descriptionBox.Name = "_descriptionBox";
			this._descriptionBox.TextChanged += new System.EventHandler(this._descriptionBox_TextChanged);
			// 
			// _nameBox
			// 
			resources.ApplyResources(this._nameBox, "_nameBox");
			this._nameBox.Name = "_nameBox";
			this._nameBox.TextChanged += new System.EventHandler(this._nameBox_TextChanged);
			// 
			// _label2
			// 
			resources.ApplyResources(this._label2, "_label2");
			this._label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._label2.Name = "_label2";
			// 
			// _label1
			// 
			resources.ApplyResources(this._label1, "_label1");
			this._label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._label1.Name = "_label1";
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
			// EditorNodes
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this._cancelButton);
			this.Controls.Add(this._okButton);
			this.Controls.Add(this._groupBox);
			this.Name = "EditorNodes";
			this.Load += new System.EventHandler(this.EditorNodes_Load);
			this._groupBox.ResumeLayout(false);
			this._groupBox.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.Button _addButton;
		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.TreeNode _currentNode;
		private System.Windows.Forms.Button _deleteButton;
		private System.Windows.Forms.TextBox _descriptionBox;
		private System.Windows.Forms.GroupBox _groupBox;
		private System.Windows.Forms.Label _label1;
		private System.Windows.Forms.Label _label2;
		private System.Windows.Forms.TextBox _nameBox;
		private System.Windows.Forms.Button _okButton;
		private System.Windows.Forms.TreeNode _root;
		private System.Windows.Forms.TreeView _treeView;
	}
}