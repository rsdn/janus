using System.Collections;

namespace Rsdn.Janus.Framework
{
	/// <summary>
	/// Энумератор 2 коллекций.
	/// </summary>
	public class TwoCollectionEnumerator : IEnumerator
	{
		private readonly IList _col1;
		private readonly IList _col2;
		private int _position;

		public TwoCollectionEnumerator(IList col1, IList col2)
		{
			_col2 = col2;
			_col1 = col1;
			((IEnumerator)this).Reset();
		}

		#region IEnumerator
		bool IEnumerator.MoveNext()
		{
			return ++_position < (_col1.Count + _col2.Count);
		}

		void IEnumerator.Reset()
		{
			_position = -1;
		}

		object IEnumerator.Current
		{
			get
			{
				return _position < _col1.Count ? _col1[_position] : _col2[_position - _col1.Count];
			}
		}
		#endregion
	}
}