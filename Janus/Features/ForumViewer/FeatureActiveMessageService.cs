using System;
using System.Collections.Generic;
using System.Linq;

using Rsdn.Janus.ObjectModel;
using Rsdn.SmartApp;

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

		public IEnumerable<IForumMessageInfo> ActiveMessages
		{
			get
			{
				return
					_activeMessagesFeature != null
						? _activeMessagesFeature.ActiveMessages.Cast<IForumMessageInfo>() 
						: Enumerable.Empty<IForumMessageInfo>();
			}
		}

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
			if (ActiveMessagesChanged != null)
				ActiveMessagesChanged(this, EventArgs.Empty);
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