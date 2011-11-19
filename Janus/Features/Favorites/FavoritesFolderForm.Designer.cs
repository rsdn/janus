namespace Rsdn.Janus
{
	public partial class FavoritesFolderForm
	{
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FavoritesFolderForm));
			this._nameBox = new System.Windows.Forms.TextBox();
			this._okBtn = new System.Windows.Forms.Button();
			this._cancelBtn = new System.Windows.Forms.Button();
			this._nameLabel = new System.Windows.Forms.Label();
			this._commentLabel = new System.Windows.Forms.Label();
			this._commentBox = new System.Windows.Forms.TextBox();
			this._isRootCheckBox = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// _nameBox
			// 
			resources.ApplyResources(this._nameBox, "_nameBox");
			this._nameBox.Name = "_nameBox";
			// 
			// _okBtn
			// 
			resources.ApplyResources(this._okBtn, "_okBtn");
			this._okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._okBtn.Name = "_okBtn";
			// 
			// _cancelBtn
			// 
			resources.ApplyResources(this._cancelBtn, "_cancelBtn");
			this._cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelBtn.Name = "_cancelBtn";
			// 
			// _nameLabel
			// 
			resources.ApplyResources(this._nameLabel, "_nameLabel");
			this._nameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._nameLabel.Name = "_nameLabel";
			// 
			// _commentLabel
			// 
			resources.ApplyResources(this._commentLabel, "_commentLabel");
			this._commentLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._commentLabel.Name = "_commentLabel";
			// 
			// _commentBox
			// 
			resources.ApplyResources(this._commentBox, "_commentBox");
			this._commentBox.Name = "_commentBox";
			// 
			// _isRootCheckBox
			// 
			resources.ApplyResources(this._isRootCheckBox, "_isRootCheckBox");
			this._isRootCheckBox.Name = "_isRootCheckBox";
			// 
			// FavoritesFolderForm
			// 
			this.AcceptButton = this._okBtn;
			this.CancelButton = this._cancelBtn;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this._isRootCheckBox);
			this.Controls.Add(this._commentLabel);
			this.Controls.Add(this._commentBox);
			this.Controls.Add(this._nameBox);
			this.Controls.Add(this._nameLabel);
			this.Controls.Add(this._cancelBtn);
			this.Controls.Add(this._okBtn);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FavoritesFolderForm";
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Button _cancelBtn;
		private System.Windows.Forms.TextBox _commentBox;
		private System.Windows.Forms.Label _commentLabel;
		private System.Windows.Forms.CheckBox _isRootCheckBox;
		private System.Windows.Forms.TextBox _nameBox;
		private System.Windows.Forms.Label _nameLabel;
		private System.Windows.Forms.Button _okBtn;
	}
}