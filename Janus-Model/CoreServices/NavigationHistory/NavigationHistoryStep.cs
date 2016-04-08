using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public class NavigationHistoryStep
	{
		private readonly Action _navigateAction;

		public NavigationHistoryStep(
			[NotNull] string displayName,
			[NotNull] Action navigateAction)
		{
			if (displayName == null)
				throw new ArgumentNullException(nameof(displayName));
			if (navigateAction == null)
				throw new ArgumentNullException(nameof(navigateAction));

			DisplayName = displayName;
			_navigateAction = navigateAction;
		}

		public string DisplayName { get; }

		public void Navigate()
		{
			_navigateAction();
		}
	}
}