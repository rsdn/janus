using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using CodeJam.Services;

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
		private readonly IServiceProvider _provider;
		private readonly Func<DockPanel> _dockPanelGetter;
		private const string _cfgFilePostfix = "DockingConfig.xml";

		private DockPanel _dockPanel;
		private readonly SortedDictionary<string, DockContent> _panes =
			new SortedDictionary<string, DockContent>();

		internal DockManager(IServiceProvider provider, Func<DockPanel> dockPanelGetter)
		{
			if (dockPanelGetter == null)
				throw new ArgumentNullException(nameof(dockPanelGetter));
			_provider = provider;
			_dockPanelGetter = dockPanelGetter;
		}

		public DockPanel DockPanel
		{
			get
			{
				if (_dockPanel == null)
				{
					_dockPanel = _dockPanelGetter();
					_dockPanel.ParentForm.FormClosing += DockManagerClosed;
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
			return
				_panes
					.Values
					.Cast<JanusDockPane>()
					.FirstOrDefault(jdp => jdp.Text == text);
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
			return DockPanel.ActiveDocument as ContentDummyForm;
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
				try
				{
					DockPanel.LoadFromXml(cfgName, DeserializeHandler);
				}
				catch (Exception ex)
				{
					var logger = _provider.GetService<ILogger>();
					logger?.LogError(ex.Message);
					DefaultInit();
				}
			else
				DefaultInit();
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

		private void DockManagerClosed(object sender, EventArgs e)
		{
			Save();
		}
	}
}
