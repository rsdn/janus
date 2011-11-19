namespace Rsdn.Janus
{
	public interface IConsoleWindowService
	{
		void Show();
		void Clear();
		void Close();

		string PromptText { get; set; }
	}
}