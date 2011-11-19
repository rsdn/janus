using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public interface INavigationPageProvider
	{
		[NotNull]
		INavigationPage CreateNavigationPage(
			[NotNull] IServiceProvider serviceProvider,
			[NotNull] string name, 
			[CanBeNull] NavigationPageState state);

		bool CanCreatePage(
			[NotNull] IServiceProvider serviceProvider,
			[NotNull] string name);
	}
}