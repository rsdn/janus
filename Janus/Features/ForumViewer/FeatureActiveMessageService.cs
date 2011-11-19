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
			Features.Instance.AfterFeatureActivate += Feature_AfterFeatureActivate;
			_activeMessagesFeature = Features.Instance.ActiveFeature as IMessagesFeature;
			if (_activeMessagesFeature != null)
				_activeMessagesFeature.ActiveMessagesChanged += Feature_ActiveMessagesChanged;
		}

		#region IActiveMessageService Members

		public IEnumerable<IMsg> ActiveMessages
		{
			get
			{
				return _activeMessagesFeature != null
					? _activeMessagesFeature.ActiveMessages 
					: Enumerable.Empty<IMsg>();
			}
		}

		public event EventHandler ActiveMessagesChanged;

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			Features.Instance.AfterFeatureActivate -= Feature_AfterFeatureActivate;
			if (_activeMessagesFeature != null)
				_activeMessagesFeature.ActiveMessagesChanged -= Feature_ActiveMessagesChanged;
		}

		#endregion

		private void OnActiveMessagesChanged()
		{
			if (ActiveMessagesChanged != null)
				ActiveMessagesChanged(this, EventArgs.Empty);
		}

		private void Feature_AfterFeatureActivate(IFeature oldFeature, IFeature newFeature)
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