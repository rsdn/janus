using System;
using System.Collections.Generic;
using System.Linq;

namespace Rsdn.Janus
{
	//[MenuProvider]
	//public class HistoryMenuProvider : DynamicMenuProvider
	//{
	//    protected override IMenuRoot CreateMenuCore(IServiceProvider serviceProvider)
	//    {
	//        throw new NotImplementedException();
	//    }

	//    private IEnumerable<IMenuCommand> CreateHistoryMenuCommands(
	//        IEnumerable<MessageViewHistoryEntry> historyEntries)
	//    {
	//        return historyEntries.Select<MessageViewHistoryEntry, IMenuCommand>(
	//            (entry, index) =>
	//                new MenuCommand(
	//                    "Janus.Forum.NavigateBackward",
	//                    new Dictionary<string, object> { { "stepsCount", index } },
	//                    entry.MessageSubject,
	//                    null,
	//                    null,
	//                    MenuItemDisplayStyle.Default,
	//                    index));
	//    }
	//}
}