using System;
using System.Collections.Generic;
using System.Linq;

using Rsdn.Janus.ObjectModel;

namespace Rsdn.Janus
{
	internal class MessageViewerActiveMessageService : IActiveMessagesService
	{
		private readonly MsgViewer _messageViewer;

		public MessageViewerActiveMessageService(MsgViewer messageViewer)
		{
			if (messageViewer == null)
				throw new ArgumentNullException("messageViewer");

			_messageViewer = messageViewer;
		}

		#region IActiveMessageService Members

		public IEnumerable<IMsg> ActiveMessages
		{
			get
			{
				return _messageViewer.Msg != null
					? new[] { _messageViewer.Msg }
					: Enumerable.Empty<IMsg>();
			}
		}

		public event EventHandler ActiveMessagesChanged
		{
			add { _messageViewer.MsgChanged += value; }
			remove { _messageViewer.MsgChanged -= value; }
		}

		#endregion
	}
}