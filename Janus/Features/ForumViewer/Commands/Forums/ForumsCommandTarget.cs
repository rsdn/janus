using System;
using System.Linq;
using System.Windows.Forms;

using Rsdn.Janus.Log;
using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Обработчик команд форумов.
	/// </summary>
	[CommandTarget]
	internal sealed class ForumsCommandTarget : CommandTarget
	{
		public ForumsCommandTarget(IServiceProvider serviceProvider)
			: base(serviceProvider) { }

		[CommandExecutor("Janus.Forum.OpenFileUploadOnRsdn")]
		public void ExecuteOpenFileUploadOnRsdn(ICommandContext context)
		{
			context.OpenUrlInBrowser(SiteUrlHelper.GetFileUploadUrl());
		}


		[CommandExecutor("Janus.Forum.SetMessagesReadMarkByDate")]
		public void ExecuteSetMessagesReadMarkByDate(ICommandContext context)
		{
			using (var mmrf = new MarkMsgReadForm(context))
			{
				var owner = context
					.GetRequiredService<IUIShell>()
					.GetMainWindowParent();

				if (mmrf.ShowDialog(owner) != DialogResult.OK)
					return;

				var msgText = SR.Forum.MarkMessageCaption;
				var resText = string.Empty;

				// Заносим данные в замыкание, так как после диспоза формы часть
				// данный теряется.
				var forumIds =
					mmrf.MarkAllForums
						? Enumerable.Empty<int>()
						: mmrf.ForumsIdsForMark;
				ProgressWorker.Run(context, false,
					pi =>
					{
						pi.SetProgressText(msgText);

						context.LogInfo(msgText);

						var markCount =
							ForumHelper.MarkMessagesByDate(
								context,
								forumIds,
								mmrf.MarkAsRead,
								mmrf.BeforeDate,
								mmrf.AfterDate,
								mmrf.ExceptAnswersMe);

						resText = SR.Forum.MarkMessageResult.FormatStr(
							markCount,
							markCount.GetDeclension(
								SR.Forum.Message1,
								SR.Forum.Message2,
								SR.Forum.Message5));

						context.LogInfo(resText);
					},
					() => MessageBox.Show(
						owner,
						resText,
						ApplicationInfo.ApplicationName,
						MessageBoxButtons.OK,
						MessageBoxIcon.Information));
			}
		}

		[CommandStatusGetter("Janus.Forum.SetMessagesReadMarkByDate")]
		public CommandStatus QuerySetMessagesReadMarkByDateStatus(ICommandContext context)
		{
			return context.GetService<IMessageMarkService>() != null
				? CommandStatus.Normal
				: CommandStatus.Unavailable;
		}


		[CommandExecutor("Janus.Forum.ShowAllArticles")]
		public void ExecuteShowAllArticles(ICommandContext context)
		{
			context.OpenUrlInBrowser(
				JanusProtocolDispatcher.FormatURI(
					JanusProtocolResourceType.ArticleList, "0"));
		}

		[CommandExecutor("Janus.Forum.ShowAllFaq")]
		public void ExecuteShowAllFaq(ICommandContext context)
		{
			context.OpenUrlInBrowser(
				JanusProtocolDispatcher.FormatURI(
					JanusProtocolResourceType.FaqList, "0"));
		}

		[CommandExecutor("Janus.Forum.ShowTeamList")]
		public void ExecuteShowTeamList(ICommandContext context)
		{
			context.OpenUrlInBrowser(
				JanusProtocolDispatcher.FormatURI(
					JanusProtocolResourceType.TeamList, string.Empty));
		}

		[CommandExecutor("Janus.Forum.TopicDownload")]
		public void ExecuteTopicDownload(ICommandContext context)
		{
			using (var etf = new EnterTopicMessageIdForm())
				if (etf.ShowDialog(context
						.GetRequiredService<IUIShell>()
						.GetMainWindowParent()) == DialogResult.OK)
					context
						.GetRequiredService<IOutboxManager>()
						.AddTopicForDownload(etf.MessageId);
		}

		[CommandExecutor("Janus.Forum.ShowUserRatingIn")]
		public void ExecuteShowUserRatingIn(ICommandContext context)
		{
			var uid = Config.Instance.SelfId;

			context.OpenUrlInBrowser(
				JanusProtocolDispatcher.FormatURI(
					JanusProtocolResourceType.UserRating, uid.ToString()));
		}

		[CommandExecutor("Janus.Forum.ShowUserRatingOut")]
		public void ExecuteShowUserRatingOut(ICommandContext context)
		{
			var uid = Config.Instance.SelfId;

			context.OpenUrlInBrowser(
				JanusProtocolDispatcher.FormatURI(
					JanusProtocolResourceType.UserOutrating, uid.ToString()));
		}

		[CommandExecutor("Janus.Forum.StateExport")]
		public void ExecuteStateExport(ICommandContext context)
		{
			using (var dialog = new SaveStateDialog())
			{
				if (dialog.ShowDialog(
						context
							.GetRequiredService<IUIShell>()
							.GetMainWindowParent()) != DialogResult.OK)
					return;

				ProgressWorker.Run(context, false,
					pi => new StateObject(context, dialog.FileName).SaveState(dialog.Options));
			}
		}

		[CommandExecutor("Janus.Forum.StateImport")]
		public void ExecuteStateImport(ICommandContext context)
		{
			using (var dialog = new RestoreStateDialog())
			{
				if (dialog.ShowDialog(context.GetRequiredService<IUIShell>().GetMainWindowParent()) != DialogResult.OK)
					return;

				ProgressWorker.Run(
					context,
					false,
					pi => new StateObject(context, dialog.FileName).RestoreState(dialog.Options));
			}
		}
	}
}