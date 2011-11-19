using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public abstract class SimpleNavigationPageProviderBase : INavigationPageProvider
	{
		private readonly string _name;
		private readonly Func<IServiceProvider, NavigationPageState, INavigationPage> _pageCreator;

		protected SimpleNavigationPageProviderBase(
			[NotNull] string name,
			[NotNull] Func<IServiceProvider, NavigationPageState, INavigationPage> pageCreator)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (pageCreator == null)
				throw new ArgumentNullException("pageCreator");

			_name = name;
			_pageCreator = pageCreator;
		}

		#region Implementation of INavigationPageProvider

		public INavigationPage CreateNavigationPage(
			[NotNull] IServiceProvider serviceProvider,
			[NotNull] string name,
			NavigationPageState state)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException("serviceProvider");
			if (name == null)
				throw new ArgumentNullException("name");
			if (_name != name)
				throw new ApplicationException();

			return _pageCreator(serviceProvider, state);
		}

		public bool CanCreatePage(
			[NotNull] IServiceProvider serviceProvider,
			[NotNull] string name)
		{
			if (serviceProvider == null) 
				throw new ArgumentNullException("serviceProvider");
			if (name == null) 
				throw new ArgumentNullException("name");

			return _name == name;
		}
		#endregion
	}
}