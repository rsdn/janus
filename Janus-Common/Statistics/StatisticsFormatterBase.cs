using System;

using CodeJam;

namespace Rsdn.Janus
{
	/// <summary>
	/// Стандартная реализация <see cref="ISyncStat"/>
	/// </summary>
	public abstract class StatisticsFormatterBase : IStatisticsFormatter
	{
		private readonly Func<string> _declension1;
		private readonly Func<string> _declension2;
		private readonly Func<string> _declension5;

		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		/// <remarks>
		/// делегаты используются, так как реальное значение зависит
		/// от текущей локали потока
		/// </remarks>
		protected StatisticsFormatterBase(
			Func<string> declension1,
			Func<string> declension2,
			Func<string> declension5)
		{
			if (declension1 == null)
				throw new ArgumentNullException(nameof(declension1));
			if (declension2 == null)
				throw new ArgumentNullException(nameof(declension2));
			if (declension5 == null)
				throw new ArgumentNullException(nameof(declension5));
			_declension1 = declension1;
			_declension2 = declension2;
			_declension5 = declension5;
		}

		public string Declension1 => _declension1();

		public string Declension2 => _declension2();

		public string Declension5 => _declension5();

		#region IStatisticsFormatter Members
		public string FormatValue(int value, IFormatProvider formatProvider)
		{
			return "{0} {1}".FormatWith(
				value.ToString(formatProvider),
				value.GetDeclension(
					Declension1,
					Declension2,
					Declension5));
		}
		#endregion
	}
}
