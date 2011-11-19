namespace Rsdn.Janus
{
	partial class FolderDummyForm
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
			this._label = new System.Windows.Forms.Label();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this._pictureBox = new System.Windows.Forms.PictureBox();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._pictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// _label
			// 
			this._label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this._label.AutoSize = true;
			this._label.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._label.Location = new System.Drawing.Point(80, 16);
			this._label.Margin = new System.Windows.Forms.Padding(0);
			this._label.Name = "_label";
			this._label.Size = new System.Drawing.Size(0, 64);
			this._label.TabIndex = 0;
			this._label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this._pictureBox);
			this.flowLayoutPanel1.Controls.Add(this._label);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(16);
			this.flowLayoutPanel1.Size = new System.Drawing.Size(493, 286);
			this.flowLayoutPanel1.TabIndex = 1;
			this.flowLayoutPanel1.WrapContents = false;
			// 
			// _pictureBox
			// 
			this._pictureBox.Location = new System.Drawing.Point(16, 16);
			this._pictureBox.Margin = new System.Windows.Forms.Padding(0);
			this._pictureBox.Name = "_pictureBox";
			this._pictureBox.Size = new System.Drawing.Size(64, 64);
			this._pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._pictureBox.TabIndex = 2;
			this._pictureBox.TabStop = false;
			// 
			// FolderDummyForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.flowLayoutPanel1);
			this.Name = "FolderDummyForm";
			this.Size = new System.Drawing.Size(493, 286);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._pictureBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label _label;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.PictureBox _pictureBox;
	}
}
