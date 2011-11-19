using System;
using System.Collections;

using Rsdn.TreeGrid;

namespace Rsdn.Janus
{
	/// <summary>
	/// Поставленная оценка.
	/// </summary>
	[OutboxItemEditor(typeof(RateMarkEditor))]
	public class RateMark : IOutboxRate, ITreeNode, IGetData, IKeyedNode
	{
		private readonly RateMarkCollection _parent;
		private readonly int _id;
		private readonly MessageRates _rateType;
		private readonly string _userNick;
		private readonly string _subject;
		private readonly string _forumName;

		public RateMark(
			RateMarkCollection parent,
			int id,
			MessageRates rateType,
			string userNick,
			string subject,
			string forumName)
		{
			_parent = parent;
			_id = id;
			_rateType = rateType;
			_userNick = userNick;
			_subject = subject;
			_forumName = forumName;
		}

		public int ID
		{
			get { return _id; }
		}

		public MessageRates RateType
		{
			get { return _rateType; }
		}

		public string UserNick
		{
			get { return _userNick; }
		}

		public string Subject
		{
			get { return _subject; }
		}

		public string ForumName
		{
			get { return _forumName; }
		}

		internal void Delete()
		{
			_parent.Delete(this);
		}

		private int GetImage()
		{
			switch (RateType)
			{
				case MessageRates.Rate1:      return OutboxImageManager.Rate1ImageIndex;
				case MessageRates.Rate2:      return OutboxImageManager.Rate2ImageIndex;
				case MessageRates.Rate3:      return OutboxImageManager.Rate3ImageIndex;
				case MessageRates.Agree:      return OutboxImageManager.RateAgreeImageIndex;
				case MessageRates.DisAgree:   return OutboxImageManager.RateDisagreeImageIndex;
				case MessageRates.Smile:      return OutboxImageManager.RateSmileImageIndex;
				case MessageRates.Plus1:      return OutboxImageManager.RatePlus1ImageIndex;
				case MessageRates.DeleteRate: return OutboxImageManager.RateDeleteImageIndex;
				default:
					return -1;
			}
		}

		#region ITreeNode Members
		bool ITreeNode.HasChildren
		{
			get { return false; }
		}

		ITreeNode ITreeNode.this[int iIndex]
		{
			get { throw new NotSupportedException(); }
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
		void IGetData.GetData(NodeInfo nodeInfo, CellInfo[] cellData)
		{
			cellData[OutboxManager.ForumColumn].Text       = ForumName;
			cellData[OutboxManager.ForumColumn].ImageIndex = GetImage();

			cellData[OutboxManager.SubjectColun].Text = Subject;

			cellData[OutboxManager.AddInfoColumn].Text = UserNick;
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