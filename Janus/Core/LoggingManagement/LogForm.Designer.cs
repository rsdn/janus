namespace Rsdn.Janus
{
	public partial class LogForm
	{
		private System.ComponentModel.IContainer components = null;

		#region Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogForm));
			this._logComboBox = new Rsdn.Janus.LogComboBox();
			this._toolStrip = new System.Windows.Forms.ToolStrip();
			this.SuspendLayout();
			// 
			// _logComboBox
			// 
			resources.ApplyResources(this._logComboBox, "_logComboBox");
			this._logComboBox.ItemHeight = 18;
			this._logComboBox.Name = "_logComboBox";
			this._logComboBox.SelectedIndex = -1;
			this._logComboBox.TopIndex = 0;
			// 
			// _toolStrip
			// 
			this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			resources.ApplyResources(this._toolStrip, "_toolStrip");
			this._toolStrip.Name = "_toolStrip";
			// 
			// LogForm
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this._logComboBox);
			this.Controls.Add(this._toolStrip);
			this.Name = "LogForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Rsdn.Janus.LogComboBox _logComboBox;
		private System.Windows.Forms.ToolStrip _toolStrip;
	}
}

