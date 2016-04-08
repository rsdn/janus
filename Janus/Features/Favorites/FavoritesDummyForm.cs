using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CodeJam.Services;

using Rsdn.Janus.Framework;
using Rsdn.Janus.ObjectModel;
using Rsdn.Shortcuts;
using Rsdn.TreeGrid;

namespace Rsdn.Janus
{
	/// <summary>
	/// Контрол для отображения подсистемы "Избранное"
	/// </summary>
	public sealed partial class FavoritesDummyForm : UserControl, IFeatureView
	{
		private readonly ServiceContainer _serviceManager;
		private readonly StripMenuGenerator _contextMenuGenerator;
		private readonly DragStartDetector _dragStartDetector = new DragStartDetector();
		private readonly DragExpandDetector _dragExpandDetector = new DragExpandDetector();
		private MsgViewer _msgViewer;
		private readonly IFavoritesManager _favManager;

		#region Images
		private readonly ImageList _treeImages = new ImageList();

		private void FillImages()
		{
			var sim = _serviceManager.GetRequiredService<IStyleImageManager>();
			const string prefix = @"MessageTree\";

			//иконки для папок избранного
			FolderStartIndex =
				sim.AppendImage(prefix + "Favorites", StyleImageType.ConstSize, _treeImages);
			EmptyFolderStartIndex =
				sim.AppendImage(prefix + "FavoritesEmpty", StyleImageType.ConstSize, _treeImages);

			// линк в избранном
			MsgLinkIndex =
				sim.AppendImage(prefix + "Link", StyleImageType.ConstSize, _treeImages);
		}

		internal ImageList TreeImages => _treeImages;

		internal int EmptyFolderStartIndex { get; private set; }
		internal int FolderStartIndex { get; private set; }
		internal int MsgLinkIndex { get; private set; }
		#endregion

		#region Constructor(s) & Реализация Singleton

		private FavoritesDummyForm(IServiceProvider provider)
		{
			_serviceManager = new ServiceContainer(provider);
			_favManager = provider.GetRequiredService<IFavoritesManager>();
			InitializeComponent();
			FillImages();
			CustomInitializeComponent();
			_serviceManager.Publish<IFavoritesDummyFormService>(new FavoritesDummyFormService(this));
			_contextMenuGenerator = new StripMenuGenerator(_serviceManager, _contextMenuStrip, "Favorites.ContextMenu");
		}

		public static FavoritesDummyForm Instance { get; } =
			new FavoritesDummyForm(ApplicationManager.Instance.ServiceProvider);
		#endregion

		#region Инициализация

		private void CustomInitializeComponent()
		{
			StyleConfig.StyleChange += OnStyleChanged;
			UpdateStyle();

			_msgViewer = new MsgViewer(_serviceManager) { Dock = DockStyle.Fill };
			_splitContainer.Panel2.Controls.Add(_msgViewer);

			_grid.Columns[0].Text = SR.TGColumnNameLink;
			_grid.Columns[1].Text = SR.TGColumnComment;

			_grid.SmallImageList = _treeImages;

			if (Config.Instance.FavoritesColumnOrder.Length == _grid.Columns.Count)
				_grid.ColumnsOrder = Config.Instance.FavoritesColumnOrder;
			if (Config.Instance.FavoritesColumnWidth.Length == _grid.Columns.Count)
				_grid.ColumnsWidth = Config.Instance.FavoritesColumnWidth;

			_favManager.FavoritesReloaded += FavoritesReloaded;

			_grid.Indent = Config.Instance.ForumDisplayConfig.GridIndent;
		}

		#endregion

		#region Обработчики событий

		private void OnStyleChanged(object sender, StyleChangeEventArgs args)
		{
			UpdateStyle();
		}

		private void FavoritesReloaded(object sender, EventArgs e)
		{
			MethodInvoker mi = delegate
			{
				LastActiveNode = null;
				Refresh();
			};
			if (InvokeRequired)
				Invoke(mi);
			else
				mi();
		}

		private void GridAfterActivateNode(ITreeNode activatedNode)
		{
			var activeLink = activatedNode as FavoritesLink;
			if (activeLink != null)
				_msgViewer.NavigateToUrl(activeLink.Link);
			else
				_msgViewer.Msg = null;

			LastActiveNode = activatedNode;

			OnSelectedEntriesChanged(EventArgs.Empty);
		}

		private void GridColumnClick(object sender, ColumnClickEventArgs e)
		{
			var sortType = Config.Instance.FavoritesMessagesSortCriteria;
			var folderSortDir = Config.Instance.FavoritesFoldersSortDirection;

			if (e.Column == 0)
			{
				sortType = sortType == SortType.BySubjectAsc
					? SortType.BySubjectDesc
					: SortType.BySubjectAsc;
			}

			if (Config.Instance.FavoritesMessagesSortCriteria != sortType)
			{
				Config.Instance.FavoritesMessagesSortCriteria = sortType;
				_favManager.RootFolder.SortLinks(sortType);
			}

			if (Config.Instance.FavoritesFoldersSortDirection != folderSortDir)
			{
				Config.Instance.FavoritesFoldersSortDirection = folderSortDir;
				_favManager.RootFolder.SortFolders(folderSortDir);
			}

			Refresh();
		}

		private void GridDragEnter(object sender, DragEventArgs e)
		{
			e.Effect = e.Data.GetDataPresent(typeof(TreeNodeBag))
				? DragDropHelper.GetDropEffect(e.KeyState, true)
				: DragDropEffects.None;
		}

		private void GridDragOver(object sender, DragEventArgs e)
		{
			var effects = DragDropEffects.None;

			if (sender == _grid)
			{
				if (e.Data.GetDataPresent(typeof(TreeNodeBag)))
				{
					int dummy;
					var point = _grid.PointToClient(new Point(e.X, e.Y));
					ITreeNode activeNode;
					_grid.GetNodeAndCellIndexAt(point.X, point.Y, out activeNode, out dummy);
					if (activeNode == null)
						activeNode = _favManager.RootFolder;

					if (activeNode != null)
					{
						var activeEntry = (IFavoritesEntry)activeNode;
						if (activeEntry.HasChildren &&
							(activeEntry.Flags & NodeFlags.Expanded) == 0 &&
							_dragExpandDetector.Detect())
						{
							_grid.ExpandNode(activeEntry);
							_grid.Refresh();
						}//if

						var dropNodes = (TreeNodeBag)e.Data.GetData(typeof(TreeNodeBag));
						if (dropNodes != null && dropNodes.Count > 0)
							if (dropNodes.IsAssignableFrom(typeof(IFavoritesEntry)))
							{
								activeEntry = activeEntry.IsContainer
									? (IFavoritesEntry)activeNode
									: (IFavoritesEntry)activeNode.Parent;
								Debug.Assert(activeEntry != null, "activeEntry != null");

								if (!dropNodes.ContainsRecursion(activeEntry))
								{
									effects = DragDropHelper.GetDropEffect(e.KeyState, true);
									// Копируем только сообщения - папки пока копировать не умеем, 
									// так как их, по-хорошему, надо копировать рекурсивно, с подпапками и сообщениями.
									// TODO: Доделать объектную модель Избранного, добавив возможность рекурсивного копирования папок.
									if ((effects == DragDropEffects.Copy && !dropNodes.IsAssignableFrom(typeof(FavoritesLink))) ||
										effects == DragDropEffects.Link)
										effects = DragDropEffects.None;
								} //if
							}
							else if (dropNodes.IsAssignableFrom(typeof(IMsg)))
								effects = DragDropEffects.Link;
					}//if
				}//if

				e.Effect = effects;
			}//if
		}

		private void GridDragDrop(object sender, DragEventArgs e)
		{
			var effect = DragDropEffects.None;

			if (sender == _grid)
			{
				int dummy;
				var point = _grid.PointToClient(new Point(e.X, e.Y));

				ITreeNode activeNode;
				_grid.GetNodeAndCellIndexAt(point.X, point.Y, out activeNode, out dummy);
				if (activeNode == null)
					activeNode = _favManager.RootFolder;

				var activeEntry = activeNode as IFavoritesEntry;
				if (activeEntry != null)
				{
					var activeFolder = activeEntry.IsContainer ? (FavoritesFolder)activeEntry : (FavoritesFolder)activeEntry.Parent;
					if (e.Data.GetDataPresent(typeof(TreeNodeBag)))
					{
						var dropNodes = (TreeNodeBag)e.Data.GetData(typeof(TreeNodeBag));
						if (dropNodes != null && dropNodes.Count > 0)
						{
							var select = LastActiveNode ?? _grid.ActiveNode;
							if (dropNodes.IsAssignableFrom(typeof(IFavoritesEntry)))
							{
								var copy = DragDropHelper.GetDropEffect(e.KeyState, true) == DragDropEffects.Copy;
								var failedItems = new List<IFavoritesEntry>();
								ProgressWorker.Run(_serviceManager, false,
									pi =>
									{
										pi.SetProgressText(SR.Favorites.MovingItems);
										// При одновременном перемещении контейнера и его элементов, 
										// элементы оказываются не в "своём" контейнере, а в том, где отпустили мышь.
										// Так и задумано
										foreach (IFavoritesEntry entry in dropNodes)
										{
											if (copy)
											{
												Debug.Assert(entry is FavoritesLink, "entry is FavoritesLink");
												var link = (FavoritesLink)entry;
												if (!_favManager.AddMessageLink(link.MessageId, link.Comment, activeFolder))
													failedItems.Add(entry);
											}
											else if (!entry.Move(activeFolder))
												failedItems.Add(entry);
										}
									},
									() =>
									{
										_favManager.Reload();

										if (failedItems.Count > 0)
										{
											var failedMessages = new StringBuilder();
											failedMessages.Append(Environment.NewLine);
											foreach (var entry in failedItems)
											{
												var itemText = string.IsNullOrEmpty(entry.Comment)
													? (entry is FavoritesLink
														? ((FavoritesLink)entry).Link
														: ((FavoritesFolder)entry).Name)
													: entry.Comment;
												failedMessages.AppendFormat(" \"{0}\"{1}", itemText, Environment.NewLine);
											}
											var message = String.Format(SR.Favorites.ItemsExists, failedMessages, activeFolder.Name);
											MessageBox.Show(this, message,
												FavoritesFeature.Instance.Info,
												MessageBoxButtons.OK, MessageBoxIcon.Warning);
										}

										effect = (dropNodes.Count > failedItems.Count) ? (copy ? DragDropEffects.Copy : DragDropEffects.Move) : DragDropEffects.None;
									});
							}
							else if (dropNodes.IsAssignableFrom(typeof(IMsg)))
							{
								var failedItems =
									dropNodes
										.Cast<IMsg>()
										.Where(
											message =>
												!_favManager.AddMessageLink(message.ID, message.Subject, activeFolder))
										.ToList();

								if (failedItems.Count > 0)
								{
									var failedMessages = new StringBuilder();
									failedMessages.Append(Environment.NewLine);
									foreach (var message in failedItems)
										failedMessages.AppendFormat("{0} - \"{1}\"{2}",
											message.ID, message.Subject, Environment.NewLine);
									var messageText = String.Format(SR.Favorites.ItemsExists, failedMessages, activeFolder.Name);
									MessageBox.Show(this, messageText,
										FavoritesFeature.Instance.Info,
										MessageBoxButtons.OK, MessageBoxIcon.Warning);
								}//if

								effect = (dropNodes.Count > failedItems.Count) ? DragDropEffects.Link : DragDropEffects.None;
							}//if

							if (select != null)
							{
								_grid.EnsureVisible(select, false);
								if (_grid.ActiveNode != select)
									_grid.ActiveNode = select;
							}//if
						}//if
					}//if
				}//if
			}//if

			e.Effect = effect;
		}

		private void GridMouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				_dragStartDetector.MouseDown(e);

			//Нафига это нужно, если LastActiveNode обновляется в _grid_AfterActivateNode?
			//ITreeNode node;
			//int column;
			//_favView.GetNodeAndCellIndexAt(e.X, e.Y, out node, out column);
			//if (node == null)
			//{
			//	LastActiveNode = null;
			//	Refresh();
			//}
		}

		private void GridMouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && _dragStartDetector.MouseMove(e))
			{
				if (_grid.SelectedNodes.Count > 0)
					_grid.DoDragDrop(
						new TreeNodeBag(_grid.SelectedNodes),
						DragDropEffects.Move | DragDropEffects.Copy | DragDropEffects.Link);

				_dragStartDetector.Reset();
				_dragExpandDetector.Reset();
			}
		}

		private void GridMouseUp(object sender, MouseEventArgs e)
		{
			_dragStartDetector.MouseUp(e);

			if (e.Button == MouseButtons.Right)
				_contextMenuStrip.Show(_grid, e.Location);
		}

		private void GridDoubleClick(object sender, EventArgs e)
		{
			//ToDo: defaultCmd
		}

		private void GridColumnReordered(object sender, ColumnReorderedEventArgs e)
		{
			Config.Instance.FavoritesColumnOrder = _grid.ColumnsOrder;
		}

		private void GridColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
		{
			Config.Instance.FavoritesColumnWidth = _grid.ColumnsWidth;
		}

		#endregion

		public IList<IFavoritesEntry> SelectedEntries
		{
			get
			{
				return _grid.SelectedNodes.ConvertAll(node => (IFavoritesEntry)node);
			}
		}

		public event EventHandler SelectedEntriesChanged;

		public ITreeNode LastActiveNode { get; set; }

		#region Обработчики команд меню

		[MethodShortcut(Shortcut.CtrlP, "Создать раздел", "Создать раздел")]
		private void AddFolder()
		{
			_serviceManager.TryExecuteCommand("Favorites.Commands.CreateFolder.DisplayName", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.Del, "Удалить элемент", "Удалить элемент")]
		private void DeleteItem()
		{
			_serviceManager.TryExecuteCommand("Favorites.Commands.DeleteItem.DisplayName", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.F2, "Переименовать раздел", "Переименовать раздел")]
		private void RenameFolder()
		{
			_serviceManager.TryExecuteCommand("Favorites.Commands.RenameFolder.DisplayName", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.CtrlM, "Переместить в раздел", "Переместить в раздел")]
		private void MoveItem()
		{
			_serviceManager.TryExecuteCommand("Janus.Favorites.MoveItem", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.CtrlF2, "Редактировать элемент", "Редактировать элемент")]
		private void EditItem()
		{
			_serviceManager.TryExecuteCommand("Favorites.Commands.EditLink.DisplayName", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.CtrlG, "Перейти к сообщению", "Перейти к сообщению")]
		private void GotoMessage()
		{
			_serviceManager.TryExecuteCommand("Favorites.Commands.OpenLink.DisplayName", new Dictionary<string, object>());
		}

		#endregion

		#region IFeatureView

		void IFeatureView.Activate(IFeature feature)
		{
			Refresh();
		}

		public override void Refresh()
		{
			_grid.Nodes = _favManager.RootFolder;

			if (_grid.ActiveNode != LastActiveNode)
				_grid.ActiveNode = LastActiveNode;
		}

		void IFeatureView.ConfigChanged()
		{
			_grid.Indent = Config.Instance.ForumDisplayConfig.GridIndent;
			_grid.GridLines = Config.Instance.ForumDisplayConfig.MsgListGridLines;
		}

		#endregion

		private void UpdateStyle()
		{
			_grid.BackColor = Config.Instance.StyleConfig.MessageTreeBack;
			_grid.Font = Config.Instance.StyleConfig.MessageTreeFont;
		}

		private void OnSelectedEntriesChanged(EventArgs e)
		{
			SelectedEntriesChanged?.Invoke(this, e);
		}
	}
}