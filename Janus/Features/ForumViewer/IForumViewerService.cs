using System.Collections.Generic;

using Rsdn.Janus.ObjectModel;
using Rsdn.SmartApp;

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