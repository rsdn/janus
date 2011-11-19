using System;

namespace Rsdn.Janus.Framework.Ipc
{
	public interface IInterProcessConnection : IDisposable
	{
		void Connect();

		void Close();

		string Read();

		byte[] ReadBytes();

		void Write(string text);

		void WriteBytes(byte[] bytes);

		InterProcessConnectionState GetState();
	}
}