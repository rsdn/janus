namespace Rsdn.Janus
{
	internal partial class FindDialogForm
	{
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindDialogForm));
			this._findStrBox = new System.Windows.Forms.TextBox();
			this._findButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			this._findWhatLabel = new System.Windows.Forms.Label();
			this._matchCaseCheckBox = new System.Windows.Forms.CheckBox();
			this._matchWholeWordCheckBox = new System.Windows.Forms.CheckBox();
			this._useRegExCheckBox = new System.Windows.Forms.CheckBox();
			this._serachGroupBox = new System.Windows.Forms.GroupBox();
			this._selOnlyRadioBtn = new System.Windows.Forms.RadioButton();
			this._allDocRadioBtn = new System.Windows.Forms.RadioButton();
			this._matchWordStartCheckBox = new System.Windows.Forms.CheckBox();
			this._reverseSearchCheckBox = new System.Windows.Forms.CheckBox();
			this._optionsGroupBox = new System.Windows.Forms.GroupBox();
			this._serachGroupBox.SuspendLayout();
			this._optionsGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// _findStrBox
			// 
			resources.ApplyResources(this._findStrBox, "_findStrBox");
			this._findStrBox.Name = "_findStrBox";
			// 
			// _findButton
			// 
			resources.ApplyResources(this._findButton, "_findButton");
			this._findButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._findButton.Name = "_findButton";
			// 
			// _cancelButton
			// 
			resources.ApplyResources(this._cancelButton, "_cancelButton");
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Name = "_cancelButton";
			// 
			// _findWhatLabel
			// 
			resources.ApplyResources(this._findWhatLabel, "_findWhatLabel");
			this._findWhatLabel.Name = "_findWhatLabel";
			// 
			// _matchCaseCheckBox
			// 
			resources.ApplyResources(this._matchCaseCheckBox, "_matchCaseCheckBox");
			this._matchCaseCheckBox.Name = "_matchCaseCheckBox";
			// 
			// _matchWholeWordCheckBox
			// 
			resources.ApplyResources(this._matchWholeWordCheckBox, "_matchWholeWordCheckBox");
			this._matchWholeWordCheckBox.Name = "_matchWholeWordCheckBox";
			// 
			// _useRegExCheckBox
			// 
			resources.ApplyResources(this._useRegExCheckBox, "_useRegExCheckBox");
			this._useRegExCheckBox.Name = "_useRegExCheckBox";
			// 
			// _serachGroupBox
			// 
			resources.ApplyResources(this._serachGroupBox, "_serachGroupBox");
			this._serachGroupBox.Controls.Add(this._selOnlyRadioBtn);
			this._serachGroupBox.Controls.Add(this._allDocRadioBtn);
			this._serachGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._serachGroupBox.Name = "_serachGroupBox";
			this._serachGroupBox.TabStop = false;
			// 
			// _selOnlyRadioBtn
			// 
			resources.ApplyResources(this._selOnlyRadioBtn, "_selOnlyRadioBtn");
			this._selOnlyRadioBtn.Name = "_selOnlyRadioBtn";
			// 
			// _allDocRadioBtn
			// 
			resources.ApplyResources(this._allDocRadioBtn, "_allDocRadioBtn");
			this._allDocRadioBtn.Checked = true;
			this._allDocRadioBtn.Name = "_allDocRadioBtn";
			this._allDocRadioBtn.TabStop = true;
			// 
			// _matchWordStartCheckBox
			// 
			resources.ApplyResources(this._matchWordStartCheckBox, "_matchWordStartCheckBox");
			this._matchWordStartCheckBox.Name = "_matchWordStartCheckBox";
			// 
			// _reverseSearchCheckBox
			// 
			resources.ApplyResources(this._reverseSearchCheckBox, "_reverseSearchCheckBox");
			this._reverseSearchCheckBox.Name = "_reverseSearchCheckBox";
			// 
			// _optionsGroupBox
			// 
			resources.ApplyResources(this._optionsGroupBox, "_optionsGroupBox");
			this._optionsGroupBox.Controls.Add(this._matchCaseCheckBox);
			this._optionsGroupBox.Controls.Add(this._reverseSearchCheckBox);
			this._optionsGroupBox.Controls.Add(this._useRegExCheckBox);
			this._optionsGroupBox.Controls.Add(this._matchWholeWordCheckBox);
			this._optionsGroupBox.Controls.Add(this._matchWordStartCheckBox);
			this._optionsGroupBox.Name = "_optionsGroupBox";
			this._optionsGroupBox.TabStop = false;
			// 
			// FindDialogForm
			// 
			this.AcceptButton = this._findButton;
			this.CancelButton = this._cancelButton;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this._optionsGroupBox);
			this.Controls.Add(this._serachGroupBox);
			this.Controls.Add(this._findWhatLabel);
			this.Controls.Add(this._findStrBox);
			this.Controls.Add(this._cancelButton);
			this.Controls.Add(this._findButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FindDialogForm";
			this.ShowInTaskbar = false;
			this._serachGroupBox.ResumeLayout(false);
			this._serachGroupBox.PerformLayout();
			this._optionsGroupBox.ResumeLayout(false);
			this._optionsGroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion

		private System.Windows.Forms.TextBox _findStrBox;
		private System.Windows.Forms.Button _findButton;
		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.Label _findWhatLabel;
		private System.Windows.Forms.CheckBox _matchCaseCheckBox;
		private System.Windows.Forms.CheckBox _matchWholeWordCheckBox;
		private System.Windows.Forms.CheckBox _useRegExCheckBox;
		private System.Windows.Forms.GroupBox _serachGroupBox;
		private System.Windows.Forms.RadioButton _allDocRadioBtn;
		private System.Windows.Forms.RadioButton _selOnlyRadioBtn;
		private System.Windows.Forms.CheckBox _matchWordStartCheckBox;
		private System.Windows.Forms.CheckBox _reverseSearchCheckBox;
		private System.Windows.Forms.GroupBox _optionsGroupBox;
	}
}