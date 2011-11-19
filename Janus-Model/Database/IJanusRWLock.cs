using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Read-write lock.
	/// </summary>
	public interface IJanusRWLock
	{
		IDisposable GetReaderLock();
		IDisposable GetWriterLock();
		IDisposable GetUpgradeableLock();
	}
}