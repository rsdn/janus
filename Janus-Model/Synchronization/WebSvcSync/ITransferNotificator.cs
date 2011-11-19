namespace Rsdn.Janus
{
	public interface ITransferNotificator
	{
		int TotalUploaded { get; }
		int TotalDownloaded { get; }
		event TransferProgressHandler TransferProgress;
		event TransferBeginHandler TransferBegin;
		event TransferCompleteHandler TransferComplete;
	}

	public delegate void TransferBeginHandler(int total, TransferDirection direction, CompressionState state);

	public delegate void TransferProgressHandler(int total, int current, TransferDirection direction);

	public delegate void TransferCompleteHandler(int total, TransferDirection direction);
}