using System;

namespace Rsdn.Janus
{
	public class EmptyJanusRWLock : IJanusRWLock
	{
		public static readonly EmptyJanusRWLock Instance =
			new EmptyJanusRWLock();

		private EmptyJanusRWLock()
		{}

		#region Implementation of IJanusRWLock
		public IDisposable GetReaderLock()
		{
			return EmptyDisposable.Instance;
		}

		public IDisposable GetWriterLock()
		{
			return EmptyDisposable.Instance;
		}

		public IDisposable GetUpgradeableLock()
		{
			return EmptyDisposable.Instance;
		}
		#endregion

		#region EmptyDisposable class
		private class EmptyDisposable : IDisposable
		{
			public static readonly EmptyDisposable Instance =
				new EmptyDisposable();

			private EmptyDisposable()
			{}

			#region Implementation of IDisposable
			public void Dispose()
			{}
			#endregion
		}
		#endregion
	}
}