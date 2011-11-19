namespace Rsdn.Shortcuts
{
	public partial class SavePresetForm
	{
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{}

			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SavePresetForm));
			this.labelPresetNameText = new System.Windows.Forms.Label();
			this.tbPresetName = new System.Windows.Forms.TextBox();
			this.buttonOk = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// labelPresetNameText
			// 
			resources.ApplyResources(this.labelPresetNameText, "labelPresetNameText");
			this.labelPresetNameText.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.labelPresetNameText.Name = "labelPresetNameText";
			// 
			// tbPresetName
			// 
			resources.ApplyResources(this.tbPresetName, "tbPresetName");
			this.tbPresetName.Name = "tbPresetName";
			// 
			// buttonOk
			// 
			this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			resources.ApplyResources(this.buttonOk, "buttonOk");
			this.buttonOk.Name = "buttonOk";
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.Name = "buttonCancel";
			// 
			// SavePresetForm
			// 
			this.AcceptButton = this.buttonOk;
			this.CancelButton = this.buttonCancel;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.buttonOk);
			this.Controls.Add(this.tbPresetName);
			this.Controls.Add(this.labelPresetNameText);
			this.Controls.Add(this.buttonCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SavePresetForm";
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label labelPresetNameText;
		private System.Windows.Forms.TextBox tbPresetName;
	}
}