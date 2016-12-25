using System;
using System.Collections;

using Rsdn.TreeGrid;

namespace Rsdn.Janus
{
	/// <summary>
	/// Новое сообщение.
	/// </summary>
	[OutboxItemEditor(typeof (NewMessageEditor))]
	public class NewMessage : IOutboxMessage, ITreeNode, IGetData, IKeyedNode
	{
		private readonly NewMessageCollection _parent;

		internal NewMessage(
			int id,
			int forumId,
			string forum,
			string subject,
			int reply,
			bool hold,
			string tags,
			NewMessageCollection parent)
		{
			ID      = id;
			ForumId = forumId;
			Forum   = forum;
			Subject = subject;
			Reply   = reply;
			Hold    = hold;
			_parent  = parent;
		}

		public int ID { get; }

		private string Forum { get; }

		public string Subject { get; }

		public string Message
		{
			get { throw new NotSupportedException(); }
		}

		private int Reply { get; }

		public bool Hold { get; }

		public int ForumId { get; }

		public int ReplyId
		{
			get { throw new NotSupportedException(); }
		}

		internal void Delete()
		{
			_parent.Delete(this);
		}

		private int GetImage()
		{
			if (Reply > 0)
			{
				return Hold
					? OutboxImageManager.MsgWaitReplyImageIndex
					: OutboxImageManager.MsgReplyImageIndex;
			}

			return Hold 
				? OutboxImageManager.MsgWaitImageIndex 
				: OutboxImageManager.MsgImageIndex;
		}

		#region ITreeNode Members
		bool ITreeNode.HasChildren => false;

		ITreeNode ITreeNode.this[int index]
		{
			get { throw new InvalidOperationException(); }
		}

		NodeFlags ITreeNode.Flags { get; set; }

		ITreeNode ITreeNode.Parent => _parent;
		#endregion

		#region ICollection Members
		bool ICollection.IsSynchronized => false;

		int ICollection.Count => 0;

		void ICollection.CopyTo(Array array, int index)
		{
			throw new NotSupportedException();
		}

		object ICollection.SyncRoot => this;
		#endregion

		#region IEnumerable Members
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}
		#endregion

		#region IGetData Members
		public void GetData(NodeInfo nodeInfo, CellInfo[] cellData)
		{
			cellData[OutboxManager.ForumColumn].Text = Forum;
			cellData[OutboxManager.ForumColumn].ImageIndex = GetImage();

			cellData[OutboxManager.SubjectColun].Text = Subject;
		}
		#endregion

		#region IKeyedNode Members
		public string Key => ID.ToString();

		public string Tags { get; set; }
		#endregion
	}
}