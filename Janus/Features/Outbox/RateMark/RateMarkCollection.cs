using System;
using System.Collections;
using System.Collections.Generic;

using LinqToDB;

using Rsdn.TreeGrid;

using System.Linq;

namespace Rsdn.Janus
{
	/// <summary>
	/// Коллекция поставленнных оценок.
	/// </summary>
	public class RateMarkCollection 
		: IOutboxRateCollection, ITreeNode, IGetData, IOutboxCollection, IKeyedNode
	{
		private readonly IServiceProvider _provider;
		private readonly OutboxManager _manager;
		private List<RateMark> _rateMarks;

		internal RateMarkCollection(IServiceProvider provider, OutboxManager manager)
		{
			_provider = provider;
			_manager = manager;
		}

		public void Refresh()
		{
			_rateMarks = null;
			_manager.Renew();
		}

		public void Add(int mid, MessageRates rate)
		{
			using (var db = _provider.CreateDBContext())
			{
				if (rate == MessageRates.DeleteLocally)
					db
						.OutboxRates(r => r.MessageID == mid)
						.Delete();
				else
				{
					var id =
						db
							.OutboxRates(r => r.RateType == rate && r.MessageID == mid)
							.Select(r => (int?)r.ID)
							.SingleOrDefault();

					if (id == null)
						db
							.OutboxRates()
								.Value(_ => _.RateType,  rate)
								.Value(_ => _.MessageID, mid)
							.Insert();
					else
						db
							.OutboxRates(r => r.ID == id)
							.Set(_ => _.RateType, _ => rate)
							.Update();
				}
			}

			Refresh();
		}

		public void Delete(RateMark rate)
		{
			using (var db = _provider.CreateDBContext())
				db.OutboxRates(r => r.ID == rate.ID).Delete();

			Refresh();
		}

		private readonly object _loadFlag = new object();

		private void CheckLoad()
		{
			if (_rateMarks == null)
			{
				lock (_loadFlag)
					if (_rateMarks == null)
						Load();
			}
		}

		private void Load()
		{
			using (var db = _provider.CreateDBContext())
				_rateMarks =
					db
						.OutboxRates()
						.OrderByDescending(r => r.ID)
						.Select(
							r =>
								new RateMark(
									this,
									r.ID,
									r.RateType,
									r.Message.UserNick,
									r.Message.Subject,
									r.Message.ServerForum.Name))
						.ToList();
		}

		#region ITreeNode Members
		bool ITreeNode.HasChildren
		{
			get
			{
				CheckLoad();
				return _rateMarks.Count > 0;
			}
		}

		ITreeNode ITreeNode.this[int index]
		{
			get
			{
				CheckLoad();
				return _rateMarks[index];
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

		public void Add(IOutboxRate item)
		{
			throw new NotSupportedException();
		}

		public void Clear()
		{
			throw new NotSupportedException();
		}

		public bool Contains(IOutboxRate item)
		{
			throw new NotSupportedException();
		}

		public void CopyTo(IOutboxRate[] array, int arrayIndex)
		{
			throw new NotSupportedException();
		}

		public bool Remove(IOutboxRate item)
		{
			throw new NotSupportedException();
		}

		public int Count
		{
			get
			{
				CheckLoad();
				return _rateMarks.Count;
			}
		}

		public bool IsReadOnly
		{
			get { throw new NotSupportedException(); }
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
		public IEnumerator<IOutboxRate> GetEnumerator()
		{
			return this.Cast<IOutboxRate>().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			CheckLoad();
			return _rateMarks.GetEnumerator();
		}
		#endregion

		#region IGetData Members
		public void GetData(NodeInfo nodeInfo, CellInfo[] cellData)
		{
			cellData[OutboxManager.ForumColumn].Text = Name;
			cellData[OutboxManager.ForumColumn].ImageIndex = 
				OutboxImageManager.RateFolderImageIndex;
		}
		#endregion

		#region IOutboxCollection Members

		public string Name
		{
			get { return SR.Outbox.RateMarkCollection; }
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