using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using CodeJam;

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
			if (size < 0)
				return "?";
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

		public static string JoinStrings(this IEnumerable<string> source)
		{
			return JoinStrings(source, null);
		}

		public static string JoinStrings(this IEnumerable<string> source, string separator)
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
		/// Преобразование <see cref="Nullable{TimeSpan}"/> к строке без долей секунды.
		/// </summary>
		public static string ToSecondsString(this TimeSpan? timespan)
		{
			return timespan == null ? "" : timespan.Value.ToSecondsString();
		}

		/// <summary>
		/// Преобразование <see cref="TimeSpan"/> к строке без долей секунды.
		/// </summary>
		public static string ToSecondsString(this TimeSpan timespan)
		{
			return
				"{1}{0}{2}{0}{3}"
					.FormatWith(
						CultureInfo.CurrentUICulture.DateTimeFormat.TimeSeparator,
						(timespan.Days * 24 + timespan.Hours)
							.ToString("00", CultureInfo.InvariantCulture),
						timespan.Minutes
							.ToString("00", CultureInfo.InvariantCulture),
						((int)Math.Round((timespan.Seconds * 1000 + timespan.Milliseconds) / 1000f))
							.ToString("00", CultureInfo.InvariantCulture));
		}

		/// <summary>
		/// Получить наименование перечислимого значения в соответствии с им самим.
		/// </summary>
		public static string GetDeclension(this int val, string one, string two, string five)
		{
			var t = (val % 100 > 20) ? val % 10 : val % 20;

			switch (t)
			{
				case 1:
					return one;
				case 2:
				case 3:
				case 4:
					return two;
				default:
					return five;
			}
		}

		/// <summary>
		/// Склеивает коллекцию в строковые серии.
		/// </summary>
		public static IEnumerable<string> JoinToStringSeries<T>(
			this IEnumerable<T> source,
			int seriesSize)
		{
			return JoinToStringSeries(source, ", ", seriesSize);
		}

		/// <summary>
		/// Склеивает коллекцию в строковые серии.
		/// </summary>
		public static IEnumerable<string> JoinToStringSeries<T>(
			this IEnumerable<T> source,
			string separator,
			int seriesSize)
		{
			return JoinToStringSeries(source, separator, seriesSize, null);
		}

		/// <summary>
		/// Склеивает коллекцию в строковые серии.
		/// </summary>
		public static IEnumerable<string> JoinToStringSeries<T>(
			this IEnumerable<T> source,
			int seriesSize,
			Func<string, T> projector)
		{
			return JoinToStringSeries(source, ", ", seriesSize, null);
		}

		/// <summary>
		/// Склеивает коллекцию в строковые серии.
		/// </summary>
		public static IEnumerable<string> JoinToStringSeries<T>(
			this IEnumerable<T> source,
			string separator,
			int seriesSize,
			Func<T, string> projector)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));
			if (separator == null)
				throw new ArgumentNullException(nameof(separator));
			if (seriesSize <= 0)
				throw new ArgumentOutOfRangeException(nameof(seriesSize));

			var sb = new StringBuilder();
			var pos = 0;
			foreach (var item in source)
			{
				if (pos > 0)
					sb.Append(separator);
				sb.Append(projector == null ? item.ToString() : projector(item));
				pos++;
				if (pos < seriesSize)
					continue;
				yield return sb.ToString();
				sb = new StringBuilder();
				pos = 0;
			}
			if (pos > 0)
				yield return sb.ToString();
		}

		/// <summary>
		/// Склеивает последовательность символов в строку.
		/// </summary>
		public static string JoinToString(this IEnumerable<char> source)
		{
			var sourceArray = source as char[];
			if (sourceArray != null)
				return new string(sourceArray);
			var sourceCollection = source as ICollection<char>;
			var sb = sourceCollection != null
				? new StringBuilder(sourceCollection.Count)
				: new StringBuilder();
			foreach (var c in source)
				sb.Append(c);
			return sb.ToString();
		}
	}
}