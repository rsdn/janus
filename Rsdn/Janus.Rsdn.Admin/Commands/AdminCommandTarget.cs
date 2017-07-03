using System;

using CodeJam;
using CodeJam.Services;
using CodeJam.Strings;

using IServiceProvider = System.IServiceProvider;

namespace Rsdn.Janus.Admin.Commands
{
	[CommandTarget]
	public class AdminCommandTarget : CommandTarget
	{
		private readonly Func<string> _siteUrl;

		private const string _adminUrl = "{0}/Admin";
		private const string _modUrlTemplate = _adminUrl + "/ModerateMessage/{1}";
		private const string _openReasonsEditorUrl = _adminUrl + "/PenaltyReasons";
		private const string _openViolationRepsUrl = _adminUrl + "/ViolationReports";
		private const string _editUrlTemplate = "{0}/Forum/NewMsg.aspx?mid={1}&edit=1";

		public AdminCommandTarget(IServiceProvider provider) : base(provider)
		{
			_siteUrl =
				() =>
					provider
						.GetRequiredService<IWebConnectionService>()
						.GetConfig()
						.SiteUrl;
		}

		[CommandExecutor("Janus.Rsdn.Admin.ModerateMessage")]
		public void ExecuteModMessage(ICommandContext context, int? messageId)
		{
			context.OpenUrlInBrowser(
				_modUrlTemplate.FormatWith(
					_siteUrl(),
					ForumCommandHelper.GetMessageId(context, messageId)));
		}

		[CommandExecutor("Janus.Rsdn.Admin.EditMessage")]
		public void ExecuteEditMessage(ICommandContext context, int? messageId)
		{
			context.OpenUrlInBrowser(
				_editUrlTemplate.FormatWith(
					_siteUrl(),
					ForumCommandHelper.GetMessageId(context, messageId)));
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
			context.OpenUrlInBrowser(_openReasonsEditorUrl.FormatWith(_siteUrl()));
		}

		[CommandExecutor("Janus.Rsdn.Admin.OpenViolationReps")]
		public void ExecuteOpenViolationReps(ICommandContext context)
		{
			context.OpenUrlInBrowser(_openViolationRepsUrl.FormatWith(_siteUrl()));
		}
	}
}