using Rsdn.Janus.Synchronization;
using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	internal class TrafficStat : ISyncStat
	{
		private readonly TransferDirection _direction;

		public TrafficStat(TransferDirection direction)
		{
			_direction = direction;
		}

		#region ISyncStat Members
		/// <summary>
		/// Форматировать значение статистики.
		/// </summary>
		public string FormatValue(int value)
		{
			return (_direction == TransferDirection.Receive
				? SyncResources.Received
				: SyncResources.Sent)
				.FormatStr(value.ToInfoSizeString());
		}
		#endregion
	}
}