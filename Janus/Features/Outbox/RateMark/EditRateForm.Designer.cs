namespace Rsdn.Janus
{
	public partial class EditRateForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditRateForm));
			this.cbRate = new System.Windows.Forms.ComboBox();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cbRate
			// 
			this.cbRate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(244)))));
			this.cbRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			resources.ApplyResources(this.cbRate, "cbRate");
			this.cbRate.Items.AddRange(new object[] {
			resources.GetString("cbRate.Items"),
			resources.GetString("cbRate.Items1"),
			resources.GetString("cbRate.Items2"),
			resources.GetString("cbRate.Items3"),
			resources.GetString("cbRate.Items4")});
			this.cbRate.Name = "cbRate";
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			resources.ApplyResources(this.button1, "button1");
			this.button1.Name = "button1";
			// 
			// button2
			// 
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.button2, "button2");
			this.button2.Name = "button2";
			// 
			// EditRateForm
			// 
			this.AcceptButton = this.button1;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(255)))), ((int)(((byte)(244)))));
			this.CancelButton = this.button2;
			resources.ApplyResources(this, "$this");
			this.ControlBox = false;
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.cbRate);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EditRateForm";
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);

		}
		#endregion


		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		public System.Windows.Forms.ComboBox cbRate;
	}
}