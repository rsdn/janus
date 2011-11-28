namespace Rsdn.Janus
{
	partial class MarkMsgReadForm
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
			System.Windows.Forms.ColumnHeader colCheck;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MarkMsgReadForm));
			System.Windows.Forms.ColumnHeader colForum;
			System.Windows.Forms.ColumnHeader colDescription;
			System.Windows.Forms.Button btnOk;
			System.Windows.Forms.Button btnCancel;
			System.Windows.Forms.Label label2;
			this._forumsList = new System.Windows.Forms.ListView();
			this._exceptAnswersMeCheck = new System.Windows.Forms.CheckBox();
			this._dateStartPicker = new System.Windows.Forms.DateTimePicker();
			this._formImages = new System.Windows.Forms.ImageList(this.components);
			this._unCheckAllButton = new System.Windows.Forms.Button();
			this._checkAllButton = new System.Windows.Forms.Button();
			this._checkInvertButton = new System.Windows.Forms.Button();
			this._toolTip = new System.Windows.Forms.ToolTip(this.components);
			this._markBeforeRadio = new System.Windows.Forms.RadioButton();
			this._markBetweenRadio = new System.Windows.Forms.RadioButton();
			this._markAfterRadio = new System.Windows.Forms.RadioButton();
			this._dateEndPicker = new System.Windows.Forms.DateTimePicker();
			this._markAsCombo = new System.Windows.Forms.ComboBox();
			this._exceptMarkedBranchesCheck = new System.Windows.Forms.CheckBox();
			this._currentForumButton = new System.Windows.Forms.Button();
			colCheck = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			colForum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			colDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			btnOk = new System.Windows.Forms.Button();
			btnCancel = new System.Windows.Forms.Button();
			label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// colCheck
			// 
			resources.ApplyResources(colCheck, "colCheck");
			// 
			// colForum
			// 
			resources.ApplyResources(colForum, "colForum");
			// 
			// colDescription
			// 
			resources.ApplyResources(colDescription, "colDescription");
			// 
			// btnOk
			// 
			resources.ApplyResources(btnOk, "btnOk");
			btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			btnOk.Name = "btnOk";
			this._toolTip.SetToolTip(btnOk, resources.GetString("btnOk.ToolTip"));
			btnOk.Click += new System.EventHandler(this.BtnOkClick);
			// 
			// btnCancel
			// 
			resources.ApplyResources(btnCancel, "btnCancel");
			btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btnCancel.Name = "btnCancel";
			this._toolTip.SetToolTip(btnCancel, resources.GetString("btnCancel.ToolTip"));
			// 
			// label2
			// 
			resources.ApplyResources(label2, "label2");
			label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			label2.Name = "label2";
			this._toolTip.SetToolTip(label2, resources.GetString("label2.ToolTip"));
			// 
			// _forumsList
			// 
			resources.ApplyResources(this._forumsList, "_forumsList");
			this._forumsList.CheckBoxes = true;
			this._forumsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            colCheck,
            colForum,
            colDescription});
			this._forumsList.FullRowSelect = true;
			this._forumsList.GridLines = true;
			this._forumsList.HideSelection = false;
			this._forumsList.Name = "_forumsList";
			this._toolTip.SetToolTip(this._forumsList, resources.GetString("_forumsList.ToolTip"));
			this._forumsList.UseCompatibleStateImageBehavior = false;
			this._forumsList.View = System.Windows.Forms.View.Details;
			// 
			// _exceptAnswersMeCheck
			// 
			resources.ApplyResources(this._exceptAnswersMeCheck, "_exceptAnswersMeCheck");
			this._exceptAnswersMeCheck.Name = "_exceptAnswersMeCheck";
			this._toolTip.SetToolTip(this._exceptAnswersMeCheck, resources.GetString("_exceptAnswersMeCheck.ToolTip"));
			// 
			// _dateStartPicker
			// 
			resources.ApplyResources(this._dateStartPicker, "_dateStartPicker");
			this._dateStartPicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this._dateStartPicker.Name = "_dateStartPicker";
			this._toolTip.SetToolTip(this._dateStartPicker, resources.GetString("_dateStartPicker.ToolTip"));
			// 
			// _formImages
			// 
			this._formImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			resources.ApplyResources(this._formImages, "_formImages");
			this._formImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// _unCheckAllButton
			// 
			resources.ApplyResources(this._unCheckAllButton, "_unCheckAllButton");
			this._unCheckAllButton.Name = "_unCheckAllButton";
			this._toolTip.SetToolTip(this._unCheckAllButton, resources.GetString("_unCheckAllButton.ToolTip"));
			this._unCheckAllButton.Click += new System.EventHandler(this.BtnUnCheckAllClick);
			// 
			// _checkAllButton
			// 
			resources.ApplyResources(this._checkAllButton, "_checkAllButton");
			this._checkAllButton.Name = "_checkAllButton";
			this._toolTip.SetToolTip(this._checkAllButton, resources.GetString("_checkAllButton.ToolTip"));
			this._checkAllButton.Click += new System.EventHandler(this.BtnCheckAllClick);
			// 
			// _checkInvertButton
			// 
			resources.ApplyResources(this._checkInvertButton, "_checkInvertButton");
			this._checkInvertButton.Name = "_checkInvertButton";
			this._toolTip.SetToolTip(this._checkInvertButton, resources.GetString("_checkInvertButton.ToolTip"));
			this._checkInvertButton.Click += new System.EventHandler(this.BtnCheckInvertClick);
			// 
			// _markBeforeRadio
			// 
			resources.ApplyResources(this._markBeforeRadio, "_markBeforeRadio");
			this._markBeforeRadio.Checked = true;
			this._markBeforeRadio.Name = "_markBeforeRadio";
			this._markBeforeRadio.TabStop = true;
			this._toolTip.SetToolTip(this._markBeforeRadio, resources.GetString("_markBeforeRadio.ToolTip"));
			this._markBeforeRadio.UseVisualStyleBackColor = true;
			this._markBeforeRadio.CheckedChanged += new System.EventHandler(this.RadioButtonsCheckedChanged);
			// 
			// _markBetweenRadio
			// 
			resources.ApplyResources(this._markBetweenRadio, "_markBetweenRadio");
			this._markBetweenRadio.Name = "_markBetweenRadio";
			this._toolTip.SetToolTip(this._markBetweenRadio, resources.GetString("_markBetweenRadio.ToolTip"));
			this._markBetweenRadio.UseVisualStyleBackColor = true;
			this._markBetweenRadio.CheckedChanged += new System.EventHandler(this.RadioButtonsCheckedChanged);
			// 
			// _markAfterRadio
			// 
			resources.ApplyResources(this._markAfterRadio, "_markAfterRadio");
			this._markAfterRadio.Name = "_markAfterRadio";
			this._toolTip.SetToolTip(this._markAfterRadio, resources.GetString("_markAfterRadio.ToolTip"));
			this._markAfterRadio.UseVisualStyleBackColor = true;
			this._markAfterRadio.CheckedChanged += new System.EventHandler(this.RadioButtonsCheckedChanged);
			// 
			// _dateEndPicker
			// 
			resources.ApplyResources(this._dateEndPicker, "_dateEndPicker");
			this._dateEndPicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this._dateEndPicker.Name = "_dateEndPicker";
			this._toolTip.SetToolTip(this._dateEndPicker, resources.GetString("_dateEndPicker.ToolTip"));
			// 
			// _markAsCombo
			// 
			resources.ApplyResources(this._markAsCombo, "_markAsCombo");
			this._markAsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._markAsCombo.FormattingEnabled = true;
			this._markAsCombo.Items.AddRange(new object[] {
            resources.GetString("_markAsCombo.Items"),
            resources.GetString("_markAsCombo.Items1")});
			this._markAsCombo.Name = "_markAsCombo";
			this._toolTip.SetToolTip(this._markAsCombo, resources.GetString("_markAsCombo.ToolTip"));
			// 
			// _exceptMarkedBranchesCheck
			// 
			resources.ApplyResources(this._exceptMarkedBranchesCheck, "_exceptMarkedBranchesCheck");
			this._exceptMarkedBranchesCheck.Name = "_exceptMarkedBranchesCheck";
			this._toolTip.SetToolTip(this._exceptMarkedBranchesCheck, resources.GetString("_exceptMarkedBranchesCheck.ToolTip"));
			// 
			// _currentForumButton
			// 
			resources.ApplyResources(this._currentForumButton, "_currentForumButton");
			this._currentForumButton.Name = "_currentForumButton";
			this._toolTip.SetToolTip(this._currentForumButton, resources.GetString("_currentForumButton.ToolTip"));
			this._currentForumButton.Click += new System.EventHandler(this.ButtonCurrentForumClick);
			// 
			// MarkMsgReadForm
			// 
			this.AcceptButton = btnOk;
			resources.ApplyResources(this, "$this");
			this.CancelButton = btnCancel;
			this.Controls.Add(this._markAsCombo);
			this.Controls.Add(this._markAfterRadio);
			this.Controls.Add(this._markBetweenRadio);
			this.Controls.Add(this._markBeforeRadio);
			this.Controls.Add(this._currentForumButton);
			this.Controls.Add(this._checkInvertButton);
			this.Controls.Add(this._checkAllButton);
			this.Controls.Add(this._unCheckAllButton);
			this.Controls.Add(this._dateEndPicker);
			this.Controls.Add(this._dateStartPicker);
			this.Controls.Add(this._exceptMarkedBranchesCheck);
			this.Controls.Add(this._exceptAnswersMeCheck);
			this.Controls.Add(btnCancel);
			this.Controls.Add(btnOk);
			this.Controls.Add(this._forumsList);
			this.Controls.Add(label2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MarkMsgReadForm";
			this.ShowInTaskbar = false;
			this._toolTip.SetToolTip(this, resources.GetString("$this.ToolTip"));
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button _checkAllButton;
		private System.Windows.Forms.Button _checkInvertButton;
		private System.Windows.Forms.Button _currentForumButton;
		private System.Windows.Forms.Button _unCheckAllButton;
		private System.Windows.Forms.ImageList _formImages;
		private System.Windows.Forms.CheckBox _exceptAnswersMeCheck;
		private System.Windows.Forms.CheckBox _exceptMarkedBranchesCheck;
		private System.Windows.Forms.DateTimePicker _dateStartPicker;
		private System.Windows.Forms.ListView _forumsList;
		private System.Windows.Forms.RadioButton _markBeforeRadio;
		private System.Windows.Forms.RadioButton _markBetweenRadio;
		private System.Windows.Forms.RadioButton _markAfterRadio;
		private System.Windows.Forms.DateTimePicker _dateEndPicker;
		private System.Windows.Forms.ComboBox _markAsCombo;
		private System.Windows.Forms.ToolTip _toolTip;
	}
}