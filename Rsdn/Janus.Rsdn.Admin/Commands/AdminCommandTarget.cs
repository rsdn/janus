using System;

using CodeJam;

using IServiceProvider = System.IServiceProvider;

namespace Rsdn.Janus.Admin.Commands
{
	[CommandTarget]
	public class AdminCommandTarget : CommandTarget
	{
		private const string _adminUrl = "http://rsdn.ru/Admin";
		private const string _modUrlTemplate = _adminUrl + "/ModerateMessage/{0}";
		private const string _openReasonsEditorUrl = _adminUrl + "/PenaltyReasons";
		private const string _openViolationRepsUrl = _adminUrl + "/ViolationReports";
		private const string _editUrlTemplate = "http://rsdn.ru/Forum/NewMsg.aspx?mid={0}&edit=1";

		public AdminCommandTarget(IServiceProvider serviceProvider) : base(serviceProvider)
		{}

		[CommandExecutor("Janus.Rsdn.Admin.ModerateMessage")]
		public void ExecuteModMessage(ICommandContext context, int? messageId)
		{
			context.OpenUrlInBrowser(_modUrlTemplate.FormatWith(ForumCommandHelper.GetMessageId(context, messageId)));
		}

		[CommandExecutor("Janus.Rsdn.Admin.EditMessage")]
		public void ExecuteEditMessage(ICommandContext context, int? messageId)
		{
			context.OpenUrlInBrowser(_editUrlTemplate.FormatWith(ForumCommandHelper.GetMessageId(context, messageId)));
		}

		[CommandStatusGetter("Janus.Rsdn.Admin.ModerateMessage")]
		[CommandStatusGetter("Janus.Rsdn.Admin.EditMessage")]
		public CommandStatus QueryMessageCommandStatus(ICommandContext context, int? messageId)
		{
			return ForumCommandHelper.GetSingleMessageCommandStatus(context, messageId);
		}

		[CommandStatusSubscriber("Janus.Rsdn.Admin.ModerateMessage")]
		[CommandStatusSubscriber("Janus.Rsdn.Admin.EditMessage")]
		public IDisposable SubscribeMessageCommandStatusChanged(IServiceProvider serviceProvider, Action handler)
		{
			return ForumCommandHelper.SubscribeMessageCommandStatusChanged(serviceProvider, handler);
		}

		[CommandExecutor("Janus.Rsdn.Admin.OpenReasonsEditor")]
		public void ExecuteOpenReasonsEditor(ICommandContext context)
		{
			context.OpenUrlInBrowser(_openReasonsEditorUrl);
		}

		[CommandExecutor("Janus.Rsdn.Admin.OpenViolationReps")]
		public void ExecuteOpenViolationReps(ICommandContext context)
		{
			context.OpenUrlInBrowser(_openViolationRepsUrl);
		}
	}
}