namespace Rsdn.Janus
{
	partial class RateForm
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
			System.Windows.Forms.FlowLayoutPanel _lytMessage;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RateForm));
			System.Windows.Forms.FlowLayoutPanel _lytButtons;
			System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
			this._pbxRateImage = new System.Windows.Forms.PictureBox();
			this._lblInfo = new System.Windows.Forms.Label();
			this._btnYes = new System.Windows.Forms.Button();
			this._btnCancel = new System.Windows.Forms.Button();
			_lytMessage = new System.Windows.Forms.FlowLayoutPanel();
			_lytButtons = new System.Windows.Forms.FlowLayoutPanel();
			tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			_lytMessage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._pbxRateImage)).BeginInit();
			_lytButtons.SuspendLayout();
			tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _lytMessage
			// 
			resources.ApplyResources(_lytMessage, "_lytMessage");
			_lytMessage.Controls.Add(this._pbxRateImage);
			_lytMessage.Controls.Add(this._lblInfo);
			_lytMessage.Name = "_lytMessage";
			// 
			// _pbxRateImage
			// 
			resources.ApplyResources(this._pbxRateImage, "_pbxRateImage");
			this._pbxRateImage.Name = "_pbxRateImage";
			this._pbxRateImage.TabStop = false;
			// 
			// _lblInfo
			// 
			resources.ApplyResources(this._lblInfo, "_lblInfo");
			_lytMessage.SetFlowBreak(this._lblInfo, true);
			this._lblInfo.Name = "_lblInfo";
			// 
			// _lytButtons
			// 
			resources.ApplyResources(_lytButtons, "_lytButtons");
			_lytButtons.Controls.Add(this._btnYes);
			_lytButtons.Controls.Add(this._btnCancel);
			_lytButtons.Name = "_lytButtons";
			// 
			// _btnYes
			// 
			resources.ApplyResources(this._btnYes, "_btnYes");
			this._btnYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
			this._btnYes.Name = "_btnYes";
			// 
			// _btnCancel
			// 
			resources.ApplyResources(this._btnCancel, "_btnCancel");
			this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.No;
			this._btnCancel.Name = "_btnCancel";
			// 
			// tableLayoutPanel1
			// 
			resources.ApplyResources(tableLayoutPanel1, "tableLayoutPanel1");
			tableLayoutPanel1.Controls.Add(_lytMessage, 0, 0);
			tableLayoutPanel1.Controls.Add(_lytButtons, 0, 1);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			// 
			// RateForm
			// 
			this.AcceptButton = this._btnYes;
			resources.ApplyResources(this, "$this");
			this.BackColor = System.Drawing.SystemColors.ControlLight;
			this.CancelButton = this._btnCancel;
			this.ControlBox = false;
			this.Controls.Add(tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RateForm";
			this.ShowInTaskbar = false;
			_lytMessage.ResumeLayout(false);
			_lytMessage.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._pbxRateImage)).EndInit();
			_lytButtons.ResumeLayout(false);
			_lytButtons.PerformLayout();
			tableLayoutPanel1.ResumeLayout(false);
			tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label _lblInfo;
		private System.Windows.Forms.Button _btnCancel;
		private System.Windows.Forms.Button _btnYes;
		private System.Windows.Forms.PictureBox _pbxRateImage;
	}
}
