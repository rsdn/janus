using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Subjects;

using CodeJam.Extensibility;

namespace Rsdn.Janus
{
	[Service(typeof(INavigationHistoryService))]
	public class NavigationHistoryService : INavigationHistoryService, IDisposable
	{
		private readonly List<NavigationHistoryStep> _backPath = new List<NavigationHistoryStep>();
		private readonly List<NavigationHistoryStep> _forwardPath = new List<NavigationHistoryStep>();
		private readonly ReadOnlyCollection<NavigationHistoryStep> _backPathReadOnly;
		private readonly ReadOnlyCollection<NavigationHistoryStep> _forwardPathReadOnly;
		private readonly Subject<EventArgs> _changed = new Subject<EventArgs>();
		private NavigationHistoryStep _current;

		public NavigationHistoryService()
		{
			_backPathReadOnly = _backPath.AsReadOnly();
			_forwardPathReadOnly = _forwardPath.AsReadOnly();
		}

		#region Implementation of INavigationHistoryService

		public void SetHistoryStep(NavigationHistoryStep step)
		{
			if (step == null)
				throw new ArgumentNullException(nameof(step));

			if (_current != null)
				_backPath.Insert(0, _current);
			_forwardPath.Clear();

			_current = step;

			_changed.OnNext(EventArgs.Empty);
		}

		public void Back(int stepsCount)
		{
			if (stepsCount < 1 || stepsCount > _backPath.Count)
				throw new ArgumentOutOfRangeException(nameof(stepsCount));

			_forwardPath.Insert(0, _current);
			if (stepsCount > 1)
				_forwardPath.InsertRange(0, _backPath.GetRange(0, stepsCount - 1));

			_current = _backPath[stepsCount - 1];

			_backPath.RemoveRange(0, stepsCount);

			_current.Navigate();

			_changed.OnNext(EventArgs.Empty);
		}

		public void Forward(int stepsCount)
		{
			if (stepsCount < 1 || stepsCount > _forwardPath.Count)
				throw new ArgumentOutOfRangeException(nameof(stepsCount));

			_backPath.Insert(0, _current);
			if (stepsCount > 1)
				_backPath.InsertRange(0, _forwardPath.GetRange(0, stepsCount - 1));

			_current = _forwardPath[stepsCount - 1];

			_forwardPath.RemoveRange(0, stepsCount);

			_current.Navigate();

			_changed.OnNext(EventArgs.Empty);
		}

		public IList<NavigationHistoryStep> BackPath => _backPathReadOnly;

		public IList<NavigationHistoryStep> ForwardPath => _forwardPathReadOnly;

		public IObservable<EventArgs> Changed => _changed;
		#endregion

		#region Implementation of IDisposable

		public void Dispose()
		{
			_changed.OnCompleted();
		}

		#endregion
	}
}