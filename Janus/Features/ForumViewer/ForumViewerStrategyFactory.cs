using System;

using CodeJam.Extensibility;
using CodeJam.Extensibility.StratFactories;

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
						(ctx, attr) => new MessageFormatterInfo(ctx.Type, attr.FormatSource, attr.FormatHtml)),
					provider.CreateStrategy<AutocompleteProviderInfo, AutocompleteProviderAttribute>(
						(ctx, attr) => new AutocompleteProviderInfo(ctx.Type)),
					provider.CreateStrategy<EditorHighlightingProviderInfo, EditorHighlightingProviderAttribute>(
						(ctx, attr) => new EditorHighlightingProviderInfo(ctx.Type))
				};
		}
	}
}