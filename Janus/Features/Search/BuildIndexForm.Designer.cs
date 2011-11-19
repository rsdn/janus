namespace Rsdn.Janus
{
	partial class BuildIndexForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.progressLabel = new System.Windows.Forms.Label();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.cleanButton = new System.Windows.Forms.Button();
			this.startButton = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.stopButton = new System.Windows.Forms.Button();
			this.optimizeCheckBox = new System.Windows.Forms.CheckBox();
			this.closeButton = new System.Windows.Forms.Button();
			this.indexBackgroundWorker = new System.ComponentModel.BackgroundWorker();
			this.actionTextBox = new System.Windows.Forms.TextBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// progressLabel
			// 
			this.progressLabel.AutoSize = true;
			this.progressLabel.Location = new System.Drawing.Point(6, 16);
			this.progressLabel.Name = "progressLabel";
			this.progressLabel.Size = new System.Drawing.Size(16, 13);
			this.progressLabel.TabIndex = 0;
			this.progressLabel.Text = "...";
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(9, 42);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(372, 23);
			this.progressBar.TabIndex = 1;
			// 
			// cleanButton
			// 
			this.cleanButton.Location = new System.Drawing.Point(9, 144);
			this.cleanButton.Name = "cleanButton";
			this.cleanButton.Size = new System.Drawing.Size(75, 23);
			this.cleanButton.TabIndex = 4;
			this.cleanButton.Text = "button1";
			this.cleanButton.UseVisualStyleBackColor = true;
			this.cleanButton.Click += new System.EventHandler(this.CleanButtonClick);
			// 
			// startButton
			// 
			this.startButton.Location = new System.Drawing.Point(90, 144);
			this.startButton.Name = "startButton";
			this.startButton.Size = new System.Drawing.Size(75, 23);
			this.startButton.TabIndex = 5;
			this.startButton.Text = "button2";
			this.startButton.UseVisualStyleBackColor = true;
			this.startButton.Click += new System.EventHandler(this.StartButtonClick);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.actionTextBox);
			this.groupBox1.Controls.Add(this.stopButton);
			this.groupBox1.Controls.Add(this.optimizeCheckBox);
			this.groupBox1.Controls.Add(this.progressLabel);
			this.groupBox1.Controls.Add(this.startButton);
			this.groupBox1.Controls.Add(this.progressBar);
			this.groupBox1.Controls.Add(this.cleanButton);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(387, 173);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// stopButton
			// 
			this.stopButton.Location = new System.Drawing.Point(171, 144);
			this.stopButton.Name = "stopButton";
			this.stopButton.Size = new System.Drawing.Size(75, 23);
			this.stopButton.TabIndex = 6;
			this.stopButton.Text = "button3";
			this.stopButton.UseVisualStyleBackColor = true;
			this.stopButton.Click += new System.EventHandler(this.StopButtonClick);
			// 
			// optimizeCheckBox
			// 
			this.optimizeCheckBox.AutoSize = true;
			this.optimizeCheckBox.Location = new System.Drawing.Point(9, 121);
			this.optimizeCheckBox.Name = "optimizeCheckBox";
			this.optimizeCheckBox.Size = new System.Drawing.Size(80, 17);
			this.optimizeCheckBox.TabIndex = 3;
			this.optimizeCheckBox.Text = "checkBox1";
			this.optimizeCheckBox.UseVisualStyleBackColor = true;
			// 
			// closeButton
			// 
			this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.closeButton.Location = new System.Drawing.Point(324, 191);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(75, 23);
			this.closeButton.TabIndex = 1;
			this.closeButton.Text = "button4";
			this.closeButton.UseVisualStyleBackColor = true;
			// 
			// indexBackgroundWorker
			// 
			this.indexBackgroundWorker.WorkerReportsProgress = true;
			this.indexBackgroundWorker.WorkerSupportsCancellation = true;
			this.indexBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.IndexBackgroundWorkerDoWork);
			this.indexBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.IndexBackgroundWorkerRunWorkerCompleted);
			this.indexBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.IndexBackgroundWorkerProgressChanged);
			// 
			// actionTextBox
			// 
			this.actionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.actionTextBox.BackColor = System.Drawing.SystemColors.Control;
			this.actionTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.actionTextBox.Location = new System.Drawing.Point(9, 71);
			this.actionTextBox.Multiline = true;
			this.actionTextBox.Name = "actionTextBox";
			this.actionTextBox.ReadOnly = true;
			this.actionTextBox.Size = new System.Drawing.Size(372, 44);
			this.actionTextBox.TabIndex = 8;
			this.actionTextBox.Text = "actionText";
			// 
			// BuildIndexForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(411, 226);
			this.Controls.Add(this.closeButton);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "BuildIndexForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "BuildIndexForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BuildIndexForm_FormClosing);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label progressLabel;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Button cleanButton;
		private System.Windows.Forms.Button startButton;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button stopButton;
		private System.Windows.Forms.CheckBox optimizeCheckBox;
		private System.Windows.Forms.Button closeButton;
		private System.ComponentModel.BackgroundWorker indexBackgroundWorker;
		private System.Windows.Forms.TextBox actionTextBox;
	}
}