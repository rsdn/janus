using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;

using Rsdn.SmartApp;

using System.Linq;

namespace Rsdn.Janus
{
	internal class XmlPageBuilder : ServiceConsumer
	{
		private const string _resourcePrefix =
			"Rsdn.Janus.Core.JanusProtocolSupport.XmlPageBuilder.";

		private readonly XmlSerializer _serializer =
			new XmlSerializer(typeof(XmlMessage));

		private readonly XslCompiledTransform _xslTransform =
			new XslCompiledTransform();

		private readonly HtmlPageBuilder _pageBuilder;
		private readonly IServiceProvider _serviceProvider;
		private readonly Lazy<FormatterData[]> _formatters;

// ReSharper disable RedundantDefaultFieldInitializer
// ReSharper disable ConvertToConstant
		[ExpectService]
		private IJanusDatabaseManager _dbManager = null;
// ReSharper restore ConvertToConstant
// ReSharper restore RedundantDefaultFieldInitializer

		public XmlPageBuilder(IServiceProvider serviceProvider, HtmlPageBuilder pageBuilder)
			: base(serviceProvider)
		{
			_serviceProvider = serviceProvider;
			_pageBuilder = pageBuilder;

			var xslStream = Assembly.GetExecutingAssembly()
				.GetRequiredResourceStream(_resourcePrefix + "Message.xsl");

			using (XmlReader xr = new XmlTextReader(xslStream))
				_xslTransform.Load(xr, null, null);

			_formatters =
				new Lazy<FormatterData[]>(
					() =>
						serviceProvider
							.GetRegisteredElements<MessageFormatterInfo>()
							.Select(info => new FormatterData(info, (IMessageFormatter)info.FormatterType.CreateInstance(serviceProvider)))
							.ToArray());
		}

		/// <summary>
		/// Получить отформатированный текст сообщения.
		/// </summary>
		/// <param name="mid">ID сообщения.</param>
		/// <returns>Отформатированный текст сообщения.</returns>
		public string GetMessageText(int mid)
		{
			bool msgExists;
			using (var db = _serviceProvider.CreateDBContext())
				msgExists = db.Messages().Count(m => m.ID == mid) > 0;

			if (!msgExists)
				return _pageBuilder.GetNotFoundMessage(mid);

			var fs = _formatters
				.Value
				.Where(data => data.Info.FormatSource)
				.ToArray();

			var message =
				XmlBuilder.BuildMessage(
					_serviceProvider,
					_dbManager,
					mid,
					src =>
						_formatters
							.Value
							.Where(data => data.Info.FormatSource)
							.Aggregate(src, (current, fmt) => fmt.Formatter.FormatSource(current)));

			var html = TransformMessage(message);
			html =
				_formatters
					.Value
					.Where(data => data.Info.FormatHtml)
					.Aggregate(html, (current, fmt) => fmt.Formatter.FormatHtml(current));
			return html;
		}

		/// <summary>
		/// Получить отформатированный текст именованного сообщения (ФАКУ).
		/// </summary>
		/// <param name="messageName">Имя сообщения.</param>
		/// <returns>Отформатированный текст сообщения.</returns>
		public string GetMessageText(string messageName)
		{
			messageName = messageName.Trim();

			var id =
				messageName.Length != 0 
					? DatabaseManager.GetMessageByName(
						_serviceProvider,
						messageName,
						msg => (int?)msg.ID)
					: null;

			return
				id == null
					? _pageBuilder.GetNotFoundMessage(messageName)
					: GetMessageText(id.Value);
		}

		private string TransformMessage(XmlMessage message)
		{
			using (Stream stream = new MemoryStream())
			{
				_serializer.Serialize(
					new XmlTextWriterEx(stream, Encoding.UTF8),
					message);

				stream.Seek(0, SeekOrigin.Begin);

				return Transform(stream);
			}
		}

		private string Transform(Stream stream)
		{
			IXPathNavigable xpd = new XPathDocument(stream);

			var args = new XsltArgumentList();
			args.AddExtensionObject("urn:XsltFormatUtils", new XsltFormatUtils(_serviceProvider));

			Stream resultStream = new MemoryStream();

			_xslTransform.Transform(xpd, args, resultStream);

			resultStream.Seek(0, SeekOrigin.Begin);

			using (TextReader tr = new StreamReader(resultStream))
			{
				return tr.ReadToEnd();
			}
		}

		#region FormatterData class
		private class FormatterData
		{
			private readonly MessageFormatterInfo _info;
			private readonly IMessageFormatter _formatter;

			public FormatterData(MessageFormatterInfo info, IMessageFormatter formatter)
			{
				_info = info;
				_formatter = formatter;
			}

			public MessageFormatterInfo Info
			{
				get { return _info; }
			}

			public IMessageFormatter Formatter
			{
				get { return _formatter; }
			}
		}
		#endregion
	}
}