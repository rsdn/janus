namespace Rsdn.Janus
{
	/// <summary>
	/// Сервис прогресса.
	/// </summary>
	public interface IProgressService
	{
		/// <summary>
		/// Создать визуализатор прогресса.
		/// </summary>
		/// <param name="allowCancel">Поддержка отмены прогресса.</param>
		IProgressVisualizer CreateVisualizer(bool allowCancel);
	}
}
