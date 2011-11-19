using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using Microsoft.Win32;

using Rsdn.Janus.Framework.Ipc;

namespace Rsdn.Janus.GoJanusNet
{
	/// <summary>
	/// Вспомогательный COM объект для обработки пунктов контекстного меню IE.
	/// </summary>
	[ComVisible(true)]
	[Guid(ClsId)]
	[ProgId("Janus.GoUrl")]
	public class GoUrl : IGoUrl
	{
		#region Private members
		private const string _pipeName = "JanusPipe";
		private const string _registryKeyPrefix = @"Software\Microsoft\Internet Explorer\MenuExt\";

		private const string _registryKeySendUrl =
			_registryKeyPrefix + "RSDN@Home: загрузить тему при след.синхр.";

		private const string _registryKeyShowMessage =
			_registryKeyPrefix + "RSDN@Home: открыть сообщение в клиенте";

		public const string ClsId = "61D7E14A-F91B-4db4-A97F-340218484E86";

		private static readonly object _filesCreateLock = new object();

		[Flags]
		private enum IEContext
		{
			Unknown = 0x00,
			//Default = 0x01,
			//Image = 0x02,
			//Control = 0x04,
			//Table = 0x08,
			//TextSelect = 0x10,
			Anchor = 0x20
		}
		#endregion

		#region IGoUrl Members
		public void SendURLToJanus(int messageID, string linkText)
		{
			if (string.IsNullOrEmpty(linkText))
				throw new ArgumentException("linkText");

			WriteToPipe(string.Format(
				"<download-topic><message-id>{0}</message-id><hint>{1}</hint></download-topic>",
				messageID, linkText));
		}

		public void ShowMessageInJanus(int messageID)
		{
			WriteToPipe(string.Format(
				"<goto-message><message-id>{0}</message-id></goto-message>",
				messageID));
		}
		#endregion

		#region Install / Uninstall
		[ComRegisterFunction]
		internal static void Install(Type t)
		{
			Console.WriteLine("Install GoJanusNet");

			InstallIeContextMenuItem(_registryKeySendUrl, ".html", IEContext.Anchor);
			InstallIeContextMenuItem(_registryKeyShowMessage, "_gm.html", IEContext.Anchor);
		}

		[ComUnregisterFunction]
		internal static void Uninstall(Type t)
		{
			Console.WriteLine("Uninstall GoJanusNet");
			Registry.CurrentUser.DeleteSubKeyTree(_registryKeySendUrl);
			Registry.CurrentUser.DeleteSubKeyTree(_registryKeyShowMessage);
		}

		private static void InstallIeContextMenuItem(string registryKeyName, string suffix,
			IEContext contexts)
		{
			lock (_filesCreateLock)
			{
				try
				{
					var resShowMessageStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(
						"Rsdn.Janus." + typeof (GoUrl).Assembly.ManifestModule.Name + suffix);
					if (resShowMessageStream == null)
						throw new ApplicationException("Could not create resource stream");

					const int copyBufSize = 8192;
					var buffer = new byte[copyBufSize];

					using (var fs = new FileStream(typeof (GoUrl).Assembly.Location + suffix, FileMode.Create))
					{
						int bytesRead;
						while ((bytesRead = resShowMessageStream.Read(buffer, 0, copyBufSize)) != 0)
							fs.Write(buffer, 0, bytesRead);
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}
			}

			using (var rk = Registry.CurrentUser.CreateSubKey(registryKeyName))
			{
				if (rk == null)
					throw new ApplicationException("Could not create registry key");
				rk.SetValue("", typeof (GoUrl).Assembly.CodeBase + suffix);
				if (contexts != IEContext.Unknown)
					rk.SetValue("contexts", (int)contexts);
				rk.SetValue(ClsId, "");
			}
		}
		#endregion

		private static void WriteToPipe(string str)
		{
			using (var cpc = new ClientPipeConnection(_pipeName))
				try
				{
					cpc.Connect();
					cpc.Write(str);
				}
//{{{-EmptyGeneralCatchClause
				catch
//{{{+EmptyGeneralCatchClause
				{}
		}
	}
}