using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public class NavigationHistoryStep
	{
		private readonly string _displayName;
		private readonly Action _navigateAction;

		public NavigationHistoryStep(
			[NotNull] string displayName,
			[NotNull] Action navigateAction)
		{
			if (displayName == null)
				throw new ArgumentNullException("displayName");
			if (navigateAction == null)
				throw new ArgumentNullException("navigateAction");

			_displayName = displayName;
			_navigateAction = navigateAction;
		}

		public string DisplayName
		{
			get { return _displayName; }
		}

		public void Navigate()
		{
			_navigateAction();
		}
	}
}