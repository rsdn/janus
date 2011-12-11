namespace Rsdn.Janus
{
	public interface IMessageFormatter
	{
		/// <summary>
		/// Обработка сообщения перед преобразованием HTML.
		/// </summary>
		string FormatSource(string source);

		/// <summary>
		/// Обработка сообщения в формате HTML.
		/// </summary>
		string FormatHtml(string html);
	}
}