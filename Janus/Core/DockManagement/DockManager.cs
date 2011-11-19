using System;
using System.Collections.Generic;
using System.IO;

using Rsdn.Janus.Framework;
using Rsdn.Janus.Log;

using WeifenLuo.WinFormsUI.Docking;

namespace Rsdn.Janus
{
	/// <summary>
	/// Управляет докингом
	/// </summary>
	public class DockManager : IInitable
	{
		private readonly Func<DockPanel> _dockPanelGetter;
		private const string _cfgFilePostfix = "DockingConfig.xml";

		private DockPanel _dockPanel;
		private readonly SortedDictionary<string, DockContent> _panes =
			new SortedDictionary<string, DockContent>();

		internal DockManager(Func<DockPanel> dockPanelGetter)
		{
			if (dockPanelGetter == null)
				throw new ArgumentNullException("dockPanelGetter");
			_dockPanelGetter = dockPanelGetter;
		}

		public DockPanel DockPanel
		{
			get
			{
				if (_dockPanel == null)
				{
					_dockPanel = _dockPanelGetter();
					_dockPanel.ParentForm.FormClosing += DockManager_Closed;
				}
				return _dockPanel;
			}
		}

		#region IInitable Members

		public void Init()
		{
			Load();
		}

		#endregion

		public void RegisterPersistablePane(DockContent content)
		{
			_panes.Add(content.DockHandler.PersistString, content);
		}

		public JanusDockPane FindPaneByText(string text)
		{
			foreach (JanusDockPane jdp in _panes.Values)
				if (jdp.Text == text)
					return jdp;
			return null;
		}

		private ContentDummyForm CreateContentPane()
		{
			var cdf = new ContentDummyForm();
			cdf.Show(DockPanel);
			return cdf;
		}

		public ContentDummyForm QueryContentPane(bool useExisting)
		{
			if (!useExisting || DockPanel.DocumentsCount == 0)
				return CreateContentPane();
			return (ContentDummyForm)DockPanel.ActiveDocument;
		}

		public ContentDummyForm QueryExistingContentPane()
		{
			return DockPanel.ActiveDocument is ContentDummyForm
				? (ContentDummyForm)DockPanel.ActiveDocument : null;
		}

		private string GetConfigFileName()
		{
			return Path.Combine(LocalUser.DatabasePath,
				DockPanel.Name + _cfgFilePostfix);
		}

		private void Load()
		{
			var cfgName = GetConfigFileName();
			if (File.Exists(cfgName))
			{
				try
				{
					DockPanel.LoadFromXml(cfgName, DeserializeHandler);
				}
				catch (Exception ex)
				{
					ApplicationManager.Instance.Logger.LogError(ex.Message);
					DefaultInit();
				}
			}
			else
			{
				DefaultInit();
			}
		}

		private void DefaultInit()
		{
			foreach (var content in _panes.Values)
				content.Show(DockPanel);
		}

		private DockContent DeserializeHandler(string persistString)
		{
			DockContent content;
			_panes.TryGetValue(persistString, out content);
			return content;
		}

		private void Save()
		{
			DockPanel.SaveAsXml(GetConfigFileName());
		}

		private void DockManager_Closed(object sender, EventArgs e)
		{
			Save();
		}
	}
}
