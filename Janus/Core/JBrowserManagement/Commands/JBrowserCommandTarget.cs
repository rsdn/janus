using System;
using System.Diagnostics;

using CodeJam;
using CodeJam.Extensibility;

namespace Rsdn.Janus
{
	/// <summary>
	/// Обработчик команд окна браузера.
	/// </summary>
	[CommandTarget]
	internal sealed class JBrowserCommandTarget : CommandTarget
	{
		private const string _commandNamePrefix = "Janus.JBrowser.";

		public JBrowserCommandTarget(IServiceProvider serviceProvider)
			: base(serviceProvider) { }

		#region Nav commands
		private const string _navBackCommand = _commandNamePrefix + "NavigateBackward";

		[CommandExecutor(_navBackCommand)]
		public void ExecuteNavigateBack(ICommandContext context)
		{
			context.GetRequiredService<IBrowserFormService>().NavigateBackward();
		}

		[CommandStatusGetter(_navBackCommand)]
		public CommandStatus QueryNavigateBackwardStatus(ICommandContext context)
		{
			return QueryJBrowserCommandStatus(context).DisabledIfNot(
				() => context.GetRequiredService<IBrowserFormService>().CanNavigateBackward);
		}

		private const string _navFwdCommand = _commandNamePrefix + "NavigateForward";

		[CommandExecutor(_navFwdCommand)]
		public void ExecuteNavigateForward(ICommandContext context)
		{
			context.GetRequiredService<IBrowserFormService>().NavigateForward();
		}

		[CommandStatusGetter(_navFwdCommand)]
		public CommandStatus QueryNavigateForwardStatus(ICommandContext context)
		{
			return QueryJBrowserCommandStatus(context).DisabledIfNot(
				() => context.GetRequiredService<IBrowserFormService>().CanNavigateForward);
		}

		[CommandStatusSubscriber(_navBackCommand)]
		[CommandStatusSubscriber(_navFwdCommand)]
		public IDisposable SubscribeNavigationStatusChanged(
			IServiceProvider serviceProvider, Action handler)
		{
			var browserFormSvc = serviceProvider.GetService<IBrowserFormService>();
			if (browserFormSvc != null)
			{
				EventHandler statusUpdater = (sender, e) => handler();
				browserFormSvc.CanNavigateBackwardChanged += statusUpdater;
				browserFormSvc.CanNavigateForwardChanged += statusUpdater;
				return Disposable.Create(
					() =>
					{
						browserFormSvc.CanNavigateBackwardChanged -= statusUpdater;
						browserFormSvc.CanNavigateForwardChanged -= statusUpdater;
					});
			}
			return Disposable.Empty;
		}
		#endregion

		[CommandExecutor("Janus.JBrowser.Refresh")]
		public void ExecuteRefresh(ICommandContext context)
		{
			context.GetRequiredService<IBrowserFormService>().Refresh();
		}

		private const string _stopCommand = _commandNamePrefix + "Stop";

		[CommandExecutor(_stopCommand)]
		public void ExecuteStop(ICommandContext context)
		{
			context.GetRequiredService<IBrowserFormService>().Stop();
		}

		[CommandStatusGetter(_stopCommand)]
		public CommandStatus QueryStopStatus(ICommandContext context)
		{
			var svc = context.GetService<IBrowserFormService>();
			return
				svc != null
					? (svc.CanStop
						? CommandStatus.Normal
						: CommandStatus.Disabled)
					: CommandStatus.Unavailable;
		}

		[CommandStatusSubscriber(_stopCommand)]
		public IDisposable SubscribeStopStatusChanged(
			IServiceProvider serviceProvider, Action handler)
		{
			var svc = serviceProvider.GetService<IBrowserFormService>();
			if (svc == null)
				return Disposable.Empty;
			EventHandler evtHandler = (sender, args) => handler();
			svc.Navigated += evtHandler;
			svc.DocumentCompleted += evtHandler;
			return Disposable.Create(
				() =>
				{
					svc.Navigated -= evtHandler;
					svc.DocumentCompleted -= evtHandler;
				});
		}

		[CommandExecutor("Janus.JBrowser.OpenInExternalBrowser")]
		public void ExecuteOpenInExternalBrowser(ICommandContext context)
		{
			Process.Start(context.GetRequiredService<IBrowserFormService>().Url);
		}

		[CommandExecutor("Janus.JBrowser.Close")]
		public void ExecuteClose(ICommandContext context)
		{
			context.GetRequiredService<IBrowserFormService>().Close();
		}

		[CommandStatusGetter("Janus.JBrowser.Refresh")]
		[CommandStatusGetter("Janus.JBrowser.OpenInExternalBrowser")]
		[CommandStatusGetter("Janus.JBrowser.Close")]
		public CommandStatus QueryJBrowserCommandStatus(ICommandContext context)
		{
			return context.GetService<IBrowserFormService>() != null
				? CommandStatus.Normal
				: CommandStatus.Unavailable;
		}
	}
}