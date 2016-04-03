using CodeJam.Extensibility;

namespace Rsdn.Janus
{
	public interface IMessageEditorService
	{
		void SendMessage();
		void SaveMessage();
		void ShowPreview();
		void Undo();
		void Redo();
		void ShowFindAndReplace();
		void SurroundText(string start, string end, bool newLine);
		void InsertText(string text);
		void Close();

		bool IsModified { get; }
		bool CanUndo { get; }
		bool CanRedo { get; }

		event EventHandler<IMessageEditorService> Modified;
		event EventHandler<IMessageEditorService> IsModifiedChanged;
	}
}