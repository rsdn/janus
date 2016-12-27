using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;

using CodeJam;
using CodeJam.Extensibility;
using CodeJam.Extensibility.Model;
using CodeJam.Services;

using Rsdn.Framework.Formatting;

namespace Rsdn.Janus
{
	// TODO: Слишком дофига ненужного функционала. Нужно вынести в статические методы в хелпере.
	/// <summary>
	/// Форматирование сообщения и расцветка кода для Rsdn@Home.
	/// </summary>
	[Service(typeof(IJanusFormatter))]
	internal class JanusFormatMessage : TextFormatter, IJanusFormatter
	{
		private readonly IServiceProvider _provider;

		#region Public Methods
		// ReSharper disable ConvertToConstant, RedundantDefaultFieldInitializer
		[ExpectService]
		private IJanusDatabaseManager _dbManager = null;
		// ReSharper restore ConvertToConstant, RedundantDefaultFieldInitializer

		/// <summary>
		/// Форматирует оценки.
		/// </summary>
		/// <param name="rate">Сумма оценок.</param>
		/// <param name="smile">Количество улыбок.</param>
		/// <param name="agree">Количество согласных.</param>
		/// <param name="disagree">Количество не согласных.</param>
		/// <returns>Форматированное представление оценок.</returns>
		public static string FormatRates(int rate, int smile, int agree, int disagree)
		{
			var list = new List<string>(4);

			if (smile != 0)
				list.Add(smile + ":)");

			if (agree != 0)
				list.Add(agree + "+");

			if (disagree != 0)
				list.Add(disagree + "-");

			if (rate != 0)
				list.Add(rate.ToString());

			return String.Join("/", list.ToArray());
		}

		/// <summary>
		/// Возвращает путь к иконке, ассоциированной с классом пользователя.
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="userClass">Класс пользователя.</param>
		/// <returns>Путь к иконке, ассоциированной с классом пользователя.</returns>
		public static string GetUserImagePath(
			IServiceProvider provider,
			UserClass userClass)
		{
			var name = userClass != UserClass.User ? userClass.ToString() : string.Empty;
			return GetImageUri(provider, @"MessageTree\User" + name, StyleImageType.ConstSize);
		}

		/// <summary>
		/// Возвращает путь к иконке сообщения.
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="isRead">Сообщение прочитано</param>
		/// <param name="isMarked">Помечено ли сообщение флагом (очками).</param>
		/// <param name="isArticle">Является ли сообщение статьей.</param>
		/// <param name="violationPenaltyType">Тип бана</param>
		/// <param name="violationReason">Основание для бана</param>
		/// <returns>Путь к иконке сообщения.</returns>
		public static string GetMessageImagePath(
			IServiceProvider provider,
			bool isRead,
			bool isMarked,
			bool isArticle,
			PenaltyType violationPenaltyType = PenaltyType.Ban,
			string violationReason = null)
		{
			string path = null;

			if (isArticle)
				path =
					isRead
						? isMarked
							? "MsgArticleMarked"
							: "MsgArticle"
						: isMarked
							? "MsgArticleUnread2Marked"
							: "MsgArticleUnread";
			else
			{
				if (!string.IsNullOrEmpty(violationReason))
					switch (violationPenaltyType)
					{
						case PenaltyType.Ban:
							path = "PenaltyBan";
							break;

						case PenaltyType.Close:
							path = "PenaltyClose";
							break;

						case PenaltyType.Warning:
							path = "PenaltyWarning";
							break;
					}

				if (path == null)
					path =
						isRead
							? isMarked
								? "MsgMarked"
								: "Msg"
							: isMarked
								? "MsgUnread2Marked"
								: "MsgUnread";
			}

			return GetImageUri(provider, @"MessageTree\" + path, StyleImageType.ConstSize);
		}

		public static string GetWeekDayImagePath(
			IServiceProvider provider,
			int weekDay,
			bool outdated)
		{
			return
				provider
					.GetRequiredService<IStyleImageManager>()
					.GetImageUri(
						$@"MessageTree\WD{(outdated ? "OUT" : string.Empty)}{weekDay}",
						StyleImageType.ConstSize);
		}

		/// <summary>
		/// Возвращает путь к иконке с оценкой.
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="rate">Оценка.</param>
		/// <returns>Путь к иконке с оценкой.</returns>
		public static string GetRateImagePath(
			IServiceProvider provider,
			MessageRates rate)
		{
			switch (rate)
			{
				case MessageRates.Rate1:
				case MessageRates.Rate2:
				case MessageRates.Rate3:
					return GetImageUri(provider, @"MessageViewer\" + rate, StyleImageType.Small);
			}

			return GetImageUri(provider, @"MessageViewer\Rate" + rate, StyleImageType.Small) ?? string.Empty;
		}

		/// <summary>
		/// Возвращает путь к иконке форума.
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="isUnread">В форуме есть не прочитанные сообщения.</param>
		public static string GetForumImagePath(IServiceProvider provider,bool isUnread)
		{
			var path =
				isUnread
					? @"NavTree\ForumUnread"
					: @"NavTree\Forum";

			return GetImageUri(provider, path, StyleImageType.ConstSize);
		}

		/// <summary>
		/// Возвращает первое не пустое значение, 
		/// иначе - Аноним
		/// </summary>
		/// <param name="nick">Псевдоним пользователя.</param>
		/// <param name="realName">Реальное имя пользователя.</param>
		/// <param name="userName">Имя пользователя.</param>
		/// <returns>Первое не пустое "имя" пользователя, иначе - Аноним.</returns>
		public static string GetUserDisplayName(string nick, string realName, string userName)
		{
			if (!string.IsNullOrEmpty(nick))     return nick;
			if (!string.IsNullOrEmpty(realName)) return realName;
			if (!string.IsNullOrEmpty(userName)) return userName;
			
			return SR.Forum.UserNickAnonymous;
		}

		/// <summary>
		/// Возвращает путь к иконке, ассоциированной с ресурсом.
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="resourceType">Тип ресурса.</param>
		/// <returns>Путь к иконке, которая ассоциирована с ресурсом.</returns>
		public static string GetResourceImagePath(
			IServiceProvider provider,
			JanusProtocolResourceType resourceType)
		{
			string name;

			switch (resourceType)
			{
				case JanusProtocolResourceType.FaqList         : name = "faqall";      break;
				case JanusProtocolResourceType.ArticleList     : name = "articleall";  break;
				case JanusProtocolResourceType.UserInfo        : name = "ExtBrowser";  break;
				case JanusProtocolResourceType.UserRating      : name = "userratein";  break;
				case JanusProtocolResourceType.UserOutrating   : name = "userrateout"; break;
				case JanusProtocolResourceType.UserMessages    : name = "ShowAllMsgs"; break;
				case JanusProtocolResourceType.UserMessagesStat: name = "sh_all_msgs"; break;
				case JanusProtocolResourceType.TeamList        : name = "admin";       break;
				case JanusProtocolResourceType.MessageRate: 
					return GetImageUri(provider, "RateGroup", StyleImageType.ConstSize);
				default:
					return GetImageUri(provider, @"ForumImages\webref", StyleImageType.ConstSize);
			}

			return GetImageUri(provider, name, StyleImageType.Small);
		}

		/// <summary>
		/// Следующие пара методов выдернуты из исходников IT (с заметными доработками правда), 
		/// так что все возможные вопросы к нему
		/// Перенес из MessageUtilities
		/// </summary>
		/// <param name="osubj"></param>
		/// <returns></returns>
		public static string ReSubj(string osubj)
		{
			string res;
			//Чистый subj без Re-префиксов
			var subj = Regex.Replace(osubj, @"^Re(\[[0-9]+\]){0,1}: ", "");

			if (osubj.Length >= 4 && osubj.Substring(0, 4) == "Re: ")
			{
				res = "Re[2]: " + subj;
			}
			else if (osubj.Length >= 3 && osubj.Substring(0, 3) == "Re[")
			{
				var si = osubj.IndexOf("[");
				try
				{
					var nn = Int32.Parse(
						osubj.Substring(si + 1, osubj.IndexOf("]") - si - 1));
					nn++;
					res = "Re[" + nn + "]: " + subj;
				}
				catch (Exception)
				{
					//На случай исключения принимаем Re равным 0
					res = "Re: " + subj;
				}
			}
			else
			{
				res = "Re: " + subj;
			}

			return res;
		}

		/// <summary>
		/// Преобразовывает тип пользователя в строку.
		/// </summary>
		/// <param name="userClass">Тип пользователя. <see cref="UserClass"/></param>
		/// <param name="isHtml">Использовать подсветку.</param>
		/// <returns>Строковое представление ипа пользователя.</returns>
		public static string FormatUserClass(UserClass userClass, bool isHtml)
		{
			var result = string.Empty;

			switch (userClass)
			{
				case UserClass.Admin:
					result = "admin";
					if (isHtml)
						result = "<font color='darkred'>" + result + "</font>";
					break;

				case UserClass.Moderator:
					result = "moderator";
					if (isHtml)
						result = "<font color='red'>" + result + "</font>";
					break;

				case UserClass.Team:
					result = "rsdn";
					if (isHtml)
						result = "<font color='blue'>" + result + "</font>";
					break;

				case UserClass.Expert:
					result = "expert";
					if (isHtml)
						result = "<font color='green'>" + result + "</font>";
					break;
			}

			return result.Length > 0 ? "[" + result + "]" : result;
		}

		public static string GetDateString(DateTime date)
		{
			return date.ToString(Config.Instance.ForumDisplayConfig.DateFormat);
		}

		public static string GetModeratorialActionName(
			int forumId,
			int msgForumId,
			string forumName,
			string forumDescription)
		{
			switch (forumId)
			{
				case 0:
					return SR.Forum.Moderatorial.Delete;
				case -2:
					return SR.Forum.Moderatorial.ExtractBranch;
				case -7:
					return SR.Forum.Moderatorial.CloseTopic;
				case -8:
					return SR.Forum.Moderatorial.OpenTopic;
				default:
					return
						forumId != msgForumId
							? SR.Forum.Moderatorial.MoveToForum.FormatWith(
								Config.Instance.ForumDisplayConfig.ShowFullForumNames
									? forumDescription
									: forumName)
							: SR.Forum.Moderatorial.LeaveIntact;
			}
		}
		#endregion

		#region Constructors
				
		private readonly IDictionary<string, ProcessUrlItself> _hostFormatting =
			new Dictionary<string, ProcessUrlItself>(StringComparer.OrdinalIgnoreCase);

		private static readonly IDictionary<string, string> _mediaUrlFormat =
			new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		static JanusFormatMessage()
		{
			// youtube.com -----------------------------------------------------
			_mediaUrlFormat["youtube.com"]         = "http://www.youtube.com/v/{0}";
			_mediaUrlFormat["www.youtube.com"]     = "http://www.youtube.com/v/{0}";
			
			// rutube.ru   -----------------------------------------------------
			_mediaUrlFormat["rutube.ru"]           = "http://video.rutube.ru/{0}";
			_mediaUrlFormat["www.rutube.ru"]       = "http://video.rutube.ru/{0}";

			// known hosts
			InitWellKnownUrlsDictionary();
		}

		private static readonly Dictionary<Regex, string> _knownUrlsRegexDictionary =
			new Dictionary<Regex, string>();

		/// <summary>
		/// Known host regulars expressions initialization.
		/// </summary>
		private static void InitWellKnownUrlsDictionary()
		{
			var document = new XmlDocument();

			const string resourcePrefix = "Rsdn.Janus.Core.JanusProtocolSupport.";

			var xmlStream =
				Assembly.GetExecutingAssembly().GetRequiredResourceStream(
					resourcePrefix + "WellKnownUrls.xml");

			using (var sr = new StreamReader(xmlStream))
				document.LoadXml(sr.ReadToEnd());

			var nodes = document.SelectNodes("/WellKnownUrls/Url");
			if (nodes != null)
				foreach (XmlNode node in nodes)
					_knownUrlsRegexDictionary.Add(
						new Regex(node.Attributes["Match"].InnerText, RegexOptions.Compiled),
						node.Attributes["ImagePath"].InnerText);
		}

		public JanusFormatMessage(IServiceProvider provider)
		{
			_provider = provider;
			this.AssignServices(provider);

			ProcessUrlItself handler =
				(match, addr, name) =>
					ProcessMediaUrl(
						provider,
						match,
						addr,
						name);

			// youtube.com -----------------------------------------------------
			_hostFormatting["youtube.com"] = handler;
			_hostFormatting["www.youtube.com"] = handler;

			// rutube.ru   -----------------------------------------------------
			_hostFormatting["rutube.ru"] = handler;
			_hostFormatting["www.rutube.ru"] = handler;

			_hostFormatting["video.rutube.ru"] = handler;
			_hostFormatting["www.video.rutube.ru"] = handler;

			HostFormatting.Clear();
			SchemeFormatting.Clear();
		}

		#endregion

		#region Overridden Methods

		private const string _defaultTemplate = @"<img border=""0"" src=""{0}"" />";

		private const string _surrogateTemplate =
			@"<img border=""0"" src=""{1}"" alt=""{2}"" onclick=""if(this.src != '{0}'){{this.src='{0}';this.alt='';return false;}}"">";

		public override string ProcessImages(Match match)
		{
			var url = match.Groups["url"].Value;
			
			if (Config.Instance.ForumDisplayConfig.ShowImagesAtImgTag)
				return string.Format(_defaultTemplate, url);
			
			var alt  = url + "\r" + SR.AltTextShowImages;
			var path = GetImageUri(_provider, "surrogate", StyleImageType.ConstSize);

			return string.Format(_surrogateTemplate, url.Replace("'", @"\'"), path, alt);
		}

		protected override string GetImagePrefix()
		{
			//return JanusProtocolInfo.FormatURI(JanusProtocolResourceType.Image,
			//	@"ForumImages" + JanusProtocolInfo.ProtocolSeparatorChar);

			return JanusProtocolInfo.FormatURI(JanusProtocolResourceType.Formatter, String.Empty);
		}

		/// <summary>
		/// Обработка ссылок вида [#message_name]
		/// </summary>
		/// <param name="match">Результат сопоставления.</param>
		/// <returns>Отформатированный адрес сообщения.</returns>
		protected override string ProcessRsdnLink(Match match)
		{
			var name = match.Groups[1].Value;
			return ProcessRsdnLinkInternal(_provider, name, name);
		}

		/// <summary>
		/// Обработка ссылок вида [url=...]...[/url]
		/// </summary>
		/// <returns>Обработанная ссылка.</returns>
		protected override string ProcessURLs(string url, string tag)
		{
			// Паттерн рег.выражения несколько отличается от ImplicitURLs,
			// в частности, отсутствует группа - scheme, hostname
			return FormatURLs(ParseUrl(url), url, tag);
		}
		/// <summary>
		/// Обработка ссылок определенные при парсинге автоматически, т.е. без явного указания. 
		/// </summary>
		/// <param name="urlMatch">Результат регулярного выражения.</param>
		/// <returns>Обработанная ссылка.</returns>
		protected override string ProcessImplicitURLs(Match urlMatch)
		{
			return FormatURLs(urlMatch, urlMatch.Value, urlMatch.Value);
		}

		protected override string FormatURLs(
			Match urlMatch, 
			string urlAddress,
			string urlName)
		{
			if (!urlMatch.Groups["scheme"].Success)
				urlAddress = "http://" + urlAddress;

			urlAddress = urlAddress.EncodeAgainstXSS();

			var hostname = urlMatch.Groups["hostname"].Value;

			ProcessUrlItself processUrl;

			if (_hostFormatting.TryGetValue(hostname, out processUrl))
				return processUrl(urlMatch, urlAddress, urlName);

			return ProcessUrlInternal(urlAddress, urlName);
		}

		#endregion

		#region Link Regex

		// Регулярные выражения перенес в JanusProtocolInfo

		#endregion

		#region Private Methods
		/// <summary>
		/// Получить путь до картинки.
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="name">Имя картинки.</param>
		/// <param name="imageType">Тип картинки.</param>
		/// <returns>Путь до картинки.</returns>
		private static string GetImageUri(
			IServiceProvider provider,
			string name,
			StyleImageType imageType)
		{
			return provider.GetRequiredService<IStyleImageManager>()
				.GetImageUri(name, imageType);
		}

		/// <summary>
		/// Получить внутренний формат ссылки для соответствующего 
		/// <paramref name="resourceType"/>.
		/// </summary>
		/// <param name="resourceType">Тип ресурса.</param>
		/// <param name="parameters">Параметр ссылки.</param>
		/// <returns>Внутренний формат ссылки.</returns>
		private static string FormatUri(
			JanusProtocolResourceType resourceType, 
			string parameters)
		{
			return JanusProtocolDispatcher.FormatURI(resourceType, parameters);
		}

		private static string ProcessRsdnLinkInternal(
			IServiceProvider provider,
			string name,
			string text)
		{
			string link;
			string imageUrl;

			int id;
			var msg =
				string.IsNullOrEmpty(name)
					? null
					:	int.TryParse(name, out id)
						? DatabaseManager.GetMessageById(
								provider,
								id,
								m => new { m.ID, m.Subject, m.Date, m.UserNick })
						: DatabaseManager.GetMessageByName(
								provider,
								name,
								m => new {m.ID, m.Subject, m.Date, m.UserNick});
			if (msg == null)
			{
				link     = SiteUrlHelper.GetInfoUrl(name);
				imageUrl = GetImageUri(provider, @"ForumImages\webref", StyleImageType.ConstSize);
			}
			else
			{
				link     = FormatUri(JanusProtocolResourceType.Message, msg.ID.ToString());
				imageUrl = GetImageUri(provider, "NameGroup", StyleImageType.ConstSize);
			}

			const string format =
				"<a class='m' href='{0}' title='{4}'><img border='0' align='absmiddle' src='{1}'></a>" +
				"&nbsp;<a class='m' href='{0}' title='{4}'>{2}</a>{3}";

			return
				string.Format(
					format,
					link,
					imageUrl,
					text,
					msg != null ? GetMsgInDbLinkPostfix(provider, msg.ID) : string.Empty,
					msg != null
						? FormatMsgLinkTitle(msg.Subject, msg.Date, msg.UserNick)
						: link);
		}

		private string ProcessUrlInternal(string url, string text)
		{
			if (url  == null) throw new ArgumentNullException(nameof(url));
			if (text == null) throw new ArgumentNullException(nameof(text));

			var info = JanusProtocolInfo.Parse(url);

			string   title;
			string   imageUrl;
			LinkType linkType;

			if (info != null && info.ResourceType == JanusProtocolResourceType.Faq)
				return ProcessRsdnLinkInternal(_provider, info.Parameters, text);

			url = RefineUrl(info, out imageUrl, out title, out linkType) ?? url;

			if(linkType == LinkType.External)
				imageUrl = RefineImageForWellKnownUrls(_provider, url) ?? imageUrl;

			url = ParseUrl(url).Groups["scheme"].Success ? url : "http://" + url;
			url =
				url
					.Replace("://rsdn.ru", "://rsdn.org")
					.Replace("://www.rsdn.ru", "://rsdn.org");
			var format =
				"<a class='m' href='{0}{1}' title='{5}'><img border='0' align='absbottom' src='{3}' style='margin-bottom:1px;margin-right: 2px;'></a>" +
				"<a class='m' href='{0}{1}' title='{5}'>{2}</a>{4}";

			return
				string.Format(
					format,
					"",
					url.EncodeAgainstXSS(),
					text,
					imageUrl,
					GetPostfixImagePath(_provider, linkType, info),
					string.IsNullOrEmpty(title) ? url : title);
		}

		private static readonly Regex _queryRx =
			new Regex(@"^\w+=", RegexOptions.Compiled);

		private static string ProcessMediaUrl(
			IServiceProvider provider,
			Match match,
			string url,
			string text)
		{
			// параметр
			var param = _queryRx.Replace(
				match.Groups[match.Groups.Count - 1].Value, "");

			string mediaSrc;
			_mediaUrlFormat.TryGetValue(match.Groups["hostname"].Value, out mediaSrc);

			mediaSrc = param.Length != 0
				? string.Format(mediaSrc ?? string.Empty, param)
				: url;

			const string format =
				"<span><a mediaSrc='{4}' class='media' href='{0}' title='{3}'>{1}" +
				"<img border='0' align='baseline' src='{2}'/></a></span>";

			return string.Format(format,
				url,
				text,
				GetImageUri(provider, @"ForumImages\tv", StyleImageType.ConstSize),
				url,
				mediaSrc);
		}

		/// <summary>
		/// Хвост для ссылок (линк на внешний ресурс) на сообщения и информации 
		/// о пользователе. Для остальных случаев возвращает пустую строку.
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="linkType">Тип ссылки.</param>
		/// <param name="info">Информация о протоколе.</param>
		/// <returns>Хвост для ссылок.</returns>
		private static string GetPostfixImagePath(
			IServiceProvider provider,
			LinkType linkType, 
			JanusProtocolInfo info)
		{
			if (info != null)
			{
				if (linkType == LinkType.Local)
					return GetMsgInDbLinkPostfix(provider, info.Id);

				if (linkType == LinkType.Absent)
					return GetMsgAbsentLinkLoadPostfix(provider, info.Id);

				if (info.ResourceType == JanusProtocolResourceType.UserInfo)
					return GetUserLinkPostfix(provider, info.Id);
			}

			return string.Empty;
		}

		/// <summary>
		/// Форматирует тултип сообщения, указывая на тему, автора 
		/// и дату постинга.
		/// </summary>
		/// <returns>Тултип сообщения.</returns>
		private static string FormatMsgLinkTitle(
			string subject,
			DateTime date,
			string nick)
		{
			return $"\"{subject}\",\r{nick}, {date.ToString(Config.Instance.ForumDisplayConfig.DateFormat)}";
		}

		/// <summary>
		/// Возвращает в случае возможности линк на внутренний ресурс 
		/// (message, user-info), иначе - оригинальный линк. 
		/// Если <paramref name="protocolInfo"/>
		/// есть <c>null</c>, то функция возвращает <c>null</c>.
		/// </summary>
		/// <param name="protocolInfo">Информация о протоколе 
		/// <see cref="Rsdn.Janus.JanusProtocolInfo"/>.</param>
		/// <param name="imageUrl">Ссылка на иконку адреса.</param>
		/// <param name="title">Всплывающая подсказка (тултип).</param>
		/// <param name="linkType">Тип ссылки <see cref="Rsdn.Janus.LinkType"/>.</param>
		/// <returns>
		/// В случае возможности линк на внутренний ресурс
		/// (message, user-info), иначе - оригинальный линк. 
		/// Если <paramref name="protocolInfo"/>
		/// есть <c>null</c>, то результатом тоже будет <c>null</c>.
		/// </returns>
		private string RefineUrl(
			JanusProtocolInfo protocolInfo,
			out string imageUrl, out string title, out LinkType linkType)
		{
			title    = string.Empty;
			linkType = LinkType.External;
			imageUrl = GetImageUri(_provider, @"ForumImages\webref", StyleImageType.ConstSize);

			if (protocolInfo == null)
				return null;

			switch (protocolInfo.ResourceType)
			{
				case JanusProtocolResourceType.Message:
				case JanusProtocolResourceType.MessageAbsent:
					using (var db = _dbManager.CreateDBContext())
					{
						if (!protocolInfo.IsId)
							return protocolInfo.OriginalUrl;

						var msg =
							db
								.Message(
									protocolInfo.Id,
									m =>
										new
										{
											m.IsRead,
											m.IsMarked,
											m.ArticleId,
											m.Subject,
											m.UserNick,
											m.Date
										});

						// сообщение не найдено
						if (msg == null)
						{
							title = SR.MessageAbsent;
							linkType = LinkType.Absent;
							imageUrl = GetImageUri(_provider, "MsgNotInDb", StyleImageType.ConstSize);

							return protocolInfo.OriginalUrl;
						}

						// сообщение найдено
						title = FormatMsgLinkTitle(msg.Subject, msg.Date, msg.UserNick);

						linkType = LinkType.Local;
						imageUrl = GetMessageImagePath(
							_provider,
							msg.IsRead,
							msg.IsMarked,
							msg.ArticleId > 0);
					}

					return protocolInfo.Url;
				
				case JanusProtocolResourceType.UserInfo:
					using (var db = _dbManager.CreateDBContext())
					{
						var user =
							protocolInfo.IsId 
								? db.User(
									protocolInfo.Id,
									u => new {u.UserClass, DisplayName = u.DisplayName()})
								: null;

						if (user == null)
						{
							title    = SR.UserInfoMissed;
							imageUrl = GetImageUri(_provider, "UserNotInDb", StyleImageType.ConstSize);
							return protocolInfo.OriginalUrl;
						}

						imageUrl = GetUserImagePath(_provider, user.UserClass);
						title    = user.DisplayName;
					}
					return protocolInfo.Url;

				default:
					imageUrl = GetResourceImagePath(_provider, protocolInfo.ResourceType);
					break;
			}

			return protocolInfo.OriginalUrl;
		}

		/// <summary>
		/// Изменяет картинку для известных адресов.
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="url">Url</param>
		private static string RefineImageForWellKnownUrls(
			IServiceProvider provider,
			string url)
		{
			return 
				_knownUrlsRegexDictionary
					.Where(item => item.Key.IsMatch(url))
					.Select(
						item => GetImageUri(provider, @"ForumImages\" + item.Value, StyleImageType.ConstSize))
					.FirstOrDefault();
		}

		private const string _imageFormatPostfix =
			@"&nbsp;<a href='{0}'><img src='{1}' align='absmiddle' border='0' title='{2}'></a>";

		/// <summary>
		/// Хвост (теги &lt;a> и &lt;img>) для ссылок 
		/// на информацию о пользователе на сайте.
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="uid">ID пользователя.</param>
		/// <returns>Ссылка.</returns>
		private static string GetUserLinkPostfix(
			IServiceProvider provider,
			int uid)
		{
			return string.Format(_imageFormatPostfix,
				SiteUrlHelper.GetUserProfileUrl(uid),
				GetImageUri(provider, "ExtBrowser", StyleImageType.Small),
				SR.MsgExtBrowserLinkTitle);
		}

		/// <summary>
		/// Хвост (теги &lt;a> и &lt;img>) для локальных ссылок.
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="mid">ID сообщения.</param>
		private static string GetMsgInDbLinkPostfix(
			IServiceProvider provider,
			int mid)
		{
			return string.Format(_imageFormatPostfix,
				SiteUrlHelper.GetMessageUrl(mid),
				GetImageUri(provider, "ExtBrowser", StyleImageType.Small),
				SR.MsgExtBrowserLinkTitle);
		}

		/// <summary>
		/// Хвост (теги &lt;a> и &lt;img>) для локальных, но отсутсвующих в базе ссылок.
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="mid">ID сообщения.</param>
		private static string GetMsgAbsentLinkLoadPostfix(
			IServiceProvider provider,
			int mid)
		{
			var absentMessageUrl = FormatUri(
				JanusProtocolResourceType.MessageAbsent, mid.ToString());

			var downloadTopicImg = GetImageUri(provider, "downloadtopic", StyleImageType.Small);

			return string.Format(_imageFormatPostfix,
				absentMessageUrl, 
				downloadTopicImg, 
				SR.MessageAbsentLoadTitle);
		}

		#endregion
	}
}
