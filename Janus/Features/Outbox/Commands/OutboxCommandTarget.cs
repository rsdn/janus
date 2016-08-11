using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows.Forms;

using CodeJam.Extensibility.Instancing;
using CodeJam.Services;

using Rsdn.Janus.ObjectModel;
using Rsdn.TreeGrid;

namespace Rsdn.Janus
{
	/// <summary>
	/// Обработчик команд Outbox'а.
	/// </summary>
	[CommandTarget]
	internal sealed class OutboxCommandTarget : CommandTarget
	{
		public OutboxCommandTarget(IServiceProvider provider)
			: base(provider) { }

		[CommandExecutor("Janus.Outbox.EditItem")]
		public void ExecuteEditItem(ICommandContext context)
		{
			var editors =
				CreateEditor(
					context,
					context
						.GetRequiredService<IOutboxManager>()
						.OutboxForm
						.SelectedNodes,
					true);

			foreach (var kvp in editors)
				kvp.Value.Edit(kvp.Key);
		}

		[CommandStatusGetter("Janus.Outbox.EditItem")]
		public CommandStatus QueryEditItemStatus(ICommandContext context)
		{
			return QueryOutboxItemCommandStatus(context).UnavailableIfNot(
				() =>
					CreateEditor(
							context,
							context
								.GetRequiredService<IActiveOutboxItemService>()
								.ActiveOutboxItems,
							true)
						.Any());
		}

		[CommandExecutor("Janus.Outbox.DeleteItem")]
		public void ExecuteDeleteItem(ICommandContext context)
		{
			if (MessageBox.Show(
					SR.Outbox.RemoveMessageConfirm,
					ApplicationInfo.ApplicationName,
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Warning) != DialogResult.Yes)
				return;

			foreach (var kvp in CreateEditor(
					context,
					context
						.GetRequiredService<IOutboxManager>()
						.OutboxForm
						.SelectedNodes,
					false))
				kvp.Value.Delete(kvp.Key);
		}

		[CommandStatusGetter("Janus.Outbox.DeleteItem")]
		public CommandStatus QueryDeleteItemStatus(ICommandContext context)
		{
			return QueryOutboxItemCommandStatus(context).UnavailableIfNot(
				() =>
					CreateEditor(
							context,
							context
								.GetRequiredService<IActiveOutboxItemService>()
								.ActiveOutboxItems,
							false)
						.Any());
		}

		private static CommandStatus QueryOutboxItemCommandStatus(IServiceProvider provider)
		{
			var activeOutboxItemSvc = provider.GetService<IActiveOutboxItemService>();
			if (activeOutboxItemSvc == null)
				return CommandStatus.Unavailable;

			return activeOutboxItemSvc.ActiveOutboxItems.Any()
				? CommandStatus.Normal
				: CommandStatus.Unavailable;
		}

		[CommandStatusSubscriber("Janus.Outbox.EditItem")]
		[CommandStatusSubscriber("Janus.Outbox.DeleteItem")]
		public IDisposable SubscribeOutboxFeatureCommandStatusChanged(
			IServiceProvider provider, Action handler)
		{
			var outboxSvc = provider.GetService<IActiveOutboxItemService>();
			if (outboxSvc != null)
			{
				EventHandler statusUpdater = (sender, e) => handler();
				Features.AfterFeatureActivateHandler statusUpdater2 =
				(oldFeature, newFeature) => handler();
				outboxSvc.ActiveOutboxItemsChanged += statusUpdater;
				Features.Instance.AfterFeatureActivate += statusUpdater2;
				return
					Disposable.Create(
					() =>
					{
						outboxSvc.ActiveOutboxItemsChanged -= statusUpdater;
						Features.Instance.AfterFeatureActivate -= statusUpdater2;
					});
			}
			return Disposable.Empty;
		}

		private static Type FindEditor(object item)
		{
			var ea = Attribute.GetCustomAttribute(
				item.GetType(),
				typeof(OutboxItemEditorAttribute)) as OutboxItemEditorAttribute;

			return ea?.EditorType;
		}

		private readonly Dictionary<Type, IOutboxItemEditor> _editorHash =
			new Dictionary<Type, IOutboxItemEditor>();

		private IOutboxItemEditor CreateEditor(
			IServiceProvider provider,
			object item)
		{
			if (item == null)
				return null;

			if (_editorHash.ContainsKey(item.GetType()))
				return _editorHash[item.GetType()];

			var et = FindEditor(item);

			if (et == null)
				return null;

			var e = (IOutboxItemEditor)et.CreateInstance(provider);

			_editorHash.Add(item.GetType(), e);
			return e;
		}

		private Dictionary<ITreeNode, IOutboxItemEditor> CreateEditor(
			IServiceProvider provider,
			ICollection<ITreeNode> nodes,
			bool edit)
		{
			var allowAdd = true;
			var editors = new Dictionary<ITreeNode, IOutboxItemEditor>(nodes.Count);

			foreach (var node in nodes)
			{
				var editor = CreateEditor(provider, node);

				allowAdd &= editor != null
					&& (edit ? editor.AllowEdit(node) : editor.AllowDelete(node));

				if (!allowAdd)
					break;

				editors.Add(node, editor);
			}

			if (!allowAdd)
				editors.Clear();

			return editors;
		}
	}
}