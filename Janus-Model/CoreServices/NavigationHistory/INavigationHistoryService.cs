using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public interface INavigationHistoryService
	{
		void SetHistoryStep([NotNull] NavigationHistoryStep step);
		void Back(int stepsCount);
		void Forward(int stepsCount);

		[NotNull]
		IList<NavigationHistoryStep> BackPath { get; }

		[NotNull]
		IList<NavigationHistoryStep> ForwardPath { get; }

		[NotNull]
		IObservable<EventArgs> Changed { get; }
	}
}