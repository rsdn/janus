using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Предоставляет возможность определения url сайта.
	/// </summary>
	public static class RsdnUrlHelper
	{
		private static string RsdnServerUrl
		{
			get
			{
				return "http://rsdn.ru/";
			}
		}

		public static string GetForumUrl(string forumName)
		{
			return String.Format(
				"{0}Forum/?group={1}",
				RsdnServerUrl,
				forumName);
		}

		public static string GetSelfModerateUrl(int messageId)
		{
			return String.Format("{0}Forum/Private/Self.aspx?mid={1}",
								 RsdnServerUrl,
								 messageId);
		}

		public static string GetWarnModeratorUrl(int messageId)
		{
			return
				String.Format("{0}ViolationReport/ReportViolation/{1}",
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
	}
}