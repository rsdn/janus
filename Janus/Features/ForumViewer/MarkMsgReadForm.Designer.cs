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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MarkMsgReadForm));
			System.Windows.Forms.ColumnHeader colCheck;
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
			this._forumsList.UseCompatibleStateImageBehavior = false;
			this._forumsList.View = System.Windows.Forms.View.Details;
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
			// _exceptAnswersMeCheck
			// 
			resources.ApplyResources(this._exceptAnswersMeCheck, "_exceptAnswersMeCheck");
			this._exceptAnswersMeCheck.Name = "_exceptAnswersMeCheck";
			// 
			// _dateStartPicker
			// 
			resources.ApplyResources(this._dateStartPicker, "_dateStartPicker");
			this._dateStartPicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this._dateStartPicker.Name = "_dateStartPicker";
			// 
			// _formImages
			// 
			this._formImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_formImages.ImageStream")));
			this._formImages.TransparentColor = System.Drawing.Color.Transparent;
			this._formImages.Images.SetKeyName(0, "");
			this._formImages.Images.SetKeyName(1, "");
			// 
			// _unCheckAllButton
			// 
			resources.ApplyResources(this._unCheckAllButton, "_unCheckAllButton");
			this._unCheckAllButton.Name = "_unCheckAllButton";
			this._unCheckAllButton.Click += new System.EventHandler(this.BtnUnCheckAllClick);
			// 
			// _checkAllButton
			// 
			resources.ApplyResources(this._checkAllButton, "_checkAllButton");
			this._checkAllButton.Name = "_checkAllButton";
			this._checkAllButton.Click += new System.EventHandler(this.BtnCheckAllClick);
			// 
			// _checkInvertButton
			// 
			resources.ApplyResources(this._checkInvertButton, "_checkInvertButton");
			this._checkInvertButton.Name = "_checkInvertButton";
			this._checkInvertButton.Click += new System.EventHandler(this.BtnCheckInvertClick);
			// 
			// _markBeforeRadio
			// 
			resources.ApplyResources(this._markBeforeRadio, "_markBeforeRadio");
			this._markBeforeRadio.Checked = true;
			this._markBeforeRadio.Name = "_markBeforeRadio";
			this._markBeforeRadio.TabStop = true;
			this._markBeforeRadio.UseVisualStyleBackColor = true;
			this._markBeforeRadio.CheckedChanged += new System.EventHandler(this.RadioButtonsCheckedChanged);
			// 
			// _markBetweenRadio
			// 
			resources.ApplyResources(this._markBetweenRadio, "_markBetweenRadio");
			this._markBetweenRadio.Name = "_markBetweenRadio";
			this._markBetweenRadio.UseVisualStyleBackColor = true;
			this._markBetweenRadio.CheckedChanged += new System.EventHandler(this.RadioButtonsCheckedChanged);
			// 
			// _markAfterRadio
			// 
			resources.ApplyResources(this._markAfterRadio, "_markAfterRadio");
			this._markAfterRadio.Name = "_markAfterRadio";
			this._markAfterRadio.UseVisualStyleBackColor = true;
			this._markAfterRadio.CheckedChanged += new System.EventHandler(this.RadioButtonsCheckedChanged);
			// 
			// _dateEndPicker
			// 
			resources.ApplyResources(this._dateEndPicker, "_dateEndPicker");
			this._dateEndPicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this._dateEndPicker.Name = "_dateEndPicker";
			// 
			// _markAsCombo
			// 
			this._markAsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._markAsCombo.FormattingEnabled = true;
			this._markAsCombo.Items.AddRange(new object[] {
			resources.GetString("_markAsCombo.Items"),
			resources.GetString("_markAsCombo.Items1")});
			resources.ApplyResources(this._markAsCombo, "_markAsCombo");
			this._markAsCombo.Name = "_markAsCombo";
			// 
			// _exceptMarkedBranchesCheck
			// 
			resources.ApplyResources(this._exceptMarkedBranchesCheck, "_exceptMarkedBranchesCheck");
			this._exceptMarkedBranchesCheck.Name = "_exceptMarkedBranchesCheck";
			// 
			// _currentForumButton
			// 
			resources.ApplyResources(this._currentForumButton, "_currentForumButton");
			this._currentForumButton.Name = "_currentForumButton";
			this._currentForumButton.Click += new System.EventHandler(this.ButtonCurrentForumClick);
			// 
			// btnOk
			// 
			resources.ApplyResources(btnOk, "btnOk");
			btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			btnOk.Name = "btnOk";
			btnOk.Click += new System.EventHandler(this.BtnOkClick);
			// 
			// btnCancel
			// 
			resources.ApplyResources(btnCancel, "btnCancel");
			btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btnCancel.Name = "btnCancel";
			// 
			// label2
			// 
			label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			resources.ApplyResources(label2, "label2");
			label2.Name = "label2";
			// 
			// MarkMsgReadForm
			// 
			this.AcceptButton = btnOk;
			this.CancelButton = btnCancel;
			resources.ApplyResources(this, "$this");
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