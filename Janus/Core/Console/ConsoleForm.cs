using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows.Forms;

using CodeJam;
using CodeJam.Extensibility;

using JetBrains.Annotations;

using Rsdn.Janus.Core.Console;
using Rsdn.Scintilla;

namespace Rsdn.Janus
{
	internal partial class ConsoleForm : JanusBaseForm
	{
		private const string _promptSuffix = ">";

		private readonly IServiceProvider _provider;
		private readonly ICommandService _commandService;
		private readonly ICommandHandlerService _commandHandlerService;
		private readonly StripMenuGenerator _toolbarGenerator;
		private readonly List<string> _history;
		private int _historyPosition;
		private int _editableStartPosition;
		private bool _isPromptMode;
		private string _promptText = _promptSuffix;
		private string _userInput;
		private ConsoleCommand _parsedUserInput;
		private readonly TextStyle _commandNameStyle;
		private readonly TextStyle _defaultStyle;
		private readonly TextStyle _parameterNameStyle;
		private readonly TextStyle _parameterValueStyle;

		public ConsoleForm([NotNull] IServiceProvider provider)
		{
			if (provider == null)
				throw new ArgumentNullException(nameof(provider));

			_provider = provider;
			_commandService = _provider.GetRequiredService<ICommandService>();
			_commandHandlerService = _provider.GetRequiredService<ICommandHandlerService>();
			_history = new List<string>();

			InitializeComponent();

			var styleImageManager = _provider.GetService<IStyleImageManager>();
			if (styleImageManager != null)
				Icon = styleImageManager.TryGetImage("console", StyleImageType.Small).ToIcon();

			_toolbarGenerator = new StripMenuGenerator(_provider, _toolStrip, "Janus.Console.Toolbar");

			var font = new Font("Courier New", 10F);
			_consoleEditor.Font = font;

			_defaultStyle = new TextStyle(
				"Default", 0, font, Color.Black, Color.White, false,
				CaseMode.Mixed, true, false, PredefinedStyle.None);
			_commandNameStyle = new TextStyle(
				"CommandName", 1, font, Color.Brown, Color.White, false,
				CaseMode.Mixed, true, false, PredefinedStyle.None);
			_parameterNameStyle = new TextStyle(
				"ParameterName", 2, font, Color.Red, Color.White, false,
				CaseMode.Mixed, true, false, PredefinedStyle.None);
			_parameterValueStyle = new TextStyle(
				"ParameterValue", 3, font, Color.Blue, Color.White, false,
				CaseMode.Mixed, true, false, PredefinedStyle.None);

			_consoleEditor.TextStyles.AddRange(
				new[] {
					_defaultStyle,
					_commandNameStyle,
					_parameterNameStyle,
					_parameterValueStyle
				});

			Prompt();
		}

		public string PromptText
		{
			get { return _promptText; }
			set
			{
				_promptText = TextMacrosHelper.ReplaceMacroses(
					_provider,
					(value ?? string.Empty) + _promptSuffix);
			}
		}

		public void Clear()
		{
			_isPromptMode = false;
			_editableStartPosition = 0;

			_consoleEditor.Model.Text = string.Empty;
			_consoleEditor.ClearUndoBuffer();

			Prompt();
		}

		private void WriteLine([NotNull] string text)
		{
			if (text == null)
				throw new ArgumentNullException(nameof(text));

			Write(text);
			Write(Environment.NewLine);
		}

		private void Write([NotNull] string text)
		{
			if (text == null)
				throw new ArgumentNullException(nameof(text));

			_consoleEditor.Model.AppendText(text);

			if (!_isPromptMode)
				_consoleEditor.ClearUndoBuffer();

			_consoleEditor.CaretPosition = _consoleEditor.Model.TextLength;
			_consoleEditor.ScrollToCaret();
		}

		private void Prompt()
		{
			Write(PromptText);

			_editableStartPosition = _consoleEditor.Model.TextLength;
			_userInput = null;
			_parsedUserInput = null;
			_isPromptMode = true;
		}

		private void ClearInput()
		{
			if (!_isPromptMode)
				throw new InvalidOperationException();

			_consoleEditor.Selection.Start = _editableStartPosition;
			_consoleEditor.Selection.End = _consoleEditor.Model.TextLength;
			_consoleEditor.Selection.Text = string.Empty;
			_consoleEditor.ClearUndoBuffer();
		}

		private void Execute()
		{
			if (!_isPromptMode)
				throw new InvalidOperationException();

			_isPromptMode = false;

			if (_userInput.NotNullNorEmpty())
				AddToHistory(_userInput);

			WriteLine(string.Empty);
			var preOutputLength = _consoleEditor.Model.TextLength;

			if (!_userInput.NotNullNorEmpty())
			{
				if (_parsedUserInput != null)
					try
					{
						_commandHandlerService.ExecuteCommand(
							_parsedUserInput.CommandName.Name,
							new CommandContext(
								_provider,
								_parsedUserInput.Arguments.ToDictionary(
									arg => arg.Name.Name,
									arg => (object)arg.Value.Value),
								Write));
					}
					catch (Exception ex)
					{
						WriteLine(ConsoleResources.CommandExecutionException);
						Write(ex.Message);
					}
				else
					Write(ConsoleResources.InvalidInputFormat);
			}

			if (IsDisposed) //Выполненная команда могла закрыть окно.
				return;

			if (_consoleEditor.Model.TextLength > preOutputLength)
				WriteLine(string.Empty);

			if (!_isPromptMode)
				Prompt();
		}

		private void AddToHistory([NotNull] string command)
		{
			if (command == null)
				throw new ArgumentNullException(nameof(command));

			if (_history.Count == 0
				|| (_historyPosition == _history.Count
					&& !_history[_historyPosition - 1].Equals(
							command, StringComparison.OrdinalIgnoreCase))
				|| (_historyPosition < _history.Count
					&& !_history[_historyPosition].Equals(
							command, StringComparison.OrdinalIgnoreCase)))
			{
				_history.Insert(_history.Count, command);
				_historyPosition = _history.Count;
			}
			else
				_historyPosition++;
		}

		private void HistoryBack()
		{
			if (!_isPromptMode)
				throw new InvalidOperationException();

			if (_history.Count == 0 || _historyPosition <= 0)
				return;

			ClearInput();
			_historyPosition--;
			Write(_history[_historyPosition]);
		}

		private void HistoryForward()
		{
			if (!_isPromptMode)
				throw new InvalidOperationException();

			if (_history.Count == 0 || _historyPosition >= _history.Count - 1)
				return;

			ClearInput();
			_historyPosition++;
			Write(_history[_historyPosition]);
		}

		private void ShowAutocomplete()
		{
			if (_parsedUserInput == null)
				return;

			var caretPosition = _consoleEditor.CaretPosition - _editableStartPosition;
			var commandNameStart = _parsedUserInput.CommandName.Position;
			var commandNameEnd = commandNameStart + _parsedUserInput.CommandName.Length;

			if (caretPosition >= commandNameStart && caretPosition <= commandNameEnd)
				_consoleEditor.ShowAutocomplete(
					caretPosition - commandNameStart,
					_commandService
						.Commands
						.Select(commandInfo => commandInfo.Name)
						.OrderBy(mommandName => mommandName));
		}

		private void ConsoleEditorKeyDown(object sender, KeyEventArgs e)
		{
			if (!_isPromptMode || _consoleEditor.AutocompleteActive)
				return;

			switch (e.KeyCode)
			{
				case Keys.Enter:
					e.SuppressKeyPress = true;
					e.Handled = true;
					Execute();
					break;

				case Keys.Up:
					e.SuppressKeyPress = true;
					e.Handled = true;
					HistoryBack();
					break;

				case Keys.Down:
					e.SuppressKeyPress = true;
					e.Handled = true;
					HistoryForward();
					break;

				case (Keys.Space):
					if (e.Control)
					{
						e.SuppressKeyPress = true;
						e.Handled = true;
						ShowAutocomplete();
					}
					break;
			}
		}

		private void ConsoleEditorModified(object sender, ModifiedEventArgs e)
		{
			//Блокиробка перед попыткой ввода в область вывода.
			if (e.Position < _editableStartPosition
				&& ((e.ModificationTypes
					& (ModificationTypes.BeforeInsert | ModificationTypes.BeforeDelete)) != 0))
			{
				_consoleEditor.ReadOnly = true;
				SystemSounds.Beep.Play();
			}
			//Обработка после произведения блокировки.
			else if (_consoleEditor.ReadOnly)
			{
				_consoleEditor.ReadOnly = false;
				_consoleEditor.CaretPosition = _consoleEditor.Model.TextLength;
				_consoleEditor.ScrollToCaret();
			}
			//Парсинг ввода.
			else if (_isPromptMode)
			{
				_userInput = _consoleEditor.Model.GetTextRange(
					_editableStartPosition, _consoleEditor.Model.TextLength);
				_parsedUserInput = ConsoleParser.Parse(_userInput);
			}
		}

		private void ConsoleEditorCharAdded(object sender, KeyPressEventArgs e)
		{
			ShowAutocomplete();
		}

		private void ConsoleEditorStyleNeeded(object sender, StyleNeededEventArgs e)
		{
			if (_parsedUserInput == null)
				return;

			_consoleEditor.StartStyling(_editableStartPosition, 31);
			var prewEndPos = 0;
			foreach (var leafNode in GetLeafNodes(_parsedUserInput))
			{
				if (leafNode.Position > prewEndPos)
					_consoleEditor.SetStyling(leafNode.Position - prewEndPos, _defaultStyle);
				_consoleEditor.SetStyling(leafNode.Length, NodeToStyle(leafNode));
				prewEndPos = leafNode.Position + leafNode.Length;
			}
		}

		private TextStyle NodeToStyle(ASTNode node)
		{
			if (node is ConsoleCommandName)
				return _commandNameStyle;
			if (node is ConsoleCommandArgName)
				return _parameterNameStyle;
			if (node is ConsoleCommandArgValue)
				return _parameterValueStyle;
			return _defaultStyle;
		}

		private static IEnumerable<ASTNode> GetLeafNodes(IEnumerable<ASTNode> parentNode)
		{
			foreach (var node in parentNode)
			{
				var parentSubNode = node as ASTParentNode;
				if (parentSubNode != null)
					foreach (var leafNode in GetLeafNodes(parentSubNode))
						yield return leafNode;
				else
					yield return node;
			}
		}
	}
}
