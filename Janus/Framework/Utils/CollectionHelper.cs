using System;
using System.Collections.Generic;
using System.Text;

namespace Rsdn.Janus
{
	public static class CollectionHelper
	{
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
			Func<string, T> projector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (separator == null)
				throw new ArgumentNullException("separator");
			if (seriesSize <= 0)
				throw new ArgumentOutOfRangeException("seriesSize");

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

		public static T[] ToArray<T>(this IEnumerable<T> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			var list = new List<T>();
			foreach (var item in source)
				list.Add(item);
			return list.ToArray();
		}

		public static IEnumerable<T2> Project<T1, T2>(
			this IEnumerable<T1> source,
			Func<T2, T1> projector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (projector == null)
				throw new ArgumentNullException("projector");

			foreach (var item in source)
				yield return projector(item);
		}

		public static IDictionary<K, V> ToDictionary<T, K, V>(
			this IEnumerable<T> source,
			Func<K, T> keyGetter,
			Func<V, T> valueGetter)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keyGetter == null)
				throw new ArgumentNullException("keyGetter");
			if (valueGetter == null)
				throw new ArgumentNullException("valueGetter");

			var dic = new Dictionary<K, V>();
			foreach (var item in source)
				dic[keyGetter(item)] = valueGetter(item);
			return dic;
		}

		public static T FirstOrDefault<T>(this IEnumerable<T> source, Predicate<T> predicate)
		{
			foreach (var item in source)
				if (predicate(item))
					return item;
			return default(T);
		}

		public static T FirstOrDefault<T>(this IEnumerable<T> source)
		{
			using (var en = source.GetEnumerator())
				if (en.MoveNext())
					return en.Current;
			return default(T);
		}

		/// <summary>
		/// Склеивает итератор итераторов в один большой итератор.
		/// </summary>
		public static IEnumerable<T> Join<T>(this IEnumerable<IEnumerable<T>> sources)
		{
			foreach (var source in sources)
				foreach (var item in source)
					yield return item;
		}

		/// <summary>
		/// Склеивает два разнотипных итератора в один большой итератор.
		/// </summary>
		public static IEnumerable<T2> Join<T1, T2>(this IEnumerable<T1> source1, Func<IEnumerable<T2>, T1> source2Getter)
		{
			foreach (var item1 in source1)
				foreach (var item2 in source2Getter(item1))
					yield return item2;
		}

		public static IEnumerable<T> GetEmptyEnumerator<T>()
		{
			yield break;
		}

		public static bool IsEmpty<T>(this IEnumerable<T> source)
		{
			using (var en = source.GetEnumerator())
				return en.MoveNext();
		}

		public static IEnumerable<T> Skip<T>(this IEnumerable<T> source, int length)
		{
			var i = 0;
			foreach (var item in source)
			{
				if (i >= length)
					yield return item;
				i++;
			}
		}

		/// <summary>
		/// Получить головную часть перечисления указанного размера.
		/// </summary>
		public static IEnumerable<T> Take<T>(this IEnumerable<T> source, int length)
		{
			var i = 0;
			foreach (var item in source)
			{
				if (i == length)
					yield break;
				yield return item;
				i++;
			}
		}
	}
}
