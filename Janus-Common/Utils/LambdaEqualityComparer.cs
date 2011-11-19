using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public sealed class LambdaEqualityComparer<T> : IEqualityComparer<T>
	{
		private readonly Func<T, T, bool> _comparer;

		public LambdaEqualityComparer([NotNull] Func<T, T, bool> comparer)
		{
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			_comparer = comparer;
		}

		public bool Equals(T x, T y)
		{
			return _comparer(x, y);
		}

		public int GetHashCode(T obj)
		{
			return obj.GetHashCode();
		}
	}
}