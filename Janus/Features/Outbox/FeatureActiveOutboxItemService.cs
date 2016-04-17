using System;
using System.Collections.Generic;

using CodeJam.Extensibility;
using CodeJam.Services;

using Rsdn.Janus.ObjectModel;
using Rsdn.TreeGrid;

namespace Rsdn.Janus
{
	[Service(typeof(IActiveOutboxItemService))]
	internal class FeatureActiveOutboxItemService : IActiveOutboxItemService, IDisposable
	{
		private readonly OutboxManager _outboxManager;
		public FeatureActiveOutboxItemService(IServiceProvider provider)
		{
			Features.Instance.AfterFeatureActivate += AfterFeatureActivate;
			_outboxManager = (OutboxManager)provider.GetRequiredService<IOutboxManager>();
			_outboxManager.OutboxForm.SelectedNodesChanged += OutboxForm_SelectedNodesChanged;
		}

		#region IDisposable Members

		public void Dispose()
		{
			Features.Instance.AfterFeatureActivate -= AfterFeatureActivate;
			_outboxManager.OutboxForm.SelectedNodesChanged -=
				OutboxForm_SelectedNodesChanged;
		}

		#endregion

		#region IOutboxDummyFormService Members

		public List<ITreeNode> ActiveOutboxItems =>
			Features.Instance.ActiveFeature is OutboxFeature
				? _outboxManager.OutboxForm.SelectedNodes
				: new List<ITreeNode>();

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
			ActiveOutboxItemsChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}