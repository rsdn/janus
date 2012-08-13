using System;

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

		[CommandExecutor("Janus.Forum.OpenModeratingOnRsdn")]
		public void ExecuteOpenModeratingOnRsdn(ICommandContext context, int? messageId)
		{
			context.OpenUrlInBrowser(
				RsdnUrlHelper.GetSelfModerateUrl(
					ForumCommandHelper.GetMessageId(context, messageId)));
		}

		[CommandExecutor("Janus.Forum.WarnModeratorOnRsdn")]
		public void ExecuteWarnModeratorOnRsdn(ICommandContext context, int? messageId)
		{
			context.OpenUrlInBrowser(
				RsdnUrlHelper.GetWarnModeratorUrl(
					ForumCommandHelper.GetMessageId(context, messageId)));
		}


		[CommandStatusGetter("Janus.Forum.OpenModeratingOnRsdn")]
		[CommandStatusGetter("Janus.Forum.WarnModeratorOnRsdn")]
		public CommandStatus QueryMessageCommandStatus(ICommandContext context, int? messageId)
		{
			return ForumCommandHelper.GetSingleMessageCommandStatus(context, messageId);
		}

		[CommandStatusSubscriber("Janus.Forum.OpenModeratingOnRsdn")]
		[CommandStatusSubscriber("Janus.Forum.WarnModeratorOnRsdn")]
		public IDisposable SubscribeMessageCommandStatusChanged(
			IServiceProvider serviceProvider, Action handler)
		{
			return ForumCommandHelper.SubscribeMessageCommandStatusChanged(serviceProvider, handler);
		}
	}
}