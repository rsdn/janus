using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	[Service(typeof(INavigationPageFactoryService))]
	public class NavigationPageFactoryService : INavigationPageFactoryService
	{
		private readonly NavigationPageProviderHierarchy _providerHierarchy;

		public NavigationPageFactoryService([NotNull] IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException("serviceProvider");

			_providerHierarchy = CreatePageProvidersHierachy(CreatePageProviders(serviceProvider));
		}

		#region Implementation of INavigationPageFactoryService

		public INavigationPage CreatePage(
			IServiceProvider serviceProvider,
			string name,
			NavigationPageState state)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException("serviceProvider");
			if (name == null)
				throw new ArgumentNullException("name");

			var provider = TryFindProvider(serviceProvider, name);
			if (provider == null)
				throw new ApplicationException(
					"Отсутствуют провайдеры, способные создать страницу '{0}'".FormatStr(name));

			return provider.CreateNavigationPage(serviceProvider, name, state);
		}

		public bool CanCreatePage(IServiceProvider serviceProvider, string name)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException("serviceProvider");
			if (name == null)
				throw new ArgumentNullException("name");

			return TryFindProvider(serviceProvider, name) != null;
		}

		#endregion

		#region Private Members

		[CanBeNull]
		private INavigationPageProvider TryFindProvider(IServiceProvider serviceProvider, string name)
		{
			var findResult = _providerHierarchy.FindNode(name.Split('/'), null);
			var traversedPath = findResult.TraversedPath;
			if (traversedPath == null)
				return null;

			if (!findResult.PathTail.Any()
					&& traversedPath.LastPathComponent.Provider != null
					&& traversedPath.LastPathComponent.Provider.CanCreatePage(serviceProvider, name))
				return traversedPath.LastPathComponent.Provider;

			while (traversedPath != null)
			{
				if (traversedPath.LastPathComponent.ChildrensProvider != null
						&& traversedPath.LastPathComponent.ChildrensProvider.CanCreatePage(serviceProvider, name))
					return traversedPath.LastPathComponent.ChildrensProvider;

				traversedPath = traversedPath.ParentPath;
			}

			return null;
		}

		private static IEnumerable<Pair<INavigationPageProvider, string>> CreatePageProviders(
			IServiceProvider serviceProvider)
		{
			var provSvc = serviceProvider.GetService<IRegElementsService<NavigationPageProviderInfo>>();

			if (provSvc == null)
				yield break;

			foreach (var info in provSvc.GetRegisteredElements())
			{
				var ctor = info.Type.GetConstructor(new Type[0]);
				if (ctor == null)
					throw new ApplicationException(
						"'{0}' does not contains appropriate constructor.".FormatStr(info.Type));

				yield return new Pair<INavigationPageProvider, string>(
					(INavigationPageProvider)ctor.Invoke(new object[0]),
					info.PathMask);
			}
		}

		private static NavigationPageProviderHierarchy CreatePageProvidersHierachy(
			IEnumerable<Pair<INavigationPageProvider, string>> providersWithMasks)
		{
			var result = new NavigationPageProviderHierarchy();
			foreach (var providerWithMask in providersWithMasks)
				result.Add(providerWithMask.Second.Split('/'), providerWithMask.First);
			return result;
		}

		#endregion

		#region NavigationPageProviderHierarchy class
		private sealed class NavigationPageProviderHierarchy
		{
			private readonly Dictionary<string, NavigationPageProviderHierarchy> _childrens
				= new Dictionary<string, NavigationPageProviderHierarchy>();

			[CanBeNull]
			public INavigationPageProvider Provider { get; private set; }

			[CanBeNull]
			public INavigationPageProvider ChildrensProvider { get; private set; }

			public void Add(
				IEnumerable<string> path,
				INavigationPageProvider provider)
			{
				var findResult = FindNode(path, null);
				findResult.TraversedPath.LastPathComponent.CreateNodes(findResult.PathTail, provider);
			}

			private void CreateNodes(
				IEnumerable<string> path,
				INavigationPageProvider provider)
			{
				var pathComponent = path.FirstOrDefault();
				switch (pathComponent)
				{
					case null:
						if (Provider != null)
							throw new ApplicationException(
								"Провайдер страниц навигации с данной маской уже существует.");
						Provider = provider;
						break;
					case "*":
						if (path.Skip(1).Any())
							throw new ApplicationException(
								"Компонент маски пути '*' поддерживается только в качестве последнего элемента пути.");
						if (ChildrensProvider != null)
							throw new ApplicationException(
								"Провайдер страниц навигации с данной маской уже существует.");
						ChildrensProvider = provider;
						break;
					default:
						var subNode = new NavigationPageProviderHierarchy();
						_childrens.Add(pathComponent, subNode);
						subNode.CreateNodes(path.Skip(1), provider);
						break;
				}
			}

			public FindNodeResult FindNode(
				[NotNull] IEnumerable<string> path,
				Path<NavigationPageProviderHierarchy> traversedPath)
			{
				var pathComponent = path.FirstOrDefault();
				NavigationPageProviderHierarchy node;
				
				return pathComponent != null && _childrens.TryGetValue(pathComponent, out node)
					? node.FindNode(path.Skip(1), traversedPath.Add(this))
					: new FindNodeResult(traversedPath.Add(this), path);
			}
		}

		private sealed class FindNodeResult
		{
			public Path<NavigationPageProviderHierarchy> TraversedPath { get; private set; }
			public IEnumerable<string> PathTail { get; private set; }

			public FindNodeResult(
				Path<NavigationPageProviderHierarchy> traversedPath,
				IEnumerable<string> pathTail)
			{
				TraversedPath = traversedPath;
				PathTail = pathTail;
			}
		}
		#endregion
	}
}