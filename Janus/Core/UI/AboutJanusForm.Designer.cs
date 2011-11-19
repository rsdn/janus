using System.ComponentModel;
using System.Windows.Forms;

namespace Rsdn.Janus
{
	partial class AboutJanusForm
	{
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
			System.Windows.Forms.FlowLayoutPanel lytButtons;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutJanusForm));
			System.Windows.Forms.RadioButton btnComponents;
			System.Windows.Forms.RadioButton btnDevelopers;
			this._asmButton = new System.Windows.Forms.RadioButton();
			this._okButton = new System.Windows.Forms.Button();
			this._panelBlack = new System.Windows.Forms.Panel();
			this._janusPicture = new System.Windows.Forms.PictureBox();
			this._titleLabel = new System.Windows.Forms.Label();
			this._versionLabel = new System.Windows.Forms.Label();
			this._webBrowser = new System.Windows.Forms.WebBrowser();
			this._fadeTimer = new System.Windows.Forms.Timer(this.components);
			lytButtons = new System.Windows.Forms.FlowLayoutPanel();
			btnComponents = new System.Windows.Forms.RadioButton();
			btnDevelopers = new System.Windows.Forms.RadioButton();
			lytButtons.SuspendLayout();
			this._panelBlack.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._janusPicture)).BeginInit();
			this.SuspendLayout();
			// 
			// lytButtons
			// 
			resources.ApplyResources(lytButtons, "lytButtons");
			lytButtons.Controls.Add(this._asmButton);
			lytButtons.Controls.Add(btnComponents);
			lytButtons.Controls.Add(btnDevelopers);
			lytButtons.Controls.Add(this._okButton);
			lytButtons.Name = "lytButtons";
			// 
			// _asmButton
			// 
			resources.ApplyResources(this._asmButton, "_asmButton");
			this._asmButton.Name = "_asmButton";
			this._asmButton.Click += new System.EventHandler(this.ButtonAssemblyInfoClick);
			// 
			// btnComponents
			// 
			resources.ApplyResources(btnComponents, "btnComponents");
			btnComponents.Name = "btnComponents";
			btnComponents.Click += new System.EventHandler(this.ButtonShowComponentsClick);
			// 
			// btnDevelopers
			// 
			resources.ApplyResources(btnDevelopers, "btnDevelopers");
			btnDevelopers.Name = "btnDevelopers";
			btnDevelopers.Click += new System.EventHandler(this.ButtonShowDevelopersClick);
			// 
			// _okButton
			// 
			resources.ApplyResources(this._okButton, "_okButton");
			this._okButton.BackColor = System.Drawing.SystemColors.Control;
			this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._okButton.Name = "_okButton";
			this._okButton.UseVisualStyleBackColor = false;
			// 
			// _panelBlack
			// 
			resources.ApplyResources(this._panelBlack, "_panelBlack");
			this._panelBlack.BackColor = System.Drawing.Color.Black;
			this._panelBlack.Controls.Add(this._janusPicture);
			this._panelBlack.Name = "_panelBlack";
			// 
			// _janusPicture
			// 
			resources.ApplyResources(this._janusPicture, "_janusPicture");
			this._janusPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._janusPicture.Name = "_janusPicture";
			this._janusPicture.TabStop = false;
			// 
			// _titleLabel
			// 
			resources.ApplyResources(this._titleLabel, "_titleLabel");
			this._titleLabel.BackColor = System.Drawing.Color.Black;
			this._titleLabel.ForeColor = System.Drawing.Color.White;
			this._titleLabel.Name = "_titleLabel";
			// 
			// _versionLabel
			// 
			resources.ApplyResources(this._versionLabel, "_versionLabel");
			this._versionLabel.BackColor = System.Drawing.Color.Black;
			this._versionLabel.ForeColor = System.Drawing.Color.White;
			this._versionLabel.Name = "_versionLabel";
			// 
			// _webBrowser
			// 
			resources.ApplyResources(this._webBrowser, "_webBrowser");
			this._webBrowser.Name = "_webBrowser";
			// 
			// _fadeTimer
			// 
			this._fadeTimer.Interval = 70;
			this._fadeTimer.Tick += new System.EventHandler(this.FadeTimerTick);
			// 
			// AboutJanusForm
			// 
			this.AcceptButton = this._okButton;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.CancelButton = this._okButton;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(lytButtons);
			this.Controls.Add(this._webBrowser);
			this.Controls.Add(this._versionLabel);
			this.Controls.Add(this._titleLabel);
			this.Controls.Add(this._panelBlack);
			this.Name = "AboutJanusForm";
			this.ShowInTaskbar = false;
			this.Closing += new System.ComponentModel.CancelEventHandler(this.AboutJanusForm_Closing);
			lytButtons.ResumeLayout(false);
			lytButtons.PerformLayout();
			this._panelBlack.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._janusPicture)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Button _okButton;
		private Panel _panelBlack;
		private PictureBox _janusPicture;
		private Label _titleLabel;
		private Label _versionLabel;
		private WebBrowser _webBrowser;

		private Timer _fadeTimer;
		private RadioButton _asmButton;
		private IContainer components;
	}
}