using System;
using System.Collections;

using Rsdn.Janus.Framework;
using Rsdn.TreeGrid;

using CodeJam.Services;

namespace Rsdn.Janus
{
	/// <summary>
	/// Класс представляющий папку Избранного
	/// </summary>
	public class FavoritesFolder : IFavoritesEntry, IGetData
	{
		private CollectionWithEvents<FavoritesLink> _links;
		private bool _showLinks;
		private CollectionWithEvents<FavoritesFolder> _subFolders;
		private readonly Lazy<IFavoritesManager> _manager;

		public FavoritesFolder(IServiceProvider provider)
		{
			Flags = NodeFlags.None;
			_showLinks = true;
			_manager =
				new Lazy<IFavoritesManager>(provider.GetRequiredService<IFavoritesManager>);
		}

		public string Name { get; set; }
		public int ParentId { get; set; }

		public bool ShowLinks
		{
			get { return _showLinks; }
			set
			{
				_showLinks = value;
				foreach (var subFolder in SubFolders)
					subFolder.ShowLinks = value;
			}
		}

		public CollectionWithEvents<FavoritesLink> Links
		{
			get
			{
				if (_links == null)
				{
					_links = new CollectionWithEvents<FavoritesLink>();
					_links.BeforeInsert +=
						delegate(object sender, BeforeInsertEventArgs<FavoritesLink> ea) { ea.Item.Parent = this; };
					_links.BeforeSet +=
						delegate(object sender, BeforeSetEventArgs<FavoritesLink> ea) { ea.Item.Parent = this; };
				}
				return _links;
			}
		}

		public CollectionWithEvents<FavoritesFolder> SubFolders
		{
			get
			{
				if (_subFolders == null)
				{
					_subFolders = new CollectionWithEvents<FavoritesFolder>();
					_subFolders.BeforeInsert +=
						delegate(object sender, BeforeInsertEventArgs<FavoritesFolder> ea) { ea.Item.Parent = this; };
					_subFolders.BeforeSet +=
						delegate(object sender, BeforeSetEventArgs<FavoritesFolder> ea) { ea.Item.Parent = this; };
				}
				return _subFolders;
			}
		}

		#region IGetData Members
		public void GetData(NodeInfo nodeInfo, CellInfo[] cellData)
		{
//			int commentCol = ShowLinks ? (int) FavoritesGridColumns.Comment
//				: (int) FavoritesGridColumns.Name;

			cellData[(int)FavoritesGridColumns.Name].Text = Name;
			cellData[(int)FavoritesGridColumns.Name].ImageIndex =
				Links.Count > 0
					? FavoritesDummyForm.Instance.FolderStartIndex
					: FavoritesDummyForm.Instance.EmptyFolderStartIndex;

			nodeInfo.Highlight = true;

//			if (ShowLinks)
//			{
//				cellData[(int) FavoritesGridColumns.ColSubj].Image = -1;
//				cellData[(int) FavoritesGridColumns.ColSubj].Text = string.Empty;
//				cellData[(int) FavoritesGridColumns.ColAuthor].Image = -1;
//				cellData[(int) FavoritesGridColumns.ColAuthor].Text = string.Empty;
//				cellData[(int) FavoritesGridColumns.ColDate].Image = -1;
//				cellData[(int) FavoritesGridColumns.ColDate].Text = string.Empty;
//				cellData[(int) FavoritesGridColumns.ColForum].Image = -1;
//				cellData[(int) FavoritesGridColumns.ColForum].Text = string.Empty;
//			}
//			cellData[commentCol].Text = Comment.ToString();
//			cellData[commentCol].Image = -1;

			cellData[(int)FavoritesGridColumns.Comment].Text = Comment;
		}
		#endregion

		#region Служебные функции
//		public int FindMessageById(int msgId)
//		{
//			int index = -1;
//			for (int i = 0; i < Links.Count; i++)
//			{
//				FavoritesLink msg = Links[i];
//
//				if (msg.ID == msgId)
//				{
//					index = i;
//					break;
//				}
//			}
//			return index;
//		}

		public void SortFolders(SortDirection sortDir)
		{
			SubFolders.Sort(new FavoritesFolderComparer(sortDir));
			foreach (var folder in SubFolders)
				folder.SortFolders(sortDir);
		}

		public void SortLinks(SortType sortType)
		{
			Links.Sort(new MsgComparer<FavoritesLink>(sortType));
			foreach (var folder in SubFolders)
				folder.SortLinks(sortType);
		}
		#endregion

		public static FavoritesFolder CreateInstance(
			IServiceProvider provider,
			int id,
			string name,
			string comment,
			FavoritesFolder parent)
		{
			var folder =
				new FavoritesFolder(provider)
					{
						Id = id,
						Name = name,
						Comment = comment,
						Parent = parent
					};

			return folder;
		}

		#region IFavoritesEntry
		private IFavoritesEntry _parent;
		public int Id { get; set; }
		public string Comment { get; set; }

		public IFavoritesEntry Parent
		{
			get { return _parent; }
			set { _parent = value; }
		}

		public bool IsContainer => true;

		public void Update()
		{
			_manager.Value.Update(this);
		}

		public void Delete()
		{
			_manager.Value.Delete(this);
		}

		public bool Move(IFavoritesEntry newParent)
		{
			return
				newParent != this && _manager.Value.Move(this, (FavoritesFolder)newParent);
		}
		#endregion

		#region ITreeNode
		ITreeNode ITreeNode.Parent => _parent;

		public NodeFlags Flags { get; set; }

		public bool HasChildren => SubFolders.Count > 0 || ShowLinks && Links.Count > 0;

		public ITreeNode this[int index] =>
			index < SubFolders.Count
				? SubFolders[index]
				: (ITreeNode)Links[index - SubFolders.Count];
		#endregion

		#region ICollection
		public void CopyTo(Array array, int index)
		{
			throw new NotSupportedException();
		}

		public int Count
		{
			get
			{
				var count = SubFolders.Count;
				if (ShowLinks)
					count += Links.Count;
				return count;
			}
		}

		public object SyncRoot
		{
			get { throw new NotSupportedException(); }
		}

		public bool IsSynchronized => false;
		#endregion

		#region IEnumerable
		public IEnumerator GetEnumerator()
		{
			foreach (var folder in SubFolders)
				yield return folder;
			foreach (var link in Links)
				yield return link;
		}
		#endregion
	}
}