using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Linq;

using Rsdn.Janus.Log;
using Rsdn.Janus.ObjectModel;
using Rsdn.Shortcuts;
using Rsdn.TreeGrid;

using System.ComponentModel;

using CodeJam.Extensibility;
using CodeJam.Extensibility.Model;
using CodeJam.Services;

namespace Rsdn.Janus
{
	/// <summary>
	/// Контрол, отображающий поиск.
	/// </summary>
	public partial class SearchDummyForm : UserControl, IFeatureView
	{
		private readonly ServiceContainer _serviceManager;
		private readonly AsyncOperation _asyncOperation;
		private StripMenuGenerator _contextMenuGenerator;
		// ReSharper disable ConvertToConstant, RedundantDefaultFieldInitializer
		[ExpectService]
		private readonly IForumImageManager _imgManager = null;
		// ReSharper restore ConvertToConstant, RedundantDefaultFieldInitializer
		private readonly MsgViewer _msgViewer;

		#region Constructor(s) & Dispose
		public SearchDummyForm(IServiceProvider provider)
		{
			_asyncOperation = AsyncHelper.CreateOperation();
			_serviceManager = new ServiceContainer(provider);
			
			_serviceManager.Publish<IDefaultCommandService>(
				new DefaultCommandService("Janus.Forum.GoToMessage"));
			
			this.AssignServices(provider);

			InitializeComponent();

			_msgViewer = new MsgViewer(_serviceManager) { Dock = DockStyle.Fill };
			_splitContainer.Panel2.Controls.Add(_msgViewer);
			CustomInitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			_contextMenuGenerator = new StripMenuGenerator(_serviceManager, _contextMenuStrip, "ForumMessage.ContextMenu");
			base.OnLoad(e);
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">
		/// true if managed resources should be disposed; otherwise, false.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				_contextMenuGenerator.Dispose();
				StyleConfig.StyleChange -= OnStyleChanged;
				_msgViewer.Dispose();
				_asyncOperation.OperationCompleted();
				components.Dispose();
			}
			base.Dispose(disposing);
		}
		#endregion

		#region IFeatureView
		private Form _form;

		void IFeatureView.Activate(IFeature feature)
		{
			comboSearchEntry.Focus();
		}

		void IFeatureView.Refresh()
		{
		}

		void IFeatureView.ConfigChanged()
		{
			//TODO: надо добавить сюда все действия которые нужно сделать при изменении конфига.

			// Подготовка выпадающего списка выбора форума для поиска
			searchInForum.Items.Clear();
			searchInForum.InitForumsComboBox(SR.Search.ForumDescAll,
				Config.Instance.SearchForumId);
		}
		#endregion IFeatureView

		#region Make Accept button on Form to get Enter Key pressing
		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			if (_form != null && _form.AcceptButton == searchButton)
			{
				_form.AcceptButton = null;
			}
		}
		#endregion

		#region Поддержка стилей
		private void OnStyleChanged(object s, StyleChangeEventArgs e)
		{
			UpdateStyle();
			Refresh();
		}

		private void UpdateStyle()
		{
			_tgMsgs.BackColor = Config.Instance.StyleConfig.MessageTreeBack;
			_tgMsgs.Font = Config.Instance.StyleConfig.MessageTreeFont;

			searchButton.Enabled = comboSearchEntry.Text.TrimEnd().Length != 0
				|| Config.Instance.AdvancedSearch;
		}
		#endregion

		#region Private Methods
		private void CustomInitializeComponent()
		{
			Dock = DockStyle.Fill;

			_tgMsgs.Indent = Config.Instance.ForumDisplayConfig.GridIndent;
			_tgMsgs.GridLines = Config.Instance.ForumDisplayConfig.MsgListGridLines;
			//searchInOverquoting.Visible = Config.Instance.IsModerator;

			comboSearchEntry.Text = Config.Instance.SearchText;
			comboSearchEntry.Items.Clear();

			if (Config.Instance.SearchList[0].Length > 0)
				comboSearchEntry.Items.AddRange(Config.Instance.SearchList);

			pnlSearchAdvanced.Visible = Config.Instance.AdvancedSearch;

			StyleConfig.StyleChange += OnStyleChanged;
			UpdateStyle();

			CustomInitializeGrid();


			advancedButton.Text = pnlSearchAdvanced.Visible
				? SR.Search.AdvLeft
				: SR.Search.AdvRight;

			// Задаём начальные значения флажков из конфига
			searchInText.Checked = Config.Instance.SearchInText;
			searchInSubject.Checked = Config.Instance.SearchInSubject;
			searchAuthor.Checked = Config.Instance.SearchAuthor;
			searchInMarked.Checked = Config.Instance.SearchInMarked;
			searchInMyMessages.Checked = Config.Instance.SearchInMyMessages;
			searchAnyWords.Checked = Config.Instance.SearchAnyWord;
			searchInOverquoting.Checked = Config.Instance.SearchInOverquoting;
			searchInQuestions.Checked = Config.Instance.SearchInQuestions;

			// Подготовка выпадающего списка выбора форума для поиска
			searchInForum.InitForumsComboBox(
				SR.Search.ForumDescAll,
				Config.Instance.SearchForumId);
		}

		private void CustomInitializeGrid()
		{
			var columns = _tgMsgs.Columns;

			var imageList = new ImageList { ColorDepth = ColorDepth.Depth32Bit };
			_tgMsgs.SmallImageList = imageList;

			// Картинки в заголовке грида
			columns[1].ImageIndex = imageList.AddImage(_imgManager.GetMarkImage(MessageFlagExistence.OnMessage));
			columns[2].ImageIndex = imageList.AddImage(
				_imgManager.GetMessageImage(
					MessageType.Ordinal,
					MessageFlagExistence.None,
					false,
					MessageFlagExistence.None,
					false));
			columns[3].ImageIndex = imageList.AddImage(_imgManager.GetUserImage(UserClass.User));
			columns[7].ImageIndex = imageList.AddImage(_imgManager.GetMessageDateImage(DateTime.Now));

			// Делаем поддержку локализации для TreeGrid'а
			// В случае изменения TreeGrid'а в Designer'е, необходимо 
			// внести изменения сюда.
			columns[2].Text = SR.TGColumnSubject;
			columns[3].Text = SR.TGColumnAuthor;
			columns[4].Text = SR.TGColumnMessageRate;
			columns[5].Text = SR.TGColumnForumName;
			columns[6].Text = SR.TGColumnAnswers;
			columns[7].Text = SR.TGColumnDate;

			if (Config.Instance.SearchColumnOrder.Length == _tgMsgs.Columns.Count)
				_tgMsgs.ColumnsOrder = Config.Instance.SearchColumnOrder;
			if (Config.Instance.SearchColumnWidth.Length == _tgMsgs.Columns.Count)
				_tgMsgs.ColumnsWidth = Config.Instance.SearchColumnWidth;
		}

		private void FillGrid(IEnumerable<MsgBase> messages)
		{
			_msgViewer.Msg = null;
			_tgMsgs.Nodes = new RootNode(messages);
			_tgMsgs.ActiveNode = null;
		}
		#endregion

		#region Public Members

		public void ClearSearchResult()
		{
			// rameel: Возникала ошибка (ArgumentOutOfRangeException), 
			// если после очистки попытаться кликнуть мышью
			//_tgMsgs.Nodes = null;
			_tgMsgs.Nodes = new RootNode(new ITreeNode[0]);

			_tgMsgs.ActiveNode = null;

			// IT: ворнинг.
			// rameel: ворнингов больше вроде как нет.
			_msgViewer.Msg = null;
			_oldMsgId = -1;
		}

		public IEnumerable<IMsg> SelectedMessages
		{
			get
			{
				return _asyncOperation.Send(() => _tgMsgs.SelectedNodes.OfType<IMsg>().ToArray());
			}
		}

		public event EventHandler SelectedMessagesChanged;

		#endregion

		#region Управляющие команды
		[MethodShortcut(Shortcut.None, "Очистить список", "Очистить список сообщений.")]
		private void ClearMessages()
		{
			_serviceManager.TryExecuteCommand("Janus.Search.ClearSearchResult", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.CtrlR, "Ответ", "Ответ на сообщение.")]
		public void ReplyMessage()
		{
			_serviceManager.TryExecuteCommand("Janus.Forum.OpenMessageInJBrowser", new Dictionary<string, object>());
		}

		// temporary doublicate code with ForumDummyForm 
		[MethodShortcut(Shortcut.CtrlO, "Открыть сообщение",
			"Открыть сообщение в JBrowser.")]
		private void ShowMessageIntoJBrowser()
		{
			_serviceManager.TryExecuteCommand("", new Dictionary<string, object>());
		}

		// temporary doublicate code with ForumDummyForm 
		[MethodShortcut(Shortcut.CtrlShiftO, "Открыть сообщение на рсдн",
			"Открыть сообщение на сайте RSDN.")]
		private void OpenMessage()
		{
			_serviceManager.TryExecuteCommand("Janus.Forum.OpenMessageOnRsdn", new Dictionary<string, object>());
		}

		// temporary doublicate code with ForumDummyForm 
		[MethodShortcut(Shortcut.CtrlM, "Модерирование",
			"Открыть модерирование на сайте RSDN.")]
		private void OpenSelfModerate()
		{
			_serviceManager.TryExecuteCommand("Janus.Forum.OpenModeratingOnRsdn", new Dictionary<string, object>());
		}

		// temporary doublicate code with ForumDummyForm 
		[MethodShortcut(Shortcut.None, "Оценки",
			"Открыть оценки на сайте RSDN.")]
		private void OpenRating()
		{
			_serviceManager.TryExecuteCommand("Janus.Forum.OpenMessageRatingOnRsdn", new Dictionary<string, object>());
		}

		// temporary doublicate code with ForumDummyForm 
		[MethodShortcut(Shortcut.None, "Адрес сообщения",
			"Скопировать адрес сообщения в буфер обмена.")]
		private void CopyMsgAddress()
		{
			_serviceManager.TryExecuteCommand("Janus.Forum.CopyMessageAddress", new Dictionary<string, object>());
		}

		// temporary doublicate code with ForumDummyForm 
		[MethodShortcut(Shortcut.CtrlShiftC, "Адрес автора сообщения",
			"Скопировать адрес автора сообщения в буфер обмена.")]
		private void CopyAuthorAddress()
		{
			_serviceManager.TryExecuteCommand("Janus.Forum.CopyMessageAuthorAddress", new Dictionary<string, object>());
		}
		#endregion

		#region Обработка событий

		private void CbxForumsKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				searchButton.PerformClick();
		}

		private void BtnSearchClick(object sender, EventArgs e)
		{
			var searchText = comboSearchEntry.Text.Trim();

			#region Проверка пустой строки поиска
			if (searchText.Trim().Length == 0
				&& !Config.Instance.AdvancedSearch)
			{
				var text = SR.Search.NoParams;
				_serviceManager.LogInfo(text);

				MessageBox.Show(text, SR.Search.Title,
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			#endregion

			#region Проверка строки поиска
			//TODO валится при вводе ]test[
			// нужно бы еще эту ситуацию доработать
			var cntLeftSquare = 0;
			var cntRightSquare = 0;

			foreach (var t in searchText)
				switch (t)
				{
					case '[':
						cntLeftSquare++;
						break;
					case ']':
						cntRightSquare++;
						break;
				}

			if (cntLeftSquare != cntRightSquare)
			{
				var text = SR.Search.EntryContainsBadCode;
				_serviceManager.LogInfo(text);

				MessageBox.Show(text, SR.Search.Title,
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				comboSearchEntry.Focus();
				return;
			}
			#endregion

			Config.Instance.SearchText = searchText;

			// Чтобы последняя срока поиска была вверху и не дублировалась
			if (comboSearchEntry.Items.Contains(searchText))
				comboSearchEntry.Items.Remove(searchText);

			comboSearchEntry.Items.Insert(0, searchText);
			comboSearchEntry.Text = searchText;

			// Защита от переполнения списка, он не безразмерный
			while (comboSearchEntry.Items.Count > comboSearchEntry.MaxDropDownItems)
				comboSearchEntry.Items.RemoveAt(comboSearchEntry.Items.Count - 1);

			// Сохраним список сразу в конфиг
			Config.Instance.SearchList = new string[comboSearchEntry.Items.Count];
			comboSearchEntry.Items.CopyTo(Config.Instance.SearchList, 0);
			var searchFromValue = searchFromDate.Checked ? searchFromDate.Value : new DateTime(0);
			var searchToValue = searchToDate.Checked ? searchToDate.Value : new DateTime(0);

			List<MsgBase> msgs = null;
			ProgressWorker.Run(_serviceManager, false,
				pi =>
				{
					pi.SetProgressText(SR.Search.Searching);

					try
					{
						msgs =
							SearchHelper
								.SearchMessagesByLucene(
									_serviceManager,
									Config.Instance.AdvancedSearch ? Config.Instance.SearchForumId : -1,
									Config.Instance.SearchText,
									Config.Instance.SearchInText,
									Config.Instance.SearchInSubject,
									Config.Instance.SearchAuthor,
									Config.Instance.SearchInMarked,
									Config.Instance.SearchInMyMessages,
									Config.Instance.SearchAnyWord,
									Config.Instance.SearchInQuestions,
									searchFromValue,
									searchToValue);
					}
					catch (FileNotFoundException)
					{
						_asyncOperation.Post(
							() =>
							{
								if (MessageBox.Show(
										this,
										Search.Search.NoSearchIndexQuestion,
										Application.ProductName,
										MessageBoxButtons.YesNo,
										MessageBoxIcon.Exclamation) == DialogResult.Yes)
									using (var bif = new BuildIndexForm(_serviceManager))
										bif.ShowDialog(this);
							});

						msgs = new List<MsgBase>();
					}

					if (searchInOverquoting.Checked)
						FilterOverquoting(msgs);

					_serviceManager
						.LogInfo(
							msgs.Count > 0
								? string.Format(SR.Search.LogFound, msgs.Count)
								: SR.Search.LogNotFound);

					pi.SetProgressText(SR.Search.OutResults);
				},
				() => FillGrid(msgs));
		}

		private void BtnAdvancedClick(object sender, EventArgs e)
		{
			pnlSearchAdvanced.Visible = !pnlSearchAdvanced.Visible;
			Config.Instance.AdvancedSearch = pnlSearchAdvanced.Visible;

			advancedButton.Text = pnlSearchAdvanced.Visible
				? SR.Search.AdvLeft
				: SR.Search.AdvRight;

			UpdateStyle();
		}

		private void TgMsgsMouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && _tgMsgs.ActiveNode != null)
				_contextMenuStrip.Show((Control)sender, new Point(e.X, e.Y));
		}

		private void TgMsgsDoubleClick(object sender, EventArgs e)
		{
			_serviceManager.ExecuteDefaultCommand();
		}

		private void TgMsgsKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				e.Handled = true;
				_serviceManager.ExecuteDefaultCommand();
			}
		}

		private int _oldMsgId = -1;

		private void TgMsgsAfterActivateNode(ITreeNode activatedNode)
		{
			if (activatedNode == null)
				return;
			var msg = (IMsg)activatedNode;
			if (msg.ID == _oldMsgId)
				return;
			_msgViewer.Msg = msg; //IT: ворнинг.
			_oldMsgId = msg.ID;
			OnSelectedMessagesChanged();
		}

		private void TgMsgsColumnClick(object sender, ColumnClickEventArgs e)
		{
			var sort = Config.Instance.SearchSortCriteria;
			switch (e.Column)
			{
				case 0:
					sort = sort == SortType.ByIdAsc
						? SortType.ByIdDesc
						: SortType.ByIdAsc;
					break;
				case 2:
					sort = sort == SortType.BySubjectAsc
						? SortType.BySubjectDesc
						: SortType.BySubjectAsc;
					break;
				case 3:
					sort = sort == SortType.ByAuthorAsc
						? SortType.ByAuthorDesc
						: SortType.ByAuthorAsc;
					break;
				case 5:
					sort = sort == SortType.ByForumAsc
						? SortType.ByForumDesc
						: SortType.ByForumAsc;
					break;
				case 7:
					sort = sort == SortType.ByDateAsc
						? SortType.ByDateDesc
						: SortType.ByDateAsc;
					break;
			}

			if (Config.Instance.SearchSortCriteria == sort)
				return;
			Config.Instance.SearchSortCriteria = sort;
			var root = _tgMsgs.Nodes as RootNode;

			if (root != null)
			{
				var active = _tgMsgs.ActiveNode;
				_tgMsgs.Nodes = null;

				root.Sort(new MsgComparer<ITreeNode>(sort));

				_tgMsgs.Nodes = root;
				_tgMsgs.ActiveNode = active;
			}
			_tgMsgs.Refresh();
		}

		private void TgMsgsColumnReordered(object sender, ColumnReorderedEventArgs e)
		{
			Config.Instance.SearchColumnOrder = _tgMsgs.ColumnsOrder;
		}

		private void TgMsgsColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
		{
			Config.Instance.SearchColumnWidth = _tgMsgs.ColumnsWidth;
		}

		private void ComboSearchEntryTextChanged(object sender, EventArgs e)
		{
			UpdateStyle();
		}

		private void OnSelectedMessagesChanged()
		{
			SelectedMessagesChanged?.Invoke(this, EventArgs.Empty);
		}
		#endregion

		#region Moderator Help
		private static readonly Regex _quotedLineRx = new Regex(
			@"(?m)^(?'prefix'\s*\S{0,5}?(?'level'>+))[^>]",
			RegexOptions.Compiled);

		private const double _quotaPartLimit = 0.8f;
		private const int _quotaCountLimit = 5;

		private void FilterOverquoting(IList<MsgBase> list)
		{
			for (var i = 0; i < list.Count; i++)
			{
				using (var sr = new StringReader(
					DatabaseManager.GetMessageBody(_serviceManager, list[i].ID)))
				{
					string line;

					var total = 0;
					var quotes = 0;

					while ((line = sr.ReadLine()) != null)
					{
						total++;

						if (_quotedLineRx.Match(line).Success)
							quotes++;
					}

					if ((quotes < _quotaCountLimit) || ((double)quotes / total > _quotaPartLimit))
					{
						list.RemoveAt(i);
						i--;
					}
				}
			}
		}
		#endregion

		#region События controls для улучшенного поиска
		private void ClearButtonClick(object sender, EventArgs e)
		{
			// Очистка элементов панели дополнительного поиска значениями по умолчанию
			searchInText.Checked = true;
			searchInSubject.Checked = false;
			searchAuthor.Checked = false;
			searchInMarked.Checked = false;
			searchInMyMessages.Checked = false;
			searchAnyWords.Checked = false;
			searchFromDate.Focus();
			searchFromDate.Checked = false;
			searchToDate.Focus();
			searchToDate.Checked = false;
			searchInOverquoting.Checked = false;
			searchInQuestions.Checked = false;
			searchInForum.SelectedIndex = 0;
			comboSearchEntry.Focus();
		}

		private void SearchInForumSelectedIndexChanged(object sender, EventArgs e)
		{
			Config.Instance.SearchForumId = searchInForum.ForumId;
		}

		private void SearchInTextCheckedChanged(object sender, EventArgs e)
		{
			Config.Instance.SearchInText = searchInText.Checked;
		}

		private void SearchInSubjectCheckedChanged(object sender, EventArgs e)
		{
			Config.Instance.SearchInSubject = searchInSubject.Checked;
		}

		private void SearchAuthorCheckedChanged(object sender, EventArgs e)
		{
			Config.Instance.SearchAuthor = searchAuthor.Checked;
		}

		private void SearchInMarkedCheckedChanged(object sender, EventArgs e)
		{
			Config.Instance.SearchInMarked = searchInMarked.Checked;
		}

		private void SearchInMyMessagesCheckedChanged(object sender, EventArgs e)
		{
			Config.Instance.SearchInMyMessages = searchInMyMessages.Checked;
		}

		private void SearchAnyWordsCheckedChanged(object sender, EventArgs e)
		{
			Config.Instance.SearchAnyWord = searchAnyWords.Checked;
		}

		private void SearchInOverquotingCheckedChanged(object sender, EventArgs e)
		{
			Config.Instance.SearchInOverquoting = searchInOverquoting.Checked;
		}

		private void SearchInQuestionsCheckedChanged(object sender, EventArgs e)
		{
			Config.Instance.SearchInQuestions = searchInQuestions.Checked;
		}
		#endregion

		#region Makeing "Search" default button
		private void PnlSearchMainEnter(object sender, EventArgs e)
		{
			_form = FindForm();
			if (_form != null)
			{
				_form.AcceptButton = searchButton;
			}
		}

		private void PnlSearchMainLeave(object sender, EventArgs e)
		{
			if (_form != null && _form.AcceptButton == searchButton)
			{
				_form.AcceptButton = null;
			}
			_form = null;
		}
		#endregion
	}
}
