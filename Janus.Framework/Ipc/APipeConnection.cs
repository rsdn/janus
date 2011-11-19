using System;

namespace Rsdn.Janus.Framework.Ipc
{
	/// <summary>
	/// An abstract class, which defines the methods for creating named pipes 
	/// connections, reading and writing data.
	/// </summary>
	/// <remarks>
	/// This class used for client and server applications respectively, which communicate
	/// using NamesPipes.
	/// </remarks>
	public abstract class APipeConnection : IInterProcessConnection
	{
		/// <summary>
		/// Boolean field used by the IDisposable implementation.
		/// </summary>
		protected bool _disposed;

		/// <summary>
		/// A object containing
		/// the native pipe handle.
		/// </summary>
		protected PipeHandle _handle = new PipeHandle(InterProcessConnectionState.NotSet);

		/// <summary>
		/// The maximum bytes that will be read from the pipe connection.
		/// </summary>
		/// <remarks>
		/// This field could be used if the maximum length of the client message
		/// is known and we want to implement some security, which prevents the
		/// server from reading larger messages.
		/// </remarks>
		protected int _maxReadBytes;

		/// <summary>
		/// The name of the named pipe.
		/// </summary>
		/// <remarks>
		/// This name is used for creating a server pipe and connecting client ones to it.
		/// </remarks>
		protected string _name;

		#region IInterProcessConnection Members
		/// <summary>
		/// Reads a message from the pipe connection and converts it to a string
		/// using the UTF8 encoding.
		/// </summary>
		/// <returns>The UTF8 encoded string representation of the data.</returns>
		public string Read()
		{
			CheckIfDisposed();
			return NamedPipeWrapper.Read(_handle, _maxReadBytes);
		}

		/// <summary>
		/// Reads a message from the pipe connection.
		/// </summary>
		/// <returns>The bytes read from the pipe connection.</returns>
		public byte[] ReadBytes()
		{
			CheckIfDisposed();
			return NamedPipeWrapper.ReadBytes(_handle, _maxReadBytes);
		}

		/// <summary>
		/// Writes a string to the pipe connection/
		/// </summary>
		/// <param name="text">The text to write.</param>
		public void Write(string text)
		{
			CheckIfDisposed();
			NamedPipeWrapper.Write(_handle, text);
		}

		/// <summary>
		/// Writes an array of bytes to the pipe connection.
		/// </summary>
		/// <param name="bytes">The bytes array.</param>
		public void WriteBytes(byte[] bytes)
		{
			CheckIfDisposed();
			NamedPipeWrapper.WriteBytes(_handle, bytes);
		}

		/// <summary>
		/// Closes the pipe connection.
		/// </summary>
		public abstract void Close();

		/// <summary>
		/// Connects a pipe connection.
		/// </summary>
		public abstract void Connect();

		/// <summary>
		/// Disposes a pipe connection by closing the underlying native handle.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// object.
		/// </summary>
		/// <returns>The pipe connection state.</returns>
		public InterProcessConnectionState GetState()
		{
			CheckIfDisposed();
			return _handle.State;
		}
		#endregion

		/// <summary>
		/// Disposes a pipe connection by closing the underlying native handle.
		/// </summary>
		/// <param name="disposing">A boolean indicating how the method is called.</param>
		protected void Dispose(bool disposing)
		{
			if (!_disposed)
				_handle.Dispose();
			_disposed = true;
		}

		/// <summary>
		/// Checks if the pipe connection is disposed.
		/// </summary>
		/// <remarks>
		/// This check is done before performing any pipe operations.
		/// </remarks>
		public void CheckIfDisposed()
		{
			if (_disposed)
				throw new ObjectDisposedException("The Pipe Connection is disposed.");
		}
	}
}