using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	internal class CommandTargetStrategy : RegElementsStrategy<CommandTargetInfo, CommandTargetAttribute>
	{
		public CommandTargetStrategy(IServicePublisher publisher)
			: base(publisher) { }

		public override CommandTargetInfo CreateElement(
			ExtensionAttachmentContext context,
			CommandTargetAttribute attr)
		{
			return new CommandTargetInfo(context.Type);
		}
	}
}