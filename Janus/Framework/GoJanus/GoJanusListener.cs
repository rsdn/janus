using System;
using System.Threading;
using System.Xml;

using CodeJam.Extensibility;

using Rsdn.Janus.Framework;
using Rsdn.Janus.Framework.Ipc;
using Rsdn.Janus.Log;

namespace Rsdn.Janus
{
	/// <summary>
	/// Слушает именованный канал на предмет передачи в приложение тем
	/// для загрузки.
	/// </summary>
	internal class GoJanusListener
	{
		private readonly IServiceProvider _provider;

		#region Private members
		private const string _pipeName = "JanusPipe";

		private readonly ServerPipeConnection _connection;
		#endregion

		#region Ctor
		private GoJanusListener(IServiceProvider provider)
		{
			_provider = provider;
			_connection = new ServerPipeConnection(_pipeName, 512, 512, 5000);
			var th = new Thread(Listen)
				{
					IsBackground = true,
					Priority = ThreadPriority.Lowest,
					CurrentUICulture = Thread.CurrentThread.CurrentUICulture
				};

			th.Start();
		}
		#endregion

		public static void Start(IServiceProvider provider)
		{
			new GoJanusListener(provider);
		}

		#region ProcessRequest & Listen
		private void ProcessRequest(string request)
		{
			try
			{
				var document = new XmlDocument();
				document.LoadXml(request);

				if (document.DocumentElement != null)
					switch (document.DocumentElement.Name)
					{
						case "goto-message":
							GotoMessage(document);
							break;
						case "download-topic":
							DownloadTopic(document);
							break;
						case "protocol-request":
							ProtocolRequest(document);
							break;
					}
			}
			catch (Exception ex)
			{
				_provider
					.LogWarning(
						string.Format(SR.GoJanusListener.InvalidReceivedMessage, ex.Message));
			}
		}

		private void Listen()
		{
			while (true)
				try
				{
					_connection.Connect();
					ProcessRequest(_connection.Read());
				}
				catch (Exception ex)
				{
					_provider.LogWarning(
						string.Format(SR.GoJanusListener.PipeListenerErrorMessage, ex.Message));
				}
				finally
				{
					_connection.Disconnect();
				}
		}
		#endregion

		#region Protocol Handlers
		private void DownloadTopic(XmlNode document)
		{
			var mid = int.Parse(document.SelectSingleNode("/download-topic/message-id").InnerText);
			var hint = document.SelectSingleNode("/download-topic/hint").InnerText;

			_provider
				.GetRequiredService<IOutboxManager>()
				.DownloadTopics
				.Add(SR.Forum.DownloadTopicIESource, mid, hint);
		}

		private void ProtocolRequest(XmlNode document)
		{
			var path = document.SelectSingleNode("/protocol-request/path").InnerText;

			_connection.WriteBytes(
				ApplicationManager
					.Instance
					.ProtocolDispatcher
					.DispatchRequest(path)
					.GetPackedBytes());
		}

		private void GotoMessage(XmlNode document)
		{
			var mid = int.Parse(document.SelectSingleNode("/goto-message/message-id").InnerText);

			WindowActivator.ActivateWindow(MainForm.GetCaption());

			if (!ApplicationManager.Instance.ForumNavigator.SelectMessage(mid))
				_provider
					.GetRequiredService<IOutboxManager>()
					.AddTopicForDownloadWithConfirm(mid);
		}
		#endregion
	}
}