using System;

using Rsdn.SmartApp;

namespace Rsdn.Janus.Admin.Commands
{
	[CommandTarget]
	public class AdminCommandTarget : CommandTarget
	{
		private const string s_ModUrlTemplate = "http://rsdn.ru/Admin/ModerateMessage/{0}";

		public AdminCommandTarget(IServiceProvider serviceProvider) : base(serviceProvider)
		{}

		[CommandExecutor("Janus.Rsdn.Admin.ModerateMessage")]
		public void ExecuteOpenMessageInJBrowser(ICommandContext context, int? messageId)
		{
			context.OpenUrlInBrowser(
				s_ModUrlTemplate.FormatStr(
					ForumCommandHelper.GetMessageId(context, messageId)));
		}

		[CommandStatusGetter("Janus.Rsdn.Admin.ModerateMessage")]
		public CommandStatus QueryMessageCommandStatus(ICommandContext context, int? messageId)
		{
			return ForumCommandHelper.GetSingleMessageCommandStatus(context, messageId);
		}

		[CommandStatusSubscriber("Janus.Rsdn.Admin.ModerateMessage")]
		public IDisposable SubscribeMessageCommandStatusChanged(IServiceProvider serviceProvider, Action handler)
		{
			return ForumCommandHelper.SubscribeMessageCommandStatusChanged(serviceProvider, handler);
		}
	}
}