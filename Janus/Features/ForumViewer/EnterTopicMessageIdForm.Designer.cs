namespace Rsdn.Janus
{
	internal partial class EnterTopicMessageIdForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnterTopicMessageIdForm));
			this._idBox = new System.Windows.Forms.TextBox();
			this._okButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			this._labelMessageIdIsText = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// _idBox
			// 
			resources.ApplyResources(this._idBox, "_idBox");
			this._idBox.Name = "_idBox";
			this._idBox.TextChanged += new System.EventHandler(this._idBox_TextChanged);
			// 
			// _okButton
			// 
			resources.ApplyResources(this._okButton, "_okButton");
			this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._okButton.Name = "_okButton";
			// 
			// _cancelButton
			// 
			resources.ApplyResources(this._cancelButton, "_cancelButton");
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Name = "_cancelButton";
			// 
			// _labelMessageIdIsText
			// 
			resources.ApplyResources(this._labelMessageIdIsText, "_labelMessageIdIsText");
			this._labelMessageIdIsText.Name = "_labelMessageIdIsText";
			// 
			// EnterTopicMessageIdForm
			// 
			this.AcceptButton = this._okButton;
			this.CancelButton = this._cancelButton;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this._labelMessageIdIsText);
			this.Controls.Add(this._cancelButton);
			this.Controls.Add(this._okButton);
			this.Controls.Add(this._idBox);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EnterTopicMessageIdForm";
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.TextBox _idBox;
		private System.Windows.Forms.Button _okButton;
		private System.Windows.Forms.Label _labelMessageIdIsText;
	}
}