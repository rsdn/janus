using System.Collections.Generic;

using CodeJam.Extensibility;

using Rsdn.Janus.ObjectModel;

namespace Rsdn.Janus
{
	internal interface IForumViewerService
	{
		void ExpandUnread();
		void CollapseAndGoRoot();
		void SmartJump();
		void SelectNodeByAttribute(
					ForumDummyForm.StepDirection dir,
					ForumDummyForm.AttrType attrType,
					ForumDummyForm.SearchMessageArea area);
		ICollection<IMsg> SelectedMessages { get; }
		event EventHandler<IForumViewerService> SelectedMessagesChanged;
	}
}