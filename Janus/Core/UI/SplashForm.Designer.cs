namespace Rsdn.Janus
{
	partial class SplashForm
	{
		///<summary>
		///Disposes of the resources (other than memory) used by the <see cref="T:System.Windows.Forms.Form"></see>.
		///</summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
				_asyncOp.OperationCompleted();
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.PictureBox pictureBox;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashForm));
			this._versionLabel = new System.Windows.Forms.Label();
			this._itemsBox = new System.Windows.Forms.PictureBox();
			this._copyrightLabel = new System.Windows.Forms.Label();
			this._statusLabel = new System.Windows.Forms.Label();
			pictureBox = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(pictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._itemsBox)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox
			// 
			pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
			pictureBox.Location = new System.Drawing.Point(1, 1);
			pictureBox.Name = "pictureBox";
			pictureBox.Size = new System.Drawing.Size(480, 300);
			pictureBox.TabIndex = 0;
			pictureBox.TabStop = false;
			// 
			// _versionLabel
			// 
			this._versionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this._versionLabel.BackColor = System.Drawing.Color.White;
			this._versionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._versionLabel.ForeColor = System.Drawing.Color.Black;
			this._versionLabel.Location = new System.Drawing.Point(153, 204);
			this._versionLabel.Name = "_versionLabel";
			this._versionLabel.Size = new System.Drawing.Size(317, 18);
			this._versionLabel.TabIndex = 2;
			this._versionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// _itemsBox
			// 
			this._itemsBox.BackColor = System.Drawing.Color.White;
			this._itemsBox.Location = new System.Drawing.Point(1, 229);
			this._itemsBox.Name = "_itemsBox";
			this._itemsBox.Size = new System.Drawing.Size(478, 78);
			this._itemsBox.TabIndex = 3;
			this._itemsBox.TabStop = false;
			this._itemsBox.Paint += new System.Windows.Forms.PaintEventHandler(this.ModulesBoxPaint);
			// 
			// _copyrightLabel
			// 
			this._copyrightLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this._copyrightLabel.BackColor = System.Drawing.Color.White;
			this._copyrightLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._copyrightLabel.ForeColor = System.Drawing.Color.Black;
			this._copyrightLabel.Location = new System.Drawing.Point(161, 302);
			this._copyrightLabel.Name = "_copyrightLabel";
			this._copyrightLabel.Size = new System.Drawing.Size(317, 14);
			this._copyrightLabel.TabIndex = 2;
			this._copyrightLabel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// _statusLabel
			// 
			this._statusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this._statusLabel.BackColor = System.Drawing.Color.White;
			this._statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._statusLabel.ForeColor = System.Drawing.Color.Black;
			this._statusLabel.Location = new System.Drawing.Point(115, 9);
			this._statusLabel.Name = "_statusLabel";
			this._statusLabel.Size = new System.Drawing.Size(299, 14);
			this._statusLabel.TabIndex = 2;
			this._statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// SplashForm
			// 
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(482, 322);
			this.Controls.Add(this._statusLabel);
			this.Controls.Add(this._copyrightLabel);
			this.Controls.Add(this._versionLabel);
			this.Controls.Add(this._itemsBox);
			this.Controls.Add(pictureBox);
			this.Font = new System.Drawing.Font("Arial", 9F);
			this.ForeColor = System.Drawing.Color.Black;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "SplashForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			((System.ComponentModel.ISupportInitialize)(pictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._itemsBox)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.Label _versionLabel;
		private System.Windows.Forms.PictureBox _itemsBox;
		private System.Windows.Forms.Label _copyrightLabel;
		private System.Windows.Forms.Label _statusLabel;
	}
}