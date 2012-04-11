using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows.Forms;

using Rsdn.Janus.ObjectModel;
using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Обработчик команд просмотрщика форумов.
	/// </summary>
	[CommandTarget]
	internal sealed class ForumViewerCommandTarget : CommandTarget
	{
		public ForumViewerCommandTarget(IServiceProvider serviceProvider)
			: base(serviceProvider) { }

		[CommandExecutor("Janus.Forum.GoToMessage")]
		public void ExecuteGoToMessage(
			ICommandContext context, int? messageId)
		{
			var parentWindow = context
				.GetRequiredService<IUIShell>()
				.GetMainWindowParent();

			if (Config.Instance.ConfirmationConfig.ConfirmJump
					&& MessageBox.Show(
							parentWindow,
							SR.Search.JumpRequest,
							SR.Search.Confirmation,
							MessageBoxButtons.YesNo,
							MessageBoxIcon.Question) != DialogResult.Yes)
				return;

			if (ApplicationManager.Instance.ForumNavigator.SelectMessage(
					ForumCommandHelper.GetMessageId(context, messageId)))
			{
				var mainWindowSvc = context.GetService<IMainWindowService>();
				if (mainWindowSvc != null)
					mainWindowSvc.EnsureVisible();
			}
			else
				MessageBox.Show(
					parentWindow,
					SR.Search.NotFound,
					SR.Search.Error,
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
		}

		[CommandStatusGetter("Janus.Forum.GoToMessage")]
		public CommandStatus QueryGoToMessageCommandStatus(
			ICommandContext context, int? messageId)
		{
			return ForumCommandHelper.GetSingleMessageCommandStatus(context, messageId)
				.UnavailableIfNot(
					() => Forums.Instance.ActiveForum == null
						|| ForumMessageCommandHelper.GetMessage(context, messageId).ForumID !=
							Forums.Instance.ActiveForum.ID);
		}

		[CommandStatusSubscriber("Janus.Forum.GoToMessage")]
		public IDisposable SubscribeGoToCommandStatusChanged(
			IServiceProvider serviceProvider, Action handler)
		{
			return ForumCommandHelper.SubscribeMessageCommandStatusChanged(serviceProvider, handler);
		}

		[CommandExecutor("Janus.Forum.GoToMessageWithPrompt")]
		public void ExecuteGoToMessageWithPrompt(ICommandContext context)
		{
			var parentWindow = context.GetRequiredService<IUIShell>().GetMainWindowParent();

			int mid;
			using (var etf = new EnterTopicMessageIdForm())
				if (etf.ShowDialog(parentWindow) == DialogResult.OK)
					mid = etf.MessageId;
				else
					return;

			if (ApplicationManager.Instance.ForumNavigator.SelectMessage(mid))
			{
				var mainWindowSvc = context.GetService<IMainWindowService>();
				if (mainWindowSvc != null)
					mainWindowSvc.EnsureVisible();
			}
			else if (MessageBox.Show(
				parentWindow,
				SR.Forum.GoToMessage.NotFound.FormatStr(mid),
				SR.Search.Error,
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Exclamation,
				MessageBoxDefaultButton.Button2) == DialogResult.Yes)
				context
					.GetRequiredService<IOutboxManager>()
					.AddTopicForDownload(mid);
		}

		[CommandExecutor("Janus.Forum.SmartJump")]
		public void ExecuteSmartJump(ICommandContext context)
		{
			ForumDummyForm.Instance.SmartJump();
		}

		[CommandExecutor("Janus.Forum.CollapseTopicAndSelectRoot")]
		public void ExecuteCollapseTopicAndSelectRoot(ICommandContext context)
		{
			ForumDummyForm.Instance.CollapseAndGoRoot();
		}

		[CommandExecutor("Janus.Forum.ExpandUnreadBranches")]
		public void ExecuteExpandUnreadBranches(ICommandContext context)
		{
			ForumDummyForm.Instance.ExpandUnread();
		}

		[CommandExecutor("Janus.Forum.SelectMessageByAttribute")]
		public void ExecuteSelectMessageByAttribute(
			ICommandContext context,
			ForumDummyForm.StepDirection direction,
			ForumDummyForm.AttrType attribute,
			ForumDummyForm.SearchMessageArea searchArea)
		{
			ForumDummyForm.Instance.SelectNodeByAttribute(direction, attribute, searchArea);
		}

		[CommandExecutor("Janus.Forum.NavigateBackward")]
		public void ExecuteNavigateBackward(ICommandContext context)
		{
			ApplicationManager.Instance.ForumNavigator.ViewHistory.Back();
		}

		[CommandStatusGetter("Janus.Forum.NavigateBackward")]
		public CommandStatus QueryNavigateBackwardStatus(ICommandContext context)
		{
			return QueryForumViewerCommandStatus(context).DisabledIfNot(
				() => ApplicationManager.Instance.ForumNavigator.ViewHistory.BackPath.Any());
		}

		[CommandExecutor("Janus.Forum.NavigateForward")]
		public void ExecuteNavigateForward(ICommandContext context)
		{
			ApplicationManager.Instance.ForumNavigator.ViewHistory.Forward();
		}

		[CommandStatusGetter("Janus.Forum.NavigateForward")]
		public CommandStatus QueryNavigateForwardStatus(ICommandContext context)
		{
			return QueryForumViewerCommandStatus(context).DisabledIfNot(
				() => ApplicationManager.Instance.ForumNavigator.ViewHistory.ForwardPath.Any());
		}

		[CommandStatusSubscriber("Janus.Forum.NavigateBackward")]
		[CommandStatusSubscriber("Janus.Forum.NavigateForward")]
		public IDisposable SubscribeNavigationCommandStatusChanged(
			IServiceProvider serviceProvider, Action handler)
		{
			EventHandler statusUpdater = (sender, e) => handler();
			ApplicationManager.Instance.ForumNavigator.MessageNavigated += statusUpdater;
			return Disposable.Create(
				() => ApplicationManager.Instance.ForumNavigator.MessageNavigated -= statusUpdater);
		}

		[CommandExecutor("Janus.Forum.LoadAllMessagesInForum")]
		public void ExecuteLoadAllMessagesInForum(ICommandContext context)
		{
			if (Forums.Instance.ActiveForum != null)
				Forums.Instance.ActiveForum.LoadAllMsg();
		}

		[CommandStatusGetter("Janus.Forum.LoadAllMessagesInForum")]
		public CommandStatus QueryLoadAllMessagesInForumStatus(ICommandContext context)
		{
			return QueryForumViewerCommandStatus(context).UnavailableIfNot(
				() => Config.Instance.ForumDisplayConfig.MaxTopicsPerForum > 0);
		}

		[CommandStatusGetter("Janus.Forum.SmartJump")]
		[CommandStatusGetter("Janus.Forum.CollapseTopicAndSelectRoot")]
		[CommandStatusGetter("Janus.Forum.ExpandUnreadBranches")]
		[CommandStatusGetter("Janus.Forum.SelectMessageByAttribute")]
		public CommandStatus QueryForumViewerCommandStatus(ICommandContext context)
		{
			return Features.Instance.ActiveFeature is Forum
				? CommandStatus.Normal
				: CommandStatus.Unavailable;
		}

		[CommandStatusSubscriber("Janus.Forum.SmartJump")]
		[CommandStatusSubscriber("Janus.Forum.CollapseTopicAndSelectRoot")]
		[CommandStatusSubscriber("Janus.Forum.ExpandUnreadBranches")]
		[CommandStatusSubscriber("Janus.Forum.SelectMessageByAttribute")]
		[CommandStatusSubscriber("Janus.Forum.NavigateBackward")]
		[CommandStatusSubscriber("Janus.Forum.NavigateForward")]
		public IDisposable SubscribeForumViewerCommandStatusChanged(
			IServiceProvider serviceProvider, Action handler)
		{
			Features.AfterFeatureActivateHandler statusUpdater =
				(oldFeature, newFeature) => handler();
			Features.Instance.AfterFeatureActivate += statusUpdater;
			return Disposable.Create(
				() => Features.Instance.AfterFeatureActivate -= statusUpdater);
		}
	}
}