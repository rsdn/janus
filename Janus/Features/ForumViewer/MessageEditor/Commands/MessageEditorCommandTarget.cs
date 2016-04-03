using System;
using System.Reactive.Disposables;
using System.Windows.Forms;

using CodeJam.Extensibility;

namespace Rsdn.Janus
{
	/// <summary>
	/// Обработчик команд редактора сообщений.
	/// </summary>
	[CommandTarget]
	internal sealed class MessageEditorCommandTarget : CommandTarget
	{
		public MessageEditorCommandTarget(IServiceProvider serviceProvider)
			: base(serviceProvider) { }

		[CommandExecutor("Janus.Forum.MessageEditor.Send")]
		public void ExecuteSend(ICommandContext context)
		{
			context.GetRequiredService<IMessageEditorService>().SendMessage();
		}

		[CommandExecutor("Janus.Forum.MessageEditor.Preview")]
		public void ExecutePreview(ICommandContext context)
		{
			context.GetRequiredService<IMessageEditorService>().ShowPreview();
		}

		
		[CommandExecutor("Janus.Forum.MessageEditor.Save")]
		public void ExecuteSave(ICommandContext context)
		{
			context.GetRequiredService<IMessageEditorService>().SaveMessage();
		}

		[CommandExecutor("Janus.Forum.MessageEditor.SaveAndClose")]
		public void ExecuteSaveAndClose(ICommandContext context)
		{
			context.GetRequiredService<IMessageEditorService>().SaveMessage();
			context.GetRequiredService<IMessageEditorService>().Close();
		}

		[CommandStatusGetter("Janus.Forum.MessageEditor.Save")]
		[CommandStatusGetter("Janus.Forum.MessageEditor.SaveAndClose")]
		public CommandStatus QuerySaveCommandStatus(ICommandContext context)
		{
			return QueryEditorCommandStatus(context).DisabledIfNot(
				() => context.GetRequiredService<IMessageEditorService>().IsModified);
		}

		[CommandStatusSubscriber("Janus.Forum.MessageEditor.Save")]
		[CommandStatusSubscriber("Janus.Forum.MessageEditor.SaveAndClose")]
		public IDisposable SubscribeSaveStatusChanged(IServiceProvider provider, Action handler)
		{
			var messageEditorSvc = provider.GetService<IMessageEditorService>();
			if (messageEditorSvc != null)
			{
				CodeJam.Extensibility.EventHandler<IMessageEditorService> statusUpdater = sender => handler();
				messageEditorSvc.IsModifiedChanged += statusUpdater;
				return Disposable.Create(() => messageEditorSvc.Modified -= statusUpdater);
			}
			return Disposable.Empty;
		}

		[CommandExecutor("Janus.Forum.MessageEditor.Undo")]
		public void ExecuteUndo(ICommandContext context)
		{
			context.GetRequiredService<IMessageEditorService>().Undo();
		}

		[CommandStatusGetter("Janus.Forum.MessageEditor.Undo")]
		public CommandStatus QueryUndoCommandStatus(ICommandContext context)
		{
			return QueryEditorCommandStatus(context).DisabledIfNot(
				() => context.GetRequiredService<IMessageEditorService>().CanUndo);
		}

		[CommandExecutor("Janus.Forum.MessageEditor.Redo")]
		public void ExecuteRedo(ICommandContext context)
		{
			context.GetRequiredService<IMessageEditorService>().Redo();
		}

		[CommandStatusGetter("Janus.Forum.MessageEditor.Redo")]
		public CommandStatus QueryRedoCommandStatus(ICommandContext context)
		{
			return QueryEditorCommandStatus(context).DisabledIfNot(
				() => context.GetRequiredService<IMessageEditorService>().CanRedo);
		}

		[CommandStatusSubscriber("Janus.Forum.MessageEditor.Undo")]
		[CommandStatusSubscriber("Janus.Forum.MessageEditor.Redo")]
		public IDisposable SubscribeUndoRedoStatusChanged(IServiceProvider provider, Action handler)
		{
			var messageEditorSvc = provider.GetService<IMessageEditorService>();
			if (messageEditorSvc != null)
			{
				CodeJam.Extensibility.EventHandler<IMessageEditorService> statusUpdater = sender => handler();
				messageEditorSvc.Modified += statusUpdater;
				return Disposable.Create(
					() => messageEditorSvc.Modified -= statusUpdater);
			}
			return Disposable.Empty;
		}

		[CommandExecutor("Janus.Forum.MessageEditor.FindAndReplace")]
		public void ExecuteFindAndReplace(ICommandContext context)
		{
			context.GetRequiredService<IMessageEditorService>().ShowFindAndReplace();
		}

		[CommandExecutor("Janus.Forum.MessageEditor.InsertSmile")]
		public void ExecuteInsertSmile(ICommandContext context, string text)
		{
			context.GetRequiredService<IMessageEditorService>().InsertText(" " + text + " ");
		}

		[CommandExecutor("Janus.Forum.MessageEditor.InsertSingleTag")]
		public void ExecuteInsertSingleTag(ICommandContext context, string text)
		{
			context.GetRequiredService<IMessageEditorService>().InsertText("[" + text + "]");
		}

		[CommandExecutor("Janus.Forum.MessageEditor.InsertPairTag")]
		public void ExecuteInsertPairTag(
			ICommandContext context, string start, string end, bool newLine)
		{
			if (start == "url=" && Clipboard.ContainsText())
			{
				var clipboardText = Clipboard.GetText();
				if (clipboardText.StartsWith("www."))
					start += "http://" + clipboardText;
				else if (Uri.IsWellFormedUriString(clipboardText, UriKind.Absolute))
					start += clipboardText;
			}

			switch (start)
			{
				case "c#":
				case "nemerle":
				case "msil":
				case "midl":
				case "asm":
				case "ccode":
				case "code":
				case "pascal":
				case "vb":
				case "sql":
				case "java":
				case "perl":
				case "php":
					Config.Instance.LastLanguageTag = start;
					break;
			}

			context.GetRequiredService<IMessageEditorService>().SurroundText(
				"[" + start + "]", "[/" + end + "]", newLine);
		}

		[CommandExecutor("Janus.Forum.MessageEditor.Close")]
		public void ExecuteClose(ICommandContext context)
		{
			context.GetRequiredService<IMessageEditorService>().Close();
		}

		[CommandStatusGetter("Janus.Forum.MessageEditor.Send")]
		[CommandStatusGetter("Janus.Forum.MessageEditor.Preview")]
		[CommandStatusGetter("Janus.Forum.MessageEditor.FindAndReplace")]
		[CommandStatusGetter("Janus.Forum.MessageEditor.InsertSingleTag")]
		[CommandStatusGetter("Janus.Forum.MessageEditor.InsertPairTag")]
		[CommandStatusGetter("Janus.Forum.MessageEditor.InsertSmile")]
		[CommandStatusGetter("Janus.Forum.MessageEditor.Close")]
		public CommandStatus QueryEditorCommandStatus(ICommandContext context)
		{
			return
				context.GetService<IMessageEditorService>() != null
					? CommandStatus.Normal
					: CommandStatus.Unavailable;
		}
	}
}