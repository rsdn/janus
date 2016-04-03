using System;
using System.Collections.Generic;
using System.Linq;

using CodeJam.Extensibility;

namespace Rsdn.Janus
{
	internal class JBrowserActiveMessageService : IActiveMessagesService
	{
		private readonly IServiceProvider _provider;
		private readonly IBrowserFormService _browserFormService;

		public JBrowserActiveMessageService(IServiceProvider provider)
		{
			_provider = provider;
			if (provider == null)
				throw new ArgumentNullException(nameof(provider));

			_browserFormService = provider.GetRequiredService<IBrowserFormService>();
		}

		private int? GetActiveMessageId()
		{
			var info = JanusProtocolInfo.Parse(_browserFormService.Url);
			return info != null
				   && info.ResourceType == JanusProtocolResourceType.Message
				   && info.IsId
			   ? (int?)info.Id : null;
		}

		#region IActiveMessageService Members

		public IEnumerable<IForumMessageInfo> ActiveMessages
		{
			get
			{
				var msgId = GetActiveMessageId();
				if (msgId != null)
				{
					var msg = DatabaseManager.GetMessageWithForum(_provider, msgId.Value);
					if (msg != null)
						return new[] { msg };
				}
				return Enumerable.Empty<IForumMessageInfo>();
			}
		}

		public event EventHandler ActiveMessagesChanged
		{
			add { _browserFormService.Navigated += value; }
			remove { _browserFormService.Navigated -= value; }
		}

		#endregion
	}
}
