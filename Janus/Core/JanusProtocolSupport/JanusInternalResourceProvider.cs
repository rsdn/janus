using Rsdn.Janus.Framework.Networking;

namespace Rsdn.Janus
{
	/// <summary>
	/// Источник ресурсов для работы JanusProtocol в пределах процесса януса.
	/// </summary>
	internal class JanusInternalResourceProvider : IResourceProvider
	{
		#region IResourceProvider Members

		public string Name => "Janus internal resource provider";

		public Resource GetData(string uri)
		{
			return ApplicationManager.Instance.ProtocolDispatcher.DispatchRequest(uri);
		}

		#endregion

	}
}
