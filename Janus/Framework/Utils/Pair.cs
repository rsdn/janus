#pragma warning disable 1692

namespace Rsdn.Janus
{
	/// <summary>
	/// Pair of elements.
	/// </summary>
	public struct Pair<T1, T2>
	{
		private readonly T1 _first;
		private readonly T2 _second;

		/// <summary>
		/// Initialize instance.
		/// </summary>
		public Pair(T1 first, T2 second)
		{
			_first = first;
			_second = second;
		}

		/// <summary>
		/// First element.
		/// </summary>
		public T1 First
		{
			get { return _first; }
		}

		/// <summary>
		/// Second element.
		/// </summary>
		public T2 Second
		{
			get { return _second; }
		}

		/// <summary>
		/// See <see cref="object.Equals(object)"/>
		/// </summary>
		public override bool Equals(object obj)
		{
			if (!(obj is Pair<T1, T2>))
				return false;
			var pair = (Pair<T1, T2>)obj;
			return Equals(_first, pair._first) && Equals(_second, pair._second);
		}

		/// <summary>
		/// See <see cref="object.GetHashCode"/>
		/// </summary>
		public override int GetHashCode()
		{
#pragma warning disable CompareNonConstrainedGenericWithNull
			return (_first != null ? _first.GetHashCode() : 0)
				+ 29*(_second != null ? _second.GetHashCode() : 0);
#pragma warning restore CompareNonConstrainedGenericWithNull
		}
	}
}