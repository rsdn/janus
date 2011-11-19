using System;
using System.IO;
using System.Reflection;

using JetBrains.Annotations;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
	internal class XsltFormatUtils
	{
		private const string _copyCodeResourceName =
			"Rsdn.Janus.Core.JanusProtocolSupport.XmlPageBuilder.CopyCode.js";

		private const string _mediaResourceName =
			"Rsdn.Janus.Core.JanusProtocolSupport.XmlPageBuilder.Media.js";

		private readonly IServiceProvider _serviceProvider;

		public XsltFormatUtils(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		[UsedImplicitly]
		public string FormatUserInfoURI(string parameters)
		{
			return JanusProtocolInfo.FormatURI(
				JanusProtocolResourceType.UserInfo, parameters);
		}

		[UsedImplicitly]
		public string FormatMessageRateURI(string parameters)
		{
			return JanusProtocolInfo.FormatURI(
				JanusProtocolResourceType.MessageRate, parameters);
		}

		[UsedImplicitly]
		public string GetUserImagePath(UserClass userClass)
		{
			return JanusFormatMessage
				.GetUserImagePath(_serviceProvider, userClass);
		}

		[UsedImplicitly]
		public string GetWeekDayImagePath(int weekDay, bool isOutdated)
		{
			return JanusFormatMessage
				.GetWeekDayImagePath(_serviceProvider, weekDay, isOutdated);
		}

		[UsedImplicitly]
		public string GetMessageImagePath(bool isRead,
			bool isMarked, bool isArticle)
		{
			return JanusFormatMessage
				.GetMessageImagePath(_serviceProvider, isRead, isMarked, isArticle);
		}

		[UsedImplicitly]
		public string GetRateImagePath(MessageRates rate)
		{
			return JanusFormatMessage.GetRateImagePath(_serviceProvider, rate);
		}

		[UsedImplicitly]
		public string GetConstSizeImagePath(string name)
		{
			var styleImageManager = _serviceProvider.GetRequiredService<IStyleImageManager>();
			return styleImageManager.GetImageUri(name, StyleImageType.ConstSize);
		}

		[UsedImplicitly]
		public string FormatUserClass(UserClass userClass)
		{
			var uc = JanusFormatMessage
				.FormatUserClass(userClass, true);

			return uc.Length != 0 ? "&nbsp;" + uc : string.Empty;
		}

		private static string _jsCopyCode;

		[UsedImplicitly]
		public string FriendlyCopyCode()
		{
			if (!Config.Instance.ForumDisplayConfig.Envelope.UseFriendlyCopyCode)
				return string.Empty;

			return _jsCopyCode ?? (_jsCopyCode = GetResource(_copyCodeResourceName));
		}

		private static string _jsMedia;

		[UsedImplicitly]
		public string MediaContentCode()
		{
			if (!Config.Instance.ForumDisplayConfig.DetectMediaContentLinks)
				return string.Empty;

			return _jsMedia ?? (_jsMedia = GetResource(_mediaResourceName));
		}

		private static string GetResource(string resourceName)
		{
			var stream = Assembly.GetExecutingAssembly()
				.GetRequiredResourceStream(resourceName);

			using (var sr = new StreamReader(stream))
				return sr.ReadToEnd();
		}
	}
}
