namespace Rsdn.Janus
{
	/// <summary>
	/// Служба для отображения информации об ошибках синхронизации.
	/// </summary>
	public interface ISyncErrorInformer
	{
		void AddError(SyncErrorInfo errorInfo);
	}
}