using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using LinqToDB;

using Rsdn.Janus.Framework;
using Rsdn.Janus.ObjectModel;
using Rsdn.TreeGrid;
using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	internal sealed class Forums : FolderFeature, IFeature, IGetData
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly IJanusDatabaseManager _dbManager;
		private readonly Lazy<IForumsAggregatesService> _forumsAggregatesService;
		private readonly List<ForumFolder> _folders = new List<ForumFolder>();
		private readonly ForumFolder _unsubscribedFolder;
		private Dictionary<int, Forum> _forumsIdHash = new Dictionary<int, Forum>();
		private List<IDisposable> _forumLiveBehaviors;
		private volatile static Forums _instance;

		public static Forums Instance
		{
			get
			{
				if (_instance == null)
					lock (typeof(Forums))
						if (_instance == null)
							_instance = new Forums(ApplicationManager.Instance.ServiceProvider);
				return _instance;
			}
		}

		// ReSharper disable MemberCanBeMadeStatic.Global
		[CanBeNull]
		public Forum ActiveForum
			// ReSharper restore MemberCanBeMadeStatic.Global
		{
			get { return ObjectModel.Features.Instance.ActiveFeature as Forum; }
			set { ObjectModel.Features.Instance.ActiveFeature = value; }
		}

		public Forum[] ForumList { get; private set; }

		public ForumFolder UnsubscribedForums
		{
			get { return _unsubscribedFolder; }
		}

		private Forums(IServiceProvider provider)
			: base(provider)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");

			_serviceProvider = provider;
			_dbManager = _serviceProvider.GetRequiredService<IJanusDatabaseManager>();
			_forumsAggregatesService = new Lazy<IForumsAggregatesService>(
				() => _serviceProvider.GetRequiredService<IForumsAggregatesService>());

			_unsubscribedFolder = new ForumFolder(_serviceProvider, this, SR.Forum.Unsubscribed.DisplayName);
			_folders.Add(_unsubscribedFolder);
			_flags = NodeFlags.AutoExpand;
			_description = SR.Forum.Incoming.DisplayName;

			LoadData();
		}

		/// <summary>
		/// Проверяет, находится ли форум, указанный с помощью <paramref name="forum"/> в списке подписанных форумов.
		/// </summary>
		/// <param name="forum">Проверяемый форум.</param>
		/// <returns><c>true</c>, если форум находится в списке подписанных форумов, иначе <c>false</c>.</returns>
		public bool IsSubscribed(Forum forum)
		{
			if (forum == null)
				throw new ArgumentNullException("forum");
			return Array.IndexOf(ForumList, forum) >= 0;
		}

		#region Интерфейс коллекции.
		public Forum this[int forumId]
		{
			get { return _forumsIdHash[forumId]; }
		}

		//IFeature
		string IFeature.Key
		{
			get { return "Inbox"; }
		}

		public override string Info
		{
			get
			{
				return ForumList != null
						? (Config.Instance.ForumDisplayConfig.ShowTotalMessages ? " {0}/{1}/{2}" : " {0}/{1}")
							.FormatStr(
							_forumsAggregatesService.Value.UnreadRepliesToMeCount,
							_forumsAggregatesService.Value.UnreadMessagesCount,
							_forumsAggregatesService.Value.MessagesCount)
						: string.Empty;
			}
		}

		public override int ImageIndex
		{
			get { return ObjectModel.Features.Instance.ForumsImageIndex; }
		}


		IFeature IFeature.this[int forumId]
		{
			get { return this[forumId]; }
		}

		ITreeNode ITreeNode.this[int index]
		{
			get
			{
				if (index < ForumList.Length)
					return ForumList[index];

				return _folders[index - ForumList.Length];
			}
		}

		bool ITreeNode.HasChildren
		{
			get { return (ForumList != null && ForumList.Length > 0) || _folders.Count > 0; }
		}

		// ICollection
		public int Count
		{
			get { return ForumList.Length + _folders.Count; }
		}

		public bool IsSynchronized
		{
			get { return ForumList.IsSynchronized; }
		}

		public object SyncRoot
		{
			get { return typeof(Forums); }
		}

		public void CopyTo(Array array, int index)
		{
			ForumList.CopyTo(array, index);
		}

		// IEnumerable
		public IEnumerator GetEnumerator()
		{
			return new TwoCollectionEnumerator(ForumList, _folders);
		}
		#endregion

		#region Работа с БД.
		private IEnumerable<Forum> LoadSubscribedForums(IDataContext db)
		{
			var q = db.SubscribedForums().OrderByDescending(f => f.Priority);
			q =
				Config.Instance.ForumDisplayConfig.ShowFullForumNames
					? q.ThenBy(f => f.Descript)
					: q.ThenBy(f => f.Name);

			var counts =
				from ti in db.TopicInfos()
				group ti by ti.ForumID into grp
				select
					new
					{
						ForumID = grp.Key,
						Count = grp.Count() + grp.Sum(iti => iti.AnswersCount),
						Unread = grp.Sum(iti => iti.AnswersUnread),
						MeUnread = grp.Sum(iti => iti.AnswersToMeUnread)
					};
			var forums =
				from f in q
				join ti in counts on f.ID equals ti.ForumID into lj
				from ti in lj.DefaultIfEmpty() // left join forcing
				select
					new Forum(_serviceProvider)
					{
						ID = f.ID,
						Name = f.Name,
						LastSync = f.LastSync,
						IsSubscribed = true,
						IsRateable = f.ServerForum.Rated,
						RateLimit = f.ServerForum.RateLimit > 0,
						MessagesCount = ti.Count,
						RepliesToMeUnread = ti.MeUnread,
						Unread = ti.Unread,
						InTop = f.ServerForum.InTop,
						Priority = f.Priority.GetValueOrDefault(),
						ForumDescription = f.Descript
					};
			return forums.ToList();
		}

		private IEnumerable<Forum> LoadServerForums(IDataContext db)
		{
			var q = db.ServerForums().OrderByDescending(f => f.SubscribedForum.Priority);
			q =
				Config.Instance.ForumDisplayConfig.ShowFullForumNames
					? q.ThenBy(f => f.Descript)
					: q.ThenBy(f => f.Name);

			var counts =
				from ti in db.TopicInfos()
				group ti by ti.ForumID into grp
				select
					new
					{
						ForumID = grp.Key,
						Count = grp.Count() + grp.Sum(iti => iti.AnswersCount),
						Unread = grp.Sum(iti => iti.AnswersUnread),
						MeUnread = grp.Sum(iti => iti.AnswersToMeUnread)
					};
			var forums =
				from f in q
				join ti in counts on f.ID equals ti.ForumID into lj
				from ti in lj.DefaultIfEmpty() // left join forcing
				where ti.Count > 0
				select
					new Forum(_serviceProvider)
					{
						ID = f.ID,
						Name = f.Name,
						LastSync = f.SubscribedForum != null ? f.SubscribedForum.LastSync : 0,
						IsSubscribed = false,
						IsRateable = f.Rated,
						RateLimit = f.RateLimit > 0,
						MessagesCount = ti.Count,
						RepliesToMeUnread = ti.MeUnread,
						Unread = ti.Unread,
						InTop = f.InTop,
						Priority = f.SubscribedForum.Priority.GetValueOrDefault(),
						ForumDescription = f.Descript
					};
			return forums.Cast<Forum>().ToList();
		}

		// TODO: Intensive refactoring required!
		private void LoadData() //FillForums
		{
			using (_dbManager.GetLock().GetWriterLock())
			using (var db = _dbManager.CreateDBContext())
			{
				if (BeforeLoadData != null)
					BeforeLoadData(this, EventArgs.Empty);

				var newLiveBehs = new List<IDisposable>();
				var newIdHash = new Dictionary<int, Forum>();
				//var forums = _dbManager.GetAccessor<IForumDataAccessor>().GetSubscribedForumList();
				var forums = LoadSubscribedForums(db);
				foreach (var forum in forums)
				{
					forum._parent = this;
					newLiveBehs.Add(forum.ActivateLiveBehavior());

					newIdHash[forum.ID] = forum;
				}

				var list = LoadServerForums(db);
				var unsubscribedForums = new List<Forum>();
				foreach (var forum in list)
				{
					if (newIdHash.ContainsKey(forum.ID))
						continue;
					newLiveBehs.Add(forum.ActivateLiveBehavior());

					unsubscribedForums.Add(forum);
					newIdHash.Add(forum.ID, forum);
				}

				var oldForums = new List<Forum>(_unsubscribedFolder.Cast<Forum>());

				if (ForumList != null)
					oldForums.AddRange(ForumList);

				ForumList = forums.ToArray();

				_unsubscribedFolder.Clear();

				foreach (var forum in unsubscribedForums)
					_unsubscribedFolder.Add(forum);

				_forumsIdHash = newIdHash;

				if (_forumLiveBehaviors != null)
					_forumLiveBehaviors.DisposeAll();
				_forumLiveBehaviors = newLiveBehs;
			}
		}

		public void Refresh()
		{
			LoadData();
		}

		#endregion

		#region События

		/// <summary>
		/// Вызывается перед загрузкой данных из БД.
		/// </summary>
		public static event EventHandler BeforeLoadData;
		
		#endregion События

		#region IGetData Members

		public void GetData(NodeInfo nodeInfo, CellInfo[] cellData)
		{
			nodeInfo.ForeColor = Config.Instance.StyleConfig.NavigationTreeForumColor;
			nodeInfo.Highlight = _forumsAggregatesService.Value.UnreadMessagesCount > 0;

			cellData[0].Text = Description;
			cellData[0].ImageIndex = ObjectModel.Features.Instance.ForumsImageIndex;
			cellData[1].Text = Info;
		}

		#endregion
	}
}