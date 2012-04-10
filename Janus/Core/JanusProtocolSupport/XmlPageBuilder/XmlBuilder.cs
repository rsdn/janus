using System;

using System.Linq;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	internal static class XmlBuilder
	{
		public static XmlMessage BuildMessage(
			IServiceProvider provider,
			IJanusDatabaseManager dbMgr,
			int mid,
			[CanBeNull] Func<string, string> sourceFormatter)
		{
			using (var db = dbMgr.CreateDBContext())
			{
				var msg =
					db
						.Message(
							mid,
							m =>
								new
								{
									m.UserID,
									m.UserClass,
									UserNick = m.UserNick.ToUserDisplayName(m.UserClass),
									m.Date,
									m.IsRead,
									m.IsMarked,
									m.ArticleId,
									m.Name,
									m.Subject,
									m.Message,
									m.User.Origin,
									Rating = m.Rating(),
									Smiles = m.SmileCount(),
									Agrees = m.AgreeCount(),
									Disagrees = m.DisagreeCount(),
									m.Violation.PenaltyType,
									m.Violation.Reason
								});
				var body = sourceFormatter != null ? sourceFormatter(msg.Message) : msg.Message;
				var formatter = provider.GetFormatter();
				var xmlMessage =
					new XmlMessage
					{
						FormattingOptions =
							{
								ShowHeader = Config.Instance.ForumDisplayConfig.Envelope.ShowHeader,
								ShowRateFrame = Config.Instance.ForumDisplayConfig.Envelope.ShowRateFrame
							},
						ID = mid,
						Author =
							{
								ID = msg.UserID,
								UserClass = (int)msg.UserClass,
								DisplayName = msg.UserNick
							},
						Date =
							{
								Value = msg.Date.ToString(Config.Instance.ForumDisplayConfig.DateFormat),
								IsOutdate =
									DateTime.Now.AddDays(-Config.Instance.ForumDisplayConfig.DaysToOutdate) > msg.Date
									&& Config.Instance.ForumDisplayConfig.DaysToOutdate != 0,
								DayOfWeek = Convert.ToInt32(msg.Date.DayOfWeek)
							},
						IsUnread = !msg.IsRead,
						IsMarked = msg.IsMarked,
						ArticleID = msg.ArticleId.GetValueOrDefault(),
						Name = msg.Name,
						Subject = msg.Subject,
						Content = formatter.Format(body, true),
						Origin = formatter.Format(msg.Origin, true),
						Rate = {Summary = JanusFormatMessage.FormatRates(msg.Rating, msg.Smiles, msg.Agrees, msg.Disagrees)},
						ViolationPenaltyType = (int) msg.PenaltyType,
						ViolationReason = msg.Reason
					};

				if (Config.Instance.ForumDisplayConfig.Envelope.ShowRateFrame)
				{
					var rateList =
						db
							.Rates(r => r.MessageID == mid)
							.OrderByDescending(r => r.Date)
							.Select(
								r =>
									new
										{
											r.RateType,
											r.Multiplier,
											r.Message.ServerForum.InTop,
											r.UserID,
											r.User.UserClass,
											DisplayName = r.User.DisplayName()
										});
					foreach (var rate in rateList)
					{
						var rateItem = new XmlMessage.MessageRateItem();
						var rateString = string.Empty;
						var intType = (int)rate.RateType;
						if (intType > 0)
							rateString = (rate.Multiplier*intType).ToString();

						rateItem.Value = rateString;
						rateItem.ForumInTop = rate.InTop;
						rateItem.Type = (int)rate.RateType;

						rateItem.Author.ID = rate.UserID;
						rateItem.Author.UserClass = (int)rate.UserClass;
						rateItem.Author.DisplayName = rate.DisplayName;

						xmlMessage.Rate.List.Add(rateItem);
					}
				}
				return xmlMessage;
			}
		}
	}
}