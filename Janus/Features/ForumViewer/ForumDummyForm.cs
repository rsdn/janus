#pragma warning disable 1692

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Windows.Forms;

using Rsdn.Janus.Framework;
using Rsdn.Janus.ObjectModel;
using Rsdn.Shortcuts;
using Rsdn.TreeGrid;
using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	internal sealed partial class ForumDummyForm : UserControl, IFeatureView
	{
		private readonly ServiceManager _serviceManager;
		private readonly AsyncOperation _asyncOperation;
		private StripMenuGenerator _menuGenerator;
		private MsgViewer _msgViewer;
		private readonly DragStartDetector _dragStartDetector = new DragStartDetector();
		private readonly IDisposable _eventsSubscription;

		// ReSharper disable ConvertToConstant, RedundantDefaultFieldInitializer
		[ExpectService]
		private readonly IForumImageManager _imageManager = null;
		// ReSharper restore ConvertToConstant, RedundantDefaultFieldInitializer

		#region StepDirection Enum
		public enum StepDirection
		{
			Up,
			Down
		}
		#endregion

		#region AttrType Enum
		public enum AttrType
		{
			Unread,
			UnreadAnswerToMe,
			Marked,
			Any
		}
		#endregion

		#region SearchMessageArea Enum
		public enum SearchMessageArea
		{
			CurrentTopic,
			CurrentForum,
			//Global
		}
		#endregion

		#region IFeatureView
		void IFeatureView.Activate(IFeature feature)
		{
			HideFilterPanel();

			//Feature feature = (Feature)feature;
			//Features.Instance.ActiveFeature = feature;
			FillMessages();
		}

		void IFeatureView.Refresh()
		{
			FillMessages();

			if (_isFilterActive)
				ApplyCurrentFilter();
		}

		void IFeatureView.ConfigChanged()
		{
			//TODO: надо добавить сюда все действия которые нужно сделать при изменении конфига.
			SetSetings();
			FillMessages();

			if (_isFilterActive)
				ApplyCurrentFilter();
		}

		private void SetSetings()
		{
			if (Forums.Instance.ActiveForum != null)
			{
				_tgMsgs.Indent = Config.Instance.ForumDisplayConfig.GridIndent;
				_tgMsgs.GridLines = Config.Instance.ForumDisplayConfig.MsgListGridLines;
			}
		}
		#endregion IFeatureView

		#region Instance Property
		private static ForumDummyForm _instance;

		public static ForumDummyForm Instance
		{
			get
			{
				return
					_instance
						?? (_instance = new ForumDummyForm(ApplicationManager.Instance.ServiceProvider));
			}
		}
		#endregion

		#region Поддержка стилей
		private void OnStyleChanged(object s, StyleChangeEventArgs e)
		{
			UpdateStyle();
			Refresh();
		}

		private void UpdateStyle()
		{
			_tgMsgs.Font = Config.Instance.StyleConfig.MessageTreeFont;
			_tgMsgs.BackColor = Config.Instance.StyleConfig.MessageTreeBack;
		}
		#endregion

		#region Состояние
		private ForumFormState _state = ForumFormState.Normal;

		public ForumFormState State
		{
			get { return _state; }
			set
			{
				if (_state == value)
					return;

				switch (value)
				{
					case ForumFormState.Normal:
						_topPanel.Visible = false;
						_bottomPanel.Visible = false;
						_splitter.Visible = true;
						break;
					case ForumFormState.ListMaximized:
						_topPanel.Visible = true;
						_bottomPanel.Visible = false;
						_splitter.Visible = false;
						break;
					case ForumFormState.MessageMaximized:
						_topPanel.Visible = false;
						_bottomPanel.Visible = true;
						_splitter.Visible = false;
						break;
				}

				_state = value;
				Config.Instance.ForumFormState = _state;
			}
		}
		#endregion

		#region Инициализация

		private ForumDummyForm(IServiceProvider provider)
		{
			_serviceManager = new ServiceManager(provider);
			_asyncOperation = AsyncHelper.CreateOperation();

			_serviceManager.Publish<IDefaultCommandService>(
				new DefaultCommandService("Janus.Forum.ReplyMessage"));

			this.AssignServices(provider);

			InitializeComponent();

			SetPositionViewMsgArea(Config.Instance.MsgPosition);
			CustomInitializeComponent();

			_eventsSubscription = EventBrokerHelper.SubscribeEventHandlers(this, _serviceManager);
		}

		private void CustomInitializeComponent()
		{
			StyleConfig.StyleChange += OnStyleChanged;

			State = Config.Instance.ForumFormState;

			var imageList = new ImageList { ColorDepth = ColorDepth.Depth32Bit };

			// Делаем поддержку локализации для TreeGrid'а
			// В случае изменения TreeGrid'а в Designer'е, необходимо внести изменения сюда
			_tgMsgs.Columns[1].ImageIndex =
				imageList.AddImage(_imageManager.GetMarkImage(MessageFlagExistence.OnMessage));

			_tgMsgs.Columns[2].Text = SR.TGColumnSubject;
			_tgMsgs.Columns[2].ImageIndex =
				imageList.AddImage(
					_imageManager.GetMessageImage(
					MessageType.Ordinal,
					MessageFlagExistence.None,
					false,
					MessageFlagExistence.None,
					false));

			_tgMsgs.Columns[3].Text = SR.TGColumnAuthor;
			_tgMsgs.Columns[3].ImageIndex =
				imageList.AddImage(_imageManager.GetUserImage(UserClass.User));

			_tgMsgs.Columns[4].Text = SR.TGColumnRate;

			_tgMsgs.Columns[5].Text = SR.TGColumnSubjectRate;

			_tgMsgs.Columns[6].Text = SR.TGColumnAnswers;

			_tgMsgs.Columns[7].Text = SR.TGColumnDate;
			_tgMsgs.Columns[7].ImageIndex =
				imageList.AddImage(_imageManager.GetMessageDateImage(DateTime.Now));

			_tgMsgs.Font = Config.Instance.StyleConfig.MessageTreeFont;
			_tgMsgs.BackColor = Config.Instance.StyleConfig.MessageTreeBack;
			_tgMsgs.GridLines = Config.Instance.ForumDisplayConfig.MsgListGridLines;
			_tgMsgs.SmallImageList = imageList;

			if (Config.Instance.ForumColumnOrder.Length == _tgMsgs.Columns.Count)
				_tgMsgs.ColumnsOrder = Config.Instance.ForumColumnOrder;
			if (Config.Instance.ForumColumnWidth.Length == _tgMsgs.Columns.Count)
				_tgMsgs.ColumnsWidth = Config.Instance.ForumColumnWidth;

			_msgViewer = new MsgViewer(_serviceManager) { Dock = DockStyle.Fill };
			_bottomPanel.Controls.Add(_msgViewer);

			ApplicationManager.Instance.ForumNavigator.MessageNavigated += MessageNavigated;

			_btnResetFilter.Text = SR.Forum.ResetFilter;
		}
		#endregion

		#region Загрузка списка сообщений и самого сообщения

		#region FillMessages

		public bool SelectMessage(int msgID)
		{
			var forum = Features.Instance.ActiveFeature as Forum;

			if (forum == null)
				return false;

			_tgMsgs.ActiveNode = forum.FindMsgById(msgID);
			return _tgMsgs.ActiveNode != null;
		}

		public bool SelectMessage(int forumID, int msgID)
		{
			var activeForum = Forums.Instance.ActiveForum;

			if (activeForum != null && activeForum.ID == forumID)
				return SelectMessage(msgID);

			using (_tgMsgs.UpdateScope())
			{
				_tgMsgs.Nodes = null;
				var forum = Forums.Instance[forumID];
				Features.Instance.ActiveFeature = forum;
				_tgMsgs.Nodes = forum.Msgs;
				_tgMsgs.ActiveNode = forum.FindMsgById(msgID);

				return _tgMsgs.ActiveNode != null;
			}
		}

		public void FillMessages()
		{
			var forum = Features.Instance.ActiveFeature as Forum;
			if (forum == null)
				return;

			// При получении активного сообщения теперь может произойти
			// подчитывание несчитанных сообщений форум. При этом список 
			// сообщений может устареть. Чтобы в _tgMsgs.Nodes не оказался
			// устаревший список сообщений нужно присваивать его после 
			// считывания активного сообщения.
			var activeMsg = forum.ActiveMsg;
			// Внимание!!! Порядок считывания forum.ActiveMsg и forum.Msgs критичен!

			_tgMsgs.Nodes = forum.Msgs;

			if (activeMsg == null)
			{
				if (_tgMsgs.Nodes != null && _tgMsgs.Nodes.Count > 0)
					_tgMsgs.ActiveNodeIndex = 0;
			}
			else
				_tgMsgs.ActiveNode = activeMsg;

			_markTimer.Enabled = false;
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);

			// Какого-то хрена при Visible == false не удается установить 
			// активную ветку. Поэтому, в момент когда Visible становится равным true
			// нужно попытаться еще раз активировать нужную ветку.
			if (Visible)
			{
				var forum = Features.Instance.ActiveFeature as Forum;
				if (forum == null)
					return;

				var activeMsg = forum.ActiveMsg;
				if (activeMsg == null)
				{
					if (_tgMsgs.Nodes != null && _tgMsgs.Nodes.Count > 0)
						_tgMsgs.ActiveNodeIndex = 0;
				}
				else
					#region Удалить закомментированный код если ошибка не будет повторятся
					//					TODO: удалить закомментированный код если ошибка не будет повторятся
					//					try
					//					{
					_tgMsgs.ActiveNode = activeMsg;
				//					}
				//					catch (Exception ex)
				//					{
				//						// Ошибка "Строка не подключена к TreGrid!" возникает
				//						// когда кто-то пытается сделать активной строку 
				//						// отсуствующей в гриде в даннымй момент.
				//						// В Янусе эта ошибка обычно возникает, когда 
				//						// переходишь из форума X в список неотправленных 
				//						// сообщений, а затем в другой форум.
				//						// При этом в activeMsg уже содержится строка из нового
				//						// форума, а в _tgMsgs.Nodes еще содержатся сообщения
				//						// старого. Причем OnVisibleChanged вызывается как-то 
				//						// не явно (похоже из хука). Так что разруливайте
				//						// логику вызова OnVisibleChanged.
				//						// Пока что я в тупую скрываю данное исключение.
				//						// Вроде как дальше по ходу пьесы все делается нормально.
				//						// То есть в грид помещаются сообщения нового форума
				//						// и происходит повторная попытка активировать 
				//						// ветку, которая должна быть активной.
				//						if (ex.Message != "Строка не подключена к TreGrid!")
				//							throw;
				//					}
					#endregion
			}
		}
		#endregion

		#endregion

		#region Shortcuts

		[MethodShortcut((Shortcut)Keys.Space, "Смарт-переход",
			"Прокрутить сообщение на страницу. При небходимости перейти к следующему сообщению.")]
		private void PageDown()
		{
			_serviceManager.TryExecuteCommand("Janus.Forum.SmartJump", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.None, "Адрес сообщения",
			"Скопировать адрес сообщения в буфер обмена.")]
		private void CopyMsgAddress()
		{
			_serviceManager.TryExecuteCommand("Janus.Forum.CopyMessageAddress", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.CtrlO, "Открыть сообщение",
			"Открыть сообщение в JBrowser.")]
		private void ShowMessageIntoJBrowser()
		{
			_serviceManager.TryExecuteCommand("Janus.Forum.OpenMessageInJBrowser", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.CtrlShiftO, "Открыть сообщение на RSDN",
			"Открыть сообщение на сайте RSDN.")]
		private void ShowMessageIntoExtBrowser()
		{
			_serviceManager.TryExecuteCommand("Janus.Forum.OpenMessageOnRsdn", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.CtrlShiftC, "Адрес автора сообщения",
			"Скопировать адрес автора сообщения в буфер обмена.")]
		private void CopyAuthorAddress()
		{
			_serviceManager.TryExecuteCommand("Janus.Forum.CopyMessageAuthorAddress", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.CtrlM, "Модерирование",
			"Открыть модерирование на сайте RSDN.")]
		private void OpenSelfModerate()
		{
			_serviceManager.TryExecuteCommand("Janus.Forum.OpenModeratingOnRsdn", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.None, "Оценки",
			"Открыть оценки на сайте RSDN.")]
		private void OpenRating()
		{
			_serviceManager.TryExecuteCommand("Janus.Forum.OpenMessageRatingOnRsdn", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.None, "Список оценок",
			"Список оценок пользователя.")]
		private void ShowUserRatingIn()
		{
			_serviceManager.TryExecuteCommand("Janus.Forum.ShowUserRatingIn", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.None, "Список всех статей",
			"Список всех статей.")]
		private void ShowAllArticle()
		{
			_serviceManager.TryExecuteCommand("Janus.Forum.ShowAllArticles", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.None, "Список статей форума",
			"Список статей форума.")]
		public void ShowArticle()
		{
			_serviceManager.TryExecuteCommand("Janus.Forum.ShowForumArticles", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.None, "Список всех ФАКУ",
			"Список всех ФАКУ.")]
		private void ShowAllFaq()
		{
			_serviceManager.TryExecuteCommand("Janus.Forum.ShowAllFaq", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.None, "Список ФАКУ форума",
			"Список ФАКУ форума.")]
		private void ShowFaq()
		{
			_serviceManager.TryExecuteCommand("Janus.Forum.ShowForumFaq", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.CtrlN, "Новое сообщение",
			"Написать новое сообщение.")]
		private void WriteMessage()
		{
			_serviceManager.TryExecuteCommand("Janus.Forum.WriteMessage", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.CtrlR, "Ответ", "Ответ на сообщение.")]
		private void ReplyMessage()
		{
			_serviceManager.TryExecuteCommand("Janus.Forum.ReplyMessage", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.CtrlQ, "Сообщение прочитано",
			"Пометить сообщение как прочитанное.")]
		private void MarkMessagesRead()
		{
			_serviceManager.TryExecuteCommand(
				"Janus.Forum.SetMessagesReadMark",
				new Dictionary<string, object> { { "isRead", true }, { "markChilds", false } });
		}

		[MethodShortcut(Shortcut.CtrlShiftQ, "Сообщение не прочитано",
			"Пометить сообщение как не прочитанное.")]
		public void MarkMessagesUnRead()
		{
			_serviceManager.TryExecuteCommand(
				"Janus.Forum.SetMessagesReadMark",
				new Dictionary<string, object> { { "isRead", false }, { "markChilds", false } });
		}

		[MethodShortcut(Shortcut.CtrlT, "Ответы прочитаны",
			"Помечать ответы на это сообщение как прочитанные.")]
		private void ReadRepliesSingle()
		{
			_serviceManager.TryExecuteCommand(
				"Janus.Forum.SetMessagesReadMark",
				new Dictionary<string, object> { { "isRead", true }, { "markChilds", true } });
		}

		[MethodShortcut(Shortcut.CtrlShiftT, "Ответы не прочитаны",
			"Помечать ответы на это сообщение как непрочитанные.")]
		private void UnreadRepliesSingle()
		{
			_serviceManager.TryExecuteCommand(
				"Janus.Forum.SetMessagesReadMark",
				new Dictionary<string, object> { { "isRead", false }, { "markChilds", true } });
		}

		[MethodShortcut(Shortcut.CtrlY, "Ответы по теме прочитаны",
			"Помечать ответы в эту тему как прочитанные.")]
		private void ReadRepliesAll()
		{
			//ToDo: implement!
		}

		[MethodShortcut(Shortcut.CtrlShiftY, "Ответы по теме прочитаны",
			"Помечать ответы в эту тему как прочитанные.")]
		private void UnreadRepliesAll()
		{
			//ToDo: delete!
		}

		[MethodShortcut(Shortcut.None, "Добавить в Избранное",
			"Добавить сообщение в Избранное")]
		private void AddToFavorites()
		{
			_serviceManager.TryExecuteCommand("Janus.Forum.AddForumMessageToFavorites", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.CtrlW, "Тема прочитана",
			"Пометить тему как прочитанную.")]
		private void MarkConvRead()
		{
			_serviceManager.TryExecuteCommand(
				"Janus.Forum.SetTopicReadMark",
				new Dictionary<string, object> { { "isRead", true } });
		}

		[MethodShortcut(Shortcut.CtrlShiftW, "Тема не прочитана",
			"Пометить тему как не прочитанную.")]
		private void MarkConvUnRead()
		{
			_serviceManager.TryExecuteCommand(
				"Janus.Forum.SetTopicReadMark",
				new Dictionary<string, object> { { "isRead", false } });
		}

		[MethodShortcut((Shortcut)Keys.None, "Предыдущее непрочитанное в теме",
			"Перейти к предыдущему непрочитанному сообщению в текущей теме.")]
		private void SelectHighlightedMsgPrevInCurTipic()
		{
			SelectNodeByAttribute(StepDirection.Up, AttrType.Unread, SearchMessageArea.CurrentTopic);
		}

		[MethodShortcut((Shortcut)(Keys.Control | Keys.Up), "Предыдущее непрочитанное",
			"Перейти к предыдущему непрочитанному сообщению.")]
		private void SelectHighlightedMsgPrev()
		{
			SelectNodeByAttribute(StepDirection.Up, AttrType.Unread, SearchMessageArea.CurrentForum);
		}

		[MethodShortcut(Shortcut.None, "Свернуть и перейти в начало",
			"Свернуть и перейти в корневое сообщение темы.")]
		private void CollapseAndGoRootShortcut()
		{
			CollapseAndGoRoot();
		}

		[MethodShortcut((Shortcut)(Keys.Control | Keys.Down), "Cледущее непрочитанное",
			"Перейти к следущему непрочитанному сообщению.")]
		private void SelectHighlightedMsgNext()
		{
			SelectNodeByAttribute(StepDirection.Down, AttrType.Unread, SearchMessageArea.CurrentForum);
		}

		[MethodShortcut((Shortcut)Keys.None, "Cледущее непрочитанное в теме",
			"Перейти к следущему непрочитанному сообщению в просматриваемой теме.")]
		public void SelectHighlightedMsgNextInCurrTopic()
		{
			SelectNodeByAttribute(StepDirection.Down, AttrType.Unread, SearchMessageArea.CurrentTopic);
		}

		[MethodShortcut(Shortcut.None, "Развернуть только непрочитанное",
			"Развернуть только непрочитанные ветки темы.")]
		private void ExpandUnreadShortcut()
		{
			ExpandUnread();
		}

		[MethodShortcut((Shortcut)(Keys.Control | Keys.Shift | Keys.Up), "Предыдущий ответ мне",
			"Перейти к предыдущему непрочитанному сообщению являющемуся ответом на мое сообщение.")]
		private void SelectHighlightedAnswerToMyMsgPrev()
		{
			SelectNodeByAttribute(StepDirection.Up, AttrType.UnreadAnswerToMe, SearchMessageArea.CurrentForum);
		}

		[MethodShortcut((Shortcut)(Keys.Control | Keys.Shift | Keys.Down), "Следущий ответ мне",
			"Перейти к следующему непрочитанному сообщению являющемуся ответом на мое сообщение.")]
		private void SelectHighlightedAnswerToMyMsgNext()
		{
			SelectNodeByAttribute(StepDirection.Down, AttrType.UnreadAnswerToMe, SearchMessageArea.CurrentForum);
		}

		[MethodShortcut((Shortcut)(Keys.Shift | Keys.Up), "Предыдущее помеченное",
			"Перейти к предыдущему помеченному сообщению.")]
		private void SelectMarkedMsgPrev()
		{
			SelectNodeByAttribute(StepDirection.Up, AttrType.Marked, SearchMessageArea.CurrentForum);
		}

		[MethodShortcut((Shortcut)(Keys.Shift | Keys.Down), "Следущее помеченное",
			"Перейти к следующему помеченному сообщению.")]
		private void SelectMarkedMsgNext()
		{
			SelectNodeByAttribute(StepDirection.Down, AttrType.Marked, SearchMessageArea.CurrentForum);
		}

		#endregion

		#region Navigation history support

		private void MessageNavigated(object sender, EventArgs e)
		{
			_msgViewer.Msg = Forums.Instance.ActiveForum.ActiveMsg;

			if (_tgMsgs.ActiveNode != Forums.Instance.ActiveForum.ActiveMsg)
				_tgMsgs.ActiveNode = Forums.Instance.ActiveForum.ActiveMsg;

			if (Config.Instance.MarkNavigatedMessages)
				StartMarkTimer();
		}

		#endregion

		#region Обработчики событий объектной модели.

		private void StartMarkTimer()
		{
			_markTimer.Enabled = false;
			_markTimer.Interval = Config.Instance.MarkMessageReadInterval;
			_markTimer.Enabled = true;
		}

		private void _markTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			_markTimer.Enabled = false;

			try
			{
				var msg = _tgMsgs.ActiveNode as IMsg;

				if (msg != null && msg.IsUnread)
					if (_tgMsgs.SelectedNodes.Count == 1)
					{
						ForumHelper.MarkMsgRead(
							_serviceManager, _tgMsgs.SelectedNodes.Cast<MsgBase>(), true, false);
					}
			}
			catch
			{
				// Если не сложилось, пробуем отложить операцию до лучших времен.
				_markTimer.Enabled = true;
			}
		}

		private void _tgMsgs_AfterActivateNode(ITreeNode ActivatedNode)
		{
			var msg = (IMsg)ActivatedNode;

			if (msg.ID == _oldMsgID)
				return;

			_oldMsgID = msg.ID;

			if (ApplicationManager.Instance.ForumNavigator.NavigationMode
				|| ActivatedNode == null)
				return;

			// Баг: http://rsdn.ru/Forum/Message.aspx?mid=2805588&only=1
			//if (msg.ID == _oldMsgID)
			//	return;

			if (Forums.Instance.ActiveForum == null)
				return;

			// UNDONE: Сомнительное решение.
			// Если наполнение тригрида не совпадает с текущим форумом,
			// то обновляем наполнение тригрида ...
			// Это мешает реализации фильтрации содержимого тригрида, 
			// да и по идее должно отлавливаться другими способами.
			// Пробуем временно убрать.
			/*
				if (forum.Msgs != _tgMsgs.Nodes)
				{
					_tgMsgs.Nodes = forum;
					curMsg = forum.FindMsgByID(msg.ID);
					if (curMsg == null)
						return;
				}
				else
				*/

			var curMsg = msg;

			// Игрушечная асинхронность, которой на самом деле нет.
			// Однако позволяет не блокировать сразу цикл обработки сообщений,
			// что позволяет быстро перещелкивать сообщения в гриде с клавиатуры
			// без неприятного замедления времени отклика
			AsyncHelper.CreateOperation().PostOperationCompleted(
				() => ApplicationManager.Instance.ForumNavigator
					.SelectMessage(curMsg.ForumID, curMsg.ID));

			//_oldMsgID = curMsg.ID;
			StartMarkTimer();

			OnSelectedMessagesChanged(EventArgs.Empty);
		}

		private void _tgMsgs_MouseUp(object sender, MouseEventArgs e)
		{
			_dragStartDetector.MouseUp(e);

			if ((e.Button & MouseButtons.Right) != 0)
				_contextMenuStrip.Show((Control)sender, e.Location);
		}

		private void _tgMsgs_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			var p = _tgMsgs.PointToClient(Cursor.Position);

			if (CheckMark(p.X, p.Y))
				return;

			_serviceManager.ExecuteDefaultCommand();
		}

		private void _tgMsgs_BeforeMouseDown(object sender, BeforeMouseDownEventArgs e)
		{
			if (CheckMark(e.X, e.Y))
				e.Cancel = true;
		}

		private void _tgMsgs_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			var sort = Config.Instance.ForumSortCriteria;

			switch (e.Column)
			{
				case 0:
					sort = sort == SortType.ByIdAsc
							? SortType.ByIdDesc
							: SortType.ByIdAsc;
					break;
				case 2:
					sort = sort == SortType.BySubjectAsc
							? SortType.BySubjectDesc
							: SortType.BySubjectAsc;
					break;
				case 3:
					sort = sort == SortType.ByAuthorAsc
							? SortType.ByAuthorDesc
							: SortType.ByAuthorAsc;
					break;
				case 7:
					sort = sort == SortType.ByLastUpdateDateDesc
							? SortType.ByLastUpdateDateAsc
							: SortType.ByLastUpdateDateDesc;
					break;
			}

			if (Config.Instance.ForumSortCriteria == sort)
				return;
			Config.Instance.ForumSortCriteria = sort;

			var forum = Features.Instance.ActiveFeature as Forum;
			if (forum != null)
			{
				forum.Refresh();

				var active = _tgMsgs.ActiveNode as IMsg;

				_tgMsgs.Nodes = forum.Msgs;

				if (active != null)
					_tgMsgs.ActiveNode = forum.FindMsgById(active.ID);

				ResetFilter();
			}
		}

		private void _tgMsgs_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && !CheckMark(e.X, e.Y))
				_dragStartDetector.MouseDown(e);
		}

		private void _tgMsgs_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left || !_dragStartDetector.MouseMove(e))
				return;
			if (_tgMsgs.SelectedNodes.Count > 0)
			{
				var nodes = new TreeNodeBag(_tgMsgs.SelectedNodes);

				try
				{
					_tgMsgs.DoDragDrop(nodes, DragDropEffects.Copy | DragDropEffects.Link);
				}
				catch (Exception ex)
				{
					Debug.Print(ex.ToString());
				} //try
			} //if

			_dragStartDetector.Reset();
		}

		private void _tgMsgs_ColumnReordered(object sender, ColumnReorderedEventArgs e)
		{
			Config.Instance.ForumColumnOrder = _tgMsgs.ColumnsOrder;
		}

		private void _tgMsgs_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
		{
			Config.Instance.ForumColumnWidth = _tgMsgs.ColumnsWidth;
		}

		private void _splitter_SplitterMoved(object sender, SplitterEventArgs e)
		{
			Config.Instance.ForumSplitterPosition = _splitter.SplitPosition;
		}

		[EventHandler(ForumEventNames.BeforeForumEntryChanged)]
		private void BeforeForumEntryChanged(ForumEntryChangedEventArgs args)
		{
			_asyncOperation.Send(
				() =>
				{
					bool isWholeForum;
					if (Forums.Instance.ActiveForum == null ||
							!args.Entries.IsContainsForum(Forums.Instance.ActiveForum.ID, out isWholeForum))
						return;

					if (isWholeForum)
						_tgMsgs.Nodes = null;
					StopMarkTimer();
				});
		}

		[EventHandler(ForumEventNames.AfterForumEntryChanged)]
		private void AfterForumEntryChanged(ForumEntryChangedEventArgs args)
		{
			_asyncOperation.Send(
				() =>
				{
					bool isWholeForum;
					if (Forums.Instance.ActiveForum == null ||
							!args.Entries.IsContainsForum(Forums.Instance.ActiveForum.ID, out isWholeForum))
						return;

					if (isWholeForum)
					{
						_tgMsgs.Nodes = Forums.Instance.ActiveForum.Msgs;
						_tgMsgs.ActiveNode = Forums.Instance.ActiveForum.ActiveMsg;
					}
					_tgMsgs.Invalidate();
				});
		}

		#endregion

		#region Helper methods
		private int _oldMsgID = -1;

		private void SetPositionViewMsgArea(bool toRight)
		{
			if (toRight)
			{
				_topPanel.Dock = DockStyle.Left;
				_splitter.Dock = DockStyle.Left;
			}
		}

		private void SetMessageMarked(MsgBase msg)
		{
			if (msg == null)
				return;
			_markTimer.Enabled = false;
			msg.SetMessageMarked(!msg.Marked);
			_tgMsgs.Update();
		}

		private static bool IsMsgConformAttribute(IMsg msg, AttrType attrType)
		{
			switch (attrType)
			{
				case AttrType.Any:
					return true;
				case AttrType.Unread:
					return msg.IsUnread;
				case AttrType.Marked:
					return msg.Marked;
				case AttrType.UnreadAnswerToMe:
					return msg.IsUnread
						&& msg.Parent.UserID > 0
							&& msg.Parent.UserID == Config.Instance.SelfId;
			}

			return false;
		}

		private void DoExpandUnread(IMsg node)
		{
			if (node.HasRepliesUnread)
				_tgMsgs.ExpandNode(node);
			else
				_tgMsgs.CollapseNode(node);

			if (node.HasChildren)
				foreach (IMsg item in node)
					DoExpandUnread(item);
		}

		private bool CheckMark(int x, int y)
		{
			int column;
			ITreeNode node;

			_tgMsgs.GetNodeAndCellIndexAt(x, y, out node, out column);

			if (column == 1)
			{
				SetMessageMarked((MsgBase)node);
				_tgMsgs.Invalidate();
				_tgMsgs.Update();

				return true;
			}

			return false;
		}

		private IMsg GetActiveMsgList()
		{
			var forum = (Forum)Features.Instance.ActiveFeature;
			return _isFilterActive
				? GetFilteredMsgList(_serviceManager, forum.Msgs, _tbxFilter.Text)
				: forum.Msgs;
		}

		//private static bool MarkItemWithChildTags(ToolStripMenuItem ti, Image markImage)
		//{
		//    var childChecked = false;
		//    foreach (ToolStripItem item in ti.DropDownItems)
		//    {
		//        var child = item as ToolStripMenuItem;
		//        if (child != null && MarkItemWithChildTags(child, markImage))
		//            childChecked = true;
		//    }
		//    if (!ti.Checked && childChecked)
		//        ti.Image = markImage;
		//    return childChecked || ti.Checked;
		//}
		#endregion

		#region Фильтрация по темам в списке сообщений
		private bool _isFilterActive;

		private string _lastFilterValue = String.Empty;

		private static IMsg GetFilteredMsgList(
			IServiceProvider provider,
			IMsg iMsg,
			string filterText)
		{
			if (iMsg is Msg)
				return Msg.FilterFirstLevel(provider, (Msg)iMsg, filterText);

			return iMsg;
		}

		private void ApplyCurrentFilter()
		{
			ApplyFilter(_tbxFilter.Text);
		}

		private void ApplyFilter(string filterText)
		{
			var forum = (Forum)Features.Instance.ActiveFeature;

			if (!string.IsNullOrEmpty(filterText))
				_tgMsgs.Nodes = GetFilteredMsgList(_serviceManager, forum.Msgs, filterText);
			else if (_tgMsgs.Nodes != forum.Msgs)
			{
				_tgMsgs.Nodes = forum.Msgs;

				// HACK: Хак, чтобы вернуть выделение ноды.
				_tgMsgs.ActiveNode = _tgMsgs.ActiveNode;
			}
		}

		private void ResetFilter()
		{
			_lastFilterValue = "";

			if (_tbxFilter.Text != "")
			{
				_tbxFilter.Text = "";
				_tbxFilter.Focus();

				_tgMsgs.Nodes = GetActiveMsgList();

				// HACK: Хак, чтобы вернуть выделение ноды.
				_tgMsgs.ActiveNode = _tgMsgs.ActiveNode;
			}
		}

		[MethodShortcut(Shortcut.F3, "Спрятать/Показать панель с фильтром",
			"Спрятать или показать панель для фильтрации тем в форуме.")]
		public void ShowHideFilterPanel()
		{
			if (!_filterPanel.Visible)
				ShowFilterPanel();
			else
				HideFilterPanel();
		}

		private void HideFilterPanel()
		{
			_filterPanel.Visible = false;
			_isFilterActive = false;

			ResetFilter();
		}

		private void ShowFilterPanel()
		{
			_filterPanel.Visible = true;
			_isFilterActive = true;

			_tbxFilter.Focus();
		}

		private void _btnHideFilterPanel_Click(object sender, EventArgs e)
		{
			HideFilterPanel();
		}

		private void _tbxFilter_KeyUp(object sender, KeyEventArgs e)
		{
			if (_lastFilterValue != _tbxFilter.Text)
				ApplyFilter(_tbxFilter.Text);
			_lastFilterValue = _tbxFilter.Text;
		}

		private void _btnResetFilter_Click(object sender, EventArgs e)
		{
			ResetFilter();
		}
		#endregion

		#region Public Members

		public void ExpandUnread()
		{
			var current = (IMsg)_tgMsgs.ActiveNode;

			if (current == null)
				return;

			//IMsg topic = current.Topic;
			//if (topic == null)
			//    return;

			DoExpandUnread(current);
			_tgMsgs.Refresh();
		}

		public void CollapseAndGoRoot()
		{
			var current = (IMsg)_tgMsgs.ActiveNode;
			if (current == null)
				return;

			var topic = current.Topic;

			if (topic == null)
				return;

			_tgMsgs.ActiveNode = topic;

			if ((topic.Flags & NodeFlags.Expanded) == NodeFlags.None)
				return;

			_tgMsgs.CollapseNode(topic);
			_tgMsgs.Refresh();
		}

		public void SmartJump()
		{
			var activeForum = ApplicationManager.Forums.ActiveForum;

			// Если активного форума нет или просматриваемое сообщение
			// еще прокручивается, то ничего не делаем.
			if (activeForum == null || _msgViewer.PageDown())
				return;

			// Поведение "Переход по пробелу".
			var behavior = Config.Instance.SmartJumpBehavior;

			// Если необходимо только перейти к следующему сообщению
			if (behavior == SmartJumpBehavior.NextAny)
			{
				SelectNodeByAttribute(StepDirection.Down, AttrType.Any, SearchMessageArea.CurrentForum);
				return;
			}

			// Ищем следующий по списку форум с непрочитанными сообщениями,
			// включая текущий.
			var unreadForum = behavior == SmartJumpBehavior.NextUnreadForum
				? Navigator.FindNextUnreadForum()
				: activeForum;

			// Ищем сообщение...
			IMsg unreadMsg = null;
			if (unreadForum != null)
				unreadMsg = unreadForum.FindNextUnreadMsg(activeForum == unreadForum);

			// Если все сообщения в форумах прочитаны, то просто
			// переходим к следующему.
			if (unreadMsg != null)
			{
				if (Forums.Instance.ActiveForum != unreadForum)
					Forums.Instance.ActiveForum = unreadForum;

				_tgMsgs.ActiveNode = unreadMsg;
			}
			else
			{
				SelectNodeByAttribute(StepDirection.Down, AttrType.Any, SearchMessageArea.CurrentForum);
			}
		}

		public void StopMarkTimer()
		{
			_markTimer.Enabled = false;
		}

		public void SelectNodeByAttribute(StepDirection dir, AttrType attrType, SearchMessageArea area)
		{
			var activeNode = (IMsg)_tgMsgs.ActiveNode;

			if (activeNode == null)
				return;

			var topic = activeNode.Topic;

			// Ищем в текущей ветке.
			var treeNodes = dir == StepDirection.Down
				? TreeGrid.TreeGrid.GetFlatArrayOfSubNodes(topic, activeNode)
				: TreeGrid.TreeGrid.GetFlatArrayOfSubNodesReverse(topic, activeNode);

			foreach (IMsg msg in treeNodes)
				if (IsMsgConformAttribute(msg, attrType))
				{
					_tgMsgs.ActiveNode = msg;
					return;
				}

			// Если не нашли, то ищем темы (кроневые ветки) имеющие ответы.
			var isCurrPassed = false;
			var messages = new Msg[_tgMsgs.Nodes.Count];

			_tgMsgs.Nodes.CopyTo(messages, 0);

			if (dir == StepDirection.Up)
				Array.Reverse(messages);

			foreach (IMsg msg in messages)
			{
				// Пропускаем все ветки пока не найдем текущую тему.
				if (!isCurrPassed && msg != topic)
					continue;

				if (!isCurrPassed)
				{
					isCurrPassed = true;
					continue; // Текущую тему тоже нужно пропустить.
				}

				// если топик сменился, а нас просили искать только в текущем топике - выходим
				if (area == SearchMessageArea.CurrentTopic && msg.TopicID != topic.TopicID)
					break;

				// Если в тему есть 
				var isFound = false;
				switch (attrType)
				{
					case AttrType.Any:
						isFound = true;
						break;
					case AttrType.Marked:
						isFound = msg.RepliesMarked > 0 || msg.Marked;
						break;
					case AttrType.Unread:
						isFound = msg.RepliesUnread > 0 || msg.IsUnread;
						break;
					case AttrType.UnreadAnswerToMe:
						isFound = msg.RepliesToMeUnread > 0;
						break;
				}

				if (isFound)
				{
					switch (attrType)
					{
						case AttrType.Any:
							break;
						case AttrType.Marked:
							isFound = msg.IsMarked;
							break;
						case AttrType.Unread:
							isFound = msg.IsUnread;
							break;
						default:
							isFound = false;
							break;
					}

					if (isFound)
					{
						_tgMsgs.ActiveNode = msg;
						return;
					}

					treeNodes = TreeGrid.TreeGrid.GetFlatArrayOfSubNodes(msg);

					if (dir == StepDirection.Up)
						Array.Reverse(treeNodes);

					foreach (IMsg subMsg in treeNodes)
						if (IsMsgConformAttribute(subMsg, attrType))
						{
							_tgMsgs.ActiveNode = subMsg;
							return;
						}

					throw new ApplicationException(
						"Аргрегированная информация не соответсвует реальному содержанию БД. Произведите " +
							"пересчет БД и обратитесь к разработчикам.");
				}
			}

			Beeper.DoBeep();
			return;
		}

		public IEnumerable<IMsg> SelectedMessages
		{
			get
			{
				IEnumerable<IMsg> result = null;
				_asyncOperation.Send(() => result = _tgMsgs.SelectedNodes.OfType<IMsg>());
				return result;
			}
		}

		public event EventHandler SelectedMessagesChanged;

		#endregion

		#region Protected Members

		protected override void OnLoad(EventArgs e)
		{
			SetSetings();

			_splitter.SplitPosition = Config.Instance.ForumSplitterPosition;

			_menuGenerator = new StripMenuGenerator(_serviceManager, _contextMenuStrip, "ForumMessage.ContextMenu");

			base.OnLoad(e);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_menuGenerator != null)
					_menuGenerator.Dispose();

				_eventsSubscription.Dispose();

				StyleConfig.StyleChange -= OnStyleChanged;

				if (components != null)
					components.Dispose();
			}

			base.Dispose(disposing);
		}

		private void OnSelectedMessagesChanged(EventArgs e)
		{
			if (SelectedMessagesChanged != null)
				SelectedMessagesChanged(this, e);
		}

		#endregion
	}
}
