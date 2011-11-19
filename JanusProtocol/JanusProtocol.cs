using System;
using System.Net.Mime;
using System.Runtime.InteropServices;

using Microsoft.Win32;

using Rsdn.Janus.Framework.Networking;

namespace Rsdn.Janus.Protocol
{
	/// <summary>
	/// Протокол janus://.
	/// </summary>
	[ComVisible(true)]
	[Guid(ClsId)]
	public class JanusProtocol : IInternetProtocol
	{
		private const string _registryKey = @"PROTOCOLS\Handler\" + ProtocolPrefix;
		public const string ClsId = "BE42DD14-A1AA-48a2-875A-5E3A02B15D40";
		public const string ProtocolDescription = ProtocolPrefix + ": Janus Data Protocol";
		public const string ProtocolPrefix = "janus";

		private static IResourceProvider _provider = new JanusPipeResourceProvider();

		private ResourceReader _resourceReader;

		#region IInternetProtocol Members
		public void Start(string url, IInternetProtocolSink protSink,
			IInternetBindInfo pOIBindInfo, uint grfPI, uint dwReserved)
		{
			Resource resource;
			try
			{
				resource = _provider.GetData(url);
			}
			catch (Exception e)
			{
				resource = new Resource(MediaTypeNames.Text.Html, e.ToString());
			}

			_resourceReader = resource.GetReader();

			protSink.ReportProgress(BINDSTATUS.BINDSTATUS_VERIFIEDMIMETYPEAVAILABLE, resource.MimeType);
			protSink.ReportData(BSCF.BSCF_DATAFULLYAVAILABLE, 1, 1);
			protSink.ReportResult(InteropConstants.S_OK, 0, null);
		}

		public void Continue(ref _tagPROTOCOLDATA pProtocolData)
		{
			throw new NotImplementedException();
		}

		public void Abort(int hrReason, uint dwOptions)
		{
			throw new NotImplementedException();
		}

		public void Terminate(uint dwOptions)
		{}

		public void Suspend()
		{
			throw new NotImplementedException();
		}

		public void Resume()
		{
			throw new NotImplementedException();
		}

		public int Read(IntPtr pv, uint cb, out uint bytesRead)
		{
			int readCount;
			_resourceReader.Read(pv, (int)cb, out readCount);
			bytesRead = (uint)readCount;
			return InteropConstants.S_FALSE;
		}

		public void Seek(_LARGE_INTEGER dlibMove, uint dwOrigin, out _ULARGE_INTEGER plibNewPosition)
		{
			throw new NotImplementedException();
		}

		public void LockRequest(uint dwOptions)
		{}

		public void UnlockRequest()
		{}
		#endregion

		public static void SetDataSource(IResourceProvider provider)
		{
			_provider = provider;
		}

		[ComRegisterFunction]
		internal static void InstallProtocol(Type t)
		{
			Console.WriteLine("Install {0} protocol", ProtocolPrefix);
			using (var rk = Registry.ClassesRoot.CreateSubKey(_registryKey))
			{
				if (rk == null)
					throw new ApplicationException("Could not create registry key");
				rk.SetValue("", ProtocolDescription);
				rk.SetValue("CLSID", "{" + ClsId + "}");
			}
		}

		[ComUnregisterFunction]
		internal static void UninstallProtocol(Type t)
		{
			Console.WriteLine("Unstall {0} protocol", ProtocolPrefix);
			Registry.ClassesRoot.DeleteSubKeyTree(_registryKey);
		}
	}
}