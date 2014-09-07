using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Rsdn.Janus
{
	using ResourceNameMap = Dictionary<JanusProtocolResourceType, string>;
	using ResourceTypeMap = Dictionary<string, JanusProtocolResourceType>;

	/// <summary>
	/// Инкапсулирует информацию о внутреннем протоколе janus://,
	/// производит разбор и форматирование во внешний и во внутренний формат.
	/// </summary>
	public class JanusProtocolInfo
	{
		#region Private members

		private readonly string _originalUrl;
		private readonly int _id;
		private readonly bool _isId;
		private readonly string _parameters;
		private readonly JanusProtocolResourceType _resourceType;

		#endregion

		#region Public Properties

		/// <summary>
		/// Строковое представление типа ресурса.
		/// </summary>
		public string ResourceName
		{
			get { return GetResourceName(ResourceType); }
		}

		/// <summary>
		/// Тип внутреннего ресурса. <see cref="Rsdn.Janus.JanusProtocolResourceType"/>
		/// </summary>
		public JanusProtocolResourceType ResourceType
		{
			get { return _resourceType; }
		}

		/// <summary>
		/// Необработанный параметр запроса.
		/// </summary>
		public string Parameters
		{
			get { return _parameters; }
		}

		/// <summary>
		/// Параметр, трактуемый как числовой идентификатор.
		/// </summary>
		public int Id
		{
			get { return _id; }
		}

		/// <summary>
		/// Возвращает true, если параметр можно трактовать как числовой идентификатор,
		/// иначе - false.
		/// </summary>
		public bool IsId
		{
			get { return _isId; }
		}

		/// <summary>
		/// Тип ссылки.
		/// </summary>
		public LinkType LinkType { get; private set; }

		/// <summary>
		/// Оригинальная строка запроса.
		/// </summary>
		public string OriginalUrl
		{
			get { return _originalUrl; }
		}

		public string Url
		{
			get { return FormatURI(ResourceType, Parameters); }
		}

		public string ExternalUrl
		{
			get { return FormatExternalURI(ResourceType, Parameters); }
		}

		public static char ProtocolSeparatorChar
		{
			get { return _janusProtocolSeparator; }
		}

		#endregion

		#region Constructors

		private static readonly ResourceTypeMap _resourceTypes =
			new ResourceTypeMap();

		private static readonly ResourceNameMap _resourceNames =
			new ResourceNameMap();

		static JanusProtocolInfo()
		{
			_resourceTypes.Add(_formatter, JanusProtocolResourceType.Formatter);
			_resourceTypes.Add(_message, JanusProtocolResourceType.Message);
			_resourceTypes.Add(_messageAbsent, JanusProtocolResourceType.MessageAbsent);
			_resourceTypes.Add(_messageRate, JanusProtocolResourceType.MessageRate);
			_resourceTypes.Add(_outboxPreview, JanusProtocolResourceType.OutboxPreview);
			_resourceTypes.Add(_style, JanusProtocolResourceType.Style);
			_resourceTypes.Add(_teamList, JanusProtocolResourceType.TeamList);
			_resourceTypes.Add(_userInfo, JanusProtocolResourceType.UserInfo);
			_resourceTypes.Add(_userMessages, JanusProtocolResourceType.UserMessages);
			_resourceTypes.Add(_userMessagesStat, JanusProtocolResourceType.UserMessagesStat);
			_resourceTypes.Add(_userRating, JanusProtocolResourceType.UserRating);
			_resourceTypes.Add(_userOutrating, JanusProtocolResourceType.UserOutrating);
			_resourceTypes.Add(_faq, JanusProtocolResourceType.Faq);
			_resourceTypes.Add(_faqList, JanusProtocolResourceType.FaqList);
			_resourceTypes.Add(_articleList, JanusProtocolResourceType.ArticleList);
			_resourceTypes.Add(_image, JanusProtocolResourceType.Image);

			foreach (var kvp in _resourceTypes)
				_resourceNames.Add(kvp.Value, kvp.Key);
		}

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="resourceType">Тип внутреннего ресурса janus://</param>
		/// <param name="parameters">Необработанный параметр запроса.</param>
		/// <param name="url">Строка запроса.</param>
		public JanusProtocolInfo(JanusProtocolResourceType resourceType, string parameters, string url)
		{
			_originalUrl = url;
			_resourceType = resourceType;
			_parameters = parameters ?? string.Empty;
			_isId = int.TryParse(parameters, out _id);

			switch (resourceType)
			{
				case JanusProtocolResourceType.Message:
					LinkType = _isId ? LinkType.Local : LinkType.External;
					break;

				case JanusProtocolResourceType.MessageAbsent:
					LinkType = _isId ? LinkType.Absent : LinkType.External;
					break;

				case JanusProtocolResourceType.Faq:
					LinkType = LinkType.Local;
					break;

				default:
					LinkType = LinkType.External;
					break;
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Извлекает информацию о протоколе. 
		/// </summary>
		/// <param name="url">Строка запроса.</param>
		/// <returns>В случае удачи - информация о протоколе, иначе - <c>null</c>.</returns>
		public static JanusProtocolInfo Parse(string url)
		{
			if (url == null)
				throw new ArgumentNullException("url");

			if (url.TrimEnd().Length == 0)
				return null;

			JanusProtocolInfo protocolInfo;

			if (TryJanusUrlParse(url, out protocolInfo))
				return protocolInfo;

			if (TryExternalUrlParse(url, out protocolInfo))
			{
				protocolInfo.LinkType = LinkType.External;
				return protocolInfo;
			}

			return null;
		}

		/// <summary>
		/// Возвращает внутреннее строковое имя ресурса для указанного типа.
		/// </summary>
		/// <param name="resourceType">Тип ресурса.</param>
		/// <returns>Строковое имя ресурса.</returns>
		public static string GetResourceName(JanusProtocolResourceType resourceType)
		{
			string resourceName;
			return _resourceNames.TryGetValue(resourceType, out resourceName)
					? resourceName
					: string.Empty;
		}

		/// <summary>
		/// Возвращает оригинальную строку запроса
		/// </summary>
		/// <returns>Оригинальная строка запроса.</returns>
		public override string ToString()
		{
			return OriginalUrl;
		}

		/// <summary>
		/// Получить внутренний формат ссылки для соответствующего <paramref name="resourceType"/>.
		/// </summary>
		/// <param name="resourceType">Тип ресурса.</param>
		/// <param name="parameters">Параметр ссылки.</param>
		/// <returns>Внутренний формат ссылки.</returns>
		public static string FormatURI(JanusProtocolResourceType resourceType, string parameters)
		{
			return string.Format(_janusProtocolTemplate,
								 _janusProtocolPrefix, GetResourceName(resourceType),
								 _janusProtocolSeparator, parameters);
		}

		/// <summary>
		/// Получить внешний формат ссылки для соответствующего ресурса. 
		/// <seealso cref="SiteUrlHelper"/>
		/// </summary>
		/// <param name="resourceType">Тип ресурса.</param>
		/// <param name="parameters">Параметр ссылки.</param>
		/// <returns>Внешний формат ссылки на ресурс.</returns>
		public static string FormatExternalURI(
			JanusProtocolResourceType resourceType,
			string parameters)
		{
			string format;

			switch (resourceType)
			{
				case JanusProtocolResourceType.Faq:
					format = _externalFaq;
					break;
				case JanusProtocolResourceType.FaqList:
					format = _externalFaqList;
					break;
				case JanusProtocolResourceType.Message:
					format = _externalMessage;
					break;
				case JanusProtocolResourceType.UserInfo:
					format = _externalUserInfo;
					break;
				case JanusProtocolResourceType.UserRating:
					format = _externalUserRating;
					break;
				case JanusProtocolResourceType.UserMessages:
					format = _externalUserMessages;
					break;
				case JanusProtocolResourceType.ArticleList:
					format = _externalArticleList;
					break;
				case JanusProtocolResourceType.UserOutrating:
					format = _externalUserOutrating;
					break;

				default:
					return string.Empty;
			}

			return string.Format(format, Config.Instance.SiteUrl, parameters);
		}

		#endregion

		#region Константы

		private const string _formatter = "formatter";
		
		private const string _message = "message";
		private const string _messageAbsent = "absent-message";
		private const string _messageRate = "message-rate";
		private const string _outboxPreview = "outbox-preview";
		private const string _style = "style";
		private const string _teamList = "team-list";
		private const string _userInfo = "user-info";
		private const string _userMessages = "user-messages";
		private const string _userMessagesStat = "user-messages-stat";
		private const string _userRating = "user-rating";
		private const string _userOutrating = "user-outrating";
		private const string _faq = "faq";
		private const string _faqList = "faq-list";
		private const string _articleList = "article-list";
		private const string _image = "image";

		private const string _janusProtocolPrefix = "janus://";
		private const string _janusProtocolTemplate = "{0}{1}{2}{3}"; // [prefix][resource][delimiter][parameters]
		private const char _janusProtocolSeparator = '/'; // разделитель ресурса и параметров

		private const string _externalArticleList = @"{0}recent/125.xml";
		private const string _externalFaq = @"{0}Forum/Info.aspx?name={1}";
		private const string _externalFaqList = @"{0}Forum/MsgList.aspx?gid={1}&flat=0&IsFAQ=1";
		private const string _externalUserInfo = @"{0}Users/Profile.aspx?uid={1}";
		private const string _externalUserRating = @"{0}Forum/?rid={1}";
		private const string _externalUserOutrating = @"{0}Forum/?rby={1}";
		private const string _externalUserMessages = @"{0}Forum/?uid={1}";
		private const string _externalMessage = @"{0}forum/message/{1}.1";

		#endregion

		#region Служебные функции

		#region Регулярные выражения

		// "^(?:janus://)?(?<resource>[-a-z]+)/(?<parameters>.*)$"
		private static readonly Regex _resourceInfoRegex = new Regex(
			string.Format("^(?:{0})?(?<resource>[-a-z]+){1}(?<parameters>.*?)(?:#.*)?$",
						  _janusProtocolPrefix, _janusProtocolSeparator),
			RegexOptions.Compiled);

		private const string _rsdnLinkPattern = @"^(http://)?((www|gzip|rsdn|rsdn3).)?rsdn.ru";

		// Варианты ссылок на сообщения
		// http://rsdn.ru/forum/test/5765306.1
		private static readonly Regex _msgUrlDetectorRx =
			new Regex(
				_rsdnLinkPattern + @"/forum/[a-zA-Z.]+/\d{1,9}(.(1|all|flat(.\d+)?|hot))?",
				RegexOptions.Compiled | RegexOptions.IgnoreCase);

		// http://www.rsdn.ru/forum/message/4234523.aspx
		// http://www.rsdn.ru/forum/message/123456.1.aspx
		// http://www.rsdn.ru/forum/message/123456.flat.aspx
		// http://www.rsdn.ru/forum/message/123456.flat.6.aspx
		// http://www.rsdn.ru/forum/flame.politics/3402512.1.aspx
		// http://rsdn.ru/forum/life/5343382
		// http://rsdn.ru/forum/life/5343382.flat
		private static readonly Regex _msgUrlDetector2Rx =
			new Regex(
				_rsdnLinkPattern
				+ @"(/|\\)[?]?forum(/|\\)[\w\.]+(/|\\)(?'mid'\d+)(\.flat)?(\.\d+)?(\.aspx)?",
				RegexOptions.Compiled | RegexOptions.IgnoreCase);

		// Поддержка именованных ресурсов
		// http://www.rsdn.ru/Forum/Info/FAQ.dotnet.reflection.emit.aspx
		// http://www.rsdn.ru/Forum/Info/FAQ/dotnet/reflection/emit.aspx
		private static readonly Regex _infoUrlDetectorRx = new Regex(_rsdnLinkPattern
																	 +
																	 @"(?:/|\\)[?]?Forum(?:/|\\)Info(?:/|\\)(?<name>.+?)\.aspx$",
																	 RegexOptions.Compiled | RegexOptions.IgnoreCase);

		// http://www.rsdn.ru/Forum/Info.aspx?name=FAQ.dotnet.reflection.emit
		private static readonly Regex _infoUrlDetectorRx2 = new Regex(_rsdnLinkPattern
																	  +
																	  @"(?:/|\\)[?]?Forum(?:/|\\)Info\.aspx\?name=(?<name>.+?)(?:\.aspx)?$",
																	  RegexOptions.Compiled | RegexOptions.IgnoreCase);

		// http://rsdn.ru/Users/Profile.aspx?uid=12345
		private static readonly Regex _profileDetectorRx = new Regex(_rsdnLinkPattern
																	 + @"(/|\\)[?]?Users(/|\\)Profile\.aspx\?uid=(?'uid'\d+)",
																	 RegexOptions.Compiled | RegexOptions.IgnoreCase);

		// http://rsdn.ru/Users/12345.aspx
		private static readonly Regex _profileDetector2Rx = new Regex(_rsdnLinkPattern
																	  + @"(/|\\)Users(/|\\)(?'uid'\d+)\.aspx",
																	  RegexOptions.Compiled | RegexOptions.IgnoreCase);

		#endregion

		private static bool TryJanusUrlParse(string url,
											 out JanusProtocolInfo protocolInfo)
		{
			var match = _resourceInfoRegex.Match(url);
			if (match.Success)
			{
				var resource = match.Groups["resource"].Value.ToLower();
				var parameters = match.Groups["parameters"].Value;

				JanusProtocolResourceType resourceType;
				if (_resourceTypes.TryGetValue(resource, out resourceType))
					return Make(resourceType, parameters, url, out protocolInfo);
			}

			protocolInfo = null;
			return false;
		}

		/// <summary>
		/// Пытается ивлечь информацию о протоколе из внешнего адреса, 
		/// который представим в Rsdn@Home - "message" и "user-info".
		/// Функция возвращает <c>true</c> - если адрес распознан.
		/// </summary>
		/// <param name="url">Внешний адрес.</param>
		/// <param name="protocolInfo">В случае удачи - информация о протоколе,
		/// иначе - <c>null</c>.</param>
		/// <returns><c>true</c> - если адрес распознан.</returns>
		private static bool TryExternalUrlParse(string url, out JanusProtocolInfo protocolInfo)
		{
			#region Сообщение

			var match = _msgUrlDetectorRx.Match(url);

			if (!match.Success)
				match = _msgUrlDetector2Rx.Match(url);

			if (match.Success)
				return Make(JanusProtocolResourceType.Message,
							match.Groups["mid"].Value, url, out protocolInfo);

			#endregion

			#region ФАКУ

			match = _infoUrlDetectorRx.Match(url);

			if (!match.Success)
				match = _infoUrlDetectorRx2.Match(url);

			if (match.Success)
				return Make(JanusProtocolResourceType.Faq,
							match.Groups["name"].Value.Replace('/', '.'),
							url, out protocolInfo);

			#endregion

			#region Профиль пользователя

			match = _profileDetectorRx.Match(url);

			if (!match.Success)
				match = _profileDetector2Rx.Match(url);

			if (match.Success)
				return Make(JanusProtocolResourceType.UserInfo,
							match.Groups["uid"].Value, url, out protocolInfo);

			#endregion

			// ничего не найдено
			protocolInfo = null;
			return false;
		}

		private static bool Make(
			JanusProtocolResourceType resourceType,
			string parameters,
			string url,
			out JanusProtocolInfo protocolInfo)
		{
			protocolInfo = new JanusProtocolInfo(resourceType, parameters, url);
			return true;
		}

		#endregion
	}
}