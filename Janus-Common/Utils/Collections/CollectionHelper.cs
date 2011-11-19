using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public static class CollectionHelper
	{
		/// <summary>
		/// Добавляет элемент в конец последовательности.
		/// </summary>
		public static IEnumerable<T> Add<T>([NotNull] this IEnumerable<T> source, T newItem)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			foreach (var item in source)
				yield return item;
			yield return newItem;
		}

		/// <summary>
		/// Проверяет, один ли элемент в последовательноcти.
		/// </summary>
		public static bool IsSingle<T>([NotNull] this IEnumerable<T> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			var isFirst = false;
#pragma warning disable 168
			foreach (var item in source)
#pragma warning restore 168
				if (!isFirst)
					isFirst = true;
				else
					return false;
			return isFirst;
		}

		public static ReadOnlyCollection<T> AsReadOnly<T>([NotNull] this IList<T> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source as ReadOnlyCollection<T>
				?? new ReadOnlyCollection<T>(source);
		}

		public static ReadOnlyDictionary<TKey, TVaue> AsReadOnly<TKey, TVaue>(
			[NotNull] this IDictionary<TKey, TVaue> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source as ReadOnlyDictionary<TKey, TVaue>
				?? new ReadOnlyDictionary<TKey, TVaue>(source);
		}

		/// <summary>
		/// Сортирует последовательность с сохранением исходного порядка элементов.
		/// </summary>
		public static IEnumerable<TItem> Sort<TItem, TKey>(
			[NotNull] this IEnumerable<TItem> source,
			[NotNull] Func<TItem, TKey> keySelector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");

			return source
				.Select((item, index) => new { Item = item, OriginalIndex = index })
				.OrderBy(sortItem => keySelector(sortItem.Item))
				.ThenBy(sortItem => sortItem.OriginalIndex)
				.Select(sortItem => sortItem.Item);
		}

		/// <summary>
		/// Сравнивает словари на предмет равенства их содержимого.
		/// </summary>
		public static bool DictionaryEquals<TKey, TValue>(
			[NotNull] this IDictionary<TKey, TValue> a,
			[NotNull] IDictionary<TKey, TValue> b)
		{
			return DictionaryEquals(a, b, null, null);
		}

		/// <summary>
		/// Сравнивает словари на предмет равенства их содержимого.
		/// </summary>
		public static bool DictionaryEquals<TKey, TValue>(
			[NotNull] this IDictionary<TKey, TValue> a,
			[NotNull] IDictionary<TKey, TValue> b,
			Func<TKey, TKey, bool> keyComparer,
			Func<TValue, TValue, bool> valueComparer)
		{
			if (a == null)
				throw new ArgumentNullException("a");
			if (b == null)
				throw new ArgumentNullException("b");

			if (ReferenceEquals(a, b))
				return true;
			if (a.Count != b.Count)
				return false;

			return !a
				.Except(
					b,
					new LambdaEqualityComparer<KeyValuePair<TKey, TValue>>(
						(x, y) =>
							keyComparer == null
								? x.Key.Equals(y.Key)
								: keyComparer(x.Key, y.Key)
							&& valueComparer == null
								? x.Value.Equals(y.Value)
								: valueComparer(x.Value, y.Value)))
				.Any();
		}

		public static IEnumerable<IEnumerable<T>> SplitToSeries<T>(
			[NotNull] this IEnumerable<T> source,
			int seriesSize)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (seriesSize <= 0) throw new ArgumentOutOfRangeException("seriesSize");
			var buffer = new T[seriesSize];
			var i = 0;
			foreach (var item in source)
			{
				buffer[i] = item;
				i++;
				if (i == seriesSize)
				{
					yield return buffer;
					i = 0;
					Array.Clear(buffer, 0, seriesSize);
				}
			}
			if (i > 0)
				yield return Enumerable.Range(0, i).Select(j => buffer[j]);
		}
	}
}