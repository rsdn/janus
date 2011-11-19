using System;

using Rsdn.Janus.Synchronization;
using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	public abstract class TrafficStatsFormatter : IStatisticsFormatter
	{
		private readonly TransferDirection _direction;

		protected TrafficStatsFormatter(TransferDirection direction)
		{
			_direction = direction;
		}

		#region IStatisticsFormatter Members
		public string FormatValue(int value, IFormatProvider formatProvider)
		{
			return (_direction == TransferDirection.Receive
				? SyncResources.Received
				: SyncResources.Sent)
				.FormatStr(value.ToInfoSizeString());
		}
		#endregion
	}
}
