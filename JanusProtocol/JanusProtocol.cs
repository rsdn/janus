using System;
using System.Runtime.InteropServices;

using Mihailik.InternetExplorer;

using Rsdn.Janus.Framework.Networking;

namespace Rsdn.Janus.Protocol
{
	/// <summary>
	/// Протокол janus://.
	/// </summary>
	public class JanusProtocol : PluggableProtocolHandler
	{
		#region Consts

		private const string _protocolPrefix = "janus";

		#endregion

		#region Fields

		private static IResourceProvider _provider = new JanusPipeResourceProvider();

		#endregion

		#region Methods

		public static void SetDataSource(IResourceProvider provider)
		{
			_provider = provider;
		}

		public static void InstallProtocol()
		{
			NonAdminComRegistration.Register<JanusProtocol>(AssemblyRegistrationFlags.None);
			PluggableProtocolRegistrationServices.RegisterPermanentProtocolHandler<JanusProtocol>(_protocolPrefix);
		}

		public static void UninstallProtocol()
		{
			PluggableProtocolRegistrationServices.UnregisterTemporaryProtocolHandler<JanusProtocol>(_protocolPrefix);
			NonAdminComRegistration.Unregister<JanusProtocol>();
		}

		#endregion

		#region Overrides of PluggableProtocolHandler

		protected override void OnProtocolStart(EventArgs e)
		{
			base.OnProtocolStart(e);

			var resource = _provider.GetData(Request.Url.ToString());

			var bytes = resource.GetPackedBytes();
			var mimeEnd = resource.MimeEnd;

			Response.ContentType = resource.MimeType;
			Response.OutputStream.Write(bytes, mimeEnd, bytes.Length - mimeEnd);
			Response.EndResponse();
		}

		#endregion
	}
}