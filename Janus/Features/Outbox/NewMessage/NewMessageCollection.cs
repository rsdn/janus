using System;
using System.Collections;
using System.Collections.Generic;

using LinqToDB;

using Rsdn.TreeGrid;

using System.Linq;

namespace Rsdn.Janus
{
	/// <summary>
	/// Коллекция новых сообщений.
	/// </summary>
	public class NewMessageCollection
		: IOutboxMessageCollection,
			ITreeNode,
			IGetData,
			IOutboxCollection,
			IKeyedNode,
			IEnumerable<NewMessage>
	{
		private readonly IServiceProvider _provider;
		private readonly OutboxManager _manager;
		private List<NewMessage> _messages;

		internal NewMessageCollection(IServiceProvider provider, OutboxManager manager)
		{
			_provider = provider;
			_manager = manager;
		}

		public void Delete(NewMessage msg)
		{
			using (var db = _provider.CreateDBContext())
				db.OutboxMessages(m => m.ID == msg.ID).Delete();
			Refresh();
		}

		public void Refresh()
		{
			_messages = null;
			_manager.Renew();
		}

		public void Add(MessageInfo msgInfo)
		{
			OutboxHelper.AddOutboxMessage(_provider, msgInfo);
			Refresh();

			var maxId = int.MinValue;
			foreach (var newMessage in this)
				if (newMessage.ID > maxId)
					maxId = newMessage.ID;
		}

		private readonly object _loadFlag = new object();
		private void CheckLoad()
		{
			if (_messages == null)
				lock (_loadFlag)
					if (_messages == null)
						Load();
		}

		private void Load()
		{
			using (var db = _provider.CreateDBContext())
				_messages =
					db
						.OutboxMessages()
						.OrderByDescending(m => m.ID)
						.Select(
							m =>
								new NewMessage(
									m.ID,
									m.ForumID,
									m.ServerForum.Name,
									m.Subject,
									m.ReplyToID,
									m.Hold,
									this))
						.ToList();
		}

		#region ITreeNode Members
		bool ITreeNode.HasChildren
		{
			get
			{
				CheckLoad();
				return _messages.Count > 0;
			}
		}

		ITreeNode ITreeNode.this[int index]
		{
			get
			{
				CheckLoad();
				return _messages[index];
			}
		}

		NodeFlags ITreeNode.Flags { get; set; }

		ITreeNode ITreeNode.Parent
		{
			get { return _manager; }
		}
		#endregion

		#region ICollection Members
		bool ICollection.IsSynchronized
		{
			get { return false; }
		}

		public int Count
		{
			get
			{
				CheckLoad();
				return _messages.Count;
			}
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
		public IEnumerator<IOutboxMessage> GetEnumerator()
		{
			CheckLoad();
			return _messages.Cast<IOutboxMessage>().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			CheckLoad();
			return _messages.GetEnumerator();
		}
		#endregion

		#region IGetData Members
		public void GetData(NodeInfo nodeInfo, CellInfo[] cellData)
		{
			cellData[OutboxManager.ForumColumn].Text = Name;
			cellData[OutboxManager.ForumColumn].ImageIndex = 
				OutboxImageManager.MsgFolderImageIndex;
		}
		#endregion

		#region IOutboxCollection Members
		public string Name
		{
			get { return SR.Outbox.NewMessageCollection; }
		}
		#endregion

		#region IKeyedNode Members
		public string Key
		{
			get { return GetType().Name; }
		}
		#endregion

		#region IEnumerable<NewMessage> Members
		///<summary>
		///Returns an enumerator that iterates through the collection.
		///</summary>
		IEnumerator<NewMessage> IEnumerable<NewMessage>.GetEnumerator()
		{
			CheckLoad();
			return _messages.GetEnumerator();
		}
		#endregion
	}
}
