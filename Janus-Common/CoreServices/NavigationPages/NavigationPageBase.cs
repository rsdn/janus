using System;
using System.Reactive.Subjects;
using System.Windows.Forms;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public abstract class NavigationPageBase : INavigationPage
	{
		private readonly string _name;
		private readonly INavigationItemHeader _header;
		private readonly Subject<EventArgs> _disposed = new Subject<EventArgs>();

		protected NavigationPageBase([NotNull] string name, [NotNull] INavigationItemHeader header)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (header == null) 
				throw new ArgumentNullException("header");

			_name = name;
			_header = header;
		}

		#region Implementation of INavigationPage

		public string Name
		{
			get { return _name; }
		}

		public INavigationItemHeader Header
		{
			get { return _header; }
		}

		public virtual NavigationPageState State
		{
			get { return null; }
		}

		public bool IsDisposed { get; private set; }

		public IObservable<EventArgs> Disposed
		{
			get { return _disposed; }
		}

		public abstract Control CreateControl();

		#endregion

		#region Implementation of IDisposable

		public virtual void Dispose()
		{
			IsDisposed = true;
			_disposed.OnNext(EventArgs.Empty);
			_disposed.OnCompleted();
		}

		#endregion
	}
}