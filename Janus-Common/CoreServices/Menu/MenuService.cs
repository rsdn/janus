using System;
using System.Linq;
using System.Reactive.Subjects;

using CodeJam.Collections;
using CodeJam.Extensibility;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	[Service(typeof(IMenuService))]
	public sealed class MenuService : IMenuService
	{
		private readonly ILookup<string, IMenuProvider> _menuProvidersLookup;
		private readonly ILazyDictionary<string, IMenuRoot> _menuCache;
		private readonly Subject<string> _menuChanged = new Subject<string>();

		public MenuService([NotNull] IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));

			var menuProviders = new ExtensionsCache<MenuProviderInfo, IMenuProvider>(serviceProvider);

			_menuProvidersLookup =
				menuProviders
					.GetAllExtensions()
					.ToLookup(
						menuProvider => menuProvider.MenuName,
						StringComparer.OrdinalIgnoreCase);

			// WTF?
			//menuProviders
			//	.GetAllExtensions()
			//	.OfType<IDynamicMenuProvider>()
			//	.Select(
			//		dynamicMenuProvider =>
			//		{
			//			_menuCache.Drop(dynamicMenuProvider.MenuName);
			//			return 
			//				dynamicMenuProvider.MenuChanged.Subscribe(
			//					arg => _menuChanged.OnNext(dynamicMenuProvider.MenuName));
			//		});

			_menuCache = LazyDictionary.Create<string, IMenuRoot>(CreateMenu, false);
		}

		#region IMenuService Members

		public IMenuRoot GetMenu(string menuName)
		{
			if (menuName == null)
				throw new ArgumentNullException(nameof(menuName));
			if (!MenuNamesValidator.IsValidMenuName(menuName))
				throw new ArgumentException(@"Аргумент имеет некорректный формат.", nameof(menuName));
			if (!_menuProvidersLookup.Contains(menuName))
				throw new ApplicationException(
					$"Меню с идентификатором '{menuName}' не может создать ни один провайдер меню.");

			return _menuCache[menuName];
		}

		public IObservable<string> MenuChanged => _menuChanged;
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