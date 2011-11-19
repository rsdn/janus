using System;
using System.Runtime.CompilerServices;

using Rsdn.TreeGrid;

using System.Collections.Generic;

[assembly: InternalsVisibleTo("Rsdn.Janus.ObjectModel.LinearTreeMsg.TypeBuilder")]
[assembly: InternalsVisibleTo("Rsdn.Janus.ObjectModel.LinearTreeMsg.TypeAccessor")]

namespace Rsdn.Janus
{
	internal class LinearTreeMsg : MsgBase, ITreeNode
	{
		public LinearTreeMsg(IServiceProvider provider) : base(provider)
		{
		}

		protected override void GetDataExt(CellInfo[] aryCellData)
		{
			var forum = Forums.Instance[ForumID];
			aryCellData[ExtInfoColumn].Text = forum != null 
												? forum.DisplayName 
												: string.Empty;
		}

		protected override IMsg GetTopic()
		{
			return DatabaseManager.GetMessageWithForum(ServiceProvider, TopicID);
		}

		protected override int GetTopicId()
		{
			return TopicIDInternal;
		}

		protected override void FillChildren()
		{
		}

		ITreeNode ITreeNode.Parent
		{
			get { return Parent; }
		}
	}

	public class RootNode : List<ITreeNode>, ITreeNode, IGetData
	{
		public RootNode(IEnumerable<ITreeNode> nodes)
			: base(nodes)
		{
		}

		public bool HasChildren
		{
			get { return Count > 0; }
		}

		public NodeFlags Flags { get; set; }

		public ITreeNode Parent
		{
			get { return null; }
		}

		public void GetData(NodeInfo nodeInfo, CellInfo[] aryCellData)
		{
		}
	}

}