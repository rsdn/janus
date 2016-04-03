using System;
using System.Collections.Generic;
using System.Linq;

using CodeJam.Extensibility;

using Rsdn.Janus.ObjectModel;

namespace Rsdn.Janus
{
	[Service(typeof(IActiveMessagesService))]
	internal class FeatureActiveMessageService : IActiveMessagesService, IDisposable
	{
		private IMessagesFeature _activeMessagesFeature;

		public FeatureActiveMessageService()
		{
			Features.Instance.AfterFeatureActivate += AfterFeatureActivate;
			_activeMessagesFeature = Features.Instance.ActiveFeature as IMessagesFeature;
			if (_activeMessagesFeature != null)
				_activeMessagesFeature.ActiveMessagesChanged += Feature_ActiveMessagesChanged;
		}

		#region IActiveMessageService Members

		public IEnumerable<IForumMessageInfo> ActiveMessages =>
			_activeMessagesFeature?.ActiveMessages ?? Enumerable.Empty<IForumMessageInfo>();

		public event EventHandler ActiveMessagesChanged;

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			Features.Instance.AfterFeatureActivate -= AfterFeatureActivate;
			if (_activeMessagesFeature != null)
				_activeMessagesFeature.ActiveMessagesChanged -= Feature_ActiveMessagesChanged;
		}

		#endregion

		private void OnActiveMessagesChanged()
		{
			ActiveMessagesChanged?.Invoke(this, EventArgs.Empty);
		}

		private void AfterFeatureActivate(IFeature oldFeature, IFeature newFeature)
		{
			if (_activeMessagesFeature != null)
				_activeMessagesFeature.ActiveMessagesChanged -= Feature_ActiveMessagesChanged;

			_activeMessagesFeature = newFeature as IMessagesFeature;
			if (_activeMessagesFeature != null)
				_activeMessagesFeature.ActiveMessagesChanged += Feature_ActiveMessagesChanged;

			OnActiveMessagesChanged();
		}

		private void Feature_ActiveMessagesChanged(object sender, EventArgs e)
		{
			OnActiveMessagesChanged();
		}
	}
}