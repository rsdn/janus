using System;
using Rsdn.Janus.ObjectModel;
using Rsdn.SmartApp;

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

		public IForum ActiveForum
		{
			get { return Features.Instance.ActiveFeature as IForum; }
		}

		public event SmartApp.EventHandler<IActiveForumService> ActiveForumChanged; 

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
			if (ActiveForumChanged != null)
				ActiveForumChanged(this);
		}
	}
}