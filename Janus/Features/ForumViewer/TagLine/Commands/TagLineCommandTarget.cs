using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows.Forms;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Обработчик команд редактора теглайнов.
	/// </summary>
	[CommandTarget]
	internal sealed class TagLineCommandTarget : CommandTarget
	{
		public TagLineCommandTarget(IServiceProvider serviceProvider)
			: base(serviceProvider) { }

		[CommandStatusSubscriber("Janus.Forum.TagLine.Edit")]
		[CommandStatusSubscriber("Janus.Forum.TagLine.Delete")]
		public IDisposable SubscribeStatusChangedCore(
			IServiceProvider serviceProvider, Action handler)
		{
			var tagLineListFormSvc = serviceProvider.GetService<ITagLineListFormService>();
			if (tagLineListFormSvc != null)
			{
				SelectedTagLinesChangedEventHandler statusUpdater = sender => handler();
				tagLineListFormSvc.SelectedTagLinesChanged += statusUpdater;
				return Disposable.Create(() => tagLineListFormSvc.SelectedTagLinesChanged -= statusUpdater);
			}
			return Disposable.Empty;
		}

		[CommandExecutor("Janus.Forum.TagLine.Add")]
		public void ExecuteAdd(ICommandContext context)
		{
			var tagLineListFormSvc = context.GetRequiredService<ITagLineListFormService>();

			var tgi = new TagLineInfo
			{
				Name = FindFittingName(context),
				Forums = !GetExcludeHash(tagLineListFormSvc.TagLines, null)
						.Contains(TagLineInfo.AllForums)
					? new[] { TagLineInfo.AllForums }
					: new int[0]
			};

			if (EditTagLine(context, tgi))
				tagLineListFormSvc.TagLines.Add(tgi);
		}

		[CommandExecutor("Janus.Forum.TagLine.Edit")]
		public void ExecuteEdit(ICommandContext context)
		{
			EditTagLine(
				context,
				context
					.GetRequiredService<ITagLineListFormService>()
					.SelectedTagLines
					.Single());
		}

		[CommandExecutor("Janus.Forum.TagLine.Delete")]
		public void ExecuteDelete(ICommandContext context)
		{
			var tagLineListFormSvc = context.GetRequiredService<ITagLineListFormService>();
			tagLineListFormSvc.SelectedTagLines.ForEach(
				tagLine => tagLineListFormSvc.TagLines.Remove(tagLine));
		}

		[CommandStatusGetter("Janus.Forum.TagLine.Add")]
		public CommandStatus QueryAddStatus(ICommandContext context)
		{
			var activeTagLineSvc = context.GetService<ITagLineListFormService>();
			return activeTagLineSvc != null ? CommandStatus.Normal : CommandStatus.Unavailable;
		}

		[CommandStatusGetter("Janus.Forum.TagLine.Edit")]
		public CommandStatus QueryEditStatus(ICommandContext context)
		{
			var activeTagLineSvc = context.GetService<ITagLineListFormService>();
			return activeTagLineSvc != null
					&& activeTagLineSvc.SelectedTagLines.Count() == 1
				? CommandStatus.Normal
				: CommandStatus.Unavailable;
		}

		[CommandStatusGetter("Janus.Forum.TagLine.Delete")]
		public CommandStatus QueryDeleteStatus(ICommandContext context)
		{
			var activeTagLineSvc = context.GetService<ITagLineListFormService>();
			return activeTagLineSvc != null
					&& activeTagLineSvc.SelectedTagLines.Any()
				? CommandStatus.Normal
				: CommandStatus.Unavailable;
		}

		private static string FindFittingName(IServiceProvider provider)
		{
			for (var i = 0; ; i++)
			{
				var name = "TagLine" + (i == 0 ? string.Empty : i.ToString());
				var founded = false;
				foreach (var tgi in provider.GetRequiredService<ITagLineListFormService>().TagLines)
					if (tgi.Name == name)
					{
						founded = true;
						break;
					}
				if (!founded)
					return name;
			}
		}

		private static bool EditTagLine(IServiceProvider provider, TagLineInfo tgi)
		{
			using (var tlef = new TagLineEditorForm(
				provider,
				GetExcludeHash(
					provider.GetRequiredService<ITagLineListFormService>().TagLines, tgi)))
			{
				tlef.TagLineName = tgi.Name;
				tlef.TagLineFormat = tgi.Format;
				tlef.Forums = tgi.Forums;
				if (tlef.ShowDialog(
						provider
							.GetRequiredService<IUIShell>()
							.GetMainWindowParent()) == DialogResult.OK)
				{
					tgi.Name = tlef.TagLineName;
					tgi.Format = tlef.TagLineFormat;
					tgi.Forums = tlef.Forums;
					return true;
				}
			}
			return false;
		}

		private static HashSet<int> GetExcludeHash(
			IEnumerable<TagLineInfo> tagLines, TagLineInfo notExclude)
		{
			var ht = new HashSet<int>();
			foreach (var tgi in tagLines)
			{
				if (tgi == notExclude)
					continue;
				foreach (var i in tgi.Forums)
					ht.Add(i);
			}
			return ht;
		}
	}
}