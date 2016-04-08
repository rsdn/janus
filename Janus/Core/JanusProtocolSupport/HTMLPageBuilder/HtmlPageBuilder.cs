using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

using CodeJam.Collections;
using CodeJam.Extensibility;
using CodeJam.Extensibility.Model;
using CodeJam.Services;

using LinqToDB;

namespace Rsdn.Janus
{
	/// <summary>
	/// Создает контент для WebBrowserForm.HtmlPageBuilder.
	/// </summary>
	public class HtmlPageBuilder : ServiceConsumer
	{
		private const string _resourcePrefix =
			"Rsdn.Janus.Core.JanusProtocolSupport.HtmlPageBuilder.";

		private static readonly ILazyDictionary<string, string> _templatesCache =
			LazyDictionary.Create<string, string>(LoadStringTemplate, true);

		private readonly IServiceProvider _serviceProvider;
		// ReSharper disable ConvertToConstant, RedundantDefaultFieldInitializer
		[ExpectService]
		private readonly IJanusDatabaseManager _dbManager = null;
		// ReSharper restore ConvertToConstant, RedundantDefaultFieldInitializer

		public HtmlPageBuilder(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		private static string LoadStringTemplate(string resName)
		{
			var reader = new StreamReader(
				Assembly
				.GetExecutingAssembly()
				.GetRequiredResourceStream(_resourcePrefix + resName));
			using (var sr = reader)
				return sr.ReadToEnd();
		}

		private static string GetStringTemplate(string resName)
		{
			return _templatesCache[resName];
		}

		/// <summary>
		/// Получить внутренний формат ссылки.
		/// </summary>
		/// <param name="resourceType">Тип ресурса ссылки.</param>
		/// <param name="parameters">Параметр ссылки.</param>
		/// <returns></returns>
		private static string FormatUri(JanusProtocolResourceType resourceType, string parameters)
		{
			return JanusProtocolDispatcher.FormatURI(resourceType, parameters);
		}

		/// <summary>
		/// Получить внешний формат ссылки для соответствующего ресурса. 
		/// <seealso cref="SiteUrlHelper"/>
		/// </summary>
		/// <param name="resourceType">Тип ресурса.</param>
		/// <param name="parameters">Параметр ссылки.</param>
		/// <returns>Внешний формат ссылки на ресурс.</returns>
		private static string FormatExternalUri(JanusProtocolResourceType resourceType, string parameters)
		{
			return JanusProtocolDispatcher.FormatExternalURI(resourceType, parameters);
		}

		public static string GetNamedStyle(string name)
		{
			return GetStringTemplate(name);
		}

		private string GetUserDisplayName(IDataContext db, int userID)
		{
			return
				db
					.Users(u => u.ID == userID)
					.Select(
						u =>
							FormatAuthor(
								u.ID,
								u.UserClass,
								u.DisplayName(),
								false))
					.Single();
		}

		#region Вывод сообщения об исключении
		private const string _templateException = "ExceptionPage.html";

		public static string GetExceptionMessage(string uri, Exception e)
		{
			return string.Format(GetStringTemplate(_templateException),
				uri, e, ApplicationInfo.NameWithVersion);
		}
		#endregion

		#region Сообщение не найдено
		private const string _imageLinkFormat =
			"<a class='m' href='{0}' title='{1}'><img border='0' align='absmiddle' src='{2}'/></a>";

		public string GetNotFoundMessage(int mid)
		{
			const string messageFormat =
				_imageLinkFormat + "&nbsp;<a class='m' href='{0}' title='{1}'>{1}</a>&nbsp;";

			var styleImageManager = _serviceProvider.GetRequiredService<IStyleImageManager>();
			var absentMessage = string.Format(messageFormat,
				FormatUri(JanusProtocolResourceType.MessageAbsent, mid.ToString()),
				SR.MessageAbsentLoadTitle,
				styleImageManager.GetImageUri("MsgNotInDb", StyleImageType.ConstSize));

			var externalLink = string.Format(_imageLinkFormat,
				SiteUrlHelper.GetMessageUrl(mid),
				SR.MsgExtBrowserLinkTitle,
				styleImageManager.GetImageUri("ExtBrowser", StyleImageType.Small));

			return string.Format(
				GetStringTemplate(_templateMessageNotFound),
				string.Format(SR.MessageNotFound, mid),
				absentMessage + externalLink);
		}

		public string GetNotFoundMessage(string messageName)
		{
			const string messageFormat =
				"<a class='m' href='{0}' title='{1}'>{1}</a>&nbsp;" + _imageLinkFormat;

			var styleImageManager = _serviceProvider.GetRequiredService<IStyleImageManager>();
			var externalLink = string.Format(messageFormat,
				SiteUrlHelper.GetInfoUrl(messageName),
				SR.MsgExtBrowserLinkTitle,
				styleImageManager.GetImageUri("ExtBrowser", StyleImageType.Small));

			return string.Format(
				GetStringTemplate(_templateMessageNotFound),
				string.Format(SR.MessageNotFound, messageName),
				externalLink);
		}
		#endregion

		#region Вставки плавкие FormatZzz
		private const string _selectorEven = "even";
		private const string _selectorNormaldate = "normaldate";
		private const string _selectorOutdate = "outdate";
		private const string _selectorUneven = "uneven";
		private const string _templateAlt = "{0}: {1}";

		private const string _templateArticleMessageItem =
			@"<tr class='{4}'><td>{0}</td><td>{1}</td>" +
				@"<td>{2}</td><td nowrap>{3}</td></tr>";

		private const string _templateBigRateItem =
			@"<tr class='{5}'>
				<td>{0}</td>
				<td>{1}</td>
				<td nowrap style='text-align: right;'>{2}</td>
				<td nowrap>{3}</td>
				<td nowrap>{4}</td>
			</tr>";

		private const string _templateHeaderItem =
			@"<img class='himg' src='{0}' alt='{1}'>&nbsp;{2}";

		private const string _templateHeaderItemAnchor =
			@"<a href='{3}'><img class='himg' src='{0}' alt='{1}'></a>&nbsp;<a href='{3}'>{2}</a>";

		private const string _templateHeaderItemAnchorBold =
			@"<a href='{3}'><img class='himg' src='{0}' alt='{1}'></a>&nbsp;<a href='{3}'><b>{2}</b></a>";

		private const string _templateHeaderItemBold =
			@"<img class='himg' src='{0}' alt='{1}'>&nbsp;<b>{2}</b>";

		private const string _templateHeaderItemForDate =
			@"<img class='himg' src='{0}' alt='{1}'>&nbsp;<span class='{3}'>{2}</span>";

		private const string _templateMessageItem =
			@"<tr class='{3}'><td>{0}</td><td>{1}</td>" +
				@"<td nowrap>{2}</td></tr>";

		private const string _templateNamedMessageItem =
			@"<tr class='{4}'><td>{0}</td><td>{1}</td>" +
				@"<td>{2}</td><td nowrap>{3}</td></tr>";

		private const string _templateRateItemWithDate =
			@"<tr class='{3}'>
				<td nowrap style='text-align: right;'>{0}</td>
				<td nowrap>{1}</td>
				<td nowrap>{2}</td>
			</tr>";

		private const string _templateRatePart = @"{0}<img class='himg' src='{1}'>";
		private const string _templateUnsubscribed = @"<span class='frmuns'>{0}</span>";

		private string FormatAuthor(
			int userId,
			UserClass userClass,
			string showName,
			bool bold)
		{
			// From : uri, icon, alt, text
			// Аноним - это когда uid == 0 и usernick == string.Empty.
			// Именованный аноним с ГДН - это когда uid == 0, но имя (usernick) отлично от string.Empty.
			// Лучше бы дали именованным анонимам с ГДН отдельный класс...

			string template;

			if (userId == 0)
			{
				template = bold ? _templateHeaderItemBold : _templateHeaderItem;

				if (showName.Length == 0)
					showName = SR.Forum.UserNickAnonymous;
			}
			else
			{
				template = bold ? _templateHeaderItemAnchorBold : _templateHeaderItemAnchor;
			}

			// добавим класс пользователя
			template += " {4}";

			var userInfoUri = FormatUri(
				JanusProtocolResourceType.UserInfo,
				userId.ToString());

			return string.Format(template,
				JanusFormatMessage.GetUserImagePath(_serviceProvider, userClass),
				string.Format(_templateAlt, SR.TGColumnAuthor, showName),
				showName,
				userInfoUri,
				JanusFormatMessage.FormatUserClass(userClass, true));
		}

		private string FormatDate(DateTime dt)
		{
			var isOutdate =
				DateTime.Now.AddDays(-Config.Instance.ForumDisplayConfig.DaysToOutdate) > dt &&
					Config.Instance.ForumDisplayConfig.DaysToOutdate != 0;

			var text = dt.ToString(Config.Instance.ForumDisplayConfig.DateFormat);

			return
				string.Format(
					_templateHeaderItemForDate,
					JanusFormatMessage.GetWeekDayImagePath(
						_serviceProvider,
						Convert.ToInt32(dt.DayOfWeek),
						isOutdate),
					string.Format(_templateAlt, SR.TGColumnDate, text),
					text,
					isOutdate ? _selectorOutdate : _selectorNormaldate);
		}

		private string FormatRateSummary(
			int mid,
			int rating,
			int smiles,
			int agrees,
			int disagrees)
		{
			// Rate
			// rate calc for headers and rate frame
			var text = JanusFormatMessage.FormatRates(
				rating, smiles, agrees, disagrees);

			if (text.Length == 0)
				return string.Empty;

			var styleImageManager = _serviceProvider.GetRequiredService<IStyleImageManager>();
			return string.Format(_templateHeaderItemAnchorBold,
				styleImageManager.GetImageUri("RateGroup", StyleImageType.ConstSize),
				string.Format(_templateAlt, SR.TGColumnRate, text),
				text,
				FormatUri(JanusProtocolResourceType.MessageRate, mid.ToString()));
		}

		private string FormatSubject(
			int messageId,
			string subject,
			bool isRead,
			bool isMarked,
			int? articleId,
			bool anchor)
		{
			var text = HttpUtility.HtmlEncode(subject);
			return
				string.Format(
					anchor
						? _templateHeaderItemAnchor
						: _templateHeaderItem,
					JanusFormatMessage.GetMessageImagePath(
						_serviceProvider,
						isRead,
						isMarked,
						articleId > 0),
					string.Format(_templateAlt, SR.TGColumnSubject, text),
					text,
					FormatUri(JanusProtocolResourceType.Message, messageId.ToString()));
		}

		private string FormatName(string name)
		{
			if (name.Length == 0)
				return string.Empty;

			var styleImageManager = _serviceProvider.GetRequiredService<IStyleImageManager>();
			return string.Format(_templateHeaderItem,
				styleImageManager.GetImageUri("NameGroup", StyleImageType.ConstSize),
				string.Format(_templateAlt, SR.UserInfoName, name), name);
		}

		private static string FormatForumName(string name, string description, bool isSubscribed)
		{
			var text = Config.Instance.ForumDisplayConfig.ShowFullForumNames
				? description
				: name;

			if (!isSubscribed)
				text = string.Format(_templateUnsubscribed, text);

			return text;
		}

		private string FormatForum(
			string forumName,
			string forumDescription)
		{
			return
				string.Format(
					_templateHeaderItem,
					JanusFormatMessage.GetForumImagePath(_serviceProvider, false),
					string.Empty,
					FormatForumName(forumName, forumDescription, false));
		}

		private string FormatRate(
			MessageRates rateType,
			int multiplier,
			bool forumInTop)
		{
			// Считается, что оценка обязательно есть, и она корректна
			var rt = string.Empty;

			if ((int)rateType > 0)
			{
				rt = (multiplier * (int)rateType).ToString();

				if (forumInTop) // учёт специфичных форумов
					rt = $@"<b>{rt}</b>";

				rt += @"&nbsp;x&nbsp;";
			}

			//	0 - rate text, 1 - path to rate icon
			return string.Format(_templateRatePart, rt,
				JanusFormatMessage.GetRateImagePath(_serviceProvider, rateType));
		}

		private string FormatRateItemWithDate(
			MessageRates rateType,
			int multiplier,
			bool forumInTop,
			int userId,
			UserClass userClass,
			string userDisplayName,
			DateTime rateDate,
			bool even)
		{
			return string.Format(_templateRateItemWithDate,
				FormatRate(rateType, multiplier, forumInTop),
				FormatAuthor(userId, userClass, userDisplayName, false),
				FormatDate(rateDate),
				even ? _selectorEven : _selectorUneven);
		}

		private string FormatBigRateItem(
			MessageRates rateType,
			int rateMultiplier,
			DateTime rateDate,
			string forumName,
			string forumDesc,
			bool forumInTop,
			int msgID,
			string msgSubj,
			bool msgIsRead,
			bool msgIsMarked,
			int? msgArticleID,
			int userID,
			UserClass userClass,
			string userDisplayName,
			bool even)
		{
			return string.Format(_templateBigRateItem,
				FormatForum(forumName, forumDesc),
				FormatSubject(
					msgID,
					msgSubj,
					msgIsRead,
					msgIsMarked,
					msgArticleID,
					true),
				FormatRate(rateType, rateMultiplier, forumInTop),
				FormatAuthor(
					userID,
					userClass,
					userDisplayName,
					false),
				FormatDate(rateDate),
				even ? _selectorEven : _selectorUneven);
		}

		private string FormatMessageItem(
			int msgId,
			string subject,
			DateTime? date,
			int? articleId,
			bool isRead,
			bool isMarked,
			string forumName,
			string forumDesc,
			bool even)
		{
			return
				string.Format(
					_templateMessageItem,
					FormatForum(forumName, forumDesc),
					FormatSubject(msgId, subject, isRead, isMarked, articleId, true),
					FormatDate(date.GetValueOrDefault()),
					even ? _selectorEven : _selectorUneven);
		}

		private string FormatNamedMessageItem(
			int msgId,
			string subject,
			string name,
			DateTime? date,
			int? articleId,
			bool isRead,
			bool isMarked,
			string forumName,
			string forumDesc,
			bool even)
		{
			return
				string.Format(
					_templateNamedMessageItem,
					FormatForum(forumName, forumDesc),
					FormatSubject(
						msgId,
						subject,
						isRead,
						isMarked,
						articleId,
						true),
					FormatName(name),
					FormatDate(date.GetValueOrDefault()),
					even ? _selectorEven : _selectorUneven);
		}

		private string FormatArticleMessageItem(
			string forumName,
			string forumDesc,
			int msgId,
			DateTime msgDate,
			string subj,
			bool isRead,
			bool isMarked,
			int? articleId,
			int userId,
			UserClass userClass,
			string userDisplayName,
			bool even)
		{
			return string.Format(_templateArticleMessageItem,
				FormatForum(forumName, forumDesc),
				FormatSubject(msgId, subj, isRead, isMarked, articleId, true),
				FormatAuthor(userId, userClass, userDisplayName, false),
				FormatDate(msgDate),
				even ? _selectorEven : _selectorUneven);
		}
		#endregion

		#region Вывод сообщения
		// Идея: 2 строки
		// 1. Автор, Дата, [Оценка] — каждое по 33% ширины
		// 2. Тема и [Имя] (всё же имя к теме ближе) — 66% и 33% ширины

		private const string _templateMessageNotFound = "MessageNotFound.html";
		#endregion

		#region Оценки сообщения
		private const string _templateMessageRatelist = "MessageRate.html";

		public string GetMessageRate(int mid)
		{
			using (var db = _dbManager.CreateDBContext())
			{
				var rates =
					db
						.Rates(r => r.MessageID == mid)
						.OrderByDescending(r => r.Date)
						.Select(
							(r, i) =>
								FormatRateItemWithDate(
									r.RateType,
									r.Multiplier,
									r.Message.ServerForum.InTop,
									r.UserID,
									r.User.UserClass,
									r.User.DisplayName(),
									r.Date,
									i % 2 == 0))
						.JoinStrings(Environment.NewLine);

				var res =
					db
						.Message(
							mid,
							m =>
								string.Format(
									GetStringTemplate(_templateMessageRatelist),
									FormatAuthor(m.UserID, m.UserClass, m.UserNick, false),
									FormatDate(m.Date),
									FormatSubject(
										m.ID,
										m.Subject,
										m.IsRead,
										m.IsMarked,
										m.ArticleId,
										true),
									FormatRateSummary(
										m.ID,
										m.Rating(),
										m.SmileCount(),
										m.AgreeCount(),
										m.DisagreeCount()),
									rates));
				return res;
			}
		}
		#endregion

		#region Работа с отсутствующим сообщением (для его хитрой закачки)
		private const string _templateAbsentMessage = "AbsentMessage.html";

		public static string GetAbsentMessageText(
			IServiceProvider provider,
			int mid)
		{
			provider
				.GetRequiredService<IOutboxManager>()
				.AddTopicForDownloadWithConfirm(mid);

			return GetStringTemplate(_templateAbsentMessage);
		}
		#endregion

		#region Список пользователей (команда) *
		private const string _evenColor = "#E4FFF4";
		private const string _oddColor = "#F4FFF4";

		private const string _tlTableRow =
			@"<tr style='background-color: {0}; font-size: 8pt;'>
				<td valign=center>
					<font size='3' color='#4580A0' face='arial'>
						&nbsp;<b>{1}</b>
					</font>
					&nbsp;[{2}]
				</td>
				<td valign=center width=50%>
					{3}
					<div style='text-align:right;'>
						<hr color='#4580A0' width='10%' align=right>
						{4}
					</div>
				</td>
			</tr>";

		private const string _tlUsersList =
			@"<img src='{0}' align='top'/>" +
				@"&nbsp;<a href='janus://user-info/{1}'>{2}</a>&nbsp;{3}&nbsp;<br/>";

		public static string GetTeamList(IServiceProvider provider)
		{
			var usersCount = 0; // счетчик пользователей таблицы по форумам
			var forumDescript = string.Empty;
			var forumName = string.Empty;

			var sb = new StringBuilder(); // конечный результат таблицы по форумам
			var sbSubtotal = new StringBuilder(); // Строка таблицы  по форумам

			var colCount = 0;
			using (var db = provider.CreateDBContext())
			{
				var users =
					db
						.Messages(
							m =>
								m.UserClass == UserClass.Admin
									|| m.UserClass == UserClass.Moderator
									|| m.UserClass == UserClass.Team)
						.OrderBy(u => u.ID)
						.Select(
							m =>
								new
								{
									ForumName = m.ServerForum.Name,
									ForumDesc = m.ServerForum.Descript,
									m.UserClass,
									m.UserID,
									UserName = m.User.DisplayName()
								});
				foreach (var user in users)
				{
					if (forumName != user.ForumName)
					{
						if (!string.IsNullOrEmpty(forumName))
						{
							colCount++;
							sb.AppendFormat(_tlTableRow,
								(colCount % 2 == 0 ? _evenColor : _oddColor),
								forumDescript,
								forumName,
								sbSubtotal,
								usersCount);
							sbSubtotal.Length = 0;
							usersCount = 0;
						}

						forumDescript = user.ForumDesc;
						forumName = user.ForumName;
					}

					sbSubtotal.AppendFormat(
						_tlUsersList,
						JanusFormatMessage.GetUserImagePath(provider, user.UserClass),
						user.UserID,
						user.UserName,
						JanusFormatMessage.FormatUserClass(user.UserClass, true));
					usersCount++;
				}
			}

			return String.Format(GetStringTemplate("StandartPage.html"),
				SR.ForumTeamList,
				"80%",
				SR.ForumTeam,
				SR.ForumTeamList,
				sb,
				Path.Combine(ApplicationManager.HomeDirectoryPath, "styles/"));
		}
		#endregion

		#region Карточка пользователя
		private const string _templateMessageInfo = @"{0} {1}"; // дата и тема
		private const string _templateOrigin = @"<div class='o'>{0}</div>";
		private const string _templateUserInfo = "UserInfo.html";
		private const string _templateUserInfoNotFound = "UserInfoNotFound.html";
		//private const string _templatePrefixImage      = "<img>";

		public string GetUserInfoText(int uid)
		{
			using (var db = _dbManager.CreateDBContext())
			{
				var user =
					db
						.Users(u => u.ID == uid)
						.Select(
							u =>
								new
								{
									u.ID,
									u.UserClass,
									u.Origin,
									u.Name,
									u.Nick,
									u.RealName,
									u.WhereFrom,
									u.Spec,
									u.HomePage
								})
						.FirstOrDefault();
				var parameter = Convert.ToString(uid);

				if (user == null)
					return
						string.Format(
							GetStringTemplate(_templateUserInfoNotFound),
							FormatExternalUri(JanusProtocolResourceType.UserInfo, parameter),
							FormatUri(JanusProtocolResourceType.UserRating, parameter),
							FormatUri(JanusProtocolResourceType.UserOutrating, parameter),
							FormatUri(JanusProtocolResourceType.UserMessages, parameter));

				var origin = _serviceProvider.GetFormatter().Format(user.Origin, true);
				if (origin.Length != 0)
					origin = string.Format(_templateOrigin, origin);

				var firstInfo = string.Empty;
				var lastInfo = string.Empty;
				var msgs =
					db
						.Messages(m => m.UserID == uid)
						.Select(
							m =>
								new
								{
									m.ID,
									m.Date,
									m.Subject,
									m.IsRead,
									m.IsMarked,
									m.ArticleId
								});
				var msgFirst =
					msgs
						.OrderBy(m => m.Date)
						.FirstOrDefault();
				if (msgFirst != null)
				{
					firstInfo =
						string.Format(
							_templateMessageInfo,
							FormatDate(msgFirst.Date),
							FormatSubject(
								msgFirst.ID,
								msgFirst.Subject,
								msgFirst.IsRead,
								msgFirst.IsMarked,
								msgFirst.ArticleId,
								true));
					var msgLast =
					msgs
						.OrderByDescending(m => m.Date)
						.First();
					lastInfo = string.Format(
						_templateMessageInfo,
						FormatDate(msgLast.Date),
						FormatSubject(
							msgLast.ID,
							msgLast.Subject,
							msgLast.IsRead,
							msgLast.IsMarked,
							msgLast.ArticleId,
							true));
				}

				return
					string.Format(
						GetStringTemplate(_templateUserInfo),
						FormatAuthor(
							user.ID,
							user.UserClass,
							JanusFormatMessage.GetUserDisplayName(user.Nick, user.RealName, user.Name),
							true),
						user.Name,
						user.Nick,
						user.RealName,
						user.WhereFrom,
						user.Spec,
						_serviceProvider.GetFormatter().Format(user.HomePage, true),
						origin,
						FormatExternalUri(JanusProtocolResourceType.UserInfo, parameter),
						FormatUri(JanusProtocolResourceType.UserRating, parameter),
						FormatUri(JanusProtocolResourceType.UserOutrating, parameter),
						FormatUri(JanusProtocolResourceType.UserMessages, parameter),
						FormatUri(JanusProtocolResourceType.UserMessagesStat, parameter),
						firstInfo,
						lastInfo,
						JanusFormatMessage.GetResourceImagePath(_serviceProvider, JanusProtocolResourceType.UserInfo),
						JanusFormatMessage.GetResourceImagePath(_serviceProvider, JanusProtocolResourceType.UserRating),
						JanusFormatMessage.GetResourceImagePath(_serviceProvider, JanusProtocolResourceType.UserOutrating),
						JanusFormatMessage.GetResourceImagePath(_serviceProvider, JanusProtocolResourceType.UserMessages),
						JanusFormatMessage.GetResourceImagePath(_serviceProvider, JanusProtocolResourceType.UserMessagesStat));
			}
		}
		#endregion

		#region Сборка до кучи с учетом настроек *
		//private static string _html = 
		//    "<html><head><title>{0}</title></head><body>{1}</body></html>";

		//public string GetAllUserInfo(int uid)
		//{
		//    StringBuilder sb = new StringBuilder();
		//    // Основное инфо
		//    sb.Append(GetUserInfoText(uid));

		//    return String.Format(_html,
		//        SR.UserInfo,
		//        sb.ToString());
		//}
		#endregion

		#region Входящие оценки пользователя
		private const string _rateStatistics =
			@"всего оценок <b>{0}</b>, баллов <b>{1}</b> из <b>{2}</b>, " +
				@"согласен <b>{3}</b>, не согласен <b>{4}</b>, улыбок <b>{5}</b>";

		private const string _templateUserRating = "UserRating.html";
		private const string _templateUserRatingNotFound = "UserRatingNotFound.html";

		public string GetUserRatingText(int uid)
		{
			var total = 0;
			var rated = 0;
			var smile = 0;
			var agree = 0;
			var disagree = 0;
			var colCount = 0;
			var sb = new StringBuilder();
			string userName;
			using (var db = _dbManager.CreateDBContext())
			{
				var q = db.Rates(r => r.Message.UserID == uid);
				if (Config.Instance.ForumDisplayConfig.ShowUserRateLastMonthOnly)
					q = q.Where(r => r.Date > DateTime.Now.AddMonths(-1));
				var rates =
					q
						.OrderByDescending(r => r.Date)
						.Select(
							r =>
								new
								{
									r.RateType,
									r.Multiplier,
									r.Date,
									r.Message.ServerForum.Name,
									r.Message.ServerForum.Descript,
									r.Message.ServerForum.InTop,
									r.MessageID,
									r.Message.Subject,
									r.Message.IsRead,
									r.Message.IsMarked,
									r.Message.ArticleId,
									r.UserID,
									r.User.UserClass,
									UserNick = r.User.DisplayName()
								})
						.ToList();

				if (rates.Count == 0)
					return string.Format(GetStringTemplate(_templateUserRatingNotFound),
						FormatExternalUri(JanusProtocolResourceType.UserRating, uid.ToString()));

				foreach (var r in rates)
				{
					switch (r.RateType)
					{
						case MessageRates.Agree:
							agree++;
							break;
						case MessageRates.DisAgree:
							disagree++;
							break;
						case MessageRates.Smile:
							smile++;
							break;
						case MessageRates.Rate1:
						case MessageRates.Rate2:
						case MessageRates.Rate3:
							var rateBy = r.Multiplier * (int)r.RateType;

							if (r.InTop)
								rated += rateBy;

							total += rateBy;

							break;
					}

					colCount++;
					sb.Append(
						FormatBigRateItem(
							r.RateType,
							r.Multiplier,
							r.Date,
							r.Name,
							r.Descript,
							r.InTop,
							r.MessageID,
							r.Subject,
							r.IsRead,
							r.IsMarked,
							r.ArticleId,
							r.UserID,
							r.UserClass,
							r.UserNick,
							colCount % 2 == 0));
				}

				userName = GetUserDisplayName(db, uid);
			}

			return
				string
					.Format(
						GetStringTemplate(_templateUserRating),
						userName,
						Config.Instance.ForumDisplayConfig.ShowUserRateLastMonthOnly
							? SR.UserInfoRateListLastMonth
							: SR.UserInfoRateListComplete,
						string.Format(
							_rateStatistics,
							colCount,
							rated,
							total,
							agree,
							disagree,
							smile),
						sb,
						FormatExternalUri(JanusProtocolResourceType.UserRating, uid.ToString()));
		}
		#endregion

		#region Исходящие оценки пользователя
		private const string _templateUserOutrating = "UserOutrating.html";
		private const string _templateUserOutratingNotFound = "UserOutratingNotFound.html";

		public string GetUserOutratingText(int uid)
		{
			var total = 0;
			var rated = 0;
			var smile = 0;
			var agree = 0;
			var disagree = 0;
			var sb = new StringBuilder();
			var colCount = 0;
			string userName;

			using (var db = _dbManager.CreateDBContext())
			{
				var q = db.Rates(r => r.UserID == uid);
				if (Config.Instance.ForumDisplayConfig.ShowUserRateLastMonthOnly)
					q = q.Where(r => r.Date > DateTime.Now.AddMonths(-1));
				var rates =
					q
						.OrderByDescending(r => r.Date)
						.Select(
							r =>
								new
								{
									r.RateType,
									r.Multiplier,
									r.Date,
									r.Message.ServerForum.Name,
									r.Message.ServerForum.Descript,
									r.Message.ServerForum.InTop,
									r.MessageID,
									r.Message.Subject,
									r.Message.IsRead,
									r.Message.IsMarked,
									r.Message.ArticleId,
									r.Message.UserID,
									r.Message.UserClass,
									r.Message.UserNick
								})
						.ToList();

				if (rates.Count == 0)
					return string.Format(
						GetStringTemplate(_templateUserOutratingNotFound),
						FormatExternalUri(JanusProtocolResourceType.UserOutrating, uid.ToString()));

				foreach (var r in rates)
				{
					switch (r.RateType)
					{
						case MessageRates.Agree:
							agree++;
							break;
						case MessageRates.DisAgree:
							disagree++;
							break;
						case MessageRates.Smile:
							smile++;
							break;
						case MessageRates.Rate1:
						case MessageRates.Rate2:
						case MessageRates.Rate3:
							var rateBy = r.Multiplier * (int)r.RateType;

							if (r.InTop)
								rated += rateBy;

							total += rateBy;

							break;
					}

					colCount++;
					sb.Append(
						FormatBigRateItem(
							r.RateType,
							r.Multiplier,
							r.Date,
							r.Name,
							r.Descript,
							r.InTop,
							r.MessageID,
							r.Subject,
							r.IsRead,
							r.IsMarked,
							r.ArticleId,
							r.UserID,
							r.UserClass,
							r.UserNick,
							colCount % 2 == 0));
				}

				userName = GetUserDisplayName(db, uid);
			}

			return string.Format(GetStringTemplate(_templateUserOutrating),
				userName,
				Config.Instance.ForumDisplayConfig.ShowUserRateLastMonthOnly
					? SR.UserInfoRateListLastMonth
					: SR.UserInfoRateListComplete,
				string.Format(
					_rateStatistics,
					colCount,
					rated,
					total,
					agree,
					disagree,
					smile),
				sb,
				FormatExternalUri(JanusProtocolResourceType.UserOutrating, uid.ToString()));
		}
		#endregion

		#region Статистика сообщений по форумам
		private const string _templateMessagesStat = "MessagesStats.html";
		private const string _templateMessagesStatNotFound = "MessagesStatNotFound.html";

		private const string _templateStatItem =
			@"<tr class='{4}'>
				<td class='ralign'>{0}&nbsp;</td>
				<td>{1}</td>
				<td class='ralign'>{2}&nbsp;</td>
				<td class='ralign'>{3}&nbsp;</td>
			</tr>";

		public string GetUserMessagesStatText(int uid)
		{
			using (var db = _dbManager.CreateDBContext())
			{
				var forums =
					db
						.Messages(m => m.UserID == uid)
						.GroupBy(
							m =>
								new
								{
									m.ForumID,
									m.ServerForum.Name,
									m.ServerForum.Descript,
									m.ServerForum.InTop
								})
						.Select(grp => new {Forum = grp.Key, Count = grp.Count()})
						.ToList();
				forums.Sort((f1, f2) => f2.Count.CompareTo(f1.Count));

				if (forums.Count == 0)
					return string.Format(
						GetStringTemplate(_templateMessagesStatNotFound), string.Empty);

				var totalCount = forums.Sum(f => f.Count);
				var sb = new StringBuilder();
				var colCount = 0;
				foreach (var f in forums)
				{
					colCount++;
					sb.AppendFormat(
						_templateStatItem,
						colCount,
						FormatForum(f.Forum.Name, f.Forum.Descript),
						f.Count,
						((double)100 * f.Count / totalCount).ToString("#0.00"),
						colCount % 2 == 0 ? _selectorEven : _selectorUneven);
				}

				var user =
					db.User(uid, u => new {u.ID, u.UserClass, DisplayName = u.DisplayName()});
				var name =
					user != null
						? FormatAuthor(
							user.ID,
							user.UserClass,
							user.DisplayName,
							true)
						: SR.UserInfoMissed;
				return
					string.Format(
						GetStringTemplate(_templateMessagesStat),
						name,
						totalCount,
						sb);
			}
		}
		#endregion

		#region Список сообщений пользователя
		private const string _templateUserMessages = "UserMessages.html";
		private const string _templateUserMessagesNotFound = "UserMessagesNotFound.html";

		public string GetUserMessagesText(int uid)
		{
			var sb = new StringBuilder();
			int colCount;
			string name;
			using (var db = _serviceProvider.CreateDBContext())
			{
				var query =
					from m in db.Messages()
					where m.UserID == uid
					orderby m.Date descending
					select
						new
						{
							m.ID,
							m.Subject,
							m.Name,
							m.Date,
							m.ArticleId,
							m.IsRead,
							m.IsMarked,
							m.ForumID,
							ForumName = m.ServerForum.Name,
							ForumDesc = m.ServerForum.Descript
						};
				if (Config.Instance.ForumDisplayConfig.ShowUserRateLastMonthOnly)
					query = query.Where(m => m.Date >= DateTime.Now.AddMonths(-1));

				var msgs = query.ToArray();
				if (msgs.Length == 0)
					return
						string.Format(
							GetStringTemplate(_templateUserMessagesNotFound),
							FormatExternalUri(
								JanusProtocolResourceType.UserMessages,
								uid.ToString()));
				colCount = msgs.Length;
				foreach (var msg in msgs)
					sb.Append(
						FormatMessageItem(
							msg.ID,
							msg.Subject,
							msg.Date,
							msg.ArticleId,
							msg.IsRead,
							msg.IsMarked,
							msg.ForumName,
							msg.ForumDesc,
							colCount%2 == 0));
				var user = db.User(uid, u => new {u.ID, u.UserClass, DisplayName = u.DisplayName()});
				name =
					user != null
						? FormatAuthor(
							user.ID,
							user.UserClass,
							user.DisplayName,
							true)
						: SR.UserInfoMissed;
			}

			return string.Format(GetStringTemplate(_templateUserMessages),
				name,
				colCount,
				Config.Instance.ForumDisplayConfig.ShowUserRateLastMonthOnly
					? SR.UserInfoMessagesLastMonthOnly
					: string.Empty,
				sb,
				FormatExternalUri(JanusProtocolResourceType.UserMessages, uid.ToString()));
		}
		#endregion

		#region Статьи
		private const string _templateArticleList = "ArticleList.html";
		private const string _templateArticleListNotFound = "ArticleListNotFound.html";

		public string GetArticleListText(int forumId)
		{
			var sb = new StringBuilder();
			int colCount;
			using (var db = _dbManager.CreateDBContext())
			{
				var q =
					db
						.Messages()
						.OrderByDescending(m => m.ID)
						.Where(m => m.ArticleId != null && m.ArticleId > 0);
				if (forumId != 0)
					q = q.Where(m => m.ForumID == forumId);
				var lines =
					q
						.Select(
							(m, i) =>
								FormatArticleMessageItem(
									m.ServerForum.Name,
									m.ServerForum.Descript,
									m.ID,
									m.Date,
									m.Subject,
									m.IsRead,
									m.IsMarked,
									m.ArticleId,
									m.UserID,
									m.UserClass,
									m.UserNick,
									i % 2 == 0))
						.ToArray();
				colCount = lines.Length;
				if (colCount == 0)
					return string.Format(GetStringTemplate(_templateArticleListNotFound), string.Empty);
				sb.Append(lines.JoinStrings());
			}

			return
				string.Format(
					GetStringTemplate(_templateArticleList),
					colCount,
					sb,
					FormatExternalUri(JanusProtocolResourceType.ArticleList, forumId.ToString()));
		}
		#endregion

		#region ФАКУ
		private const string _templateFaqList = "FaqList.html";
		private const string _templateFaqListNotFound = "FaqListNotFound.html";

		public string GetFaqListText(int gid)
		{
			var sb = new StringBuilder();
			var colCount = 0;
			using (var mgr = _serviceProvider.CreateDBContext())
			{
				var query =
					from m in mgr.Messages()
					where !string.IsNullOrEmpty(m.Name)
					orderby m.ID descending
					select
						new
						{
							m.ID,
							m.Subject,
							m.Name,
							m.Date,
							m.ArticleId,
							m.IsRead,
							m.IsMarked,
							m.ForumID,
							ForumName = m.ServerForum.Name,
							ForumDesc = m.ServerForum.Descript
						};
				if (gid != 0)
					query = query.Where(msg => msg.ForumID == gid);

				var msgs = query.ToArray();

				if (msgs.Length == 0)
					return string.Format(GetStringTemplate(_templateFaqListNotFound), string.Empty);

				foreach (var msg in msgs)
				{
					colCount++;
					sb.Append(
						FormatNamedMessageItem(
							msg.ID,
							msg.Subject,
							msg.Name,
							msg.Date,
							msg.ArticleId,
							msg.IsRead,
							msg.IsMarked,
							msg.ForumName,
							msg.ForumDesc,
							colCount%2 == 0));
				}
			}

			return
				string.Format(
					GetStringTemplate(_templateFaqList),
					colCount,
					sb,
					FormatExternalUri(JanusProtocolResourceType.FaqList, gid.ToString()));
		}
		#endregion
	}
}