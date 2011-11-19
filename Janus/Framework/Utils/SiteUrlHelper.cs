using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Предоставляет возможность определения url сайта.
	/// </summary>
	public static class SiteUrlHelper
	{
		private static string RsdnServerUrl
		{
			get
			{
				// Перенес вычисление базового пути в конфиг.
				return Config.Instance.SiteUrl;
			}
		}

		public static string GetForumUrl(string forumName)
		{
			return String.Format(
				"{0}Forum/?group={1}",
				RsdnServerUrl,
				forumName);
		}

		public static string GetMessageUrl(int messageId)
		{
			return JanusProtocolInfo.FormatExternalURI(
				JanusProtocolResourceType.Message, messageId.ToString());
		}

		public static string GetUserProfileUrl(int userId)
		{
			return JanusProtocolInfo.FormatExternalURI(
				JanusProtocolResourceType.UserInfo, userId.ToString());

			//return String.Format("{0}/Users/Profile.aspx?uid={1}",
			//	SiteUrlHelper.RsdnServerUrl,
			//	userID);
		}

		public static string GetSelfModerateUrl(int messageId)
		{
			return String.Format("{0}Forum/Private/Self.aspx?mid={1}",
								 RsdnServerUrl,
								 messageId);
		}

		public static string GetRatingUrl(int messageId)
		{
			return String.Format("{0}Forum/RateList.aspx?mid={1}",
								 RsdnServerUrl,
								 messageId);
		}

		public static string GetFileUploadUrl()
		{
			return String.Format("{0}Tools/Private/FileList.aspx",
								 RsdnServerUrl);
		}

		public static string GetInfoUrl(string name)
		{
			return JanusProtocolInfo
				.FormatExternalURI(JanusProtocolResourceType.Faq, name);
		}
	}
}