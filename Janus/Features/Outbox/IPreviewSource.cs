namespace Rsdn.Janus
{
	/// <summary>
	/// Источник данных для предварительного просмотра.
	/// </summary>
	public interface IPreviewSource
	{
		/// <summary>
		/// Получить данные для предварительного просмотра.
		/// </summary>
		string GetData();
	}
}
