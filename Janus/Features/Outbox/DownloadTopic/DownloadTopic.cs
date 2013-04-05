using System;
using System.Collections;

using Rsdn.TreeGrid;

namespace Rsdn.Janus
{
	/// <summary>
	/// Тема для скачивания.
	/// </summary>
	[OutboxItemEditor(typeof(DownloadTopicEditor))]
	public abstract class DownloadTopic : IDownloadTopic, ITreeNode, IGetData, IKeyedNode
	{
		private readonly DownloadTopicCollection _parent;

		protected DownloadTopic(DownloadTopicCollection parent)
		{
			_parent = parent;
		}

		public abstract int ID { get; set; }
		public abstract string Source { get; set; }
		public abstract int MessageID { get; set; }
		public abstract string Hint { get; set; }

		internal void Delete()
		{
			_parent.Delete(this);
		}

		#region ITreeNode Members
		bool ITreeNode.HasChildren
		{
			get { return false; }
		}

		ITreeNode ITreeNode.this[int iIndex]
		{
			get { throw new NotImplementedException(); }
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
			throw new NotImplementedException();
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
			cellData[OutboxManager.ForumColumn].Text = Source;
			cellData[OutboxManager.ForumColumn].ImageIndex = 
				OutboxImageManager.RegetTopicImageIndex;

			cellData[OutboxManager.SubjectColun].Text = Hint;
			
			cellData[OutboxManager.AddInfoColumn].Text = "ID = " + MessageID;
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