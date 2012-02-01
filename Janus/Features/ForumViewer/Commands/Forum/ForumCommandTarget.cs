using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Windows.Forms;
using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Обработчик команд форума.
	/// </summary>
	[CommandTarget]
	internal sealed class ForumCommandTarget : CommandTarget
	{
		public ForumCommandTarget(IServiceProvider serviceProvider)
			: base(serviceProvider) { }

		[CommandExecutor("Janus.Forum.OpenForumOnRsdn")]
		public void ExecuteOpenForumOnRsdn(
			ICommandContext context, int? forumId)
		{
			context.OpenUrlInBrowser(
				SiteUrlHelper.GetForumUrl(GetForum(context, forumId).Name));
		}

		[CommandExecutor("Janus.Forum.ShowForumArticles")]
		public void ExecuteShowForumArticles(ICommandContext context, int? forumId)
		{
			context.OpenUrlInBrowser(
				JanusProtocolDispatcher.FormatURI(
					JanusProtocolResourceType.ArticleList,
					GetForumId(context, forumId).ToString()));
		}

		[CommandExecutor("Janus.Forum.ShowForumFaq")]
		public void ExecuteShowForumFaq(
			ICommandContext context, int? forumId)
		{
			context.OpenUrlInBrowser(
				JanusProtocolDispatcher.FormatURI(
					JanusProtocolResourceType.FaqList,
					GetForumId(context, forumId).ToString())
				);
		}

		[CommandExecutor("Janus.Forum.WriteMessage")]
		public void ExecuteWriteMessage(
			ICommandContext context, int? forumId)
		{
			MessageEditor.EditMessage(
				context,
				MessageFormMode.Add,
				new MessageInfo(GetForumId(context, forumId)));
		}

		[CommandExecutor("Janus.Forum.SetAllMessagesInForumReadMark")]
		public void ExecuteSetAllMessagesInForumReadMark(
			ICommandContext context, int[] forumIds, bool isRead)
		{
			if (Config.Instance.ConfirmationConfig.ConfirmMarkAll
					&& MessageBox.Show(
						context.GetRequiredService<IUIShell>().GetMainWindowParent(),
						SR.Forum.PromptMarkAll.FormatStr(
							isRead ? SR.Forum.PromptRead : SR.Forum.PromptUnread),
						ApplicationInfo.ApplicationName,
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Question,
						MessageBoxDefaultButton.Button2) != DialogResult.Yes)
				return;

			ProgressWorker.Run(
				context,
				false,
				info => ForumHelper.SetForumsReadMark(context, GetForums(context, forumIds), isRead));
		}

		[CommandStatusGetter("Janus.Forum.SetAllMessagesInForumReadMark")]
		public CommandStatus QuerySetAllMessagesInForumReadMarkStatus(
			ICommandContext context, int[] forumIds, bool isRead)
		{
			return QueryForumsCommandStatus(context, forumIds)
				.UnavailableIfNot(() => context.GetService<IMessageMarkService>() != null)
				.DisabledIfNot(
					() => GetForums(context, forumIds).Any(forum => forum.CanSetForumReadMark(isRead)));
		}

		private static CommandStatus QueryForumsCommandStatus(ICommandContext context, int[] forumIds)
		{
			if (forumIds != null)
				return CommandStatus.Normal;

			var activeForumSvc = context.GetService<IActiveForumService>();
			return activeForumSvc != null && activeForumSvc.ActiveForum != null
				? CommandStatus.Normal
				: CommandStatus.Unavailable;
		}

		[CommandStatusSubscriber("Janus.Forum.SetAllMessagesInForumReadMark")]
		public IDisposable SubscribeSetAllMessagesInForumReadMarkStatusChanged(
			IServiceProvider provider, Action handler)
		{
			return provider
				.GetRequiredService<IEventBroker>()
				.Subscribe(
					ForumEventNames.AfterForumEntryChanged,
					Observer.Create<ForumEntryChangedEventArgs>(
						args =>
						{
							if ((args.ChangeType & ForumEntryChangeType.ReadMark) != 0)
								handler();
						}));
		}


		[CommandExecutor("Janus.Forum.MessageExport")]
		public void ExecuteSetAllMessagesInForumReadMark(ICommandContext context)
		{
			MessageExporter.Export(context);
		}

		[CommandStatusGetter("Janus.Forum.MessageExport")]
		public CommandStatus QueryMessageExportStatus(ICommandContext context)
		{
			return ForumCommandHelper.GetMultipleMessagesCommandStatus(context, null)
				.Or(QueryForumCommandStatus(context, null));
		}

		[CommandStatusSubscriber("Janus.Forum.MessageExport")]
		public IDisposable SubscribeMessageExportStatusChanged(
			IServiceProvider serviceProvider, Action handler)
		{
			return ForumCommandHelper.SubscribeMessageCommandStatusChanged(serviceProvider, handler);
		}


		[CommandStatusGetter("Janus.Forum.OpenForumOnRsdn")]
		[CommandStatusGetter("Janus.Forum.ShowForumArticles")]
		[CommandStatusGetter("Janus.Forum.ShowForumFaq")]
		[CommandStatusGetter("Janus.Forum.WriteMessage")]
		public CommandStatus QueryForumCommandStatus(
			ICommandContext context, int? forumId)
		{
			if (forumId != null)
				return CommandStatus.Normal;

			var activeForumSvc = context.GetService<IActiveForumService>();
			return activeForumSvc != null && activeForumSvc.ActiveForum != null
				? CommandStatus.Normal
				: CommandStatus.Unavailable;
		}

		[CommandStatusSubscriber("Janus.Forum.OpenForumOnRsdn")]
		[CommandStatusSubscriber("Janus.Forum.ShowForumArticles")]
		[CommandStatusSubscriber("Janus.Forum.ShowForumFaq")]
		[CommandStatusSubscriber("Janus.Forum.WriteMessage")]
		[CommandStatusSubscriber("Janus.Forum.SetAllMessagesInForumReadMark")]
		[CommandStatusSubscriber("Janus.Forum.MessageExport")]
		public IDisposable SubscribeForumCommandStatusChanged(
			IServiceProvider provider, Action handler)
		{
			var activeForumSvc = provider.GetService<IActiveForumService>();
			if (activeForumSvc != null)
			{
				SmartApp.EventHandler<IActiveForumService> statusUpdater = sender => handler();
				activeForumSvc.ActiveForumChanged += statusUpdater;
				return Disposable.Create(
					() => activeForumSvc.ActiveForumChanged -= statusUpdater);
			}
			return Disposable.Empty;
		}

		private static IForum GetForum(IServiceProvider provider, int? forumId)
		{
			return forumId != null
				? Forums.Instance[forumId.Value]
				: provider
					.GetRequiredService<IActiveForumService>()
					.ActiveForum;
		}

		private static IEnumerable<IForum> GetForums(IServiceProvider provider, int[] forumIds)
		{
			return forumIds != null
				? (IEnumerable<IForum>)forumIds.Select(id => Forums.Instance[id])
				: new[] { provider.GetRequiredService<IActiveForumService>().ActiveForum };
		}

		private static int GetForumId(IServiceProvider provider, int? forumId)
		{
			if (forumId != null)
				return forumId.Value;

			var currentForum =
				provider
					.GetRequiredService<IActiveForumService>()
					.ActiveForum;
			return currentForum != null ? currentForum.ID : -1;
		}
	}
}