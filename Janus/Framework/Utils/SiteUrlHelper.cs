using System;
using System.Web;

namespace Rsdn.Janus
{
	/// <summary>
	/// Предоставляет возможность определения url сайта.
	/// </summary>
	public static class SiteUrlHelper
	{
		private static string RsdnServerUrl => Config.Instance.SiteUrl;

		public static string GetForumUrl(string forumName)
		{
			return $"{RsdnServerUrl}Forum/?group={forumName}";
		}

		public static string GetTagUrl(string tag)
		{
			return $"{RsdnServerUrl}tag/messages?tag={HttpUtility.UrlEncode(tag)}";
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
			return $"{RsdnServerUrl}Forum/Private/Self.aspx?mid={messageId}";
		}

		public static string GetRatingUrl(int messageId)
		{
			return $"{RsdnServerUrl}Forum/RateList.aspx?mid={messageId}";
		}

		public static string GetFileUploadUrl()
		{
			return $"{RsdnServerUrl}Tools/Private/FileList.aspx";
		}

		public static string GetInfoUrl(string name)
		{
			return JanusProtocolInfo.FormatExternalURI(JanusProtocolResourceType.Faq, name);
		}
	}
}