using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Вспомогательные методы для работы со строками.
	/// </summary>
	public static class StringHelper
	{
		public static string ToInfoSizeString(this int size)
		{
			return ToInfoSizeString((long)size);
		}

		public static string ToInfoSizeString(this long size)
		{
			string dimUnit;
			int k;
			if (size < 1024)
			{
				dimUnit = "B";
				k = 0;
			}
			else if (size < 1024 * 1024)
			{
				dimUnit = "KiB";
				k = 10;
			}
			else
			{
				dimUnit = "MiB";
				k = 20;
			}
			return ((double)size / (1 << k)).ToString("#.0") + dimUnit;
		}

		public static string Join(this IEnumerable<string> source)
		{
			return Join(source, null);
		}

		public static string Join(this IEnumerable<string> source, string separator)
		{
			var sb = new StringBuilder();
			foreach (var str in source)
			{
				if (separator != null && sb.Length > 0)
					sb.Append(separator);
				sb.Append(str);
			}
			return sb.ToString();
		}

		/// <summary>
		/// Преобразование <see cref="Nullable{Timespan}"/> к строке
		/// без долей секунды.
		/// </summary>
		public static string ToSecondsString(this TimeSpan? timespan)
		{
			return timespan == null ? "" : timespan.Value.ToSecondsString();
		}

		/// <summary>
		/// Преобразование <see cref="TimeSpan"/> к строке
		/// без долей секунды.
		/// </summary>
		public static string ToSecondsString(this TimeSpan timespan)
		{
			return
				"{1}{0}{2}{0}{3}"
				.FormatStr(
					CultureInfo.CurrentUICulture.DateTimeFormat.TimeSeparator,
					(timespan.Days * 24 + timespan.Hours)
						.ToString("00", CultureInfo.InvariantCulture),
					timespan.Minutes
						.ToString("00", CultureInfo.InvariantCulture),
					((int)Math.Round((timespan.Seconds * 1000 + timespan.Milliseconds) / 1000f))
						.ToString("00", CultureInfo.InvariantCulture));
		}
	}
}
