namespace Rsdn.Janus
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin1 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
			WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin1 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient1 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient2 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient3 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient4 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient5 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient3 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient6 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient7 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			this._menuStrip = new System.Windows.Forms.MenuStrip();
			this._shortcutManager = new Rsdn.Shortcuts.ShortcutManager();
			this._dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
			this._navigationBox = new System.Windows.Forms.ComboBox();
			this._toolStrip = new System.Windows.Forms.ToolStrip();
			((System.ComponentModel.ISupportInitialize)(this._shortcutManager)).BeginInit();
			this.SuspendLayout();
			// 
			// _menuStrip
			// 
			this._menuStrip.Location = new System.Drawing.Point(0, 0);
			this._menuStrip.Name = "_menuStrip";
			this._menuStrip.Size = new System.Drawing.Size(784, 24);
			this._menuStrip.TabIndex = 1;
			// 
			// _shortcutManager
			// 
			this._shortcutManager.MainForm = this;
			this._shortcutManager.Nodes.Add(new Rsdn.Shortcuts.CustomShortcut(typeof(Rsdn.Janus.MainForm), "Главная форма", new Rsdn.Shortcuts.CustomShortcut[] {
				new Rsdn.Shortcuts.CustomShortcut(typeof(Rsdn.Janus.ForumDummyForm), "Дерево сообщений"),
				new Rsdn.Shortcuts.CustomShortcut(typeof(Rsdn.Janus.OutboxDummyForm), "К отправке"),
				new Rsdn.Shortcuts.CustomShortcut(typeof(Rsdn.Janus.MessageForm), "Редактор сообщений"),
				new Rsdn.Shortcuts.CustomShortcut(typeof(Rsdn.Janus.SearchDummyForm), "Поиск")}));
			// 
			// _dockPanel
			// 
			this._dockPanel.ActiveAutoHideContent = null;
			this._dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._dockPanel.DockBackColor = System.Drawing.SystemColors.Control;
			this._dockPanel.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
			this._dockPanel.Location = new System.Drawing.Point(0, 70);
			this._dockPanel.Name = "_dockPanel";
			this._dockPanel.Size = new System.Drawing.Size(784, 392);
			dockPanelGradient1.EndColor = System.Drawing.SystemColors.ControlLight;
			dockPanelGradient1.StartColor = System.Drawing.SystemColors.ControlLight;
			autoHideStripSkin1.DockStripGradient = dockPanelGradient1;
			tabGradient1.EndColor = System.Drawing.SystemColors.Control;
			tabGradient1.StartColor = System.Drawing.SystemColors.Control;
			tabGradient1.TextColor = System.Drawing.SystemColors.ControlDarkDark;
			autoHideStripSkin1.TabGradient = tabGradient1;
			dockPanelSkin1.AutoHideStripSkin = autoHideStripSkin1;
			tabGradient2.EndColor = System.Drawing.SystemColors.ControlLightLight;
			tabGradient2.StartColor = System.Drawing.SystemColors.ControlLightLight;
			tabGradient2.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripGradient1.ActiveTabGradient = tabGradient2;
			dockPanelGradient2.EndColor = System.Drawing.SystemColors.Control;
			dockPanelGradient2.StartColor = System.Drawing.SystemColors.Control;
			dockPaneStripGradient1.DockStripGradient = dockPanelGradient2;
			tabGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
			tabGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
			tabGradient3.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripGradient1.InactiveTabGradient = tabGradient3;
			dockPaneStripSkin1.DocumentGradient = dockPaneStripGradient1;
			tabGradient4.EndColor = System.Drawing.SystemColors.ActiveCaption;
			tabGradient4.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			tabGradient4.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
			tabGradient4.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
			dockPaneStripToolWindowGradient1.ActiveCaptionGradient = tabGradient4;
			tabGradient5.EndColor = System.Drawing.SystemColors.Control;
			tabGradient5.StartColor = System.Drawing.SystemColors.Control;
			tabGradient5.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripToolWindowGradient1.ActiveTabGradient = tabGradient5;
			dockPanelGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
			dockPanelGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
			dockPaneStripToolWindowGradient1.DockStripGradient = dockPanelGradient3;
			tabGradient6.EndColor = System.Drawing.SystemColors.GradientInactiveCaption;
			tabGradient6.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			tabGradient6.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
			tabGradient6.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripToolWindowGradient1.InactiveCaptionGradient = tabGradient6;
			tabGradient7.EndColor = System.Drawing.Color.Transparent;
			tabGradient7.StartColor = System.Drawing.Color.Transparent;
			tabGradient7.TextColor = System.Drawing.SystemColors.ControlDarkDark;
			dockPaneStripToolWindowGradient1.InactiveTabGradient = tabGradient7;
			dockPaneStripSkin1.ToolWindowGradient = dockPaneStripToolWindowGradient1;
			dockPanelSkin1.DockPaneStripSkin = dockPaneStripSkin1;
			this._dockPanel.Skin = dockPanelSkin1;
			this._dockPanel.TabIndex = 0;
			// 
			// _navigationBox
			// 
			this._navigationBox.Dock = System.Windows.Forms.DockStyle.Top;
			this._navigationBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._navigationBox.FormattingEnabled = true;
			this._navigationBox.Location = new System.Drawing.Point(0, 49);
			this._navigationBox.Name = "_navigationBox";
			this._navigationBox.Size = new System.Drawing.Size(784, 21);
			this._navigationBox.TabIndex = 3;
			// 
			// _toolStrip
			// 
			this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this._toolStrip.Location = new System.Drawing.Point(0, 24);
			this._toolStrip.Name = "_toolStrip";
			this._toolStrip.Size = new System.Drawing.Size(784, 25);
			this._toolStrip.TabIndex = 2;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 462);
			this.Controls.Add(this._dockPanel);
			this.Controls.Add(this._navigationBox);
			this.Controls.Add(this._toolStrip);
			this.Controls.Add(this._menuStrip);
			this.MainMenuStrip = this._menuStrip;
			this.Name = "MainForm";
			((System.ComponentModel.ISupportInitialize)(this._shortcutManager)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip _menuStrip;
		private Shortcuts.ShortcutManager _shortcutManager;
		private WeifenLuo.WinFormsUI.Docking.DockPanel _dockPanel;
		private System.Windows.Forms.ComboBox _navigationBox;
		private System.Windows.Forms.ToolStrip _toolStrip;
	}
}