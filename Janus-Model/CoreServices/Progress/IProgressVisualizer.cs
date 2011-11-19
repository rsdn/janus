namespace Rsdn.Janus
{
	/// <summary>
	/// Визуализатор прогресса.
	/// </summary>
	public interface IProgressVisualizer
	{
		/// <summary>
		/// Установить текст сообщения.
		/// </summary>
		/// <param name="text">Текст сообщения.</param>
		void SetProgressText(string text);

		/// <summary>
		/// Установить степень завершенности прогресса.
		/// </summary>
		/// <param name="total">Максимальное значение.</param>
		/// <param name="current">Текущее значение.</param>
		void ReportProgress(int total, int current);

		/// <summary>
		/// Сообщить визуализатору, что он больше не нужен.
		/// </summary>
		void Complete();

		/// <summary>
		/// Признак того, что запрошена отмена.
		/// </summary>
		bool CancelRequested { get; }
	}
}