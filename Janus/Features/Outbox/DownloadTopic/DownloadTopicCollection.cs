using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using LinqToDB;

using Rsdn.TreeGrid;

namespace Rsdn.Janus
{
	/// <summary>
	/// Коллекция топиков для скачивания.
	/// </summary>
	public class DownloadTopicCollection :
		IDownloadTopicCollection,
		IEnumerable<DownloadTopic>, 
		ITreeNode, IGetData, IOutboxCollection, IKeyedNode
	{
		private volatile List<DownloadTopic> _topicList;
		private readonly IServiceProvider _provider;
		private readonly OutboxManager _manager;

		internal DownloadTopicCollection(IServiceProvider provider, OutboxManager manager)
		{
			_provider = provider;
			_manager = manager;
		}

		private List<DownloadTopic> TopicList
		{
			get
			{
				if (_topicList == null)
					lock (_loadFlag)
						if (_topicList == null)
							using (var db = _provider.CreateDBContext())
							{
								var q =
									db
										.DownloadTopics()
										.Select(
											dt =>
												new DownloadTopicImpl(this)
												{
													ID = dt.ID,
													MessageID = dt.MessageID,
													Source = dt.Source,
													Hint = dt.Hint
												});
								_topicList =
									((IEnumerable<DownloadTopicImpl>)q)
										.Cast<DownloadTopic>()
										.ToList();
							}
				return _topicList;
			}
		}

		public void Refresh()
		{
			_topicList = null;
			_manager.Renew();
		}

		public bool Add(string source, int messageId, string hint)
		{
			// Check for existence
			if (TopicList.Any(dt => dt.MessageID == messageId))
				return false;

			using (var db = _provider.CreateDBContext())
				db
					.DownloadTopics()
						.Value(_ => _.Source,    source)
						.Value(_ => _.MessageID, messageId)
						.Value(_ => _.Hint,      hint)
					.Insert();

			Refresh();

			return true;
		}

		[Obsolete]
		public void Delete(DownloadTopic topic)
		{
			using (var db = _provider.CreateDBContext())
				db.DownloadTopics(dt => dt.ID == topic.ID).Delete();

			Refresh();
		}

		public void Clear(IDataContext db)
		{
			db.DownloadTopics().Delete();

			if (_topicList != null)
				_topicList.Clear();
			_manager.Renew();
		}

		private readonly object _loadFlag = new object();

		#region ICollection Members
		public bool IsSynchronized
		{
			get { return false; }
		}

		public int Count
		{
			get { return TopicList.Count; }
		}

		void ICollection.CopyTo(Array array, int index)
		{
			throw new NotSupportedException();
		}

		public object SyncRoot
		{
			get { return ((IList)TopicList).SyncRoot; }
		}
		#endregion

		#region IEnumerable Members
		IEnumerator<IDownloadTopic> IEnumerable<IDownloadTopic>.GetEnumerator()
		{
			return _topicList.Cast<IDownloadTopic>().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<DownloadTopic> GetEnumerator()
		{
			return TopicList.GetEnumerator();
		}
		#endregion

		#region ITreeNode Members
		bool ITreeNode.HasChildren
		{
			get { return TopicList.Count > 0; }
		}

		ITreeNode ITreeNode.this[int iIndex]
		{
			get { return TopicList[iIndex]; }
		}

		NodeFlags ITreeNode.Flags { get; set; }

		ITreeNode ITreeNode.Parent
		{
			get { return _manager; }
		}
		#endregion

		#region IGetData Members
		public void GetData(NodeInfo nodeInfo, CellInfo[] cellData)
		{
			cellData[OutboxManager.ForumColumn].Text = Name;
			cellData[OutboxManager.ForumColumn].ImageIndex = 
				OutboxImageManager.RegetTopicFolderImageIndex;
		}
		#endregion

		#region IOutboxCollection Members

		public string Name
		{
			get { return SR.Outbox.DownloadTopicsCollection; }
		}

		#endregion

		#region IKeyedNode Members
		public string Key
		{
			get { return GetType().Name; }
		}

		#endregion
	}
}