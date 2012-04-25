using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using JetBrains.Annotations;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	[Service(typeof(INavigationTreeService))]
	public class NavigationTreeService : INavigationTreeService, IDisposable
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly Dictionary<string, Path<INavigationTreeNode>> _pageNodeMap =
			new Dictionary<string, Path<INavigationTreeNode>>();
		private readonly List<INavigationTreeNode> _nodes = new List<INavigationTreeNode>();
		private readonly ReadOnlyCollection<INavigationTreeNode> _nodesReadOnly;
		private readonly List<INavigationTreeNodeSource> _staticNodes = new List<INavigationTreeNodeSource>();
		private readonly Dictionary<IDynamicNavigationTreeProvider, INavigationTreeNodeSource[]> _dynamicNodes =
			new Dictionary<IDynamicNavigationTreeProvider, INavigationTreeNodeSource[]>();
		private readonly Subject<EventArgs> _treeChanged = new Subject<EventArgs>();

		public NavigationTreeService([NotNull] IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException("serviceProvider");

			_serviceProvider = serviceProvider;
			_nodesReadOnly = _nodes.AsReadOnly();

			foreach (var provider in 
				new ExtensionsCache<NavigationTreeProviderInfo, INavigationTreeProvider>(_serviceProvider)
					.GetAllExtensions())
			{
				var dynamicProvider = provider as IDynamicNavigationTreeProvider;
				if (dynamicProvider != null)
				{
					_dynamicNodes.Add(dynamicProvider, dynamicProvider.CreateNodes(_serviceProvider).ToArray());
					dynamicProvider.SubscribeNodesChanged(
						_serviceProvider,
						Observer.Create<EventArgs>(
							arg =>
							{
								_dynamicNodes[dynamicProvider] =
									dynamicProvider.CreateNodes(_serviceProvider).ToArray();
								UpdateTree();
								_treeChanged.OnNext(EventArgs.Empty);
							}));
				}
				else
					_staticNodes.AddRange(provider.CreateNodes(_serviceProvider));
			}
			UpdateTree();
		}

		#region Implementation of INavigationTreeService

		public IList<INavigationTreeNode> Nodes
		{
			get { return _nodesReadOnly; }
		}

		public IObservable<EventArgs> TreeChanged
		{
			get { return _treeChanged; }
		}

		public Path<INavigationTreeNode> GetPageNodePath(string page)
		{
			Path<INavigationTreeNode> result;
			return _pageNodeMap.TryGetValue(page, out result) ? result : null;
		}

		#endregion

		#region Implementation of IDisposable

		public void Dispose()
		{
			_treeChanged.OnCompleted();
		}

		#endregion

		#region Private Members

		private void UpdateTree()
		{
			_nodes.Clear();
			_pageNodeMap.Clear();
			_nodes.AddRange(MergeNodes(_staticNodes.Concat(_dynamicNodes.SelectMany(kvp => kvp.Value))));
			RegisterNodesPages(null, _nodes);
		}

		private void RegisterNodesPages(
			Path<INavigationTreeNode> parentPath,
			IEnumerable<INavigationTreeNode> nodes)
		{
			foreach (var node in nodes)
			{
				if (_pageNodeMap.ContainsKey(node.NavigationPageName))
					throw new ApplicationException(
						"Страница навигации '{0}' указана в нескольких элементах дерева навигации."
							.FormatStr(node.NavigationPageName));
				var nodePath = parentPath.Add(node);
				_pageNodeMap.Add(node.NavigationPageName, nodePath);
				RegisterNodesPages(nodePath, node.Childrens);
			}
		}

		private static IEnumerable<INavigationTreeNode> MergeNodes(
			IEnumerable<INavigationTreeNodeSource> nodeSources)
		{
			return 
				nodeSources
					.GroupBy(node => node.Name)
					.Select(MergeNode)
					.Sort(node => node.OrderIndex);
		}

		private static INavigationTreeNode MergeNode(
			IEnumerable<INavigationTreeNodeSource> nodeSources)
		{
			var first = nodeSources.OfType<INavigationTreeNode>().FirstOrDefault();
			if (first == null)
				throw new ApplicationException();
			return nodeSources.IsSingle()
				? first
				: new NavigationTreeNode(
					first.Name,
					first.Header,
					first.NavigationPageName,
					MergeNodes(
						nodeSources.SelectMany(
							source =>
							{
								var defintionNode = source as INavigationTreeNode;
								if (defintionNode != null)
									return defintionNode.Childrens;
								var extensionNode = source as INavigationTreeNodeExtension;
								if (extensionNode != null)
									return extensionNode.Childrens;
								return Enumerable.Empty<INavigationTreeNodeSource>();
							})),
						first.OrderIndex);
		}

		#endregion
	}
}