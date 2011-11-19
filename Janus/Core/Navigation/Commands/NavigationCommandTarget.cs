using System;

using Rsdn.Janus.ObjectModel;
using Rsdn.SmartApp;

using WeifenLuo.WinFormsUI.Docking;

namespace Rsdn.Janus
{
	/// <summary>
	/// Обработчик команд навигации.
	/// </summary>
	[CommandTarget]
	internal sealed class NavigationCommandTarget : CommandTarget
	{
		public NavigationCommandTarget(IServiceProvider serviceProvider)
			: base(serviceProvider) { }

		[CommandExecutor("Janus.Navigation.SelectNextFeature")]
		public void ExecuteSelectNextFeature(ICommandContext context)
		{
			ApplicationManager.Instance.Navigator.SelectNext();
		}

		[CommandExecutor("Janus.Navigation.SelectPreviousFeature")]
		public void ExecuteSelectPreviousFeature(ICommandContext context)
		{
			ApplicationManager.Instance.Navigator.SelectPrevious();
		}

		[CommandExecutor("Janus.Navigation.SelectFeatureByKey")]
		public void ExecuteSelectFeatureByKey(ICommandContext context, string key)
		{
			Features.Instance.ActiveFeature = Features.Instance.FindFeatureByKey(key);
		}

		[CommandExecutor("Janus.Navigation.ToggleNavigationWindowVisibility")]
		public void ExecuteToggleNavigationWindowVisibility(ICommandContext context)
		{
			var dockManager = context.GetRequiredService<DockManager>();
			var pane = dockManager.FindPaneByText(SR.Navigation.NavTree.DockName);

			if (pane.DockState == DockState.Hidden || pane.DockState == DockState.Unknown)
				pane.Show(dockManager.DockPanel);
			else
				pane.Hide();
		}
	}
}