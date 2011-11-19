namespace Rsdn.Janus
{
	/// <summary>
	/// Статистика синхронизации.
	/// </summary>
	public interface ISyncStat
	{
		/// <summary>
		/// Форматировать значение статистики.
		/// </summary>
		string FormatValue(int value);
	}
}