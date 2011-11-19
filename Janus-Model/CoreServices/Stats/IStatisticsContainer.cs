namespace Rsdn.Janus
{
	/// <summary>
	/// Контейнер со значениями статистики.
	/// </summary>
	public interface IStatisticsContainer
	{
		void AddValue(string statsName, int value);
		string[] GetStatsNames();
		int GetTotalValue(string statsName);
	}
}
