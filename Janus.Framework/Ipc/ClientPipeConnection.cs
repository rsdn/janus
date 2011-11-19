using System;

namespace Rsdn.Janus.Framework.Ipc
{
	/// <summary>
	/// Used by client applications to communicate with server ones by using named pipes.
	/// </summary>
	public sealed class ClientPipeConnection : APipeConnection
	{
		/// <summary>
		/// The network name of the server where the server pipe is created.
		/// </summary>
		/// <remarks>
		/// If "." is used as a server name then the pipe is connected to the local machine.
		/// </remarks>
		private readonly string Server = ".";

		/// <summary>
		/// Creates an instance of the ClientPipeConnection assuming that the server pipe
		/// is created on the same machine.
		/// </summary>
		/// <remarks>
		/// The maximum bytes to read from the client is set to be Int32.MaxValue.
		/// </remarks>
		/// <param name="name">The name of the server pipe.</param>
		public ClientPipeConnection(string name)
		{
			_name = name;
			Server = ".";
			_maxReadBytes = Int32.MaxValue;
		}

		/// <summary>
		/// Creates an instance of the ClientPipeConnection specifying the network name
		/// of the server.
		/// </summary>
		/// <remarks>
		/// The maximum bytes to read from the client is set to be Int32.MaxValue.
		/// </remarks>
		/// <param name="name">The name of the server pipe.</param>
		/// <param name="server">The network name of the machine, where the server pipe is created.</param>
		public ClientPipeConnection(string name, string server)
		{
			_name = name;
			Server = server;
			_maxReadBytes = Int32.MaxValue;
		}

		/// <summary>
		/// Closes a client named pipe connection.
		/// </summary>
		/// <remarks>
		/// A client pipe connection is closed by closing the underlying pipe handle.
		/// </remarks>
		public override void Close()
		{
			CheckIfDisposed();
			_handle.Dispose();
		}

		/// <summary>
		/// Connects a client pipe to an existing server one.
		/// </summary>
		public override void Connect()
		{
			CheckIfDisposed();
			_handle = NamedPipeWrapper.ConnectToPipe(_name, Server);
		}

		/// <summary>
		/// Attempts to establish a connection to the a server named pipe. 
		/// </summary>
		/// <remarks>
		/// If the attempt is successful the method creates the 
		/// field.<br/><br/>
		/// This method is used when it is not known whether a server pipe already exists.
		/// </remarks>
		/// <returns>True if a connection is established.</returns>
		public bool TryConnect()
		{
			CheckIfDisposed();
			var ReturnVal = NamedPipeWrapper.TryConnectToPipe(_name, Server, out _handle);

			return ReturnVal;
		}

		/// <summary>
		/// Object destructor.
		/// </summary>
		~ClientPipeConnection()
		{
			Dispose(false);
		}
	}
}