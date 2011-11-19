using System;
using System.Linq;
using System.Reactive;
using System.Windows.Forms;

using Rsdn.Framework.Formatting;
using Rsdn.Janus.Framework;
using Rsdn.Janus.ObjectModel;
using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Обработчик команд сообщений форума.
	/// </summary>
	[CommandTarget]
	internal sealed class ForumMessageCommandTarget : CommandTarget
	{
		public ForumMessageCommandTarget(IServiceProvider serviceProvider)
			: base(serviceProvider) { }

		[CommandExecutor("Janus.Forum.AddForumMessageToFavorites")]
		public void ExecuteAddForumMessageToFavorites(
			ICommandContext context, int? messageId)
		{
			var manager = context.GetRequiredService<IFavoritesManager>();
			var activeMsg = ForumMessageCommandHelper.GetMessage(context, messageId);

			using (var selForm =
				new FavoritesSelectFolderForm(
					context,
					manager.RootFolder,
					true))
			{
				selForm.Comment = activeMsg.Subject;

				var windowParent = context.GetRequiredService<IUIShell>().GetMainWindowParent();

				if (selForm.ShowDialog(windowParent) == DialogResult.OK)
				{
					var folder = selForm.SelectedFolder ?? manager.RootFolder;
					if (!manager.AddMessageLink(activeMsg.ID, selForm.Comment, folder))
						//сообщения уже есть в разделе
						MessageBox.Show(
							windowParent,
							SR.Favorites.ItemExists.FormatStr(
								activeMsg.ID, folder.Name),
							ApplicationInfo.ApplicationName,
							MessageBoxButtons.OK,
							MessageBoxIcon.Information);
				}
			}
		}

		[CommandExecutor("Janus.Forum.CopyMessageAddress")]
		public void ExecuteCopyMessageAddress(ICommandContext context, int? messageId)
		{
			var msg = ForumMessageCommandHelper.GetMessage(context, messageId);
			ClipboardHelper.CopyUrl(SiteUrlHelper.GetMessageUrl(msg.ID), msg.Subject);
		}

		[CommandExecutor("Janus.Forum.CopyMessageAuthorAddress")]
		public void ExecuteCopyMessageAuthorAddress(ICommandContext context, int? messageId)
		{
			var msg = ForumMessageCommandHelper.GetMessage(context, messageId);
			ClipboardHelper.CopyUrl(SiteUrlHelper.GetUserProfileUrl(msg.UserID), msg.UserNick);
		}

		[CommandExecutor("Janus.Forum.OpenMessageInJBrowser")]
		public void ExecuteOpenMessageInJBrowser(ICommandContext context, int? messageId)
		{
			var url = JanusProtocolDispatcher.FormatURI(
				JanusProtocolResourceType.Message,
				ForumMessageCommandHelper.GetMessageId(context, messageId).ToString());

			context.OpenUrlInBrowser(url, UrlBehavior.InternalBrowser);
		}

		[CommandExecutor("Janus.Forum.OpenMessageOnRsdn")]
		public void ExecuteOpenMessageOnRsdn(ICommandContext context, int? messageId)
		{
			context.OpenUrlInBrowser(
				SiteUrlHelper.GetMessageUrl(
					ForumMessageCommandHelper.GetMessageId(context, messageId)));
		}

		[CommandExecutor("Janus.Forum.OpenMessageRatingOnRsdn")]
		public void ExecuteOpenMessageRatingOnRsdn(ICommandContext context, int? messageId)
		{
			context.OpenUrlInBrowser(
				SiteUrlHelper.GetRatingUrl(
					ForumMessageCommandHelper.GetMessageId(context, messageId)));
		}


		[CommandExecutor("Janus.Forum.Moderating")]
		public void ExecuteModerating(ICommandContext context, int? messageId)
		{
			using (var frm = new ModeratingForm(
					context,
					ForumMessageCommandHelper.GetMessageId(context, messageId)))
				frm.ShowDialog(context.GetRequiredService<IUIShell>().GetMainWindowParent());
		}

		[CommandStatusGetter("Janus.Forum.Moderating")]
		public CommandStatus QueryModeratingStatus(ICommandContext context, int? messageId)
		{
			return
				QueryMessageCommandStatus(context, messageId)
					.DisabledIfNot(
						() =>
						{
							var id = ForumMessageCommandHelper.GetMessageId(context, messageId);
							using (var mgr = context.CreateDBContext())
								return mgr.Moderatorials().Any(m => m.MessageID == id);
						});
		}

		[CommandExecutor("Janus.Forum.OpenModeratingOnRsdn")]
		public void ExecuteOpenModeratingOnRsdn(ICommandContext context, int? messageId)
		{
			context.OpenUrlInBrowser(
				SiteUrlHelper.GetSelfModerateUrl(
					ForumMessageCommandHelper.GetMessageId(context, messageId)));
		}

		[CommandExecutor("Janus.Forum.ShowMessageRating")]
		public void ExecuteShowMessageRating(ICommandContext context, int? messageId)
		{
			context.OpenUrlInBrowser(
				SiteUrlHelper.GetRatingUrl(
					ForumMessageCommandHelper.GetMessageId(context, messageId)));
		}


		[CommandExecutor("Janus.Forum.ReplyMessage")]
		public void ExecuteReplyMessage(ICommandContext context, int? messageId)
		{
			var msg = ForumMessageCommandHelper.GetMessage(context, messageId);

			var messageInfo = new MessageInfo(
				msg.ForumID,
				msg.ID,
				msg.Subject,
				Format.Forum.GetEditMessage(
					DatabaseManager.GetMessageBody(context, msg.ID),
					msg.UserNick));

			MessageEditor.EditMessage(MessageFormMode.Reply, messageInfo);
		}

		[CommandStatusGetter("Janus.Forum.ReplyMessage")]
		public CommandStatus QueryReplyMessageStatus(ICommandContext context, int? messageId)
		{
			return QueryMessageCommandStatus(context, messageId).DisabledIfNot(
				() => !ForumMessageCommandHelper.GetMessage(context, messageId).Closed);
		}


		[CommandStatusGetter("Janus.Forum.AddForumMessageToFavorites")]
		[CommandStatusGetter("Janus.Forum.CopyMessageAddress")]
		[CommandStatusGetter("Janus.Forum.CopyMessageAuthorAddress")]
		[CommandStatusGetter("Janus.Forum.OpenMessageInJBrowser")]
		[CommandStatusGetter("Janus.Forum.OpenMessageOnRsdn")]
		[CommandStatusGetter("Janus.Forum.OpenMessageRatingOnRsdn")]
		[CommandStatusGetter("Janus.Forum.OpenModeratingOnRsdn")]
		[CommandStatusGetter("Janus.Forum.ShowMessageRating")]
		public CommandStatus QueryMessageCommandStatus(ICommandContext context, int? messageId)
		{
			return ForumMessageCommandHelper.GetSingleMessageCommandStatus(context, messageId);
		}


		[CommandExecutor("Janus.Forum.RateMessage")]
		public void ExecuteRateMessage(ICommandContext context, int[] messageIds, MessageRates rate)
		{
			using (var rateForm = new RateForm(context, rate))
				if (rateForm.ShowDialog(
						context
							.GetRequiredService<IUIShell>()
							.GetMainWindowParent()) == DialogResult.Yes)
					ForumMessageCommandHelper
						.GetMessageIds(context, messageIds)
						.ForEach(
							msgId =>
								context
									.GetRequiredService<IOutboxManager>()
									.RateMarks
									.Add(msgId, rate));
		}

		[CommandStatusGetter("Janus.Forum.RateMessage")]
		public CommandStatus QueryRateMessageStatus(ICommandContext context, int[] messageIds)
		{
			return QueryMessagesCommandStatus(context, messageIds).DisabledIfNot(
				() => !ForumMessageCommandHelper.GetMessages(context, messageIds)
					.Any(msg => msg.UserID == Config.Instance.SelfId));
		}


		[CommandExecutor("Janus.Forum.RegetTopic")]
		public void ExecuteRegetTopic(ICommandContext context, int[] messageIds)
		{
			foreach (var msg in ForumMessageCommandHelper.GetMessages(context, messageIds))
				context
					.GetOutboxManager()
					.DownloadTopics
					.Add(
						SR.Forum.DownloadTopicRegetSource,
						msg.ID,
						msg.Subject);
		}

		[CommandStatusGetter("Janus.Forum.RegetTopic")]
		public CommandStatus QueryMessagesCommandStatus(ICommandContext context, int[] messageIds)
		{
			return ForumMessageCommandHelper.GetMultipleMessagesCommandStatus(context, messageIds);
		}


		[CommandExecutor("Janus.Forum.SetTopicReadMark")]
		public void ExecuteSetTopicReadMark(ICommandContext context, int[] messageIds, bool isRead)
		{
			if (Features.Instance.ActiveFeature is Forum)
				ForumDummyForm.Instance.StopMarkTimer();

			ForumHelper.MarkMsgRead(
				context,
				ForumMessageCommandHelper
					.GetMessages(context, messageIds)
					.Select(msg => msg.Topic)
					.Cast<MsgBase>(),
				isRead,
				true);
		}

		[CommandStatusGetter("Janus.Forum.SetTopicReadMark")]
		public CommandStatus QuerySetTopicReadMarkStatus(
			ICommandContext context, int[] messageIds, bool isRead)
		{
			return QueryMessagesCommandStatus(context, messageIds).DisabledIfNot(
				() => ForumMessageCommandHelper.GetMessages(context, messageIds)
					.Select(msg => msg.Topic)
					.Any(msg => msg.CanSetMessageReadMark(isRead, true)));
		}


		[CommandExecutor("Janus.Forum.SetMessagesReadMark")]
		public void ExecuteSetMessageReadMark(
			ICommandContext context, int[] messageIds, bool isRead, bool markChilds)
		{
			if (Features.Instance.ActiveFeature is Forum)
				ForumDummyForm.Instance.StopMarkTimer();

			ForumHelper.MarkMsgRead(
			   context,
			   ForumMessageCommandHelper.GetMessages(context, messageIds).Cast<MsgBase>(),
			   isRead,
			   markChilds);
		}

		[CommandStatusGetter("Janus.Forum.SetMessagesReadMark")]
		public CommandStatus QuerySetMessagesReadMarkStatus(
			ICommandContext context, int[] messageIds, bool isRead, bool markChilds)
		{
			return QueryMessagesCommandStatus(context, messageIds).DisabledIfNot(
				() => ForumMessageCommandHelper.GetMessages(context, messageIds)
					.Any(msg => msg.CanSetMessageReadMark(isRead, markChilds)));
		}


		[CommandStatusSubscriber("Janus.Forum.SetMessagesReadMark")]
		[CommandStatusSubscriber("Janus.Forum.SetTopicReadMark")]
		public IDisposable SubscribeReadMarkCommandStatusChanged(
			IServiceProvider provider, Action handler)
		{
			return provider.GetRequiredService<IEventBroker>().Subscribe(
				ForumEventNames.AfterForumEntryChanged,
				Observer.Create<ForumEntryChangedEventArgs>(
					args =>
					{
						if ((args.ChangeType & ForumEntryChangeType.ReadMark) != 0)
							handler();
					}));
		}


		[CommandExecutor("Janus.Forum.SetMessagesRepliesAutoReadMark")]
		public void ExecuteSetMessagesRepliesAutoReadMark(
			ICommandContext context, int[] messageIds, bool isEnabled)
		{
			ForumHelper.SetMessageRepliesAutoReadMark(
				context,
				ForumMessageCommandHelper.GetMessages(context, messageIds).Cast<MsgBase>(),
				isEnabled);
		}

		[CommandStatusGetter("Janus.Forum.SetMessagesRepliesAutoReadMark")]
		public CommandStatus QuerySetMessagesRepliesAutoReadMarkStatus(
			ICommandContext context, int[] messageIds, bool isEnabled)
		{
			return QueryMessagesCommandStatus(context, messageIds).DisabledIfNot(
				() => ForumMessageCommandHelper
					.GetMessages(context, messageIds)
					.Any(msg => msg.ReadReplies != isEnabled));
		}

		[CommandStatusSubscriber("Janus.Forum.SetMessagesRepliesAutoReadMark")]
		public IDisposable SubscribeAutoReadMarkCommandStatusChanged(
			IServiceProvider provider, Action handler)
		{
			return provider.GetRequiredService<IEventBroker>().Subscribe(
				ForumEventNames.AfterForumEntryChanged,
				Observer.Create<ForumEntryChangedEventArgs>(
					args =>
					{
						if ((args.ChangeType & ForumEntryChangeType.AutoReadMark) != 0)
							handler();
					}));
		}


		[CommandStatusSubscriber("Janus.Forum.AddForumMessageToFavorites")]
		[CommandStatusSubscriber("Janus.Forum.CopyMessageAddress")]
		[CommandStatusSubscriber("Janus.Forum.CopyMessageAuthorAddress")]
		[CommandStatusSubscriber("Janus.Forum.OpenMessageInJBrowser")]
		[CommandStatusSubscriber("Janus.Forum.OpenMessageOnRsdn")]
		[CommandStatusSubscriber("Janus.Forum.OpenMessageRatingOnRsdn")]
		[CommandStatusSubscriber("Janus.Forum.Moderating")]
		[CommandStatusSubscriber("Janus.Forum.OpenModeratingOnRsdn")]
		[CommandStatusSubscriber("Janus.Forum.ShowMessageRating")]
		[CommandStatusSubscriber("Janus.Forum.ReplyMessage")]
		[CommandStatusSubscriber("Janus.Forum.RateMessage")]
		[CommandStatusSubscriber("Janus.Forum.RegetTopic")]
		[CommandStatusSubscriber("Janus.Forum.SetTopicReadMark")]
		[CommandStatusSubscriber("Janus.Forum.SetMessagesReadMark")]
		[CommandStatusSubscriber("Janus.Forum.SetMessagesRepliesAutoReadMark")]
		public IDisposable SubscribeMessageCommandStatusChanged(
			IServiceProvider serviceProvider, Action handler)
		{
			return ForumMessageCommandHelper
				.SubscribeMessageCommandStatusChanged(serviceProvider, handler);
		}
	}
}