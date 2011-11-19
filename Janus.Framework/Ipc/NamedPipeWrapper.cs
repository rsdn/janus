using System;
using System.Text;

namespace Rsdn.Janus.Framework.Ipc
{
	/// <summary>
	/// A utility class that exposes named pipes operations.
	/// </summary>
	/// <remarks>
	/// This class uses the exposed exposed kernel32.dll methods by the 
	/// <see cref="Ipc.NamedPipeNative">NamedPipeNative</see> class
	/// to provided controlled named pipe functionality.
	/// </remarks>
	public static class NamedPipeWrapper
	{
		/// <summary>
		/// The number of retries when creating a pipe or connecting to a pipe.
		/// </summary>
		private const int ATTEMPTS = 2;

		/// <summary>
		/// Wait time for the 
		/// <see cref="Ipc.NamedPipeNative.WaitNamedPipe">NamedPipeNative.WaitNamedPipe</see> 
		/// operation.
		/// </summary>
		private const int WAIT_TIME = 5000;

		/// <summary>
		/// Reads a string from a named pipe using the UTF16 encoding.
		/// </summary>
		/// <param name="handle">The pipe handle.</param>
		/// <param name="maxBytes">The maximum bytes to read.</param>
		/// <returns>A UTF16 string.</returns>
		/// <remarks>This function uses 
		/// <see cref="Ipc.NamedPipeWrapper.ReadBytes">AppModule.Ipc.ReadBytes</see> 
		/// to read the bytes from the pipe and then converts them to string.<br/><br/>
		/// The first four bytes of the pipe data are expected to contain 
		/// the data length of the message. This method first reads those four 
		/// bytes and converts them to integer. It then continues to read from the pipe using 
		/// the extracted data length.
		/// </remarks>
		public static string Read(PipeHandle handle, int maxBytes)
		{
			var returnVal = "";
			var bytes = ReadBytes(handle, maxBytes);
			if (bytes != null)
				returnVal = Encoding.Unicode.GetString(bytes);
			return returnVal;
		}

		/// <summary>
		/// Reads the bytes from a named pipe.
		/// </summary>
		/// <param name="handle">The pipe handle.</param>
		/// <param name="maxBytes">The maximum bytes to read.</param>
		/// <returns>An array of bytes.</returns>
		/// <remarks>This method expects that the first four bytes in the pipe define 
		/// the length of the data to read. If the data length is greater than 
		/// <b>maxBytes</b> the method returns null.<br/><br/>
		/// The first four bytes of the pipe data are expected to contain 
		/// the data length of the message. This method first reads those four 
		/// bytes and converts them to integer. It then continues to read from the pipe using 
		/// the extracted data length.
		/// </remarks>
		public static byte[] ReadBytes(PipeHandle handle, int maxBytes)
		{
			var numReadWritten = new byte[4];
			var intBytes = new byte[4];

			// Set the Handle state to Reading
			handle.State = InterProcessConnectionState.Reading;
			// Read the first four bytes and convert them to integer
			if (NamedPipeNative.ReadFile(handle.Handle, intBytes, 4, numReadWritten, 0))
			{
				var len = BitConverter.ToInt32(intBytes, 0);
				var msgBytes = new byte[len];
				// Read the rest of the data
				if (NamedPipeNative.ReadFile(handle.Handle, msgBytes, (uint)len, numReadWritten, 0))
				{
					handle.State = InterProcessConnectionState.ReadData;
					if (len > maxBytes)
						return null;
					return msgBytes;
				}
			}

			handle.State = InterProcessConnectionState.Error;
			var error = NamedPipeNative.GetLastError();
			throw new NamedPipeIOException("Error reading from pipe. Internal error: " + error,
				error);
		}

		/// <summary>
		/// Writes a string to a named pipe.
		/// </summary>
		/// <param name="handle">The pipe handle.</param>
		/// <param name="text">The text to write to the pipe.</param>
		/// <remarks>This method converts the text into an array of bytes, using the 
		/// UTF16 encoding and the uses 
		/// <see cref="Ipc.NamedPipeWrapper.WriteBytes">AppModule.Ipc.WriteBytes</see>
		/// to write to the pipe.<br/><br/>
		/// When writing to a pipe the method first writes four bytes that define the data length.
		/// It then writes the whole message.</remarks>
		public static void Write(PipeHandle handle, string text)
		{
			WriteBytes(handle, Encoding.Unicode.GetBytes(text));
		}

		/// <summary>
		/// Writes an array of bytes to a named pipe.
		/// </summary>
		/// <param name="handle">The pipe handle.</param>
		/// <param name="bytes">The bytes to write.</param>
		/// <remarks>If we try bytes array we attempt to write is empty then this method write a space character to the pipe. This is necessary because the other end of the pipe uses a blocking Read operation so we must write someting.<br/><br/>
		/// The bytes length is restricted by the <b>maxBytes</b> parameter, which is done primarily for security reasons.<br/><br/>
		/// When writing to a pipe the method first writes four bytes that define the data length.
		/// It then writes the whole message.</remarks>
		public static void WriteBytes(PipeHandle handle, byte[] bytes)
		{
			var numReadWritten = new byte[4];

			if (bytes == null || bytes.Length == 0)
				bytes = Encoding.Unicode.GetBytes(" ");

			// Get the message length
			var len = (uint)bytes.Length;
			handle.State = InterProcessConnectionState.Writing;
			// Write four bytes that define the message length
			if (NamedPipeNative.WriteFile(handle.Handle, BitConverter.GetBytes(len), 4, numReadWritten, 0))
				// Write the whole message
				if (NamedPipeNative.WriteFile(handle.Handle, bytes, len, numReadWritten, 0))
				{
					handle.State = InterProcessConnectionState.Flushing;
					Flush(handle);
					handle.State = InterProcessConnectionState.FlushedData;
					return;
				}

			handle.State = InterProcessConnectionState.Error;
			var error = NamedPipeNative.GetLastError();
			throw new NamedPipeIOException("Error writing to pipe. Internal error: " + error,
				error);
		}

		/// <summary>
		/// Tries to connect to a named pipe on the same machine.
		/// </summary>
		/// <param name="pipeName">The name of the pipe.</param>
		/// <param name="handle">The resulting pipe handle.</param>
		/// <returns>Return true if the attempt succeeds.</returns>
		/// <remarks>This method is used mainly when stopping the pipe server. It unblocks the existing pipes, which wait for client connection.</remarks>
		public static bool TryConnectToPipe(string pipeName, out PipeHandle handle)
		{
			return TryConnectToPipe(pipeName, ".", out handle);
		}

		/// <summary>
		/// Tries to connect to a named pipe.
		/// </summary>
		/// <param name="pipeName">The name of the pipe.</param>
		/// <param name="serverName">The name of the server.</param>
		/// <param name="handle">The resulting pipe handle.</param>
		/// <returns>Return true if the attempt succeeds.</returns>
		/// <remarks>This method is used mainly when stopping the pipe server. It unblocks the existing pipes, which wait for client connection.</remarks>
		public static bool TryConnectToPipe(string pipeName, string serverName, out PipeHandle handle)
		{
			// Build the pipe name string
			var name = string.Format(@"\\{0}\pipe\{1}", serverName, pipeName);

			// Try to connect to a server pipe
			handle = new PipeHandle(
				NamedPipeNative.CreateFile(name, NamedPipeNative.GENERIC_READ | NamedPipeNative.GENERIC_WRITE, 0,
					null, NamedPipeNative.OPEN_EXISTING, 0, 0),
				InterProcessConnectionState.ConnectingToServer);

			if (handle.Handle.ToInt32() != NamedPipeNative.INVALID_HANDLE_VALUE)
			{
				handle.State = InterProcessConnectionState.ConnectedToServer;
				return true;
			}
			handle.State = InterProcessConnectionState.Error;
			return false;
		}

		/// <summary>
		/// Connects to a server named pipe on the same machine.
		/// </summary>
		/// <param name="pipeName">The pipe name.</param>
		/// <returns>The pipe handle, which also contains the pipe state.</returns>
		/// <remarks>This method is used by clients to establish a pipe connection with a server pipe.</remarks>
		public static PipeHandle ConnectToPipe(string pipeName)
		{
			return ConnectToPipe(pipeName, ".");
		}

		/// <summary>
		/// Connects to a server named pipe.
		/// </summary>
		/// <param name="pipeName">The pipe name.</param>
		/// <param name="serverName">The server name.</param>
		/// <returns>The pipe handle, which also contains the pipe state.</returns>
		/// <remarks>This method is used by clients to establish a pipe connection with a server pipe.</remarks>
		public static PipeHandle ConnectToPipe(string pipeName, string serverName)
		{
			// Build the name of the pipe.
			var name = @"\\" + serverName + @"\pipe\" + pipeName;

			for (var i = 1; i <= ATTEMPTS; i++)
			{
				// Try to connect to the server
				var handle = new PipeHandle(
					NamedPipeNative.CreateFile(name, NamedPipeNative.GENERIC_READ | NamedPipeNative.GENERIC_WRITE,
						0, null, NamedPipeNative.OPEN_EXISTING, 0, 0),
					InterProcessConnectionState.ConnectingToServer);

				if (handle.Handle.ToInt32() != NamedPipeNative.INVALID_HANDLE_VALUE)
				{
					// The client managed to connect to the server pipe
					handle.State = InterProcessConnectionState.ConnectedToServer;

					// Set the read mode of the pipe channel
					var mode = NamedPipeNative.PIPE_READMODE_MESSAGE;
					if (NamedPipeNative.SetNamedPipeHandleState(handle.Handle, ref mode, IntPtr.Zero, IntPtr.Zero))
						return handle;

					if (i >= ATTEMPTS)
					{
						handle.State = InterProcessConnectionState.Error;
						throw new NamedPipeIOException(
							"Error setting read mode on pipe " + name + " . Internal error: " +
								NamedPipeNative.GetLastError(), NamedPipeNative.GetLastError());
					}
				}
				if (i >= ATTEMPTS)
					if (NamedPipeNative.GetLastError() != NamedPipeNative.ERROR_PIPE_BUSY)
					{
						handle.State = InterProcessConnectionState.Error;
						// After a certain number of unsuccessful attempt raise an exception
						throw new NamedPipeIOException(
							"Error connecting to pipe " + name + " . Internal error: " +
								NamedPipeNative.GetLastError(), NamedPipeNative.GetLastError());
					}
					else
					{
						handle.State = InterProcessConnectionState.Error;
						throw new NamedPipeIOException(
							"Pipe " + name + " is too busy. Internal error: " + NamedPipeNative.GetLastError(),
							NamedPipeNative.GetLastError());
					}
				if (NamedPipeNative.GetLastError() == NamedPipeNative.ERROR_PIPE_BUSY)
					NamedPipeNative.WaitNamedPipe(name, WAIT_TIME);
				// The pipe is busy so lets wait for some time and try again
			}

			return new PipeHandle(InterProcessConnectionState.ConnectingToServer);
		}

		/// <summary>
		/// Creates a server named pipe.
		/// </summary>
		/// <param name="name">The name of the pipe.</param>
		/// <param name="outBuffer">The size of the outbound buffer.</param>
		/// <param name="inBuffer">The size of the inbound buffer.</param>
		/// <returns>The pipe handle.</returns>
		public static PipeHandle Create(string name, uint outBuffer, uint inBuffer)
		{
			name = @"\\.\pipe\" + name;
			for (var i = 1; i <= ATTEMPTS; i++)
			{
				var handle = new PipeHandle(NamedPipeNative.CreateNamedPipe(
					name,
					NamedPipeNative.PIPE_ACCESS_DUPLEX,
					NamedPipeNative.PIPE_TYPE_MESSAGE | NamedPipeNative.PIPE_READMODE_MESSAGE |
						NamedPipeNative.PIPE_WAIT,
					NamedPipeNative.PIPE_UNLIMITED_INSTANCES,
					outBuffer,
					inBuffer,
					NamedPipeNative.NMPWAIT_WAIT_FOREVER,
					IntPtr.Zero),
					InterProcessConnectionState.Creating);
				if (handle.Handle.ToInt32() != NamedPipeNative.INVALID_HANDLE_VALUE)
				{
					handle.State = InterProcessConnectionState.Created;
					return handle;
				}
				if (i >= ATTEMPTS)
				{
					handle.State = InterProcessConnectionState.Error;
					throw new NamedPipeIOException(
						string.Format("Error creating named pipe {0} . Internal error: {1}", name, NamedPipeNative.GetLastError()), NamedPipeNative.GetLastError());
				}
			}

			return new PipeHandle(InterProcessConnectionState.Creating);
		}

		/// <summary>
		/// Starts waiting for client connections.
		/// </summary>
		/// <remarks>
		/// Blocks the current execution until a client pipe attempts to establish a connection.
		/// </remarks>
		/// <param name="handle">The pipe handle.</param>
		public static void Connect(PipeHandle handle)
		{
			handle.State = InterProcessConnectionState.WaitingForClient;
			var connected = NamedPipeNative.ConnectNamedPipe(handle.Handle, null);
			handle.State = InterProcessConnectionState.ConnectedToClient;
			if (!connected && NamedPipeNative.GetLastError() != NamedPipeNative.ERROR_PIPE_CONNECTED)
			{
				handle.State = InterProcessConnectionState.Error;
				throw new NamedPipeIOException(
					"Error connecting pipe. Internal error: " + NamedPipeNative.GetLastError(),
					NamedPipeNative.GetLastError());
			}
		}

		/// <summary>
		/// Returns the number of instances of a named pipe.
		/// </summary>
		/// <param name="handle">The pipe handle.</param>
		/// <returns>The number of instances.</returns>
		public static uint NumberPipeInstances(PipeHandle handle)
		{
			uint curInstances = 0;

			if (NamedPipeNative.GetNamedPipeHandleState(handle.Handle, IntPtr.Zero, ref curInstances,
				IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero))
				return curInstances;
			throw new NamedPipeIOException(
				"Error getting the pipe state. Internal error: " + NamedPipeNative.GetLastError(),
				NamedPipeNative.GetLastError());
		}

		/// <summary>
		/// Flushes all the data in a named pipe.
		/// </summary>
		/// <param name="handle">The pipe handle.</param>
		public static void Flush(PipeHandle handle)
		{
			handle.State = InterProcessConnectionState.Flushing;
			NamedPipeNative.FlushFileBuffers(handle.Handle);
			handle.State = InterProcessConnectionState.FlushedData;
		}

		/// <summary>
		/// Disconnects a server named pipe from the client.
		/// </summary>
		/// <remarks>
		/// Server pipes can be reused by first disconnecting them from the client and then
		/// calling the <see cref="Ipc.NamedPipeWrapper.Connect">Connect</see>
		/// method to start listening. This improves the performance as it is not necessary
		/// to create new pipe handles.
		/// </remarks>
		/// <param name="handle">The pipe handle.</param>
		public static void Disconnect(PipeHandle handle)
		{
			handle.State = InterProcessConnectionState.Disconnecting;
			NamedPipeNative.DisconnectNamedPipe(handle.Handle);
			handle.State = InterProcessConnectionState.Disconnected;
		}
	}
}