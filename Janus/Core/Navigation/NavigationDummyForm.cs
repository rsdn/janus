using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using CodeJam.Extensibility;
using CodeJam.Extensibility.EventBroker;
using CodeJam.Extensibility.Model;

using JetBrains.Annotations;

using Rsdn.Janus.Framework;
using Rsdn.Janus.ObjectModel;
using Rsdn.TreeGrid;

namespace Rsdn.Janus
{
	internal class NavigationDummyForm : JanusDockPane
	{
		private readonly IServiceProvider _provider;
		private readonly StripMenuGenerator _menuGenerator;
		private readonly IDisposable _eventsSubscription;
		private readonly JanusGrid _treeGrid;
		private readonly ContextMenuStrip _contextMenuStrip;
		private bool _selfActivation;

		private readonly DragStartDetector _dragStartDetector =
			new DragStartDetector();

		private readonly DragExpandDetector _dragExpandDetector =
			new DragExpandDetector();

		[ExpectService]
#pragma warning disable 169
		private IFavoritesManager _favManager = null;
#pragma warning restore 169

		[ExpectService]
#pragma warning disable 169
		private IOutboxManager _outboxManager = null;
#pragma warning restore 169

		public NavigationDummyForm([NotNull] IServiceProvider provider)
		{
			if (provider == null)
				throw new ArgumentNullException(nameof(provider));

			_provider = provider;

			this.AssignServices(provider);

			InitializeComponent();

			TabText = SR.Navigation.NavTree.DockName;
			Text = SR.Navigation.NavTree.DockName;

			_contextMenuStrip = new ContextMenuStrip();

			#region Инициализация грида

			_treeGrid = new JanusGrid
			{
				AllowDrop = true,
				ContextMenuStrip = _contextMenuStrip,
				Dock = DockStyle.Fill,
				Indent = Config.Instance.ForumDisplayConfig.GridIndent,
				FullRowSelect = true,
				HideSelection = false,
				MultiSelect = false
			};

			_treeGrid.Columns.AddRange(
				new[]
				{
					new ColumnHeader
						{
							Name = "Name",
							Text = SR.Navigation.NavTree.NameColumn
						},
					new ColumnHeader
						{
							Name = "Info",
							Text = SR.Navigation.NavTree.InfoColumn,
							TextAlign = HorizontalAlignment.Right
						}
				});

			_treeGrid.ColumnsWidth = Config.Instance.NavTreeColumnWidth;

			_treeGrid.AfterActivateNode += TreeGridAfterActivateNode;
			_treeGrid.ColumnWidthChanged += TreeGridColumnWidthChanged;
			_treeGrid.MouseDown += OnTreeGridMouseDown;
			_treeGrid.MouseMove += OnTreeGridMouseMove;
			_treeGrid.MouseUp += OnTreeGridMouseUp;
			_treeGrid.DragEnter += OnTreeGridDragEnter;
			_treeGrid.DragOver += OnTreeGridDragOver;
			//_treeGrid.DragDrop += OnTreeGridDragDrop;

			Controls.Add(_treeGrid);
			#endregion

			_menuGenerator = new StripMenuGenerator(_provider, _contextMenuStrip, "NavigationBox.ContextMenu");

			Features.Instance.AfterFeatureActivate += FeaturesAfterFeatureActivate;

			UpdateStyle();
			StyleConfig.StyleChange += StyleConfigStyleChange;

			_eventsSubscription = EventBrokerHelper.SubscribeEventHandlers(this, _provider);
		}

		public ImageList FeatureImages
		{
			get { return _treeGrid.SmallImageList; }
			set { _treeGrid.SmallImageList = value; }
		}

		internal void RefreshTree()
		{
			using (_treeGrid.UpdateScope())
			{
				_treeGrid.Nodes = null;
				_treeGrid.Nodes = Features.Instance;
				_treeGrid.ActiveNode = Features.Instance.ActiveFeature;
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_eventsSubscription.Dispose();
				_menuGenerator.Dispose();
				_treeGrid.Dispose();
				_contextMenuStrip.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NavigationDummyForm));
			this.SuspendLayout();
			// 
			// NavigationDummyForm
			// 
			resources.ApplyResources(this, "$this");
			this.Name = "NavigationDummyForm";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);

		}
		#endregion

		private void UpdateStyle()
		{
			_treeGrid.BackColor = StyleConfig.Instance.NavigationTreeBack;
			_treeGrid.Font = StyleConfig.Instance.NavigationTreeFont;
		}

		private void StyleConfigStyleChange(object s, StyleChangeEventArgs e)
		{
			UpdateStyle();
		}

		private void FeaturesAfterFeatureActivate(IFeature oldFeature, IFeature newFeature)
		{
			if (!_selfActivation)
				_treeGrid.ActiveNode = newFeature;
		}

		private void TreeGridAfterActivateNode(ITreeNode activatedNode)
		{
			try
			{
				_selfActivation = true;
				if (Features.Instance.ActiveFeature != activatedNode)
					Features.Instance.ActiveFeature = (Feature)activatedNode;
			}
			finally
			{
				_selfActivation = false;
			}
		}

		private void TreeGridColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
		{
			Config.Instance.NavTreeColumnWidth = _treeGrid.ColumnsWidth;
		}

		[EventHandler("Janus.Forum.AggregatesChanged")]
		private void AggregatesChanged(EventArgs arg)
		{
			_treeGrid.Invalidate();
		}

		#region TreeGrid Mouse Events

		private void OnTreeGridMouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				_dragStartDetector.MouseDown(e);
		}

		private void OnTreeGridMouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && _dragStartDetector.MouseMove(e))
			{
				if (_treeGrid.SelectedNodes.Count > 0)
					_treeGrid.DoDragDrop(new TreeNodeBag(_treeGrid.SelectedNodes), DragDropEffects.Move);

				_dragStartDetector.Reset();
				_dragExpandDetector.Reset();
			}
		}

		private void OnTreeGridMouseUp(object sender, MouseEventArgs e)
		{
			_dragStartDetector.MouseUp(e);

			if ((e.Button & MouseButtons.Right) != 0)
				_contextMenuStrip.Show(this, e.Location);
		}

		#endregion TreeGrid Mouse Events

		#region TreeGrid DragDrop

		/// <summary>
		/// Маска, указывающая на наличие в списке форумов подписанных и не подписанных форумов.
		/// </summary>
		[Flags]
		private enum ForumsSubscribtionStates
		{
			/// <summary>
			/// Неопределённое значение, соответствующее пустому списку форумов.
			/// </summary>
			None = 0,

			/// <summary>
			/// Все форумы в списке являются подписанными.
			/// </summary>
			Subscribed = 0x01,

			/// <summary>
			/// Все форумы в списке являются не подписанными.
			/// </summary>
			Unsubscribed = 0x02,

			/// <summary>
			/// В списке форумов присутствуют ка кподписанные, так и не подписанные форумы.
			/// </summary>
			Mixed = Subscribed | Unsubscribed,
		}

		/// <summary>
		/// Проверяет, являются ли подписанными или нет все форумы, доступные в параметре <c>forums</c>.
		/// </summary>
		/// <param name="forums">Набор форумов</param>
		/// <returns>Одно из значений <see cref="ForumsSubscribtionStates"/></returns>
		private static ForumsSubscribtionStates GetForumsSubscribedStatus(IEnumerable<ITreeNode> forums)
		{
			if (forums == null)
				throw new ArgumentNullException(nameof(forums));

			var states = ForumsSubscribtionStates.None;
			foreach (Forum forum in forums)
				if (states == ForumsSubscribtionStates.None)
					states = Forums.Instance.IsSubscribed(forum)
						? ForumsSubscribtionStates.Subscribed
						: ForumsSubscribtionStates.Unsubscribed;
				else if (Forums.Instance.IsSubscribed(forum)
					!= (states == ForumsSubscribtionStates.Subscribed))
					return ForumsSubscribtionStates.Mixed;

			return states;
		}

		/// <summary>
		/// Проверяет, являются ли подписанными или нет все форумы, доступные в параметре <c>forums</c>.
		/// </summary>
		/// <returns>
		/// <list type="table">
		///   <item>
		///     <term><c>0</c></term>
		///     <description>В наборе имеются как подписанные, так и не подписанные форумы.</description>
		///   </item>
		///   <item>
		///     <term><c>1</c></term>
		///     <description>В наборе имеются только подписанные форумы.</description>
		///   </item>
		///   <item>
		///     <term><c>-1</c></term>
		///     <description>В наборе имеются только не подписанные форумы.</description>
		///   </item>
		/// </list>
		/// </returns>
		//private static int GetForumsSubscribedStatus(IEnumerable<ITreeNode> forums)
		//{
		//  if(forums == null)
		//    throw new ArgumentNullException("forums");

		//  int subscribed = 0;
		//  foreach(Forum forum in forums)
		//    if(subscribed == 0)
		//      subscribed = Forums.Instance.IsSubscribed(forum) ? 1 : -1;
		//    else if(Forums.Instance.IsSubscribed(forum) != (subscribed == 1))
		//      return 0;
		//  return subscribed;
		//}

		private static void OnTreeGridDragEnter(object sender, DragEventArgs e)
		{
			e.Effect = e.Data.GetDataPresent(typeof(TreeNodeBag))
				? DragDropEffects.All
				: DragDropEffects.None;
		}

		private void OnTreeGridDragOver(object sender, DragEventArgs e)
		{
			var effects = DragDropEffects.None;

			var point = _treeGrid.PointToClient(new Point(e.X, e.Y));
			ITreeNode activeNode;
			int dummy;
			_treeGrid.GetNodeAndCellIndexAt(point.X, point.Y, out activeNode, out dummy);
			if (activeNode != null)
			{
				if (activeNode.HasChildren && (activeNode.Flags & NodeFlags.Expanded) == 0 && _dragExpandDetector.Detect())
				{
					_treeGrid.ExpandNode(activeNode);
					_treeGrid.Refresh();
				}//if

				if (e.Data.GetDataPresent(typeof(TreeNodeBag)))
				{
					var activeFeature = (IFeature)activeNode;
					var dropNodes = (TreeNodeBag)e.Data.GetData(typeof(TreeNodeBag));
					if (dropNodes != null && dropNodes.Count > 0)
						if (dropNodes.IsAssignableFrom(typeof(Forum)))
						{
							if (activeFeature is Forum || activeFeature is ForumFolder ||
								activeFeature == Forums.Instance)
							{
								var subscribtionStates = GetForumsSubscribedStatus(dropNodes);
								var allowDrop = false;
								if ((subscribtionStates & ForumsSubscribtionStates.Unsubscribed) ==
									ForumsSubscribtionStates.Unsubscribed)
									// Если среди перетаскиваемых форумов есть неподписанные, то переместить их можно только в "Подписанные".
									for (var node = activeNode;
										!allowDrop && node != null && node != Forums.Instance.UnsubscribedForums;
										node = node.Parent)
										allowDrop = node == Forums.Instance;
								else
									// Подписанные форумы можно перетащить куда угодно (и в "Подписанные" (изменение приоритета) и в "Неподписанные" (то есть отписка)).
									allowDrop = true;
								effects = allowDrop ? DragDropEffects.Move : DragDropEffects.Copy;
							} //if
						}
						else if (dropNodes.IsAssignableFrom(typeof(IMsg)))
							if (activeFeature == FavoritesFeature.Instance)
							{
								if (_dragExpandDetector.Detect())
								{
									var clientMouseLocation = _treeGrid.PointToClient(new Point(e.X, e.Y));
									var hitTestInfo = _treeGrid.HitTest(clientMouseLocation);
									if ((hitTestInfo.Location & ListViewHitTestLocations.Label) ==
										ListViewHitTestLocations.Label)
										_treeGrid.ActiveNode = FavoritesFeature.Instance;
								} //if
								effects = DragDropEffects.Link;
							}
							else if (activeNode == OutboxFeature.Instance)
								effects = DragDropEffects.Link;
							else if (dropNodes.IsAssignableFrom(typeof(IFavoritesEntry)))
								if (activeNode == OutboxFeature.Instance)
									effects = DragDropEffects.Link;
				}//if
			}//if

			e.Effect = effects;
		}

		private const int _minForumPriority = 0;
		private const int _maxForumPriority = 9;

		// Закомментировано до проведения рефакторинга на предмет обобщенияперемещения элементов.
		//private void OnTreeGridDragDrop(object sender, DragEventArgs e)
		//{
		//  var effect = DragDropEffects.None;

		//  var point = _treeGrid.PointToClient(new Point(e.X, e.Y));
		//  ITreeNode activeNode;
		//  int dummy;
		//  _treeGrid.GetNodeAndCellIndexAt(point.X, point.Y, out activeNode, out dummy);
		//  if (activeNode != null)
		//  {
		//    var dropNodes = (TreeNodeBag)e.Data.GetData(typeof(TreeNodeBag));
		//    if (dropNodes != null && dropNodes.Count > 0)
		//      if (dropNodes.IsAssignableFrom(typeof(Forum)) && e.Effect == DragDropEffects.Move)
		//      {
		//        bool? subscribe = null;
		//        // Что пользователь хочет сделать? Мышка отпущена над "Подписанными" или "Неподписанными"?
		//        for (var node = activeNode; node != null && subscribe == null; node = node.Parent)
		//          if (node == Forums.Instance)
		//            subscribe = true;
		//          else if (node == Forums.Instance.UnsubscribedForums)
		//            subscribe = false;

		//        if (subscribe.HasValue)
		//        {
		//          if (subscribe.Value) // Подписаться можно
		//          {
		//            var priority = _minForumPriority;
		//            if (activeNode == Forums.Instance)
		//              priority = _maxForumPriority;
		//            else if (activeNode is Forum)
		//              priority = ((Forum) activeNode).Priority;

		//            foreach (Forum forum in dropNodes)
		//              if (forum.Priority != priority)
		//                DatabaseManager.UpdateForumPriority(_provider, forum.ID, priority);
		//          }

		//          ForumsSubscriptionHelper.UpdateForumsSubscriptions(
		//            _provider, 
		//            dropNodes
		//              .OfType<Forum>()
		//              .Where(forum => Forums.Instance.IsSubscribed(forum) != subscribe)
		//              .Select(forum => new ForumSubscriptionRequest(forum.ID, subscribe.Value)),
		//            false);

		//          effect = DragDropEffects.Move;
		//        }
		//      }
		//      else if (dropNodes.IsAssignableFrom(typeof(IMsg)))
		//      // TODO: Обобщить обработку перемещения элементов в панели навигации.
		//      {
		//        if (activeNode == _favManager.RootFolder && e.Effect == DragDropEffects.Link)
		//        {
		//          var failedItems =
		//            dropNodes
		//              .Cast<IMsg>()
		//              .Where(
		//                message =>
		//                  !_favManager.AddMessageLink(message.ID, message.Subject, _favManager.RootFolder))
		//              .ToList();
		//          if (failedItems.Count > 0)
		//          {
		//            var failedMessages = new StringBuilder();
		//            failedMessages.Append(Environment.NewLine);
		//            foreach (var message in failedItems)
		//              failedMessages.AppendLine(String.Format("{0} - \"{1}\"", message.ID, message.Subject));
		//            var messageText = String.Format(SR.Favorites.ItemsExists, failedMessages,
		//              _favManager.RootFolder.Name);
		//            MessageBox.Show(this, messageText,
		//              FavoritesFeature.Instance.Info,
		//              MessageBoxButtons.OK, MessageBoxIcon.Warning);
		//          } //if

		//          effect = (dropNodes.Count > failedItems.Count) ? DragDropEffects.Link : DragDropEffects.None;
		//        }
		//        else if (activeNode == OutboxFeature.Instance && e.Effect == DragDropEffects.Link)
		//        {
		//          var failedItems =
		//            dropNodes
		//              .Cast<IMsg>()
		//              .Where(
		//                item =>
		//                  !_outboxManager.DownloadTopics.Add(SR.Forum.DownloadTopicRegetSource, item.ID, item.Subject))
		//              .ToList();
		//          if (failedItems.Count > 0)
		//          {
		//            var failedMessages = new StringBuilder();
		//            failedMessages.AppendLine();
		//            foreach (var message in failedItems)
		//              failedMessages.AppendLine(String.Format("{0} - \"{1}\"", message.ID, message.Subject));
		//            var messageText = String.Format(SR.Outbox.DownloadTopic.ItemsExists, failedMessages);
		//            MessageBox.Show(this, messageText,
		//              FavoritesFeature.Instance.Info,
		//              MessageBoxButtons.OK, MessageBoxIcon.Warning);
		//          } //if

		//          effect = (dropNodes.Count > failedItems.Count) ? DragDropEffects.Link : DragDropEffects.None;
		//        } //if
		//      }
		//      else if (dropNodes.IsAssignableFrom(typeof(IFavoritesEntry)) && e.Effect == DragDropEffects.Link)
		//      {
		//        if (activeNode == OutboxFeature.Instance)
		//        {
		//          var failedItems =
		//            GetFavoritesLinks(dropNodes)
		//              .Where(
		//                item =>
		//                  !_outboxManager.DownloadTopics.Add(SR.Forum.DownloadTopicRegetSource, item.MessageId, item.Comment))
		//              .ToList();
		//          if (failedItems.Count > 0)
		//          {
		//            var failedMessages = new StringBuilder();
		//            failedMessages.AppendLine();
		//            foreach (var message in failedItems)
		//              failedMessages.AppendLine(String.Format("{0} - \"{1}\"", message.MessageId,
		//                message.Comment));
		//            var messageText = String.Format(SR.Outbox.DownloadTopic.ItemsExists, failedMessages);
		//            MessageBox.Show(this, messageText,
		//              FavoritesFeature.Instance.Info,
		//              MessageBoxButtons.OK, MessageBoxIcon.Warning);
		//          } //if

		//          effect = (dropNodes.Count > failedItems.Count)
		//            ? DragDropEffects.Link
		//            : DragDropEffects.None;
		//        } //if
		//      } //if
		//  }//if

		//  e.Effect = effect;
		//}

		private static IEnumerable<FavoritesLink> GetFavoritesLinks(IEnumerable<ITreeNode> nodes)
		{
			if (nodes == null)
				throw new ArgumentNullException(nameof(nodes));

			foreach (IFavoritesEntry entry in nodes)
			{
				var link = entry as FavoritesLink;
				if (link != null)
					yield return link;
				else
				{
					var folder = entry as FavoritesFolder;
					if (folder != null)
						foreach (var item in GetFavoritesLinks(folder))
							yield return item;
				}//if
			}//for
		}

		private static IEnumerable<FavoritesLink> GetFavoritesLinks(FavoritesFolder folder)
		{
			if (folder == null)
				throw new ArgumentNullException(nameof(folder));

			foreach (var link in folder.Links)
				yield return link;
			foreach (var subFolder in folder.SubFolders)
				foreach (var link in GetFavoritesLinks(subFolder))
					yield return link;
		}

		#endregion TreeGrid DragDrop
	}
}
