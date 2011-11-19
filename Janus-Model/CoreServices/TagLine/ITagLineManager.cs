namespace Rsdn.Janus
{
	public interface ITagLineManager
	{
		/// <summary>
		/// Возвращает таглайн по форматной строке.
		/// </summary>
		string GetTagLine(string format);

		/// <summary>
		/// Найти подходящий для форума тег-лайн
		/// </summary>
		string FindAppropriateTagLine(int forumId);
	}
}