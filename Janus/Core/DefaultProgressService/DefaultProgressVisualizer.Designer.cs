namespace Rsdn.Janus.Framework
{
	partial class DefaultProgressVisualizer
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DefaultProgressVisualizer));
			this._cancelButton = new System.Windows.Forms.Button();
			this._messageLabel = new System.Windows.Forms.Label();
			this._progressBar = new System.Windows.Forms.ProgressBar();
			this.SuspendLayout();
			// 
			// _cancelButton
			// 
			resources.ApplyResources(this._cancelButton, "_cancelButton");
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.UseVisualStyleBackColor = true;
			// 
			// _messageLabel
			// 
			resources.ApplyResources(this._messageLabel, "_messageLabel");
			this._messageLabel.Name = "_messageLabel";
			// 
			// _progressBar
			// 
			resources.ApplyResources(this._progressBar, "_progressBar");
			this._progressBar.MinimumSize = new System.Drawing.Size(200, 14);
			this._progressBar.Name = "_progressBar";
			this._progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			// 
			// DefaultProgressVisualizer
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this._cancelButton);
			this.Controls.Add(this._messageLabel);
			this.Controls.Add(this._progressBar);
			this.Name = "DefaultProgressVisualizer";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.Label _messageLabel;
		private System.Windows.Forms.ProgressBar _progressBar;


	}
}
