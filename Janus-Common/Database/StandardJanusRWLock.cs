using System;
using System.Threading;

using CodeJam.Threading;

namespace Rsdn.Janus
{
	/// <summary>
	/// Standard <see cref="IJanusRWLock"/> implementation, based on <see cref="ReaderWriterLock"/>.
	/// </summary>
	public class StandardJanusRWLock : IJanusRWLock
	{
		private readonly ReaderWriterLockSlim _rwLock;

		public StandardJanusRWLock() : this(new ReaderWriterLockSlim())
		{
		}

		public StandardJanusRWLock(ReaderWriterLockSlim rwLock)
		{
			_rwLock = rwLock;
		}

		#region Implementation of IJanusRWLock
		public IDisposable GetReaderLock()
		{
			return _rwLock.GetReadLock();
		}

		public IDisposable GetWriterLock()
		{
			return _rwLock.GetWriteLock();
		}

		public IDisposable GetUpgradeableLock()
		{
			return _rwLock.GetUpgradeableReadLock();
		}
		#endregion
	}
}