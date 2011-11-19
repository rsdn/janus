using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	internal class MessageEditorService : IMessageEditorService, IDisposable
	{
		private readonly MessageForm _form;

		public MessageEditorService([NotNull] MessageForm form)
		{
			if (form == null)
				throw new ArgumentNullException("form");

			_form = form;
			_form.Modified+= EditorModified;
			_form.IsModifiedChanged += EditorIsModifiedChanged;
		}

		#region IDisposable Members

		public void Dispose()
		{
			_form.Modified -= EditorModified;
			_form.IsModifiedChanged -= EditorIsModifiedChanged;
		}

		#endregion

		#region IMessageEditorService Members

		public void SendMessage()
		{
			_form.SendMessage();
		}

		public void SaveMessage()
		{
			_form.SaveMessage();
		}

		public void ShowPreview()
		{
			_form.ShowPreview();
		}

		public void Undo()
		{
			_form.Undo();
		}

		public void Redo()
		{
			_form.Redo();
		}

		public void ShowFindAndReplace()
		{
			_form.ShowFindAndReplace();
		}

		public void SurroundText(string start, string end, bool newLine)
		{
			_form.SurroundText(start, end, newLine);
		}

		public void InsertText(string text)
		{
			_form.InsertText(text);
		}

		public void Close()
		{
			_form.Close();
		}

		public bool IsModified
		{
			get { return _form.IsModified; }
		}

		public bool CanUndo
		{
			get { return _form.CanUndo; }
		}

		public bool CanRedo
		{
			get { return _form.CanRedo; }
		}

		public event SmartApp.EventHandler<IMessageEditorService> Modified;
		public event SmartApp.EventHandler<IMessageEditorService> IsModifiedChanged;

		#endregion

		private void EditorModified(MessageForm sender)
		{
			if (Modified != null)
				Modified(this);
		}

		private void EditorIsModifiedChanged(MessageForm sender)
		{
			if (IsModifiedChanged != null)
				IsModifiedChanged(this);
		}
	}
}
