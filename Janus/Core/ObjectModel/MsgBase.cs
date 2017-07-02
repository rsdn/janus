using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

using CodeJam;
using CodeJam.Extensibility;
using CodeJam.Extensibility.Model;
using CodeJam.Strings;

using LinqToDB.Mapping;

using Rsdn.TreeGrid;

namespace Rsdn.Janus
{
	/// <summary>
	/// Базовый класс для сообщения форума.
	/// </summary>
	
public abstract class MsgBase : IMsg, ICollection<MsgBase>, IGetData
	{
		private const string _reSubj = "…{0}: {1}";
		private static readonly Regex _reRx =
			new Regex(@"Re(\[(?'num'\d+)\])?:\s* (?'body'.*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

#pragma warning disable 0649
		[ExpectService]
		private readonly IForumImageManager _imgManager;

		[ExpectService]
		private readonly IJanusDatabaseManager _dbManager;

		[ExpectService]
		private readonly IFavoritesManager _favManager;
#pragma warning restore 0649

		private readonly IServiceProvider _serviceProvider;

		protected MsgBase(IServiceProvider provider)
		{
			_serviceProvider = provider;
			this.AssignServices(_serviceProvider);
			UserClass = (int)Rsdn.Janus.UserClass.Anonym;
			IsRead = true;
		}

		protected IServiceProvider ServiceProvider => _serviceProvider;

		[Column("mid")]
		public int ID { get; set; }
		[Column("pid")]
		public int ParentID { get; set; }
		[Column("gid")]
		public int ForumID { get; set; }
		[Column("name")]
		public string Name { get; set; }
		[Column("lastmoderated")]
		public DateTime? LastModerated { get; protected set; }
		[Column("dte")]
		public DateTime Date { get; set; }
		[Column("smiles")]
		// ReSharper disable MemberCanBePrivate.Global
		public int Smiles { get; set; } // Количество :)г текущего сообщения
		[Column("agree")]
		public int Agrees { get; set; } // Количество + в текущем сообщении.
		[Column("disagree")]
		public int Disagrees { get; set; } // Количество - в текущем сообщении.
		[Column("rate"), Nullable]
		public int Rating { get; set; } // Рейтинг текущего сообщения
		// ReSharper restore MemberCanBePrivate.Global
		[Column("moders")]
		protected int Moderatorials { private get; set; }	// Количество бомбочек текущего сообщения
		[Column("a_agree")]
		protected int RepliesAgree { private get; set; }

		IForumMessageInfo IForumMessageInfo.Topic => Topic;

		[Column("a_count")]
		public int RepliesCount { get; set; }
		[Column("a_unread")]
		public int RepliesUnread { get; protected set; }

		IForumMessageInfo IForumMessageInfo.Parent => Parent;

		[Column("a_rate")]
		protected int RepliesRate { private get; set; }			// Общий ретинг по дочерним сообщениям.
		[Column("a_smiles")]
		protected int RepliesSmiles { private get; set; }		// Количество :) в дочерних сообщениях.
		[Column("a_disagree")]
		protected int RepliesDisagree { private get; set; }		// Количество - в дочерних сообщениях.
		[Column("a_me_unread")]
		public int RepliesToMeUnread { get; protected set; } // Количество ответов текущему пользователю в дочерних сообщениях.
		[Column("a_marked")]
		public int RepliesMarked { get; protected set; } // Количество помеченых ответов на сообщение.
		[Column("a_moders")]
		protected int RepliesModeratorials { private get; set; }	// Количество бомбочек в дочерних сообщениях.

		[Column("isread")]
		public bool IsRead { get; set; }
		[Column("tid")]
		public int TopicIDInternal { protected get; set; }

		[Column("subject")]
		public string Subject { get; set; }
		[Column("article_id")]
		public int? ArticleId { get; set; }
		[Column("ismarked")]
		public bool IsMarked { get; set; }
		[Column("uclass")]
		public short UserClass { get; set; }
		[Column("uid")]
		public int UserID { get; set; }
		[Column("readreplies")]
		public bool ReadReplies { get; set; }

		[Column("closed")]
		public bool Closed { get; protected set; }

		#region IForumMessageHeader Members
		public bool AutoRead => ReadReplies;
		#endregion

		[Column("usernick"), Nullable]
		public string UserNick { get; set; }

		[NotColumn]
		public MsgBase Parent { get; protected set; }

		// Есть ли помеченные ответы.
		[NotColumn]
		public bool Marked => IsMarked;

// Есть ли помеченные ответы.
		[NotColumn]
		public bool HasRepliesUnread => RepliesUnread > 0;

// Еслить ли нечитанные ответы на данное сообщение.

		[NotColumn]
		public IMsg Topic => GetTopic();

// Возвращает корневое сообщение (тему).
		[NotColumn]
		public int TopicID => GetTopicId();

// Возвращает ID корневого сообщения (темы).

		[NotColumn]
		protected bool IsChild { private get; set; }

		[NotColumn]
		private string DisplaySubject { get; set; }

		/// <summary>
		/// Сообщение помечено как не прочитанное.
		/// </summary>
		[NotColumn]
		public bool IsUnread => (_flags & NodeFlags.Highlight) != 0;

		/// <summary>Текст сообщения.</summary>
		[NotColumn]
		public string Body => DatabaseManager.GetMessageBody(ServiceProvider, ID);

		#region Интерфейс сообщения.

		public void SetMessageMarked(bool isMarked)
		{
			using (_dbManager.GetLock().GetWriterLock())
			{
				DatabaseManager.SetMessageMarked(ServiceProvider, this, isMarked);
				SetMarked(isMarked);
			}
		}

		private void SetMarked(bool value)
		{
			IsMarked = value;
			var curr = Parent;
			var iAdd = value ? (short)1 : (short)-1;
			while (curr != null)
			{
				curr.RepliesMarked += iAdd;
				curr = curr.Parent;
			}
		}

		protected internal void SetUnread(bool value)
		{
			if (((_flags & NodeFlags.Highlight) == 0) == (value == false))
				return;

			short iAddUnread;
			short iAddToMeUnread;
			if (value)
			{
				iAddUnread = 1;
				iAddToMeUnread = 1;
				_flags |= NodeFlags.Highlight;
			}
			else
			{
				iAddUnread = -1;
				iAddToMeUnread = -1;
				_flags &= ~NodeFlags.Highlight;
			}
			if (Parent != null && Parent.UserID != Config.Instance.SelfId)
				iAddToMeUnread = 0;
			var curr = this;
			while (curr.Parent != null)
			{
				curr.RepliesToMeUnread += iAddToMeUnread;
				curr.RepliesUnread += iAddUnread;
				curr = curr.Parent;
			}
		}

		protected abstract IMsg GetTopic();
		protected abstract int GetTopicId();

		/// <summary>Количество помеченных (очками) в дочерних сообщениях.</summary>

		public string GetFormattedRating()
		{
			return JanusFormatMessage.FormatRates(Rating, Smiles, Agrees, Disagrees);
		}

		protected string GetFormattedRatingForReplies()
		{
			return JanusFormatMessage.FormatRates(RepliesRate, RepliesSmiles, RepliesAgree,
												  RepliesDisagree);
		}

		#endregion

		#region Private Members

		/// <summary>
		/// Рекурсивная процедура добавления чилдов. Используется процедурой 
		/// FillChildren.
		/// </summary>
		/// <param name="child">Добавляемая ветка.</param>
		/// <returns>true - Ветка успешно добавлена.
		/// false - Не найдена родительская ветка.</returns>
		private bool AddChild(MsgBase child)
		{
			if (ID == child.ParentID)
			{
				RepliesCount++;

				child.Parent = this;

				if (Children == null)
					Children = new List<MsgBase>();

				Children.Add(child);

				Flags |= NodeFlags.AutoExpand;

				return true;
			}

			if (Children == null)
				return false;

			if (Children.Any(msg => msg.AddChild(child)))
			{
				RepliesCount++;
				return true;
			}

			return false;
		}

		#endregion

		#region ITreeNode - реализация интерфейса.

		ITreeNode ITreeNode.Parent => Parent;

		private NodeFlags _flags;

		public virtual NodeFlags Flags
		{
			get { return _flags; }
			set { _flags = value; }
		}

		public virtual bool HasChildren => RepliesCount > 0;

		protected List<MsgBase> Children { get; set; }

		ITreeNode ITreeNode.this[int index] => Children[index];

		// ICollection - реализация интерфейса.
		public int Count
		{
			get
			{
				if (HasChildren && Children == null)
					FillChildren();

				return Children?.Count ?? 0;
			}
		}

		public void CopyTo(Array array, int index)
		{
			Children.CopyTo((MsgBase[])array, index);
		}

		public bool IsSynchronized => false;

		public object SyncRoot => this;

		public IEnumerator GetEnumerator()
		{
			if (HasChildren && Children == null)
				FillChildren();

			if (Children == null)
				yield break;

			foreach (var c in Children)
				yield return c;
		}

		#endregion

		#region IGetData - реализация интерфейса.
		private const int _idColumn = 0;
		private const int _markColumn = 1;
		private const int _subjColumn = 2;
		private const int _userNameColumn = 3;
		// Рейтинг темы в дереве сообщений и имя форума в поиске.
		private const int _rateThisColumn = 4;
		protected const int ExtInfoColumn = 5;
		private const int _replCountColumn = 6;
		private const int _dateColumn = 7;

		public virtual void GetData(NodeInfo nodeInfo, CellInfo[] cellData)
		{
			var cnfg = Config.Instance;
			var style = cnfg.StyleConfig;
			nodeInfo.Highlight = IsUnread;

			cellData[_idColumn].Text = Config.Instance.ForumDisplayConfig.ShowMessageId ? ID.ToString() : "";

			if (ReadReplies)
			{
				cellData[_idColumn].CellImageType = CellImageType.Image;
				cellData[_idColumn].Image = _imgManager.GetAutoReadImage(MessageFlagExistence.OnMessage);
			}

			cellData[_markColumn].CellImageType = CellImageType.Image;
			cellData[_markColumn].Image = _imgManager.GetMarkImage(
				IsMarked
					? MessageFlagExistence.OnMessage
					: RepliesMarked > 0
					  	? MessageFlagExistence.OnChildren
					  	: MessageFlagExistence.None);

			cellData[_subjColumn].Text = DisplaySubject;
			if (UserID == cnfg.SelfId)
				nodeInfo.ForeColor = style.SelfMessageColor;
			else if (Parent != null && Parent.UserID == cnfg.SelfId)
				nodeInfo.ForeColor = style.RepliesToSelfMessageColor;
			else if (_favManager.IsFavorite(ID))
				nodeInfo.ForeColor = style.FavoriteMessageColor;
			else
				nodeInfo.ForeColor = UserID == -1
										? style.MissingTopicColor
										: style.MessageColor;

			cellData[_subjColumn].CellImageType = CellImageType.Image;
			cellData[_subjColumn].Image =
				_imgManager
					.GetMessageImage(
						ArticleId > 0 ? MessageType.Article : MessageType.Ordinal,
						IsUnread
							? MessageFlagExistence.OnMessage
							: RepliesUnread > 0
						  		? MessageFlagExistence.OnChildren
						  		: MessageFlagExistence.None,
						RepliesToMeUnread > 0,
						Moderatorials > 0
							? MessageFlagExistence.OnMessage
							: RepliesModeratorials > 0
						  		? MessageFlagExistence.OnChildren
						  		: MessageFlagExistence.None,
						Closed);

			cellData[_userNameColumn].Text = UserNick;
			cellData[_userNameColumn].CellImageType = CellImageType.Image;
			cellData[_userNameColumn].Image = _imgManager.GetUserImage((UserClass)UserClass);

			var corrReplUnread = IsUnread ? RepliesUnread - 1 : RepliesUnread;
			var replUnreadStr = corrReplUnread > 0 ? $"({corrReplUnread})" : string.Empty;
			var repliesStr = RepliesCount > 0 ? RepliesCount.ToString() : string.Empty;
			cellData[_replCountColumn].Text = repliesStr + replUnreadStr;

			cellData[_rateThisColumn].Text = GetFormattedRating();

			cellData[_dateColumn].Text = JanusFormatMessage.GetDateString(Date);
			cellData[_dateColumn].CellImageType = CellImageType.Image;
			cellData[_dateColumn].Image = _imgManager.GetMessageDateImage(Date);

			GetDataExt(cellData);
		}

		protected abstract void GetDataExt(CellInfo[] aryCellData);
		#endregion

		#region ICollection<MsgBase> - реализация интерфейса.

		/// <exception cref="InvalidOperationException"></exception>
		public void Add(MsgBase item)
		{
			throw new InvalidOperationException();
		}

		/// <exception cref="InvalidOperationException"></exception>
		public void Clear()
		{
			throw new InvalidOperationException();
		}

		/// <exception cref="NotSupportedException"></exception>
		public bool Contains(MsgBase item)
		{
			throw new NotSupportedException();
		}

		public void CopyTo(MsgBase[] array, int arrayIndex)
		{
			Children.CopyTo(array, arrayIndex);
		}

		public bool IsReadOnly => true;

		/// <exception cref="InvalidOperationException"></exception>
		public bool Remove(MsgBase item)
		{
			throw new InvalidOperationException();
		}

		IEnumerator<MsgBase> IEnumerable<MsgBase>.GetEnumerator()
		{
			if (HasChildren && Children == null)
				FillChildren();

			if (Children == null)
				yield break;

			foreach (var c in Children)
				yield return c;
		}

		#endregion

		/// <summary>
		/// Этот метод нужно реализовать в потомке, чтобы заполнять дочерние ветки.
		/// </summary>
		protected abstract void FillChildren();

		public override string ToString()
		{
			return ID + (Subject == null ? " <<root>>" : " '" + Subject + "'");
		}

		private static bool IsAutoReSubj(string subject, string parentSubject)
		{
			var match = _reRx.Match(subject);
			if (!match.Success)
				return false;
			var parentMatch = _reRx.Match(parentSubject);
			var parentBody = parentMatch.Success ? parentMatch.Groups["body"].Value : parentSubject;
			var body = match.Groups["body"].Value;
			if (parentBody.Length > body.Length)
				parentBody = parentBody.Substring(0, body.Length);
			return body == parentBody;
		}

		private int _reNum;

		public void EndMapping()
		{
			UserNick = UserNick.ToUserDisplayName((UserClass)UserClass);

			if (!IsRead)
				Flags |= NodeFlags.Highlight;

			DisplaySubject = Subject;

			if (IsChild)
			{
				if (!Parent.AddChild(this))
				{
					// Если дочерняя ветка не найдена, а это происходит если ветка 
					// "оборвана", добавляем ее в корень темы.
					ParentID = Parent.ID;

					var ok = Parent.AddChild(this);

					Trace.Assert(ok, "Сбой при добавлении подветки.");
				}

				_reNum = Parent._reNum + 1;

				if (IsMarked)
				{
					IsMarked = false;
					SetMarked(true);
				}

				if (Moderatorials > 0)
				{
					var cur = Parent;
					while (cur.Parent != null)
					{
						cur.RepliesModeratorials += Moderatorials;
						cur = cur.Parent;
					}
				}

				DisplaySubject =
					_reSubj.FormatWith(UserNick,
						!IsAutoReSubj(Subject, Parent.Subject)
							? Subject
							: "Re[{0}]".FormatWith(_reNum));
			}

			if (IsUnread)
			{
				Flags = Flags & ~NodeFlags.Highlight;
				SetUnread(true);

				if (!IsChild && RepliesUnread > 0)
					RepliesUnread--;
			}
		}
	}
}


