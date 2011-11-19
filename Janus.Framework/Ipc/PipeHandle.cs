using System;

namespace Rsdn.Janus.Framework.Ipc
{
	/// <summary>
	/// Holds the operating system native handle and the current state of the pipe connection.
	/// </summary>
	public sealed class PipeHandle : IDisposable
	{
		private IntPtr _handle;
		private InterProcessConnectionState _state;

		/// <summary>
		/// Creates a PipeHandle instance using the provided native handle and state.
		/// </summary>
		/// <param name="hnd">The native handle.</param>
		/// <param name="state">The state of the pipe connection.</param>
		public PipeHandle(IntPtr hnd, InterProcessConnectionState state)
		{
			_handle = hnd;
			_state = state;
		}

		/// <summary>
		/// Creates a PipeHandle instance using the passed native handle.
		/// </summary>
		/// <param name="hnd">The native handle.</param>
		public PipeHandle(IntPtr hnd)
			: this(hnd, InterProcessConnectionState.NotSet)
		{}

		/// <summary>
		/// Creates a PipeHandle instance with an invalid native handle.
		/// </summary>
		public PipeHandle(InterProcessConnectionState state)
			: this(new IntPtr(NamedPipeNative.INVALID_HANDLE_VALUE), state)
		{}

		/// <summary>
		/// The operating system native handle.
		/// </summary>
		public IntPtr Handle
		{
			get { return _handle; }
		}

		/// <summary>
		/// The current state of the pipe connection.
		/// </summary>
		public InterProcessConnectionState State
		{
			get { return _state; }
			set { _state = value; }
		}

		#region IDisposable Members
		/// <summary>
		/// Closes a named pipe and releases the native handle.
		/// </summary>
		public void Dispose()
		{
			_state = InterProcessConnectionState.Closing;
			NamedPipeNative.CloseHandle(_handle);
			_handle = IntPtr.Zero;
			_state = InterProcessConnectionState.Closed;
		}
		#endregion

		/// <summary>
		/// Object destructor.
		/// </summary>
		~PipeHandle()
		{
			Dispose();
		}
	}
}