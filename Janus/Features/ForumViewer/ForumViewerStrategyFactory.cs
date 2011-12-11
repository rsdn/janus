using System;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	[ExtensionStrategyFactory]
	public class ForumViewerStrategyFactory : IExtensionStrategyFactory
	{
		public IExtensionAttachmentStrategy[] CreateStrategies(IServiceProvider provider)
		{
			return
				new[]
				{
					provider.CreateStrategy<MessageFormatterInfo, MessageFormatterAttribute>(
						(ctx, attr) => new MessageFormatterInfo(ctx.Type, attr.FormatSource, attr.FormatHtml))
				};
		}
	}
}