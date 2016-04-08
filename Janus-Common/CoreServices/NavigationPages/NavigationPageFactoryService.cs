using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using CodeJam.Extensibility;
using CodeJam.Extensibility.Registration;
using CodeJam.Services;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	[Service(typeof(INavigationPageFactoryService))]
	public class NavigationPageFactoryService : INavigationPageFactoryService
	{
		private readonly NavigationPageProviderHierarchy _providerHierarchy;

		public NavigationPageFactoryService([NotNull] IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));

			_providerHierarchy = CreatePageProvidersHierachy(CreatePageProviders(serviceProvider));
		}

		#region Implementation of INavigationPageFactoryService

		public INavigationPage CreatePage(
			IServiceProvider serviceProvider,
			string name,
			NavigationPageState state)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));
			if (name == null)
				throw new ArgumentNullException(nameof(name));

			var provider = TryFindProvider(serviceProvider, name);
			if (provider == null)
				throw new ApplicationException($"Отсутствуют провайдеры, способные создать страницу '{name}'");

			return provider.CreateNavigationPage(serviceProvider, name, state);
		}

		public bool CanCreatePage(IServiceProvider serviceProvider, string name)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));
			if (name == null)
				throw new ArgumentNullException(nameof(name));

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

		private static IEnumerable<Tuple<INavigationPageProvider, string>> CreatePageProviders(
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
						$"'{info.Type}' does not contains appropriate constructor.");

				yield return new Tuple<INavigationPageProvider, string>(
					(INavigationPageProvider)ctor.Invoke(new object[0]),
					info.PathMask);
			}
		}

		private static NavigationPageProviderHierarchy CreatePageProvidersHierachy(
			IEnumerable<Tuple<INavigationPageProvider, string>> providersWithMasks)
		{
			var result = new NavigationPageProviderHierarchy();
			foreach (var providerWithMask in providersWithMasks)
				result.Add(providerWithMask.Item2.Split('/'), providerWithMask.Item1);
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
				var pathArray = path.ToArray();
				var pathComponent = pathArray.FirstOrDefault();
				switch (pathComponent)
				{
					case null:
						if (Provider != null)
							throw new ApplicationException(
								"Провайдер страниц навигации с данной маской уже существует.");
						Provider = provider;
						break;
					case "*":
						if (pathArray.Skip(1).Any())
							throw new ApplicationException(
								"Компонент маски пути '*' поддерживается только в качестве последнего элемента пути.");
						if (ChildrensProvider != null)
							throw new ApplicationException(
								"Провайдер страниц навигации с данной маской уже существует.");
						ChildrensProvider = provider;
						break;
					default:
						var subNode = new NavigationPageProviderHierarchy();
						Debug.Assert(pathComponent != null, "pathComponent != null");
						_childrens.Add(pathComponent, subNode);
						subNode.CreateNodes(pathArray.Skip(1), provider);
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
			public Path<NavigationPageProviderHierarchy> TraversedPath { get; }
			public IEnumerable<string> PathTail { get; }

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