using System;
using System.Reactive.Disposables;
using System.Windows.Forms;

using Rsdn.SmartApp;

using WeifenLuo.WinFormsUI.Docking;

namespace Rsdn.Janus
{
	[CheckStateSource]
	internal sealed class NavigationCheckStateSource : CheckStateSource
	{
		[CheckStateGetter("Janus.Navigation.NavigationWindowVisibility")]
		public CheckState? GetNavigationWindowVisibilityCheckState(IServiceProvider provider)
		{
			var dockManager = provider.GetService<DockManager>();
			if (dockManager == null)
				return null;
			var pane = dockManager.FindPaneByText(SR.Navigation.NavTree.DockName);
			if (pane == null)
				return null;
			return !(pane.DockState == DockState.Hidden || pane.DockState == DockState.Unknown)
				? CheckState.Checked : CheckState.Unchecked;
		}

		[CheckStateSubscriber("Janus.Navigation.NavigationWindowVisibility")]
		public IDisposable SubscrubeNavigationWindowVisibilityChanged(
			IServiceProvider serviceProvider, Action handler)
		{
			var dockManager = serviceProvider.GetService<DockManager>();
			if (dockManager == null)
				return Disposable.Empty;
			var pane = dockManager.FindPaneByText(SR.Navigation.NavTree.DockName);
			if (pane == null)
				return Disposable.Empty;

			EventHandler statusUpdater = (sender, e) => handler();
			pane.DockStateChanged += statusUpdater;
			return Disposable.Create(() => pane.DockStateChanged -= statusUpdater);
		}
	}
}
