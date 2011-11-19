using System;
using System.Runtime.InteropServices;

namespace Rsdn.Janus.Framework.Networking
{
	public class ResourceReader
	{
		private readonly byte[] _bytes;
		private readonly int _startPosition;
		private int _readPosition;

		public ResourceReader(byte[] bytes, int startPosition)
		{
			_bytes = bytes;
			_startPosition = startPosition;
			_readPosition = startPosition;
		}

		public ResourceReader(byte[] bytes)
			: this(bytes, 0)
		{}

		public void Reset()
		{
			_readPosition = _startPosition;
		}

		public bool Read(IntPtr pv, int count, out int bytesRead)
		{
			if (_readPosition >= _bytes.Length)
			{
				bytesRead = 0;
				return false;
			}

			var bytesLeave = _bytes.Length - _readPosition;
			bytesRead = count >= bytesLeave ? bytesLeave : count;

			Marshal.Copy(_bytes, _readPosition, pv, bytesRead);
			_readPosition += bytesRead;

			return true;
		}
	}
}