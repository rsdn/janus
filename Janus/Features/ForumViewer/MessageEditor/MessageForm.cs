using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;

using BLToolkit.Data.Linq;

using JetBrains.Annotations;

using Rsdn.Janus.Framework;
using Rsdn.Janus.Log;
using Rsdn.Scintilla;
using Rsdn.Shortcuts;
using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	internal partial class MessageForm : JanusBaseForm, IPreviewSource
	{
		#region Delegate & Events
		public delegate void SendMessageEventHandler(object sender, int messageId);

		public static event SendMessageEventHandler MessageSent;
		#endregion

		#region Editor Declarations
		private static readonly Regex _quotedLineRx = new Regex(
			@"(?m)^(?'prefix'\s*\S{0,5}?(?'level'>+))[^>]",
			RegexOptions.Compiled);

		private static readonly Regex _tagRx = new Regex(
			@"\[/?(?<tag>[^\[\]=]+)=*[^\]]*\]",
			RegexOptions.Compiled);

		// TODO : генерировать автоматом из _knownSmiles и _knownTags
		private static readonly Regex _smileRx = new Regex(
			@":up:|:down:|:super:|:shuffle:" +
			@"|:crash:|:maniac:|:user:|:wow:|:beer:|:team:|:no:|" +
			@":nopont:|:xz:|(?<!:):-?\)\)\)|(?<!:):-?\)\)|(?<!:):-?\)|" +
			@"(?<!;|amp|gt|lt|quot);[-oO]?\)|(?<!:):-?\(|(?<!:):-[\\/]|:\?\?\?:");

		private static readonly HashSet<string> _knownSmiles =
			new HashSet<string>
				{
					":", // not smile, but for prevention of automatic replacement ':' by ':)'
					":)",
					":))",
					":)))",
					":(",
					//";)", // this single smile, not started with ':', not used in autocomplete
					":-\\",
					":)",
					":up:",
					":down:",
					":super:",
					":shuffle:",
					":crash:",
					":maniac:",
					":user:",
					":wow:",
					":beer:",
					":team:",
					":no:",
					":nopont:",
					":xz:",
					":???:?0" // scintilla use '?' as image marker, '?0' is never used
				};

		private static readonly Dictionary<string, bool> _knownTags =
			new Dictionary<string, bool>
				{
					{ "b", true },
					{ "i", true },
					{ "c#", true },
					{ "url", true },
					{ "img", true },
					{ "msil", true },
					{ "list", true },
					{ "*", false },
					{ "midl", true },
					{ "asm", true },
					{ "ccode", true },
					{ "code", true },
					{ "pascal", true },
					{ "vb", true },
					{ "sql", true },
					{ "java", true },
					{ "email", true },
					{ "msdn", true },
					{ "perl", true },
					{ "php", true },
					{ "erlang", true },
					{ "haskell", true },
					{ "lisp", true },
					{ "ml", true },
					{ "py", true },
					{ "rb", true },
					{ "prolog", true },
					{ "q", true },
					{ "hr", false },
					{ "article", false },
					{ "h1", true },
					{ "h2", true },
					{ "h3", true },
					{ "h4", true },
					{ "h5", true },
					{ "h6", true },
					{ "t", true },
					{ "th", true },
					{ "tr", true },
					{ "td", true },
					{ "xml", true }
				};

		private const int _level1Style = 1;
		private const int _level2Style = 10;
		private const int _level3Style = 11;
		private const int _quotaPrefixStyle = 2;
		#endregion

		#region Declarations
		/// <summary>
		/// Режим формы
		/// </summary>
		private MessageFormMode _formMode;
		/// <summary>
		/// DTO для данных формы
		/// </summary>
		private readonly MessageInfo _messageInfo;
		private readonly int _previewSourceNum;
		private QuoteAnalyzer _quoteAnalyzer;
		private SmilesToolbar _tagsBar;
		private readonly ServiceManager _serviceManager;
		private readonly StripMenuGenerator _menuGenerator;
		private readonly StripMenuGenerator _toolbarGenerator;
		private readonly SmilesToolbarGenerator _tagsbarGenerator;
		private bool _isModified;
		#endregion

		#region Constructor(s)
		public MessageForm(
			[NotNull] IServiceProvider provider,
			MessageFormMode mode,
			MessageInfo message)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");

			_serviceManager = new ServiceManager(provider);

			InitializeComponent();

			_formMode = mode;
			_messageInfo = message ?? new MessageInfo();
			_previewSourceNum = OutboxManager.RegisterPreviewSource(this);

			_serviceManager.Publish<IMessageEditorService>(
				new MessageEditorService(this));

			CustomInitializeComponent();

			_menuGenerator = new StripMenuGenerator(_serviceManager, _menuStrip, "MessageEditor.Menu");
			_toolbarGenerator = new StripMenuGenerator(_serviceManager, _toolStrip, "MessageEditor.Toolbar");
			_tagsbarGenerator = new SmilesToolbarGenerator(_serviceManager, "Forum.MessageEditor.TagsBar", _tagsBar);
		}
		#endregion

		#region Scintilla Events
		private void MessageEditorCharAdded(object sender, KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
				case '\r':
					var newLine = _messageEditor.Model.LineFromPosition(_messageEditor.CaretPosition);
					var ep = _messageEditor.Model.EndPositionFromLine(newLine);
					if (_messageEditor.CaretPosition < ep)
					{
						var m = _quotedLineRx.Match(_messageEditor.Model.GetLine(newLine - 1));

						if (m.Success)
						{
							_messageEditor.Model.InsertText(
								_messageEditor.Model.PositionFromLine(newLine),
								m.Groups["prefix"].Value);
						}
						else
							SmartIndent();
					}
					else
						SmartIndent();
					break;
				case '[':
					_messageEditor.ShowAutocomplete(1,
						_knownTags
							.OrderBy(pair => pair.Key)
							.Select(pair => (pair.Value ? "[{0}][/{0}]" : "[{0}]").FormatStr(pair.Key)));
					break;
				case ':':
					_messageEditor.ShowAutocomplete(
						1, _knownSmiles.OrderBy(smile => smile));
					break;
			}
		}

		private void MessageEditorModified(object sender, ModifiedEventArgs e)
		{
			if ((e.ModificationTypes & (ModificationTypes.InsertText
				| ModificationTypes.DeleteText)) > 0)
				OnModified();

			if (((e.ModificationTypes & ModificationTypes.DeleteText) > 0) &
				(e.LinesAdded == -1))
			{
				var newLine = _messageEditor.Model.LineFromPosition(e.Position);
				if (_messageEditor.Model.PositionFromLine(newLine) != e.Position)
				{
					var text = _messageEditor.Model.GetTextRange(
						e.Position, _messageEditor.Model.EndPositionFromLine(newLine));

					var qm = _quotedLineRx.Match(text);

					if (qm.Success)
						AsyncHelper.Post(
							() =>
							{
								_messageEditor.Selection.Start = e.Position;
								_messageEditor.Selection.End = e.Position + qm.Groups["prefix"].Length;
								_messageEditor.Selection.Text = string.Empty;
							});
				}
			}
		}

		private void MessageEditorStyleNeeded(object sender, StyleNeededEventArgs e)
		{
			var sl = _messageEditor.Model.LineFromPosition(e.StartPosition);
			var el = _messageEditor.Model.LineFromPosition(e.EndPosition);

			for (var i = sl; i <= el; i++)
			{
				var line = _messageEditor.Model.GetLine(i);
				var qm = _quotedLineRx.Match(line);

				_messageEditor.StartStyling(
					_messageEditor.Model.PositionFromLine(i), 31);

				if (qm.Success)
				{
					var styleNum = _level3Style;

					switch (qm.Groups["level"].Value.Length)
					{
						case 1:
							styleNum = _level1Style;
							break;
						case 2:
							styleNum = _level2Style;
							break;
					}

					_messageEditor.SetStyling(qm.Groups["prefix"].Value.Length,
						_messageEditor.TextStyles[_quotaPrefixStyle]);
					_messageEditor.SetStyling(
						_messageEditor.Model.GetLineLength(i) - qm.Groups["prefix"].Value.Length,
						_messageEditor.TextStyles[styleNum]);
				}
				else
				{
					var tm = _tagRx.Match(line);
					var tags = new Hashtable();

					while (tm.Success)
					{
						tags.Add(tm.Index, tm);
						tm = tm.NextMatch();
					}

					var sm = _smileRx.Match(line);
					var smiles = new Dictionary<int, Match>();

					while (sm.Success)
					{
						smiles.Add(sm.Index, sm);
						sm = sm.NextMatch();
					}

					var j = 0;
					while (j < line.Length)
						if (tags.Contains(j))
						{
							var cm = (Match)tags[j];
							var style = _knownTags.ContainsKey(cm.Groups["tag"].Value)
								? _messageEditor.TextStyles[3]
								: _messageEditor.TextStyles[5];

							_messageEditor.SetStyling(cm.Length, style);
							j += cm.Length;
						}
						else if (smiles.ContainsKey(j))
						{
							var cm = smiles[j];
							_messageEditor.SetStyling(cm.Length, _messageEditor.TextStyles[4]);
							j += cm.Length;
						}
						else
						{
							_messageEditor.SetStyling(1, _messageEditor.TextStyles[0]);
							j++;
						}
				}
			}
		}

		private void MessageEditorAutocompleteSelected(object sender, AutocompleteSelectedEventArgs e)
		{
			if (e.Text[0] == '[')
			{
				var middlePos = e.Text.IndexOf(']') + 1;
				SynchronizationContext
					.Current
					.Post(state => _messageEditor.CaretPosition = e.WordStart + middlePos, null);
			}
		}

		private void MessageEditorIsModifiedChanged(object sender, EventArgs e)
		{
			OnIsModifiedChanged();
		}

		private void UpdateOutboxMessage(IOutboxMessage mi)
		{
			using (var db = _serviceManager.CreateDBContext())
				db
					.OutboxMessages(m => m.ID == mi.ID)
					.Set(_ => _.ForumID, m => mi.ForumId)
					.Set(_ => _.Subject, m => mi.Subject)
					.Set(_ => _.Body, m => mi.Message)
					.Set(_ => _.Hold, m => mi.Hold)
					.Update();
		}
		#endregion

		#region Override Events
		protected override void OnClosing(CancelEventArgs e)
		{
			if (IsModified)
			{
				var dlgRes = MessageBox.Show(
					this,
					SR.MessageEditor.SavePromptRequired,
					SR.MessageEditor.Message,
					MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

				if (dlgRes == DialogResult.Cancel)
				{
					e.Cancel = true;
					return;
				}

				if (dlgRes == DialogResult.Yes)
				{
					if (!_messageInfo.Hold)
					{
						dlgRes = MessageBox.Show(
							this,
							SR.MessageEditor.SendPromptRequired,
							SR.MessageEditor.Message,
							MessageBoxButtons.YesNo, MessageBoxIcon.Question);

						_messageInfo.Hold = dlgRes != DialogResult.Yes;
					}

					SaveMessage(true);
				}
			}

			// обновляем сообщение согласно исходным данным
			UpdateOutboxMessage(_messageInfo);
			_serviceManager.GetRequiredService<IOutboxManager>().NewMessages.Refresh();
		}

		protected override void OnClosed(EventArgs e)
		{
			OutboxManager.UnregisterPreviewSource(_previewSourceNum);

			Config.Instance.MessageFormBounds.Bounds =
				WindowState == FormWindowState.Normal ? Bounds : RestoreBounds;

			Config.Instance.MessageFormBounds
				.Maximized = WindowState == FormWindowState.Maximized;

			Config.Instance.ShowMessageFormTagBar = _tagsBar.Visible;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				Close();
			else
				base.OnKeyDown(e);
		}
		#endregion

		#region Shortcuts (Шорткаты)

		[MethodShortcut(Shortcut.CtrlB, "Выделить жирным ([b])",
			"Поместить тег [b][/b] вокруг выделенной области.")]
		public void InsertBoldTag()
		{
			_serviceManager.TryExecuteCommand(
				"Janus.Forum.MessageEditor.InsertPairTag",
				new Dictionary<string, object>
				{
					{ "start", "b" }, { "end", "b" }, { "newLine", false }
				});
		}

		[MethodShortcut(Shortcut.CtrlI, "Выделить курсивом ([i])",
			"Выделить текст курсивом")]
		public void InsertItalicTag()
		{
			_serviceManager.TryExecuteCommand(
				"Janus.Forum.MessageEditor.InsertPairTag",
				new Dictionary<string, object>
				{
					{ "start", "i" }, { "end", "i" }, { "newLine", false }
				});
		}

		[MethodShortcut(
			Shortcut.CtrlK,
			"Зачеркнуть ([s])",
			"Зачеркнуть текст")]
		public void InsertStrikeTag()
		{
			_serviceManager.TryExecuteCommand(
				"Janus.Forum.MessageEditor.InsertPairTag",
				new Dictionary<string, object>
				{
					{ "start", "s" }, { "end", "s" }, { "newLine", false }
				});
		}

		[MethodShortcut(
			Shortcut.CtrlU,
			"Подчеркнуть ([u])",
			"Подчеркнуть текст")]
		public void InsertUnderlineTag()
		{
			_serviceManager.TryExecuteCommand(
				"Janus.Forum.MessageEditor.InsertPairTag",
				new Dictionary<string, object>
				{
					{ "start", "u" }, { "end", "u" }, { "newLine", false }
				});
		}

		[MethodShortcut(Shortcut.CtrlL, "Вставить URL",
			"Вставить URL")]
		public void InsertUrlTag()
		{
			_serviceManager.TryExecuteCommand(
				"Janus.Forum.MessageEditor.InsertPairTag",
				new Dictionary<string, object>
				{
					{ "start", "url=" }, { "end", "url" }, { "newLine", false }
				});
		}

		[MethodShortcut(Shortcut.CtrlQ, "Цитата ([q])",
			"Поместить выделенный блок как цитаты.")]
		public void InsertQTag()
		{
			_serviceManager.TryExecuteCommand(
				"Janus.Forum.MessageEditor.InsertPairTag",
				new Dictionary<string, object>
				{
					{ "start", "q" }, { "end", "q" }, { "newLine", true }
				});
		}

		[MethodShortcut((Shortcut)(Keys.Alt | Keys.Oemtilde),
			"Последний использованный язык",
			"Применить тэг последнего использованного языка.")]
		public void InsertLastLanguageTag()
		{
			_serviceManager.TryExecuteCommand(
				"Janus.Forum.MessageEditor.InsertPairTag",
				new Dictionary<string, object>
				{
					{ "start", Config.Instance.LastLanguageTag },
					{ "end", Config.Instance.LastLanguageTag }, 
					{ "newLine", true }
				});
		}

		[MethodShortcut(Shortcut.Alt1, "Код ([code])",
			"Поместить выделенный блок как код.")]
		public void InsertCodeTag()
		{
			_serviceManager.TryExecuteCommand(
				"Janus.Forum.MessageEditor.InsertPairTag",
				new Dictionary<string, object>
				{
					{ "start", "code" }, { "end", "code" }, { "newLine", true }
				});
		}

		[MethodShortcut(Shortcut.Alt3, "Код C# ([c#])",
			"Поместить выделенный блок как код на языке C#.")]
		public void InsertCsTag()
		{
			_serviceManager.TryExecuteCommand(
				"Janus.Forum.MessageEditor.InsertPairTag",
				new Dictionary<string, object>
				{
					{ "start", "c#" }, { "end", "c#" }, { "newLine", true }
				});
		}

		[MethodShortcut((Shortcut)(Keys.Alt | Keys.V), "Код VB ([vb])",
			"Поместить выделенный блок как код на языке VB.")]
		public void InsertVbTag()
		{
			_serviceManager.TryExecuteCommand(
				"Janus.Forum.MessageEditor.InsertPairTag",
				new Dictionary<string, object>
				{
					{ "start", "vb" }, { "end", "vb" }, { "newLine", true }
				});
		}

		#endregion

		#region Private Methods

		private void CustomInitializeComponent()
		{

			// init data
			_forumsComboBox.InitForumsComboBox(_messageInfo.ForumId);

			if (_formMode == MessageFormMode.Add || _formMode == MessageFormMode.Reply)
			{
				if (_formMode == MessageFormMode.Add)
				{
					_subjectTextBox.Text = _messageInfo.Subject;
					ActiveControl = _subjectTextBox;
				}

				if (_formMode == MessageFormMode.Reply)
				{
					_forumsComboBox.Enabled = false;
					_quoteAnalyzer = new QuoteAnalyzer(_messageInfo.Message);
					_subjectTextBox.Text = JanusFormatMessage.ReSubj(_messageInfo.Subject);
					ActiveControl = _messageEditor;
				}

				_messageEditor.Text = _messageInfo.Message;
			}

			if (_formMode == MessageFormMode.Edit)
			{
				_subjectTextBox.Text = _messageInfo.Subject;
				_messageEditor.Text = _messageInfo.Message;
				ActiveControl = _messageEditor;
			}

			_fromUserLabel.Text = Config.Instance.Login;

			Bounds = Config.Instance.MessageFormBounds.Bounds;
			if (Config.Instance.MessageFormBounds.Maximized)
				WindowState = FormWindowState.Maximized;

			// style
			var font = new Font("Courier New", 9F);
			_messageEditor.TextStyles.AddRange(new[]
				{
					new TextStyle("Default", 0, font, Color.Black, Color.White, false, CaseMode.Mixed, true, false,
						PredefinedStyle.None),
					new TextStyle("Quoted text level 1", 1, font, Color.FromArgb(0, 92, 0), Color.White, false,
						CaseMode.Mixed, true, false, PredefinedStyle.None),
					new TextStyle("Quotes prefix", 2, font, Color.FromArgb(0, 64, 96), Color.White, false,
						CaseMode.Mixed, true, false, PredefinedStyle.None),
					new TextStyle("Known tags", 3, font, Color.DarkGreen, Color.LightGray, false, CaseMode.Mixed,
						true, false, PredefinedStyle.None),
					new TextStyle("Smiles", 4, font, Color.DarkBlue, Color.LightGray, false, CaseMode.Mixed, true,
						false, PredefinedStyle.None),
					new TextStyle("Unknown tag", 5, font, Color.DarkRed, Color.LightGray, false, CaseMode.Mixed,
						true, false, PredefinedStyle.None),
					new TextStyle("TagContent", 6, font, Color.Black, Color.LemonChiffon, true, CaseMode.Mixed,
						true, false, PredefinedStyle.None),
					new TextStyle("PredefinedBraceLight", 34, font, Color.Blue, Color.White, false, CaseMode.Mixed,
						true, false, PredefinedStyle.BraceLight),
					new TextStyle("PredefinedBraceBad", 35, font, Color.Red, Color.White, false, CaseMode.Mixed,
						true, false, PredefinedStyle.BraceBad),
					new TextStyle("PredefinedDefault", 32, font, Color.Black, Color.White, false, CaseMode.Mixed,
						true, false, PredefinedStyle.Default),
					new TextStyle("Quoted text level 2", 10, font, Color.Green, Color.White, false, CaseMode.Mixed,
						true, false, PredefinedStyle.None),
					new TextStyle("Quoted text level 3", 11, font, Color.FromArgb(0, 160, 0), Color.White, false,
						CaseMode.Mixed, true, false, PredefinedStyle.None)
				});
			foreach (var style in _messageEditor.TextStyles.Cast<TextStyle>())
				_messageEditor.SetStyleCharset(style.Number, ScintillaCharset.Russian);

			_subjectTextBox.Modified = false;
			_messageEditor.ClearUndoBuffer(); // Savepoint also set by this call
			OnIsModifiedChanged();
			_messageEditor.MultipleSelection = true;
			_messageEditor.AdditionalSelectionTyping = true;

			StyleConfig.StyleChange += OnStyleChanged;
			UpdateStyle();
		}

		/// <summary>
		/// Конструирует отступ с повторением рисунков пробелов предыдущей строки.
		/// </summary>
		private void SmartIndent()
		{
			var editor = _messageEditor;
			var model = _messageEditor.Model;

			var line = model.LineFromPosition(editor.CaretPosition);
			var startPos = model.PositionFromLine(line);

			// Текст отступа
			var indent =
				model
					.GetLine(line - 1)
					.TakeWhile(ch => char.IsWhiteSpace(ch) && ch != '\r' && ch != '\n')
					.Aggregate(new StringBuilder(), (current, ch) => current.Append(ch))
					.ToString();

			model.InsertText(startPos, indent);
			editor.Selection.Start += indent.Length;
		}

		private void OnStyleChanged(object s, StyleChangeEventArgs e)
		{
			UpdateStyle();
			Refresh();
		}

		private void UpdateStyle()
		{
			_messageEditor.TextStyles[_level1Style].ForeColor =
				StyleConfig.Instance.Level1QuotaColor;
			_messageEditor.TextStyles[_level2Style].ForeColor =
				StyleConfig.Instance.Level2QuotaColor;
			_messageEditor.TextStyles[_level3Style].ForeColor =
				StyleConfig.Instance.Level3QuotaColor;
			_messageEditor.TextStyles[_quotaPrefixStyle].ForeColor =
				StyleConfig.Instance.QuotaPrefixColor;
			_messageEditor.TabWidth = StyleConfig.Instance.WriteMessageTabSize;
		}

		private void SaveMessage(bool closeOnSave)
		{
			// subject validation
			if (_subjectTextBox.Text.Trim().Length == 0)
			{
				MessageBox.Show(
					SR.MessageEditor.SubjectRequired,
					ApplicationInfo.ApplicationName,
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			// fill object from controls
			if (_forumsComboBox.ForumId != -1)
				_messageInfo.ForumId = _forumsComboBox.ForumId;

			_messageInfo.Subject = _subjectTextBox.Text.Trim();
			_messageInfo.Message = _messageEditor.Text.Trim();

			if (!_messageInfo.Hold)
			{
				// overquote validation
				if (_quoteAnalyzer != null
					&& _quoteAnalyzer.IsOverquoted(_messageEditor.Text))
				{
					// Предлагаем вернуться к редактированию сообщения.
					if (MessageBox.Show(
						this,
						SR.MessageEditor.QuoteWarning,
						ApplicationInfo.ApplicationName,
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Warning) != DialogResult.Yes)
					{
						return;
					}
				}
			}

			if (_formMode == MessageFormMode.Add || _formMode == MessageFormMode.Reply)
			{
				OutboxHelper.AddOutboxMessage(_serviceManager, _messageInfo);

				if (!closeOnSave)
				{
					using (var db = _serviceManager.CreateDBContext())
						_messageInfo.ID = db.OutboxMessages().Max(m => m.ID);
					_formMode = MessageFormMode.Edit;
				}

				_serviceManager.LogInfo(string.Format(
					SR.MessageEditor.LogNewMsg, _forumsComboBox.Text));
			}
			else
			{
				UpdateOutboxMessage(_messageInfo);
				_serviceManager.LogInfo(string.Format(
					SR.MessageEditor.LogChangeMsg, _forumsComboBox.Text));
			}

			_messageEditor.SetSavePoint();
			_subjectTextBox.Modified = false;
			OnIsModifiedChanged();

			if (MessageSent != null)
				MessageSent(this, _messageInfo.ID);

			if (closeOnSave)
				NativeMethods.PostMessage(Handle, NativeMethods.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

			_serviceManager.GetRequiredService<IOutboxManager>().NewMessages.Refresh();
		}

		private void OnModified()
		{
			if (Modified != null)
				Modified(this);
		}

		private void OnIsModifiedChanged()
		{
			_isModified = _messageEditor.IsModified || _subjectTextBox.Modified;
			if (IsModifiedChanged != null)
				IsModifiedChanged(this);
		}

		#endregion

		#region Control Events

		private void SubjectTextBoxTextChanged(object sender, EventArgs e)
		{
			OnModified();
		}

		private void SubjectTextBoxModifiedChanged(object sender, EventArgs e)
		{
			OnIsModifiedChanged();
		}

		#endregion

		#region IPreviewSource Members

		#region Адаптированный дубликат кода из HtmlPageBuilder.cs
		private const string _templateAlt = "{0}: {1}";

		private const string _templateHeaderItem =
			@"<table class='header' width='100%' border='0' cellspacing='0'><tbody>
				<tr>
					<td nowrap>&nbsp;<img class='himg' src='{0}' alt='{1}'>&nbsp;{2}</td>
				</tr>
			</tbody></table>";

		private string FormatSubject()
		{
			var text = HttpUtility.HtmlEncode(_subjectTextBox.Text);

			return string.Format(_templateHeaderItem,
				JanusFormatMessage.GetMessageImagePath(_serviceManager, true, false, false),
				string.Format(_templateAlt, SR.TGColumnSubject, text),
				text);
		}
		#endregion

		private const string _messagePreviewTemplateResource =
			"Rsdn.Janus.Features.ForumViewer.MessageEditor.PreviewMessage.html";

		private static string _messagePreviewTemplate;

		string IPreviewSource.GetData()
		{
			if (_messagePreviewTemplate == null)
			{
				var messagePreviewStream = typeof(MessageEditor).Assembly
					.GetRequiredResourceStream(_messagePreviewTemplateResource);
				using (var sr = new StreamReader(messagePreviewStream))
					_messagePreviewTemplate = sr.ReadToEnd();
			}

			var tlm = _serviceManager.GetRequiredService<ITagLineManager>();
			var tl = tlm.GetTagLine(tlm.FindAppropriateTagLine(_forumsComboBox.ForumId));

			return _messagePreviewTemplate.FormatStr(
				SR.MessageEditor.PreviewTitle,
				FormatSubject(),
				_serviceManager
					.GetFormatter()
					.Format(
						"{0}\r\n[tagline]{1}[/tagline]".FormatStr(_messageEditor.Model.Text, tl),
					true));
		}
		#endregion

		#region Public Members

		public void SendMessage()
		{
			_messageInfo.Hold = false;
			SaveMessage(true);
		}

		public void SaveMessage()
		{
			_messageInfo.Hold = true;
			SaveMessage(false);
		}

		public void ShowPreview()
		{
			_serviceManager.OpenUrlInBrowser("janus://outbox-preview/" + _previewSourceNum);
		}

		public void Undo()
		{
			_messageEditor.Undo();
		}

		public void Redo()
		{
			_messageEditor.Redo();
		}

		public bool CanUndo
		{
			get { return _messageEditor.CanUndo; }
		}

		public bool CanRedo
		{
			get { return _messageEditor.CanRedo; }
		}

		public void ShowFindAndReplace()
		{
			using (var fdf = new FindDialogForm())
			{
				if (fdf.ShowDialog(_messageEditor) != DialogResult.OK)
					return;
				var sf = SearchFlags.None;

				if (fdf.MatchCase)
					sf |= SearchFlags.MatchCase;

				if (fdf.MatchWholeWord)
					sf |= SearchFlags.WholeWord;

				if (fdf.MatchWordStart)
					sf |= SearchFlags.WordStart;

				if (fdf.UseRegex)
					sf |= SearchFlags.RegExp;

				var start = fdf.SelectionOnly
					? _messageEditor.Selection.Start
					: 0;
				var end = fdf.SelectionOnly
					? _messageEditor.Selection.End
					: _messageEditor.Model.TextLength;

				if (fdf.Reverse)
				{
					var b = start;
					start = end;
					end = b;
				}

				var res = _messageEditor.Model
					.FindText(start, end, fdf.TextToFind, sf);

				if (!res.IsSuccess)
				{
					MessageBox.Show(
						SR.MessageEditor.FindNothing,
						ApplicationInfo.ApplicationName,
						MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
				{
					_messageEditor.Selection.Start = res.FindStart;
					_messageEditor.Selection.End = res.FindEnd;
				}
			}
		}

		public void SurroundText(string start, string end, bool newLine)
		{
			_messageEditor.Selection.Text = "{0}{1}{2}{1}{3}".FormatStr(
				start, newLine ? Environment.NewLine : string.Empty, _messageEditor.Selection.Text, end);
		}

		public void InsertText(string text)
		{
			_messageEditor.Selection.Text = text;
		}

		public bool IsModified
		{
			get { return _isModified; }
		}

		public event SmartApp.EventHandler<MessageForm> Modified;

		public event SmartApp.EventHandler<MessageForm> IsModifiedChanged;

		#endregion
	}
}