namespace Rsdn.Janus
{
	/// <summary>
	/// Отображает информацию о ходе синхронизации.
	/// </summary>
	public interface ISyncProgressVisualizer
	{
		void ReportProgress(int total, int current);
		void SetProgressText(string text);
		void SetCompressionSign(CompressionState state);
	}
}