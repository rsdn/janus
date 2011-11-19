using System;
using System.Threading;

using Rsdn.SmartApp;

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
			return _rwLock.GetReaderLock();
		}

		public IDisposable GetWriterLock()
		{
			return _rwLock.GetWriterLock();
		}

		public IDisposable GetUpgradeableLock()
		{
			return _rwLock.GetUpgradeableReaderLock();
		}
		#endregion
	}
}