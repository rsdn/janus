using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public interface INavigationPageService
	{
		[CanBeNull]
		INavigationPage CurrentPage { get; set; }

		[NotNull]
		IObservable<INavigationPage> CurrentPageChanged { get; }

		[NotNull]
		ICollection<INavigationPage> OpenedPages { get; }

		[NotNull]
		IObservable<PagesChangedEventArgs> OpenedPagesChanged { get; }

		void ShowPage(
			[NotNull] IServiceProvider serviceProvider, 
			[NotNull] string name,
			NavigationPageState state,
			bool replaceCurrentTab);

		bool CanShowPage(
			[NotNull] IServiceProvider serviceProvider, 
			[NotNull] string name);

		void ReloadPage(
			[NotNull] IServiceProvider serviceProvider,
			[NotNull] INavigationPage page);
	}
}