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
		private readonly int    _id;
		private readonly int _forumId;
		private readonly string _forum;
		private readonly string _subject;
		private readonly int    _reply;
		private readonly bool   _hold;
		private readonly NewMessageCollection _parent;

		internal NewMessage(int id, int forumId, string forum, string subject, int reply,
							bool hold, NewMessageCollection parent)
		{
			_id      = id;
			_forumId = forumId;
			_forum   = forum;
			_subject = subject;
			_reply   = reply;
			_hold    = hold;
			_parent  = parent;
		}

		public int ID
		{
			get { return _id; }
		}

		private string Forum
		{
			get { return _forum; }
		}

		public string Subject
		{
			get { return _subject; }
		}

		public string Message
		{
			get { throw new NotSupportedException(); }
		}

		private int Reply
		{
			get { return _reply; }
		}

		public bool Hold
		{
			get { return _hold; }
		}

		public int ForumId
		{
			get { return _forumId; }
		}

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
		bool ITreeNode.HasChildren
		{
			get { return false; }
		}

		ITreeNode ITreeNode.this[int index]
		{
			get { throw new InvalidOperationException(); }
		}

		NodeFlags ITreeNode.Flags { get; set; }

		ITreeNode ITreeNode.Parent
		{
			get { return _parent; }
		}
		#endregion

		#region ICollection Members
		bool ICollection.IsSynchronized
		{
			get { return false; }
		}

		int ICollection.Count
		{
			get { return 0; }
		}

		void ICollection.CopyTo(Array array, int index)
		{
			throw new NotSupportedException();
		}

		object ICollection.SyncRoot
		{
			get { return this; }
		}
		#endregion

		#region IEnumerable Members
		IEnumerator IEnumerable.GetEnumerator()
		{
			return null;
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
		public string Key
		{
			get { return ID.ToString(); }
		}
		#endregion
	}
}