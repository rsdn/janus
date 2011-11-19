using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class TagsBarProvider : ResourceMenuProvider
	{
		public TagsBarProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Features.ForumViewer.MessageEditor.Menu.TagsBar.xml",
				"Rsdn.Janus.Features.ForumViewer.MessageEditor.Menu.TagsResources") { }
	}
}