using System;
using System.Collections.Generic;

using Rsdn.Janus.ObjectModel;
using Rsdn.SmartApp;
using Rsdn.TreeGrid;

namespace Rsdn.Janus
{
	[Service(typeof(IActiveOutboxItemService))]
	internal class FeatureActiveOutboxItemService : IActiveOutboxItemService, IDisposable
	{
		public FeatureActiveOutboxItemService()
		{
			Features.Instance.AfterFeatureActivate += AfterFeatureActivate;
			ApplicationManager.Instance.OutboxManager.OutboxForm.SelectedNodesChanged +=
				OutboxForm_SelectedNodesChanged;
		}

		#region IDisposable Members

		public void Dispose()
		{
			Features.Instance.AfterFeatureActivate -= AfterFeatureActivate;
			ApplicationManager.Instance.OutboxManager.OutboxForm.SelectedNodesChanged -=
				OutboxForm_SelectedNodesChanged;
		}

		#endregion

		#region IOutboxDummyFormService Members

		public List<ITreeNode> ActiveOutboxItems
		{
			get
			{
				return Features.Instance.ActiveFeature is OutboxFeature
					? ApplicationManager.Instance.OutboxManager.OutboxForm.SelectedNodes
					: new List<ITreeNode>();
			}
		}

		public event EventHandler ActiveOutboxItemsChanged;

		#endregion

		private void AfterFeatureActivate(IFeature oldFeature, IFeature newFeature)
		{
			if (oldFeature is OutboxFeature || newFeature is OutboxFeature)
				OnActiveOutboxItemsChanged();
		}

		void OutboxForm_SelectedNodesChanged(object sender, EventArgs e)
		{
			if (Features.Instance.ActiveFeature is OutboxFeature)
				OnActiveOutboxItemsChanged();
		}

		private void OnActiveOutboxItemsChanged()
		{
			if (ActiveOutboxItemsChanged != null)
				ActiveOutboxItemsChanged(this, EventArgs.Empty);
		}
	}
}