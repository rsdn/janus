#pragma warning disable 1692
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Reflection;

using CodeJam.Services;

using Rsdn.Janus.Framework.Imaging;
using Rsdn.Janus.Framework.Networking;
using Formatter = Rsdn.Framework.Formatting.Resources;

namespace Rsdn.Janus
{
	/// <summary>
	/// Распределяет запросы протокола по обработчикам.
	/// </summary>
	public class JanusProtocolDispatcher
	{
		#region JanusProtocolEventArgs

		/// <summary>
		/// Внутренний класс для хранения параметров и результата обработки.
		/// </summary>
		private class JanusProtocolEventArgs : EventArgs
		{
			private readonly string _parameters;

			public JanusProtocolEventArgs(string parameters)
			{
				_parameters = parameters ?? string.Empty;
			}
			/// <summary>
			/// Необработанная строка параметров.
			/// </summary>
			public string Parameters => _parameters;

			/// <summary>
			/// Результат обработки.
			/// </summary>
			public Resource Response { get; set; } = new Resource(_mimeTypeHtml, string.Empty);

			/// <summary>
			/// Параметр, трактуемый как числовой идентификатор.
			/// </summary>
			public int Id => int.Parse(_parameters);
		}
		
		#endregion

		#region Private members
		
		private delegate void JanusProtocolEventHandler(object sender, JanusProtocolEventArgs e);

		private readonly IServiceProvider _serviceProvider;

		// Постороители страниц
		private readonly HtmlPageBuilder _pageBuilder;
		private readonly XmlPageBuilder  _xmlPageBuilder;

		// Обработчики.
		private readonly Dictionary<string, JanusProtocolEventHandler> _handlers =
			new Dictionary<string, JanusProtocolEventHandler>();

		#endregion

		#region Resource Type constants
		
		//private const string _resourceHome             = "home";
		private const string _resourceFormatter		   = "formatter";
		private const string _resourceMessage		   = "message";
		private const string _resourceMessageAbsent    = "absent-message";
		private const string _resourceMessageRate      = "message-rate";
		private const string _resourceOutboxPreview    = "outbox-preview";
		private const string _resourceStyle            = "style";
		private const string _resourceTeamList         = "team-list";
		private const string _resourceUserInfo         = "user-info";
		private const string _resourceUserMessages     = "user-messages";
		private const string _resourceUserMessagesStat = "user-messages-stat";
		private const string _resourceUserRating       = "user-rating";
		private const string _resourceUserOutrating    = "user-outrating";
		private const string _resourceFaq              = "faq";
		private const string _resourceFaqList          = "faq-list";
		private const string _resourceArticleList      = "article-list";
		private const string _resourceImage            = "image";

		#endregion

		#region Mime Types

		private const string _mimeTypeHtml = MediaTypeNames.Text.Html;

		#endregion

		public JanusProtocolDispatcher(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;

			_pageBuilder    = new HtmlPageBuilder(serviceProvider);
			_xmlPageBuilder = new XmlPageBuilder(serviceProvider, _pageBuilder);

			var methods = GetType().GetMethods(
				BindingFlags.DeclaredOnly
				| BindingFlags.Instance
				| BindingFlags.Public
				| BindingFlags.NonPublic
				| BindingFlags.Static);

			foreach (var mi in methods)
			{
				var attribute =
					Attribute.GetCustomAttribute(mi, typeof(JanusProtocolEventHandlerAttribute))
						as JanusProtocolEventHandlerAttribute;
				
				if (attribute != null)
					_handlers.Add(attribute.Id,
						(JanusProtocolEventHandler)Delegate.CreateDelegate(typeof(JanusProtocolEventHandler), this, mi.Name));
			}
		}

		#region Генератор внутренних ссылок

		/// <summary>
		/// Получить внутренний формат ссылки для соответствующего 
		/// <paramref name="resourceType"/>.
		/// </summary>
		/// <param name="resourceType">Тип ресурса.</param>
		/// <param name="parameters">Параметр ссылки.</param>
		/// <returns>Внутренний формат ссылки.</returns>
		public static string FormatURI(JanusProtocolResourceType resourceType, string parameters)
		{
			return JanusProtocolInfo.FormatURI(resourceType, parameters);
		}

		#endregion

		#region Генератор внешних ссылок

		/// <summary>
		/// Получить внешний формат ссылки для соответствующего ресурса. 
		/// <seealso cref="SiteUrlHelper"/>
		/// </summary>
		/// <param name="resourceType">Тип ресурса.</param>
		/// <param name="parameters">Параметр ссылки.</param>
		/// <returns>Внешний формат ссылки на ресурс.</returns>
		public static string FormatExternalURI(JanusProtocolResourceType resourceType, string parameters)
		{
			return JanusProtocolInfo
				.FormatExternalURI(resourceType, parameters);
		}

		#endregion

		#region Public Methods
		/// <summary>
		/// Распределяет запросы протокола по обработчикам.
		/// </summary>
		/// <param name="uri">Путь запроса.</param>
		/// <returns>Результат обработки запроса.</returns>
		public Resource DispatchRequest(string uri)
		{
			try
			{
				var info = JanusProtocolInfo.Parse(uri);

				var resource = info != null ? info.ResourceName : uri;

				if (!_handlers.ContainsKey(resource))
					throw new ArgumentException(string.Format(SR.ResourceNotFound, resource));

				System.Diagnostics.Debug.Assert(info != null);

				var jpea = new JanusProtocolEventArgs(info.Parameters);
				_handlers[resource](this, jpea);
				return jpea.Response;
			}
			catch (Exception e)
			{
				return new Resource(_mimeTypeHtml, HtmlPageBuilder.GetExceptionMessage(uri, e));
			}
		}

		#endregion

		#region Обработчики протокола
		[JanusProtocolEventHandler(_resourceFormatter)]
		private void FormatterEventHandler(object sender, JanusProtocolEventArgs e)
		{
			using (var resr = Formatter.ResourceProvider.ReadResource(e.Parameters))
			{
				if (resr.Binary)
					e.Response = new Resource(resr.GetContentType(), (byte[])resr.Read());
				else
				{
					var prx = JanusProtocolInfo.FormatURI(JanusProtocolResourceType.Formatter, String.Empty);
					var src = ((String)resr.Read()).Replace("%URL%", prx);
					e.Response = new Resource(resr.GetContentType(), src);
				}
			}
		}


		[JanusProtocolEventHandler(_resourceMessage)]
		private void MessageEventHandler(object sender, JanusProtocolEventArgs e)
		{
			//e.Response = _pageBuilder.GetMessageText(e.Id);
			e.Response = new Resource(_mimeTypeHtml, _xmlPageBuilder.GetMessageText(e.Id));
		}

		[JanusProtocolEventHandler(_resourceMessageAbsent)]
		private void MessageAbsentEventHandler(object sender, JanusProtocolEventArgs e)
		{
			e.Response = new Resource(_mimeTypeHtml, HtmlPageBuilder.GetAbsentMessageText(_serviceProvider, e.Id));
		}
		
		[JanusProtocolEventHandler(_resourceMessageRate)]
		private void MessageRateEventHandler(object sender, JanusProtocolEventArgs e)
		{
			e.Response = new Resource(_mimeTypeHtml, _pageBuilder.GetMessageRate(e.Id));
		}
		
		[JanusProtocolEventHandler(_resourceOutboxPreview)]
		private void OutboxPreviewEventHandler(object sender, JanusProtocolEventArgs e)
		{
			e.Response =
				new Resource(
					_mimeTypeHtml,
					OutboxManager.GetPreviewData(e.Id));
		}

		[JanusProtocolEventHandler(_resourceStyle)]
		private void StyleEventHandler(object sender, JanusProtocolEventArgs e)
		{
			e.Response = new Resource(_mimeTypeHtml, HtmlPageBuilder.GetNamedStyle(e.Parameters));
		}
		
		[JanusProtocolEventHandler(_resourceTeamList)]
		private void TeamListEventHandler(object sender, JanusProtocolEventArgs e)
		{
			e.Response = new Resource(_mimeTypeHtml, HtmlPageBuilder.GetTeamList(_serviceProvider));
		}
		
		[JanusProtocolEventHandler(_resourceUserInfo)]
		private void UserInfoEventHandler(object sender, JanusProtocolEventArgs e)
		{
			e.Response = new Resource(_mimeTypeHtml, _pageBuilder.GetUserInfoText(e.Id));
		}
		
		[JanusProtocolEventHandler(_resourceUserMessages)]
		private void UserMessagesEventHandler(object sender, JanusProtocolEventArgs e)
		{
			e.Response = new Resource(_mimeTypeHtml, _pageBuilder.GetUserMessagesText(e.Id));
		}

		[JanusProtocolEventHandler(_resourceUserMessagesStat)]
		private void UserMessagesStatEventHandler(object sender, JanusProtocolEventArgs e)
		{
			e.Response = new Resource(_mimeTypeHtml, _pageBuilder.GetUserMessagesStatText(e.Id));
		}

		[JanusProtocolEventHandler(_resourceUserRating)]
		private void UserRatingEventHandler(object sender, JanusProtocolEventArgs e)
		{
			e.Response = new Resource(_mimeTypeHtml, _pageBuilder.GetUserRatingText(e.Id));
		}
		
		[JanusProtocolEventHandler(_resourceUserOutrating)]
		private void UserOutratingEventHandler(object sender, JanusProtocolEventArgs e)
		{
			e.Response = new Resource(_mimeTypeHtml, _pageBuilder.GetUserOutratingText(e.Id));
		}
		
		[JanusProtocolEventHandler(_resourceArticleList)]
		private void ArticleEventHandler(object sender, JanusProtocolEventArgs e)
		{
			e.Response = new Resource(_mimeTypeHtml, _pageBuilder.GetArticleListText(e.Id));
		}

		[JanusProtocolEventHandler(_resourceFaq)]
		private void FaqEventHandler(object sender, JanusProtocolEventArgs e)
		{
			e.Response = new Resource(_mimeTypeHtml, _xmlPageBuilder.GetMessageText(e.Parameters));
		}

		[JanusProtocolEventHandler(_resourceFaqList)]
		private void FaqListEventHandler(object sender, JanusProtocolEventArgs e)
		{
			e.Response = new Resource(_mimeTypeHtml, _pageBuilder.GetFaqListText(e.Id));
		}

		[JanusProtocolEventHandler(_resourceImage)]
		private void ImageEventHandler(object sender, JanusProtocolEventArgs e)
		{
			var styleImageManager = _serviceProvider.GetRequiredService<IStyleImageManager>();
			var img = styleImageManager.GetImage(e.Parameters);
			using (var ms = new MemoryStream())
			{
				var ifi = ImageFormatInfo.FromImageFormat(img.RawFormat);
				img.Save(ms, img.RawFormat);
				e.Response = new Resource(ifi.MimeType, ms.ToArray());
			}
		}
		#endregion
	}
}
