using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public interface INavigationPageFactoryService
	{
		[NotNull]
		INavigationPage CreatePage(
			[NotNull] IServiceProvider serviceProvider,
			[NotNull] string name,
			NavigationPageState state);

		bool CanCreatePage(
			[NotNull] IServiceProvider serviceProvider,
			[NotNull] string name);
	}
}