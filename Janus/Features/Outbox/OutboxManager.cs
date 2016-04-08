using System;
using System.Collections;
using System.Windows.Forms;

using CodeJam.Extensibility;
using CodeJam.Services;

using Rsdn.TreeGrid;
using Rsdn.Janus.ObjectModel;

namespace Rsdn.Janus
{
	/// <summary>
	/// Управляет исходящими.
	/// </summary>
	[Service(typeof (IOutboxManager))]
	public class OutboxManager : IOutboxManager, IInitable, ITreeNode, IKeyedNode
	{
		private readonly IServiceProvider _serviceProvider;
		internal const int ForumColumn = 0;
		internal const int SubjectColun = 1;
		internal const int AddInfoColumn = 2;

		// Old
		//private const int _bugReportTopicId = 1099540;
		// New (from r830)
		private const int _bugReportTopicId = 2855740;
		private const int _bugReportForumId = 30;

		private volatile OutboxDummyForm _outboxForm;
		private readonly ITreeNode[] _collections;

		public OutboxManager(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
			_newMessages = new NewMessageCollection(serviceProvider, this);
			_rateMarks = new RateMarkCollection(serviceProvider, this);
			_downloadTopics = new DownloadTopicCollection(serviceProvider, this);
			_collections = new ITreeNode[]
				{
					_newMessages, _rateMarks, _downloadTopics
				};
		}

		private readonly object _formLockFlag = new object();

		IOutboxForm IOutboxManager.OutboxForm => OutboxForm;

		public OutboxDummyForm OutboxForm
		{
			get
			{
				if (_outboxForm == null)
					lock (_formLockFlag)
						if (_outboxForm == null)
							_outboxForm = new OutboxDummyForm(_serviceProvider, this);
				return _outboxForm;
			}
		}

		private readonly NewMessageCollection _newMessages;

		IOutboxMessageCollection IOutboxManager.NewMessages => NewMessages;

		private NewMessageCollection NewMessages => _newMessages;

		private readonly RateMarkCollection _rateMarks;

		IOutboxRateCollection IOutboxManager.RateMarks => RateMarks;

		private RateMarkCollection RateMarks => _rateMarks;

		private readonly DownloadTopicCollection _downloadTopics;

		private DownloadTopicCollection DownloadTopics => _downloadTopics;

		IDownloadTopicCollection IOutboxManager.DownloadTopics => _downloadTopics;

		/// <summary>
		/// Перечитать данные из коллекций.
		/// </summary>
		public void Renew()
		{
			_outboxForm?.RefreshList();

			Features.Instance.FeatureChanged(OutboxFeature.Instance);
		}

		private static readonly Hashtable _previewSources
			= new Hashtable();
		private static int _previewSourceCounter;

		/// <summary>
		/// Зарегистрировать источник данных для предварительного просмотра.
		/// </summary>
		public static int RegisterPreviewSource(IPreviewSource src)
		{
			var num = _previewSourceCounter++;
			_previewSources.Add(num, src);
			return num;
		}

		/// <summary>
		/// Убрать регистрацию источника данных для предварительного просмотра.
		/// </summary>
		/// <param name="num"></param>
		public static void UnregisterPreviewSource(int num)
		{
			_previewSources.Remove(num);
		}

		/// <summary>
		/// Получить данные предварительного просмотра.
		/// </summary>
		public static string GetPreviewData(int num)
		{
			if (!_previewSources.Contains(num))
				return null;

			return ((IPreviewSource)_previewSources[num]).GetData();
		}

		public void AddBugReport(
			string bugName,
			string bugDescription,
			string stackTrace,
			bool showEditor)
		{
			var mi = new MessageInfo(_bugReportForumId, _bugReportTopicId,
				bugName, string.Format(
					SR.Outbox.BugReportTemplate,
					bugName, bugDescription,
					ApplicationInfo.NameWithVersion,
					Config.Instance.DbDriver,
					stackTrace));

			if (showEditor)
				MessageEditor.EditMessage(_serviceProvider, MessageFormMode.Add, mi);
			else
				NewMessages.Add(mi);
		}

		/// <summary>
		/// Выводит окно с запросом на добавление темы в очередь загрузки.
		/// </summary>
		/// <param name="mid">ID сообщения</param>
		public void AddTopicForDownloadWithConfirm(int mid)
		{
			var result = MessageBox.Show(
				SR.MessageAbsentMsgBoxMessage,
				SR.MessageAbsentMsgBoxTitle,
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button1);

			if (result != DialogResult.Yes)
				return;
			var hint = string.Format(SR.Forum.DownloadTopicHint, mid);
			DownloadTopics.Add(SR.Forum.DownloadTopicClickSource, mid, hint);
		}

		/// <summary>
		/// Поместить сообщение в очередь загрузки.
		/// </summary>
		/// <param name="mid"></param>
		public void AddTopicForDownload(int mid)
		{
			var hint = string.Format(SR.Forum.DownloadTopicHint, mid);
			DownloadTopics.Add(SR.Forum.DownloadTopicManualSource, mid, hint);
		}

		#region IInitable Members
		public void Init()
		{
			_serviceProvider
				.GetRequiredService<ISynchronizer>().EndSync.Subscribe(
					arg =>
					{
						_newMessages.Refresh();
						_rateMarks.Refresh();
						_downloadTopics.Refresh();
					});
		}
		#endregion

		#region ITreeNode Members
		bool ITreeNode.HasChildren => true;

		ITreeNode ITreeNode.this[int iIndex] => _collections[iIndex];

		NodeFlags ITreeNode.Flags { get; set; }

		ITreeNode ITreeNode.Parent => null;
		#endregion

		#region ICollection Members
		bool ICollection.IsSynchronized => false;

		int ICollection.Count => _collections.Length;

		void ICollection.CopyTo(Array array, int index)
		{
			throw new NotSupportedException();
		}

		object ICollection.SyncRoot => _collections.SyncRoot;
		#endregion

		#region IEnumerable Members
		IEnumerator IEnumerable.GetEnumerator()
		{
			return _collections.GetEnumerator();
		}
		#endregion

		#region IKeyedNode Members
		string IKeyedNode.Key => GetType().Name;
		#endregion
	}
}