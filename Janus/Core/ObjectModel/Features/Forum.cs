using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using CodeJam;
using CodeJam.Extensibility;

using LinqToDB.Mapping;

using Rsdn.Janus.ObjectModel;
using Rsdn.TreeGrid;

namespace Rsdn.Janus
{
	/// <summary>
	/// Отражает отдельный форум.
	/// </summary>
	public class Forum : Feature, IMessagesFeature, IGetData, IForum
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly IForumImageManager _imageManager;

		public Forum(IServiceProvider provider)
		{
			_serviceProvider = provider;
			_imageManager = _serviceProvider.GetRequiredService<IForumImageManager>();
		}

		public IDisposable ActivateLiveBehavior()
		{
			Features.Instance.BeforeFeatureActivate += InstanceBeforeFeatureActivate;
			return Disposable.Create(
				() => Features.Instance.BeforeFeatureActivate -= InstanceBeforeFeatureActivate);
		}

		[Column("id")]
		public int ID { get; set; }
		[Column("name")]
		public string Name { get; set;  }
		[Column("lastSync")]
		public int LastSync { get; set; }
		[Column("subscribed")]
		public bool IsSubscribed { get; set; }
		[Column("rated")]
		public bool IsRateable { get; set; }
		[Column("ratelimit")]
		public bool RateLimit { get; set; }

		[Column("messagesCount"), Nullable]
		public int MessagesCount { get; set; }
		[Column("repliesToMeUnread"), Nullable]
		public int RepliesToMeUnread { get; set; }
		[Column("unread"), Nullable]
		public int Unread { get; set; }
		[Column("intop"), Nullable]
		public bool InTop { get; set; }
		[Column("priority"), Nullable]
		public int Priority { get; set; }
		[Column("descript")]
		public string ForumDescription { get; set; }

		[NotColumn]
		public string DisplayName => Config.Instance.ForumDisplayConfig.ShowFullForumNames ? Description : Name;

		[NotColumn]
		public override string Description => ForumDescription;

		public event EventHandler BeforeLoadData;

		public override string ToString()
		{
			return DisplayName + " " + string.Format(
										SR.Forum.DisplayName, MessagesCount, Unread, RepliesToMeUnread);
		}

		private void InstanceBeforeFeatureActivate(
			IFeature oldFeature, IFeature newFeature, ref bool cancel)
		{
			if (this == oldFeature && !Config.Instance.RestoreForumPosition)
				ActiveMsg = null;
		}

		#region Данные.
		/// <summary>Ссылка на корень форума (типа Msg).</summary>
		private readonly WeakReference _weakMsgRoot = new WeakReference(null);
		#endregion

		#region Интерфейс.
		private int _activeMsgId = -1;
		private readonly WeakReference _activeMsg = new WeakReference(null);

		[NotColumn]
		public IMsg ActiveMsg
		{
			get
			{
				lock (this)
				{
					var activeMsg = (IMsg)_activeMsg.Target;

					if (activeMsg != null)
						return activeMsg;

					if (_activeMsgId >= 0)
					{
						activeMsg = FindMsgById(_activeMsgId);
						_activeMsg.Target = activeMsg;

						return activeMsg;
					}

					var config = Config.Instance;

					if (config.RestoreForumPosition
						&& config.LastReadMessage.ContainsKey(ID))
					{
						var i = config.LastReadMessage[ID];

						if (i >= 0)
							_activeMsgId = i;

						activeMsg = FindMsgById(_activeMsgId);
						_activeMsg.Target = activeMsg;

						return activeMsg;
					}

					return null;
				}
			}
			set
			{
				lock (this)
				{
					if (value == null)
					{
						_activeMsgId = -1;
						_activeMsg.Target = null;

						Config.Instance.LastReadMessage.Remove(ID);

						return;
					}

					if (value == _weakMsgRoot.Target)
						return;

					var curr = value;
					var root = Msgs;

					while (curr.Parent != null)
					{
						if (root == curr.Parent)
						{
							_activeMsgId = value.ID;
							_activeMsg.Target = value;

							Config.Instance.LastReadMessage[ID] = _activeMsgId;
							return;
						}

						curr = curr.Parent;
					}

					throw new ApplicationException($"Сообщение '{value}' отсутствует в списке сообщений.");
				}
			}
		}

		/// <summary>
		/// Если false, значит сообщения форума загружены не полностью.
		/// </summary>
		private bool _isLoadAll = true;

		[NotColumn]
		public bool IsAllMsgLoaded => _isLoadAll;

		public IMsg LoadAllMsg()
		{
			return ReadMsg(true);
		}

		[NotColumn]
		public IMsg Msgs
		{
			get
			{
				var root = (IMsg)_weakMsgRoot.Target ??
							ReadMsg(Config.Instance.ForumDisplayConfig.MaxTopicsPerForum <= 0);
				return root;
			}
		}

		/// <summary>
		/// Помечает форум как требующий перечитки.
		/// </summary>
		public void Refresh()
		{
			RefreshInfo();

			_activeMsg.Target = null;
			_weakMsgRoot.Target = null;
		}

		/// <summary>
		/// Обновляет информацию о форуме не перечитывая сообщения.
		/// </summary>
		public void RefreshInfo()
		{
			LoadData(ID);
			Features.Instance.FeatureChanged(this);
		}

		/// <summary>
		/// Детали реализации FindMsgByID...
		/// </summary>
		/// <param name="idMsg">ID сообщения которое нужно найти.</param>
		/// <returns>Найденая ветка.</returns>
		public IMsg FindMsgById(int idMsg)
		{
			var root = Msgs;
			int? idTopic;
			using (var mgr = _serviceProvider.CreateDBContext())
				idTopic =
					mgr
						.Messages()
						.Where(msg => msg.ID == idMsg)
						.Select(msg => (int?)msg.TopicID)
						.SingleOrDefault();

			if (!idTopic.HasValue)
				return null;

			if (idTopic.Value == 0)
				idTopic = idMsg;

			// Если сообщения форума загружены не полностью, делаем две 
			// попытки поиска
			for (; ; )
			{
				var message = root.Cast<IMsg>()
					.FirstOrDefault(msg => msg.ID == idTopic);

				if (message != null)
					return message.Flatten().First(msg => msg.ID == idMsg);

				// Если сообщения форума загружены полностью,
				// то ловить больше нечего.
				if (IsAllMsgLoaded)
					return null;

				// Иначе загружаем все собщения и повторяем поиск.
				root = LoadAllMsg();
			}
		}

		/// <summary>
		/// Читает все сообщения форума не обращая внимание на значение
		/// Config.Instance.MaxTopicsPerForum
		/// </summary>
		private IMsg ReadMsg(bool isLoadAll)
		{
			BeforeLoadData?.Invoke(this, EventArgs.Empty);

			_isLoadAll = isLoadAll;

			// На всякий пожарный сбрасываем активное сообщение.
			_activeMsg.Target = null;

			IMsg root = Msg.GetTopics(_serviceProvider, ID, Config.Instance.ForumSortCriteria, _isLoadAll);
			//IMsg root = Msg.GetTopicsKeyset(_serviceProvider, ID, Config.Instance.ForumSortCriteria, _isLoadAll);

			_weakMsgRoot.Target = root;

			return root;
		}

		#endregion

		#region IGetData

		void IGetData.GetData(NodeInfo nodeInfo, CellInfo[] cellData)
		{
			var styleConfig = Config.Instance.StyleConfig;

			nodeInfo.ForeColor = Priority == 0
									? styleConfig.NavigationTreeForumColor
									: styleConfig.ForumPriorityColor[Priority];

			nodeInfo.Highlight = Unread > 0;
			cellData[0].Text = DisplayName;
			cellData[0].CellImageType = CellImageType.Image;
			cellData[0].Image = _imageManager.GetForumImage(
				Unread > 0,
				InTop,
				RepliesToMeUnread > 0);

			cellData[1].Text = Info;
		}

		#endregion IGetData

		#region IFeature

		[NotColumn]
		public override string Info =>
			$" {RepliesToMeUnread}/{Unread}{(Config.Instance.ForumDisplayConfig.ShowTotalMessages ? "/" + MessagesCount : string.Empty)}";

		[NotColumn]
		string IFeature.Key => "Outbox" + Name;
		#endregion ITreeNode

		#region IMessagesFeature

		public IEnumerable<IMsg> ActiveMessages => ForumDummyForm.Instance.SelectedMessages;

		public event EventHandler ActiveMessagesChanged
		{
			add { ForumDummyForm.Instance.SelectedMessagesChanged += value; }
			remove { ForumDummyForm.Instance.SelectedMessagesChanged -= value; }
		}

		#endregion

		#region IFeatureGui

		protected override Control CreateGuiControl()
		{
			return ForumDummyForm.Instance;
		}

		#endregion IFeatureGui

		#region Работа с БД.

		internal void LoadData(int id) //FillForums
		{
			using (var db = _serviceProvider.CreateDBContext())
			{
				var forum =
					db
						.ServerForums(f => f.ID == id)
						.Select(
						f =>
						new
							{
								f.ID,
								f.Name,
								f.Descript,
								MsgCount = f.TopicInfos.Sum(ti => ti.AnswersCount),
								Unread = f.TopicInfos.Sum(ti => ti.AnswersUnread),
								MeUnread = f.TopicInfos.Sum(ti => ti.AnswersToMeUnread)
							})
						.Single();
				ID = forum.ID;
				Name = forum.Name;
				ForumDescription = forum.Descript;
				MessagesCount = forum.MsgCount;
				Unread = forum.Unread;
				RepliesToMeUnread = forum.MeUnread;
			}
		}

		#endregion
	}
}