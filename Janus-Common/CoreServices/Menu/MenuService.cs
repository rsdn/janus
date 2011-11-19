using System;
using System.Linq;
using System.Reactive.Subjects;
using JetBrains.Annotations;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	[Service(typeof(IMenuService))]
	public sealed class MenuService : IMenuService
	{
		private readonly ILookup<string, IMenuProvider> _menuProvidersLookup;
		private readonly ElementsCache<string, IMenuRoot> _menuCache;
		private readonly Subject<string> _menuChanged = new Subject<string>();

		public MenuService([NotNull] IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException("serviceProvider");

			var menuProviders = new ExtensionsCache<MenuProviderInfo, IMenuProvider>(serviceProvider);

			_menuProvidersLookup =
				menuProviders
					.GetAllExtensions()
					.ToLookup(
						menuProvider => menuProvider.MenuName,
						StringComparer.OrdinalIgnoreCase);

			// WTF?
			menuProviders
				.GetAllExtensions()
				.OfType<IDynamicMenuProvider>()
				.Select(
					dynamicMenuProvider =>
					{
						_menuCache.Drop(dynamicMenuProvider.MenuName);
						return 
							dynamicMenuProvider.MenuChanged.Subscribe(
								arg => _menuChanged.OnNext(dynamicMenuProvider.MenuName));
					});
			
			_menuCache = new ElementsCache<string, IMenuRoot>(CreateMenu);
		}

		#region IMenuService Members

		public IMenuRoot GetMenu([NotNull] string menuName)
		{
			if (menuName == null)
				throw new ArgumentNullException("menuName");
			if (!MenuNamesValidator.IsValidMenuName(menuName))
				throw new ArgumentException(@"Агрумент имеет некорректный формат.", "menuName");
			if (!_menuProvidersLookup.Contains(menuName))
				throw new ApplicationException(
					"Меню с идентификатором '{0}' не может создать ни один провайдер меню.".FormatStr(menuName));

			return _menuCache.Get(menuName);
		}

		public IObservable<string> MenuChanged
		{
			get { return _menuChanged; }
		}

		#endregion

		#region Private Members

		private IMenuRoot CreateMenu(string menuName)
		{
			return
				MenuMergingHelper
					.MergeMenuRoots(_menuProvidersLookup[menuName].Select(menuProvider => menuProvider.CreateMenu()))
					.Single();
		}

		#endregion
	}
}