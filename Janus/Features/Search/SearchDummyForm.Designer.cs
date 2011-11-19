namespace Rsdn.Janus
{
	partial class SearchDummyForm
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchDummyForm));
			this._splitContainer = new System.Windows.Forms.SplitContainer();
			this._tgMsgs = new Rsdn.Janus.JanusGrid();
			this._idCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._markedCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._subjectCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._userNickCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._rateThisCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._forumNameCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._repliesCountCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._msgDateCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.pnlSearchAdvanced = new System.Windows.Forms.Panel();
			this.searchInOverquoting = new System.Windows.Forms.CheckBox();
			this.searchInQuestions = new System.Windows.Forms.CheckBox();
			this.searchAnyWords = new System.Windows.Forms.CheckBox();
			this.searchInMyMessages = new System.Windows.Forms.CheckBox();
			this.searchInMarked = new System.Windows.Forms.CheckBox();
			this.clearButton = new System.Windows.Forms.Button();
			this.lblSplitLine = new System.Windows.Forms.Label();
			this.lblAdditionalParameters = new System.Windows.Forms.Label();
			this.lblSearchTo = new System.Windows.Forms.Label();
			this.lblSearchFrom = new System.Windows.Forms.Label();
			this.searchFromDate = new System.Windows.Forms.DateTimePicker();
			this.searchToDate = new System.Windows.Forms.DateTimePicker();
			this.searchAuthor = new System.Windows.Forms.CheckBox();
			this.searchInSubject = new System.Windows.Forms.CheckBox();
			this.searchInText = new System.Windows.Forms.CheckBox();
			this.lblForum = new System.Windows.Forms.Label();
			this.searchInForum = new Rsdn.Janus.ForumsBox(_serviceManager);
			this.pnlSearchMain = new System.Windows.Forms.Panel();
			this.comboSearchEntry = new System.Windows.Forms.ComboBox();
			this.lblSearch = new System.Windows.Forms.Label();
			this.searchButton = new System.Windows.Forms.Button();
			this.advancedButton = new System.Windows.Forms.Button();
			this._contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this._splitContainer.Panel1.SuspendLayout();
			this._splitContainer.SuspendLayout();
			this.pnlSearchAdvanced.SuspendLayout();
			this.pnlSearchMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// _splitContainer
			// 
			resources.ApplyResources(this._splitContainer, "_splitContainer");
			this._splitContainer.Name = "_splitContainer";
			// 
			// _splitContainer.Panel1
			// 
			this._splitContainer.Panel1.Controls.Add(this._tgMsgs);
			this._splitContainer.Panel1.Controls.Add(this.pnlSearchAdvanced);
			this._splitContainer.Panel1.Controls.Add(this.pnlSearchMain);
			// 
			// _tgMsgs
			// 
			this._tgMsgs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(255)))), ((int)(((byte)(244)))));
			this._tgMsgs.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._tgMsgs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this._idCol,
			this._markedCol,
			this._subjectCol,
			this._userNickCol,
			this._rateThisCol,
			this._forumNameCol,
			this._repliesCountCol,
			this._msgDateCol});
			this._tgMsgs.ColumnsOrder = new int[] {
		0,
		1,
		2,
		3,
		4,
		5,
		6,
		7};
			this._tgMsgs.ColumnsWidth = new int[] {
		47,
		64,
		200,
		80,
		80,
		80,
		80,
		80};
			resources.ApplyResources(this._tgMsgs, "_tgMsgs");
			this._tgMsgs.FullRowSelect = true;
			this._tgMsgs.GridLines = true;
			this._tgMsgs.HideSelection = false;
			this._tgMsgs.Indent = 4;
			this._tgMsgs.MultiSelect = false;
			this._tgMsgs.Name = "_tgMsgs";
			this._tgMsgs.OwnerDraw = true;
			this._tgMsgs.TreeColumnIndex = 2;
			this._tgMsgs.UseCompatibleStateImageBehavior = false;
			this._tgMsgs.View = System.Windows.Forms.View.Details;
			this._tgMsgs.VirtualMode = true;
			this._tgMsgs.AfterActivateNode += new Rsdn.TreeGrid.AfterActivateNodeHandler(this.TgMsgsAfterActivateNode);
			this._tgMsgs.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.TgMsgsColumnClick);
			this._tgMsgs.ColumnReordered += new System.Windows.Forms.ColumnReorderedEventHandler(this.TgMsgsColumnReordered);
			this._tgMsgs.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.TgMsgsColumnWidthChanged);
			this._tgMsgs.DoubleClick += new System.EventHandler(this.TgMsgsDoubleClick);
			this._tgMsgs.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TgMsgsKeyDown);
			this._tgMsgs.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TgMsgsMouseUp);
			// 
			// _idCol
			// 
			resources.ApplyResources(this._idCol, "_idCol");
			// 
			// _markedCol
			// 
			resources.ApplyResources(this._markedCol, "_markedCol");
			// 
			// _subjectCol
			// 
			resources.ApplyResources(this._subjectCol, "_subjectCol");
			// 
			// _userNickCol
			// 
			resources.ApplyResources(this._userNickCol, "_userNickCol");
			// 
			// _rateThisCol
			// 
			resources.ApplyResources(this._rateThisCol, "_rateThisCol");
			// 
			// _forumNameCol
			// 
			resources.ApplyResources(this._forumNameCol, "_forumNameCol");
			// 
			// _repliesCountCol
			// 
			resources.ApplyResources(this._repliesCountCol, "_repliesCountCol");
			// 
			// _msgDateCol
			// 
			resources.ApplyResources(this._msgDateCol, "_msgDateCol");
			// 
			// pnlSearchAdvanced
			// 
			this.pnlSearchAdvanced.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSearchAdvanced.Controls.Add(this.searchInOverquoting);
			this.pnlSearchAdvanced.Controls.Add(this.searchInQuestions);
			this.pnlSearchAdvanced.Controls.Add(this.searchAnyWords);
			this.pnlSearchAdvanced.Controls.Add(this.searchInMyMessages);
			this.pnlSearchAdvanced.Controls.Add(this.searchInMarked);
			this.pnlSearchAdvanced.Controls.Add(this.clearButton);
			this.pnlSearchAdvanced.Controls.Add(this.lblSplitLine);
			this.pnlSearchAdvanced.Controls.Add(this.lblAdditionalParameters);
			this.pnlSearchAdvanced.Controls.Add(this.lblSearchTo);
			this.pnlSearchAdvanced.Controls.Add(this.lblSearchFrom);
			this.pnlSearchAdvanced.Controls.Add(this.searchFromDate);
			this.pnlSearchAdvanced.Controls.Add(this.searchToDate);
			this.pnlSearchAdvanced.Controls.Add(this.searchAuthor);
			this.pnlSearchAdvanced.Controls.Add(this.searchInSubject);
			this.pnlSearchAdvanced.Controls.Add(this.searchInText);
			this.pnlSearchAdvanced.Controls.Add(this.lblForum);
			this.pnlSearchAdvanced.Controls.Add(this.searchInForum);
			resources.ApplyResources(this.pnlSearchAdvanced, "pnlSearchAdvanced");
			this.pnlSearchAdvanced.Name = "pnlSearchAdvanced";
			this.pnlSearchAdvanced.TabStop = true;
			// 
			// searchInOverquoting
			// 
			resources.ApplyResources(this.searchInOverquoting, "searchInOverquoting");
			this.searchInOverquoting.Name = "searchInOverquoting";
			this.searchInOverquoting.CheckedChanged += new System.EventHandler(this.SearchInOverquotingCheckedChanged);
			// 
			// searchInQuestions
			// 
			resources.ApplyResources(this.searchInQuestions, "searchInQuestions");
			this.searchInQuestions.Name = "searchInQuestions";
			this.searchInQuestions.CheckedChanged += new System.EventHandler(this.SearchInQuestionsCheckedChanged);
			// 
			// searchAnyWords
			// 
			resources.ApplyResources(this.searchAnyWords, "searchAnyWords");
			this.searchAnyWords.Name = "searchAnyWords";
			this.searchAnyWords.CheckedChanged += new System.EventHandler(this.SearchAnyWordsCheckedChanged);
			// 
			// searchInMyMessages
			// 
			resources.ApplyResources(this.searchInMyMessages, "searchInMyMessages");
			this.searchInMyMessages.Name = "searchInMyMessages";
			this.searchInMyMessages.CheckedChanged += new System.EventHandler(this.SearchInMyMessagesCheckedChanged);
			// 
			// searchInMarked
			// 
			resources.ApplyResources(this.searchInMarked, "searchInMarked");
			this.searchInMarked.Name = "searchInMarked";
			this.searchInMarked.CheckedChanged += new System.EventHandler(this.SearchInMarkedCheckedChanged);
			// 
			// clearButton
			// 
			resources.ApplyResources(this.clearButton, "clearButton");
			this.clearButton.Name = "clearButton";
			this.clearButton.UseVisualStyleBackColor = true;
			this.clearButton.Click += new System.EventHandler(this.ClearButtonClick);
			// 
			// lblSplitLine
			// 
			resources.ApplyResources(this.lblSplitLine, "lblSplitLine");
			this.lblSplitLine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblSplitLine.Name = "lblSplitLine";
			// 
			// lblAdditionalParameters
			// 
			resources.ApplyResources(this.lblAdditionalParameters, "lblAdditionalParameters");
			this.lblAdditionalParameters.Name = "lblAdditionalParameters";
			// 
			// lblSearchTo
			// 
			resources.ApplyResources(this.lblSearchTo, "lblSearchTo");
			this.lblSearchTo.Name = "lblSearchTo";
			// 
			// lblSearchFrom
			// 
			resources.ApplyResources(this.lblSearchFrom, "lblSearchFrom");
			this.lblSearchFrom.Name = "lblSearchFrom";
			// 
			// searchFromDate
			// 
			this.searchFromDate.Checked = false;
			resources.ApplyResources(this.searchFromDate, "searchFromDate");
			this.searchFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.searchFromDate.Name = "searchFromDate";
			this.searchFromDate.ShowCheckBox = true;
			// 
			// searchToDate
			// 
			this.searchToDate.Checked = false;
			resources.ApplyResources(this.searchToDate, "searchToDate");
			this.searchToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.searchToDate.Name = "searchToDate";
			this.searchToDate.ShowCheckBox = true;
			// 
			// searchAuthor
			// 
			resources.ApplyResources(this.searchAuthor, "searchAuthor");
			this.searchAuthor.Name = "searchAuthor";
			this.searchAuthor.CheckedChanged += new System.EventHandler(this.SearchAuthorCheckedChanged);
			// 
			// searchInSubject
			// 
			resources.ApplyResources(this.searchInSubject, "searchInSubject");
			this.searchInSubject.Name = "searchInSubject";
			this.searchInSubject.CheckedChanged += new System.EventHandler(this.SearchInSubjectCheckedChanged);
			// 
			// searchInText
			// 
			resources.ApplyResources(this.searchInText, "searchInText");
			this.searchInText.Name = "searchInText";
			this.searchInText.CheckedChanged += new System.EventHandler(this.SearchInTextCheckedChanged);
			// 
			// lblForum
			// 
			resources.ApplyResources(this.lblForum, "lblForum");
			this.lblForum.Name = "lblForum";
			// 
			// searchInForum
			// 
			this.searchInForum.BackColor = System.Drawing.SystemColors.Window;
			this.searchInForum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.searchInForum.FormattingEnabled = true;
			resources.ApplyResources(this.searchInForum, "searchInForum");
			this.searchInForum.Name = "searchInForum";
			this.searchInForum.SelectedIndexChanged += new System.EventHandler(this.SearchInForumSelectedIndexChanged);
			// 
			// pnlSearchMain
			// 
			this.pnlSearchMain.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSearchMain.Controls.Add(this.comboSearchEntry);
			this.pnlSearchMain.Controls.Add(this.lblSearch);
			this.pnlSearchMain.Controls.Add(this.searchButton);
			this.pnlSearchMain.Controls.Add(this.advancedButton);
			resources.ApplyResources(this.pnlSearchMain, "pnlSearchMain");
			this.pnlSearchMain.Name = "pnlSearchMain";
			this.pnlSearchMain.TabStop = true;
			this.pnlSearchMain.Enter += new System.EventHandler(this.PnlSearchMainEnter);
			this.pnlSearchMain.Leave += new System.EventHandler(this.PnlSearchMainLeave);
			// 
			// comboSearchEntry
			// 
			resources.ApplyResources(this.comboSearchEntry, "comboSearchEntry");
			this.comboSearchEntry.FormattingEnabled = true;
			this.comboSearchEntry.Name = "comboSearchEntry";
			this.comboSearchEntry.TextChanged += new System.EventHandler(this.ComboSearchEntryTextChanged);
			this.comboSearchEntry.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CbxForumsKeyDown);
			// 
			// lblSearch
			// 
			resources.ApplyResources(this.lblSearch, "lblSearch");
			this.lblSearch.Name = "lblSearch";
			// 
			// searchButton
			// 
			resources.ApplyResources(this.searchButton, "searchButton");
			this.searchButton.Name = "searchButton";
			this.searchButton.UseVisualStyleBackColor = true;
			this.searchButton.Click += new System.EventHandler(this.BtnSearchClick);
			// 
			// advancedButton
			// 
			resources.ApplyResources(this.advancedButton, "advancedButton");
			this.advancedButton.Name = "advancedButton";
			this.advancedButton.UseVisualStyleBackColor = true;
			this.advancedButton.Click += new System.EventHandler(this.BtnAdvancedClick);
			// 
			// _contextMenuStrip
			// 
			this._contextMenuStrip.Name = "_contextMenuStrip";
			resources.ApplyResources(this._contextMenuStrip, "_contextMenuStrip");
			// 
			// SearchDummyForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._splitContainer);
			this.Name = "SearchDummyForm";
			this._splitContainer.Panel1.ResumeLayout(false);
			this._splitContainer.ResumeLayout(false);
			this.pnlSearchAdvanced.ResumeLayout(false);
			this.pnlSearchAdvanced.PerformLayout();
			this.pnlSearchMain.ResumeLayout(false);
			this.pnlSearchMain.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private Rsdn.Janus.JanusGrid _tgMsgs;
		private Rsdn.Janus.ForumsBox searchInForum;
		private System.Windows.Forms.CheckBox searchAnyWords;
		private System.Windows.Forms.CheckBox searchInMyMessages;
		private System.Windows.Forms.CheckBox searchInOverquoting;
		private System.Windows.Forms.CheckBox searchInQuestions;
		private System.Windows.Forms.CheckBox searchInMarked;
		private System.Windows.Forms.CheckBox searchAuthor;
		private System.Windows.Forms.CheckBox searchInSubject;
		private System.Windows.Forms.CheckBox searchInText;
		private System.Windows.Forms.ColumnHeader _idCol;
		private System.Windows.Forms.ColumnHeader _markedCol;
		private System.Windows.Forms.ColumnHeader _subjectCol;
		private System.Windows.Forms.ColumnHeader _userNickCol;
		private System.Windows.Forms.ColumnHeader _rateThisCol;
		private System.Windows.Forms.ColumnHeader _forumNameCol;
		private System.Windows.Forms.ColumnHeader _repliesCountCol;
		private System.Windows.Forms.ColumnHeader _msgDateCol;
		private System.Windows.Forms.Panel pnlSearchAdvanced;
		private System.Windows.Forms.Panel pnlSearchMain;
		private System.Windows.Forms.Button advancedButton;
		private System.Windows.Forms.Button clearButton;
		private System.Windows.Forms.Button searchButton;
		private System.Windows.Forms.Label lblAdditionalParameters;
		private System.Windows.Forms.Label lblSplitLine;
		private System.Windows.Forms.Label lblForum;
		private System.Windows.Forms.Label lblSearchTo;
		private System.Windows.Forms.Label lblSearchFrom;
		private System.Windows.Forms.Label lblSearch;
		private System.Windows.Forms.DateTimePicker searchFromDate;
		private System.Windows.Forms.DateTimePicker searchToDate;
		private System.Windows.Forms.ComboBox comboSearchEntry;
		private System.Windows.Forms.ContextMenuStrip _contextMenuStrip;
		private System.Windows.Forms.SplitContainer _splitContainer;

	}
}
