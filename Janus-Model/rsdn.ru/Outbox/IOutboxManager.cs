namespace Rsdn.Janus
{
	public interface IOutboxManager
	{
		IOutboxForm OutboxForm { get; }
		IOutboxMessageCollection NewMessages { get; }
		IOutboxRateCollection RateMarks { get; }
		IDownloadTopicCollection DownloadTopics { get; }

		void AddTopicForDownload(int mid);
		void AddTopicForDownloadWithConfirm(int mid);

		void AddBugReport(
			string bugName,
			string bugDescription,
			string stackTrace,
			bool showEditor);
	}
}