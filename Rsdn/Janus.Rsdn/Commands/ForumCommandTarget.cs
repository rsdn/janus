using System;
using System.Reactive.Disposables;
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

		[CommandExecutor("Janus.Forum.SubscribeOrUnsubscribeForum")]
		public void ExecuteSubscribeOrUnsubscribeForum(
			ICommandContext context, int? forumId)
		{
			forumId = GetForumId(context, forumId);
			ForumsSubscriptionHelper.UpdateForumsSubscriptions(
				context,
				new[]
				{
					new ForumSubscriptionRequest(
						forumId.Value,
						!context.GetRequiredService<IRsdnForumService>().IsForumSubscribed(forumId.Value))
				},
				true);
		}

		[CommandExecutor("Janus.Forum.Subscription")]
		public void ExecuteSubscription(ICommandContext context)
		{
			using (var sf = new SubscribeForm(context))
				sf.ShowDialog(context.GetRequiredService<IUIShell>().GetMainWindowParent());
		}

		[CommandStatusGetter("Janus.Forum.SubscribeOrUnsubscribeForum")]
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

		[CommandStatusSubscriber("Janus.Forum.SubscribeOrUnsubscribeForum")]
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

		private static int GetForumId(IServiceProvider provider, int? forumId)
		{
			if (forumId != null)
				return forumId.Value;

			var currentForum = provider
				.GetRequiredService<IActiveForumService>()
				.ActiveForum;
			return currentForum != null ? currentForum.ID : -1;
		}
	}
}