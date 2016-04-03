using System;
using System.IO;
using System.Windows.Forms;

using CodeJam.Extensibility;

using Rsdn.Janus.ObjectModel;

namespace Rsdn.Janus
{
	/// <summary>
	/// Обработчик команд приложения.
	/// </summary>
	[CommandTarget]
	internal sealed class ApplicationCommandTarget : CommandTarget
	{
		public ApplicationCommandTarget(IServiceProvider serviceProvider)
			: base(serviceProvider) { }

		[CommandExecutor("Janus.Application.Exit")]
		public void ExecuteExit(ICommandContext context)
		{
			Application.Exit();
		}

		[CommandExecutor("Janus.Application.About")]
		public void ExecuteShowAbout(ICommandContext context)
		{
			using (var aboutJanusForm = new AboutJanusForm(context))
				aboutJanusForm.ShowDialog(
					context.GetRequiredService<IUIShell>().GetMainWindowParent());
		}

		[CommandExecutor("Janus.Application.Help")]
		public void ExecuteShowHelp(ICommandContext context)
		{
			var helpFilePath = Path.Combine(
				EnvironmentHelper.GetJanusRootDir(), "JanusDoc.chm");

			if (File.Exists(helpFilePath))
				Help.ShowHelp(new Control(), helpFilePath);
			//ToDo: если файла нет, то нужно выдавать предупреждение
		}

		[CommandExecutor("Janus.Application.SendBugReport")]
		public void ExecuteSendBugReport(ICommandContext context)
		{
			context
				.GetRequiredService<IOutboxManager>()
				.AddBugReport(
					SR.MainForm.BugReport.DefaultName,
					SR.MainForm.BugReport.DefaultDescription,
					SR.MainForm.BugReport.DefaultStackTrace,
					true);
		}

		[CommandExecutor("Janus.Application.UserOptions")]
		public void ExecuteShowUserOptions(ICommandContext context)
		{
			var orgdbp = LocalUser.DatabasePath;

			using (var ouf = new OptionsUserForm(context, false))
			{
				if (ouf.ShowDialog(
						context
							.GetRequiredService<IUIShell>()
							.GetMainWindowParent()) != DialogResult.OK)
					return;
				if (orgdbp == LocalUser.DatabasePath)
					return;
				Forums.Instance.Refresh();
				context.GetRequiredService<IMainWindowService>().UpdateText();
			}
		}

		[CommandExecutor("Janus.Application.AppOptions")]
		public void ExecuteShowAppOptions(ICommandContext context)
		{
			var oldValue = Config.Instance.ForumDisplayConfig.ShowUnreadThreadsOnly;
			using (var of = new OptionsForm())
			{
				var owner = context.GetRequiredService<IUIShell>().GetMainWindowParent();

				var res = of.ShowDialog(owner);

				if ((of.ActionType & ChangeActionType.Refresh) == ChangeActionType.Refresh
						&& res == DialogResult.OK)
				{
					if (oldValue != Config.Instance.ForumDisplayConfig.ShowUnreadThreadsOnly)
						Forums.Instance.Refresh();

					Features.Instance.ConfigChanged();

					if (Config.Instance.TickerConfig.ShowTicker)
						Ticker.ShowTicker(context);
					else
						Ticker.HideTicker();

					context.GetRequiredService<IMainWindowService>().Refresh();
				}

				if ((of.ActionType & ChangeActionType.Restart) == ChangeActionType.Restart
						&& res == DialogResult.OK)
					MessageBox.Show(
						owner,
						SR.MainForm.AppNeedRestart,
						ApplicationInfo.ApplicationName,
						MessageBoxButtons.OK,
						MessageBoxIcon.Error);
			}
		}

		[CommandExecutor("Janus.Application.ShowMainForm")]
		public void ExecuteRestoreMainForm(ICommandContext context)
		{
			context.GetRequiredService<IMainWindowService>().EnsureVisible();
		}

		[CommandStatusGetter("Janus.Application.ShowMainForm")]
		public CommandStatus QueryShowMainFormStatus(ICommandContext context)
		{
			return context.GetService<IMainWindowService>() != null 
				? CommandStatus.Normal
				: CommandStatus.Unavailable;
		}
	}
}