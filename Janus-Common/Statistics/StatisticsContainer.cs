using System;
using System.Collections.Generic;
using System.Threading;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Стандартная реализация <see cref="IStatisticsContainer"/>
	/// </summary>
	/// <remarks>Потокобезопасен</remarks>
	public class StatisticsContainer : IStatisticsContainer
	{
		private readonly Dictionary<string, int> _values =
			new Dictionary<string, int>();
		private readonly ReaderWriterLockSlim _valuesLock = new ReaderWriterLockSlim();

		#region IStatisticsContainer Members
		public void AddValue(string statsName, int value)
		{
			if (value < 0)
				throw new ArgumentOutOfRangeException("value");
			if (value == 0)
				return; // nothing to add
			using (_valuesLock.GetWriterLock())
			{
				int existingValue;
				_values.TryGetValue(statsName, out existingValue);
				_values[statsName] = existingValue + value;
			}
		}

		public string[] GetStatsNames()
		{
			using (_valuesLock.GetReaderLock())
				return _values.Keys.ToArray();
		}

		public int GetTotalValue(string statsName)
		{
			using (_valuesLock.GetReaderLock())
			{
				int value;
				_values.TryGetValue(statsName, out value);
				return value;
			}
		}
		#endregion
	}
}
