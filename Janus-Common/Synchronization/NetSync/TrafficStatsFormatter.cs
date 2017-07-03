using System;

using CodeJam;
using CodeJam.Strings;

using Rsdn.Janus.Synchronization;

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
				.FormatWith(value.ToInfoSizeString());
		}
		#endregion
	}
}
