using Rsdn.Janus.Framework.Ipc;

namespace GoJanusCmd
{
	internal class SendCommand
	{
		public void Download(int topicId)
		{
			Send(string.Format("<download-topic><message-id>{0}</message-id><hint/></download-topic>",
				topicId));
		}

		public void Go(int topicId)
		{
			Send(string.Format("<goto-message><message-id>{0}</message-id></goto-message>", topicId));
		}

		private void Send(string msg)
		{
			using (var cpc = new ClientPipeConnection("JanusPipe"))
			{
				cpc.Connect();
				cpc.Write(msg);
			}
		}
	}
}