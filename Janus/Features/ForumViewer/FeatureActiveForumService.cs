using System;

using CodeJam.Extensibility;

using Rsdn.Janus.ObjectModel;

namespace Rsdn.Janus
{
	[Service(typeof(IActiveForumService))]
	public class FeatureActiveForumService : IActiveForumService, IDisposable
	{
		public FeatureActiveForumService()
		{
			Features.Instance.AfterFeatureActivate += AfterFeatureActivate;
		}

		#region IActiveForumService Members

		public IForum ActiveForum => Features.Instance.ActiveFeature as IForum;

		public event CodeJam.Extensibility.EventHandler<IActiveForumService> ActiveForumChanged; 

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			Features.Instance.AfterFeatureActivate -= AfterFeatureActivate;
		}

		#endregion

		private void AfterFeatureActivate(IFeature oldFeature,IFeature newFeature)
		{
			OnActiveForumChanged();
		}

		private void OnActiveForumChanged()
		{
			ActiveForumChanged?.Invoke(this);
		}
	}
}