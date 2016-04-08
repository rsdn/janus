using System;
using System.Reactive.Disposables;

using CodeJam.Services;

using Rsdn.Janus.ObjectModel;

namespace Rsdn.Janus
{
	/// <summary>
	/// Обработчик команд поиска.
	/// </summary>
	[CommandTarget]
	internal sealed class SearchCommandTarget : CommandTarget
	{
		public SearchCommandTarget(IServiceProvider serviceProvider)
			: base(serviceProvider) { }

		[CommandExecutor("Janus.Search.BuildSearchIndex")]
		public void ExecuteBuildSearchIndex(ICommandContext context)
		{
			using (var bif = new BuildIndexForm(context))
				bif.ShowDialog(
					context
						.GetRequiredService<IUIShell>()
						.GetMainWindowParent());
		}

		[CommandExecutor("Janus.Search.ClearSearchResult")]
		public void ExecuteClearSearchResult(ICommandContext context)
		{
			SearchFeature.Instance.SearchForm.ClearSearchResult();
		}

		[CommandStatusGetter("Janus.Search.ClearSearchResult")]
		public CommandStatus QuerySearchFeatureCommandStatus(ICommandContext context)
		{
			return Features.Instance.ActiveFeature is SearchFeature 
				? CommandStatus.Normal 
				: CommandStatus.Unavailable;
		}

		[CommandStatusSubscriber("Janus.Search.ClearSearchResult")]
		public IDisposable SubscribeStatusChangedCore(
			IServiceProvider serviceProvider, Action handler)
		{
			Features.AfterFeatureActivateHandler statusUpdater = 
				(oldFeature, newFeature) => handler();
			Features.Instance.AfterFeatureActivate += statusUpdater;
			return Disposable.Create(() => Features.Instance.AfterFeatureActivate -= statusUpdater);
		}
	}
}