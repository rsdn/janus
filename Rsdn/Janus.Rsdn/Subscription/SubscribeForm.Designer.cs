namespace Rsdn.Janus
{

	public partial class SubscribeForm
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
			System.Windows.Forms.FlowLayoutPanel _lytButtons;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubscribeForm));
			this._applyButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			this._okButton = new System.Windows.Forms.Button();
			this._forumListView = new System.Windows.Forms.ListView();
			this._typeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._forumColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._descriptionColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._formImages = new System.Windows.Forms.ImageList(this.components);
			this._messageLabel = new System.Windows.Forms.Label();
			this._syncForumsButton = new System.Windows.Forms.Button();
			this._priority = new System.Windows.Forms.NumericUpDown();
			_lytButtons = new System.Windows.Forms.FlowLayoutPanel();
			_lytButtons.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._priority)).BeginInit();
			this.SuspendLayout();
			// 
			// _lytButtons
			// 
			resources.ApplyResources(_lytButtons, "_lytButtons");
			_lytButtons.Controls.Add(this._applyButton);
			_lytButtons.Controls.Add(this._cancelButton);
			_lytButtons.Controls.Add(this._okButton);
			_lytButtons.Name = "_lytButtons";
			// 
			// _applyButton
			// 
			resources.ApplyResources(this._applyButton, "_applyButton");
			this._applyButton.Name = "_applyButton";
			this._applyButton.Click += new System.EventHandler(this.BtnApplyClick);
			// 
			// _cancelButton
			// 
			resources.ApplyResources(this._cancelButton, "_cancelButton");
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Name = "_cancelButton";
			// 
			// _okButton
			// 
			resources.ApplyResources(this._okButton, "_okButton");
			this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._okButton.Name = "_okButton";
			this._okButton.Click += new System.EventHandler(this.BtnOkClick);
			// 
			// _forumListView
			// 
			resources.ApplyResources(this._forumListView, "_forumListView");
			this._forumListView.CheckBoxes = true;
			this._forumListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this._typeColumnHeader,
			this._forumColumnHeader,
			this._descriptionColumnHeader});
			this._forumListView.FullRowSelect = true;
			this._forumListView.GridLines = true;
			this._forumListView.HideSelection = false;
			this._forumListView.Name = "_forumListView";
			this._forumListView.SmallImageList = this._formImages;
			this._forumListView.UseCompatibleStateImageBehavior = false;
			this._forumListView.View = System.Windows.Forms.View.Details;
			this._forumListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ForumListViewColumnClick);
			this._forumListView.SelectedIndexChanged += new System.EventHandler(this.ForumListViewSelectedIndexChanged);
			// 
			// _typeColumnHeader
			// 
			resources.ApplyResources(this._typeColumnHeader, "_typeColumnHeader");
			// 
			// _forumColumnHeader
			// 
			resources.ApplyResources(this._forumColumnHeader, "_forumColumnHeader");
			// 
			// _descriptionColumnHeader
			// 
			resources.ApplyResources(this._descriptionColumnHeader, "_descriptionColumnHeader");
			// 
			// _formImages
			// 
			this._formImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_formImages.ImageStream")));
			this._formImages.TransparentColor = System.Drawing.Color.Transparent;
			this._formImages.Images.SetKeyName(0, "");
			this._formImages.Images.SetKeyName(1, "");
			// 
			// _messageLabel
			// 
			resources.ApplyResources(this._messageLabel, "_messageLabel");
			this._messageLabel.Name = "_messageLabel";
			// 
			// _syncForumsButton
			// 
			resources.ApplyResources(this._syncForumsButton, "_syncForumsButton");
			this._syncForumsButton.Name = "_syncForumsButton";
			this._syncForumsButton.Click += new System.EventHandler(this.BtnSyncForumsClick);
			// 
			// _priority
			// 
			resources.ApplyResources(this._priority, "_priority");
			this._priority.Maximum = new decimal(new int[] {
			9,
			0,
			0,
			0});
			this._priority.Name = "_priority";
			this._priority.ValueChanged += new System.EventHandler(this.PriorityValueChanged);
			// 
			// SubscribeForm
			// 
			this.AcceptButton = this._okButton;
			resources.ApplyResources(this, "$this");
			this.CancelButton = this._cancelButton;
			this.Controls.Add(_lytButtons);
			this.Controls.Add(this._priority);
			this.Controls.Add(this._syncForumsButton);
			this.Controls.Add(this._messageLabel);
			this.Controls.Add(this._forumListView);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SubscribeForm";
			this.ShowInTaskbar = false;
			_lytButtons.ResumeLayout(false);
			_lytButtons.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._priority)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		#endregion

		private System.Windows.Forms.Button _applyButton;
		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.ColumnHeader _descriptionColumnHeader;
		private System.Windows.Forms.ImageList _formImages;
		private System.Windows.Forms.ColumnHeader _forumColumnHeader;
		private System.Windows.Forms.ListView _forumListView;
		private System.Windows.Forms.Label _messageLabel;
		private System.Windows.Forms.Button _okButton;
		private System.Windows.Forms.NumericUpDown _priority;
		private System.Windows.Forms.Button _syncForumsButton;
		private System.Windows.Forms.ColumnHeader _typeColumnHeader;
	}
}