using System;
using System.IO;

namespace Rsdn.Janus.Framework.Ipc
{
	public interface IClientChannel : IDisposable
	{
		string HandleRequest(string request);

		string HandleRequest(Stream request);

		object HandleRequest(object request);

		IClientChannel Create();
	}
}