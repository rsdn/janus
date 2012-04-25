using System;
using System.Collections;

using BLToolkit.Mapping;

using Rsdn.TreeGrid;
using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	internal enum FavoritesGridColumns
	{
		Name = 0,
		Comment = 1
	}

	/// <summary>
	/// Класс сообщение Избранного
	/// </summary>
	public class FavoritesLink : IFavoritesEntry, IGetData
	{
		//private const int _missedIdentifier = -1;
		private readonly Lazy<IFavoritesManager> _favManager;

		public FavoritesLink(IServiceProvider provider)
		{
			_favManager = new Lazy<IFavoritesManager>(provider.GetRequiredService<IFavoritesManager>);
		}

		public int FolderId { get; set; }

		[MapField("MsgId")]
		[Nullable]
		public int MessageId { get; set; }

		[MapField("Url")]
		[Nullable]
		public string Url { get; set; }

		public string Link
		{
			// непонятно к чему это?
			//get { return MessageId >= 0?
			//          ApplicationManager.Instance.ProtocolDispatcher.FormatExternalURI(
			//          JanusProtocolResourceType.ArticleList, MessageId.ToString()): Url; }
			get
			{
				if (!string.IsNullOrEmpty(Url))
					return Url;

				return JanusProtocolInfo.FormatURI(
					JanusProtocolResourceType.Message, MessageId.ToString());
			}
			set { Url = value; }
		}

		private static FavoritesLink CreateInstance(IServiceProvider provider)
		{
			return new FavoritesLink(provider);
		}

		public static FavoritesLink CreateInstance(
			IServiceProvider provider,
			int id,
			int messageID,
			string comment,
			FavoritesFolder parent)
		{
			var link = CreateInstance(provider);
			link.Id = id;
			link.MessageId = messageID;
			link.Comment = comment;
			link.Parent = parent;
			return link;
		}

		public static FavoritesLink CreateInstance(
			IServiceProvider provider,
			int id,
			string url,
			string comment,
			FavoritesFolder parent)
		{
			var link = CreateInstance(provider);
			link.Id = id;
			link.Url = url;
			link.Comment = comment;
			link.Parent = parent;
			return link;
		}

		#region IFavoritesEntry
		private FavoritesFolder _folder;
		public int Id { get; set; }

		public IFavoritesEntry Parent
		{
			get { return _folder; }
			set { _folder = (FavoritesFolder)value; }
		}

		public bool IsContainer
		{
			get { return false; }
		}

		public string Comment { get; set; }

		public void Update()
		{
			_favManager.Value.Update(this);
		}

		public void Delete()
		{
			_favManager.Value.Delete(this);
		}

		public bool Move(IFavoritesEntry newParent)
		{
			return _favManager.Value.Move(this, (FavoritesFolder)newParent);
		}
		#endregion

		#region ITreeNode
		ITreeNode ITreeNode.Parent
		{
			get { return _folder; }
		}

		public NodeFlags Flags { get; set; }

		public bool HasChildren
		{
			get { return false; }
		}

		public ITreeNode this[int iIndex]
		{
			get { throw new NotSupportedException(); } /*set;*/
		}
		#endregion

		#region ICollection
		public void CopyTo(Array array, int index)
		{
			throw new NotSupportedException();
		}

		public int Count
		{
			get { throw new NotSupportedException(); }
		}

		public object SyncRoot
		{
			get { throw new NotSupportedException(); }
		}

		public bool IsSynchronized
		{
			get { return false; }
		}

		public IEnumerator GetEnumerator()
		{
			yield break;
		}
		#endregion

		#region IGetData
		public void GetData(NodeInfo nodeInfo, CellInfo[] cellData)
		{
			cellData[(int)FavoritesGridColumns.Name].Text = Link;
			cellData[(int)FavoritesGridColumns.Name].ImageIndex = FavoritesDummyForm.Instance.MsgLinkIndex;
			cellData[(int)FavoritesGridColumns.Comment].Text = Comment;
/*
			cellData[(int) FavoritesGridColumns.ColId].Text = ((ID == _missedIdentifier) || !Config.Instance.ForumDisplayConfig.ShowMessageId) ? String.Empty : ID.ToString();
			cellData[(int) FavoritesGridColumns.ColId].Image = -1;
			cellData[(int) FavoritesGridColumns.ColSubj].Text = ID == _missedIdentifier ? Url : Subject;
			cellData[(int) FavoritesGridColumns.ColSubj].Image = ID == _missedIdentifier ? 
				MessageTreeImageManager.Instance.GetLinkIndex() :
				MessageTreeImageManager.Instance.GetMessageImage(MessageImageType.Msg);
			cellData[(int) FavoritesGridColumns.ColAuthor].Text = UserNick;
			cellData[(int) FavoritesGridColumns.ColAuthor].Image = ID == _missedIdentifier ? -1 :
				MessageTreeImageManager.Instance.GetUserClassImage((UserClass) UserClass);
			cellData[(int) FavoritesGridColumns.ColDate].Text = Date == DateTime.MinValue ? String.Empty : Date.ToString();
			cellData[(int) FavoritesGridColumns.ColDate].Image =
				MessageTreeImageManager.Instance.GetWeekDayImage((int) Date.DayOfWeek, Date);

			if(ForumID == _missedIdentifier)
			{
				cellData[(int) FavoritesGridColumns.ColForum].Text = String.Empty;
				cellData[(int) FavoritesGridColumns.ColForum].Image = -1;
			}
			else
			{
				Forum forum = Forums.Instance[ForumID];
				if (forum != null)
				{
					cellData[(int) FavoritesGridColumns.ColForum].Text = forum.Description;
					cellData[(int) FavoritesGridColumns.ColForum].Image = -1;
				}
			}
			cellData[(int) FavoritesGridColumns.Comment].Text = Comment;
			cellData[(int) FavoritesGridColumns.ColDate].Image = -1;
*/
		}
		#endregion
	}
}