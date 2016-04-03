using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Forms;

using CodeJam.Collections;
using CodeJam.Extensibility;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public class StripMenuGenerator :
		IDisposable,
		IMenuItemVisitor<StripMenuGenerator.MenuGeneratorContext>
	{
		private static readonly Font _defaultMenuCommandFont;

		private readonly IServiceProvider _serviceProvider;
		private readonly IMenuService _menuService;
		private readonly ICommandHandlerService _commandHandlerService;
		private readonly IDefaultCommandService _defaultCommandService;
		private readonly IStyleImageManager _styleImageManager;
		private readonly ICheckStateService _checkStateService;
		private readonly ToolStrip _toolStrip;
		private readonly string _menuName;
		private readonly TargetMenuType _menuType;
		private readonly bool _useSmallImages;
		private readonly Dictionary<string, List<ToolStripItem>> _commandControls =
			new Dictionary<string, List<ToolStripItem>>(StringComparer.OrdinalIgnoreCase);
		private readonly Dictionary<string, List<ToolStripItem>> _checkControls =
			new Dictionary<string, List<ToolStripItem>>(StringComparer.OrdinalIgnoreCase);
		private readonly AsyncOperation _uiAsyncOperation;
		private bool _isDisposed;
		private readonly CompositeDisposable _disposables = new CompositeDisposable();

		#region Constructors

		static StripMenuGenerator()
		{
			_defaultMenuCommandFont = new Font(SystemFonts.MenuFont, FontStyle.Bold);
		}

		public StripMenuGenerator(
			[NotNull] IServiceProvider serviceProvider,
			[NotNull] ToolStrip toolStrip,
			[NotNull] string menuName)
			: this(serviceProvider, toolStrip, menuName, false) { }

		public StripMenuGenerator(
			[NotNull] IServiceProvider serviceProvider,
			[NotNull] ToolStrip toolStrip,
			[NotNull] string menuName,
			bool useSmallImages)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));
			if (menuName == null)
				throw new ArgumentNullException(nameof(menuName));
			if (toolStrip == null)
				throw new ArgumentNullException(nameof(toolStrip));

			_serviceProvider = serviceProvider;
			_toolStrip = toolStrip;
			_menuName = menuName;
			_useSmallImages = useSmallImages;

			if (toolStrip is ContextMenuStrip)
				_menuType = TargetMenuType.ContextMenu;
			else if (toolStrip is MenuStrip || toolStrip is ToolStripDropDownMenu)
				_menuType = TargetMenuType.Menu;
			else
				_menuType = TargetMenuType.Toolbar;

			_styleImageManager = _serviceProvider.GetRequiredService<IStyleImageManager>();
			_menuService = _serviceProvider.GetRequiredService<IMenuService>();
			_commandHandlerService = _serviceProvider.GetRequiredService<ICommandHandlerService>();
			_defaultCommandService = _serviceProvider.GetService<IDefaultCommandService>();
			_checkStateService = _serviceProvider.GetService<ICheckStateService>();

			_uiAsyncOperation = _serviceProvider.GetRequiredService<IUIShell>().CreateUIAsyncOperation();

			Init();

			_disposables.Add(
				_menuService
					.MenuChanged
					.Where(changedMenuName => changedMenuName == _menuName)
					.Subscribe(arg=> Init()));

			_disposables.Add(
				_commandHandlerService.SubscribeCommandStatusChanged(_serviceProvider, CommandStatusChanged));

			if (_checkStateService != null)
				_disposables.Add(
					_checkStateService.SubscribeCheckStateChanged(_serviceProvider, CheckStateChanged));

			if (_defaultCommandService != null)
				_disposables.Add(
					_defaultCommandService.DefaultCommandChanged.Subscribe(
						arg => _uiAsyncOperation.Post(Init)));
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if (_isDisposed)
				return;

			_disposables.Dispose();
			_uiAsyncOperation.Post(() => _toolStrip.Items.Clear());

			_isDisposed = true;
		}

		#endregion

		#region Private Members

		private void Init()
		{
			_commandControls.Clear();
			_toolStrip.Items.Clear();

			_toolStrip.Items.AddRange(CreateStripItems(_menuService.GetMenu(_menuName).Groups, _menuType));

			UpdateItemsVisibility(_toolStrip.Items);
		}

		private ToolStripItem[] CreateStripItems(IEnumerable<IMenuGroup> groups, TargetMenuType menuType)
		{
			var context = new MenuGeneratorContext(menuType);

			foreach (var group in groups)
			{
				if (context.Result.Count > 0)
					context.Result.Add(new ToolStripSeparator());

				foreach (var item in group.Items)
					item.AcceptVisitor(this, context);
			}

			return context.Result.ToArray();
		}

		#region MenuGeneratorContext

		private sealed class MenuGeneratorContext
		{
			private readonly List<ToolStripItem> _result;

			public MenuGeneratorContext(TargetMenuType menuType)
			{
				MenuType = menuType;
				_result = new List<ToolStripItem>();
			}

			public TargetMenuType MenuType { get; }

			public IList<ToolStripItem> Result => _result;
		}

		#endregion

		#region IMenuItemVisitor<MenuGeneratorContext> Members

		void IMenuItemVisitor<MenuGeneratorContext>.Visit(
			IMenu menu, MenuGeneratorContext context)
		{
			ToolStripItem toolStripItem;
			if (context.MenuType == TargetMenuType.Menu
					|| context.MenuType == TargetMenuType.ContextMenu)
				toolStripItem = new ToolStripMenuItem(
					menu.Text, null, CreateStripItems(menu.Groups, TargetMenuType.Menu));
			else
				toolStripItem = new ToolStripDropDownButton(
					menu.Text, null, CreateStripItems(menu.Groups, TargetMenuType.Menu));

			toolStripItem.Tag = menu;

			context.Result.Add(toolStripItem);
		}

		void IMenuItemVisitor<MenuGeneratorContext>.Visit(
			IMenuCommand menuCommand, MenuGeneratorContext context)
		{
			var toolStripItem = CreateStripButton(context.MenuType);
			InitStripItem(toolStripItem, menuCommand, context.MenuType);

			if (context.MenuType == TargetMenuType.ContextMenu
				&& _defaultCommandService?.CommandName != null
				&& _defaultCommandService.CommandName.Equals(menuCommand.CommandName, StringComparison.OrdinalIgnoreCase)
				&& _defaultCommandService.Parameters.DictionaryEquals(
					menuCommand.Parameters,
					StringComparer.OrdinalIgnoreCase.Equals,
					null))
				toolStripItem.Font = _defaultMenuCommandFont;

			UpdateMenuCommandStatus(toolStripItem);

			RegisterCommandMenuItem(toolStripItem, menuCommand.CommandName);

			toolStripItem.Click += MenuCommandClick;

			context.Result.Add(toolStripItem);
		}

		void IMenuItemVisitor<MenuGeneratorContext>.Visit(
			IMenuSplitButton menuSplitButton, MenuGeneratorContext context)
		{
			var splitButton = new ToolStripSplitButton();
			InitStripItem(splitButton, menuSplitButton, context.MenuType);

			splitButton.DropDownItems.AddRange(
				CreateStripItems(menuSplitButton.Groups, TargetMenuType.Menu));

			UpdateMenuCommandStatus(splitButton);

			RegisterCommandMenuItem(splitButton, menuSplitButton.CommandName);

			splitButton.ButtonClick += MenuCommandClick;

			context.Result.Add(splitButton);
		}

		void IMenuItemVisitor<MenuGeneratorContext>.Visit(
			IMenuCheckCommand menuCheckCommand, MenuGeneratorContext context)
		{
			var menuCommandItem = CreateStripButton(context.MenuType);
			InitStripItem(menuCommandItem, menuCheckCommand, context.MenuType);

			RegisterCommandMenuItem(menuCommandItem, menuCheckCommand.CheckCommandName);
			RegisterCommandMenuItem(menuCommandItem, menuCheckCommand.UncheckCommandName);
			RegisterCheckMenuItem(menuCommandItem, menuCheckCommand.CheckStateName);

			UpdateMenuCheckComand(menuCommandItem);

			menuCommandItem.Click += MenuCommandClick;

			context.Result.Add(menuCommandItem);
		}

		#endregion

		private void MenuCommandClick(object sender, EventArgs e)
		{
			string commandName;
			IDictionary<string, object> commandParameters;
			GetMenuItemCommandAndParameters(
				(IMenuItem)((ToolStripItem)sender).Tag, out commandName, out commandParameters);
			_commandHandlerService.ExecuteCommand(
				commandName,
				new CommandContext(_serviceProvider, commandParameters));
		}

		private void CommandStatusChanged(ICommandHandlerService sender, string[] commandNames)
		{
			_uiAsyncOperation.Post(
				() =>
				{
					var updated = false;
					commandNames.ForEach(
						commandName =>
						{
							List<ToolStripItem> commandControls;
							if (_commandControls.TryGetValue(commandName, out commandControls))
							{
								commandControls.ForEach(UpdateMenuCommandStatus);
								updated = true;
							}
						});
					if (updated)
						UpdateItemsVisibility(_toolStrip.Items);
				});
		}

		private void CheckStateChanged(object sender, string[] names)
		{
			_uiAsyncOperation.Post(
				() =>
				{
					var updated = false;
					names.ForEach(
						name =>
						{
							List<ToolStripItem> checkControls;
							if (_checkControls.TryGetValue(name, out checkControls))
							{
								checkControls.ForEach(UpdateMenuCheckComand);
								updated = true;
							}
						});
					if (updated)
						UpdateItemsVisibility(_toolStrip.Items);
				});
		}

		private void UpdateMenuCommandStatus(ToolStripItem toolStripItem)
		{
			string commandName;
			IDictionary<string, object> commandParameters;
			GetMenuItemCommandAndParameters(
				(IMenuItem)toolStripItem.Tag, out commandName, out commandParameters);

			var commandStatus = _commandHandlerService.QueryStatus(
				commandName,
				new CommandContext(_serviceProvider, commandParameters));

			switch (commandStatus)
			{
				case CommandStatus.Normal:
					toolStripItem.Available = true;
					toolStripItem.Enabled = true;
					break;

				case CommandStatus.Disabled:
					toolStripItem.Available = true;
					toolStripItem.Enabled = false;
					break;

				case CommandStatus.Unavailable:
					toolStripItem.Available = false;
					toolStripItem.Enabled = false;
					break;
			}
		}

		private void UpdateMenuCheckComand(ToolStripItem toolStripItem)
		{
			var menuCheckCommand = (IMenuCheckCommand)toolStripItem.Tag;

			var checkState = _checkStateService.GetCheckState(
				_serviceProvider, menuCheckCommand.CheckStateName);

			var toolStripMenuItem = toolStripItem as ToolStripMenuItem;
			if (toolStripMenuItem != null)
				toolStripMenuItem.CheckState = checkState;
			else
			{
				var toolStripButton = toolStripItem as ToolStripButton;
				if (toolStripButton != null)
					toolStripButton.CheckState = checkState;
			}

			UpdateMenuCommandStatus(toolStripItem);
		}

		private void RegisterCommandMenuItem(ToolStripItem toolStripItem, string commandName)
		{
			List<ToolStripItem> existing;
			if (_commandControls.TryGetValue(commandName, out existing))
				existing.Add(toolStripItem);
			else
				_commandControls[commandName] = new List<ToolStripItem> { toolStripItem };
		}

		private void RegisterCheckMenuItem(ToolStripItem toolStripItem, string checkName)
		{
			List<ToolStripItem> existing;
			if (_checkControls.TryGetValue(checkName, out existing))
				existing.Add(toolStripItem);
			else
				_checkControls[checkName] = new List<ToolStripItem> { toolStripItem };
		}

		private static bool UpdateItemsVisibility(ToolStripItemCollection items)
		{
			ToolStripItem lastVisibleSeparator = null;
			var currentGroupVisible = false;
			var hasAvaliableItems = false;

			for (var i = 0; i < items.Count; i++)
			{
				var item = items[i];

				if (item is ToolStripSeparator)
				{
					item.Available = currentGroupVisible;
					if (currentGroupVisible)
					{
						lastVisibleSeparator = item;
						currentGroupVisible = false;
					}
				}
				else
				{
					var dropDownItem = item as ToolStripDropDownItem;
					if (dropDownItem != null)
						if (dropDownItem.Tag is Menu)
							dropDownItem.Available = UpdateItemsVisibility(dropDownItem.DropDownItems);
						else if (dropDownItem.Tag is MenuSplitButton)
							UpdateItemsVisibility(dropDownItem.DropDownItems);

					if (!currentGroupVisible && item.Available)
					{
						lastVisibleSeparator = null;
						currentGroupVisible = true;
						hasAvaliableItems = true;
					}
				}
			}
			if (lastVisibleSeparator != null)
				lastVisibleSeparator.Available = false;

			return hasAvaliableItems;
		}

		private static ToolStripItem CreateStripButton(TargetMenuType menuType)
		{
			return menuType == TargetMenuType.Toolbar
				? new ToolStripButton()
				: (ToolStripItem)new ToolStripMenuItem();
		}

		private void InitStripItem(
			ToolStripItem toolStripItem,
			IMenuItemWithTextAndImage menuItem,
			TargetMenuType menuType)
		{
			toolStripItem.Text = menuItem.Text;
			toolStripItem.ImageScaling = ToolStripItemImageScaling.None;
			toolStripItem.Image = GetImage(menuItem.Image, menuType);
			toolStripItem.DisplayStyle = GetDisplayStyle(
				menuItem.DisplayStyle, menuItem.Image != null, menuType);
			toolStripItem.Tag = menuItem;
			if (menuItem.Description != null)
				toolStripItem.ToolTipText = menuItem.Description;
		}

		private void GetMenuItemCommandAndParameters(
			IMenuItem menuItem,
			out string commandName,
			out IDictionary<string, object> commandParameters)
		{
			var menuCheckCommand = menuItem as IMenuCheckCommand;
			if (menuCheckCommand != null)
			{
				var checkState = _serviceProvider
					.GetRequiredService<ICheckStateService>()
					.GetCheckState(_serviceProvider, menuCheckCommand.CheckStateName);

				if (checkState == CheckState.Checked || checkState == CheckState.Indeterminate)
				{
					commandName = menuCheckCommand.UncheckCommandName;
					commandParameters = menuCheckCommand.UncheckCommandParameters;
				}
				else
				{
					commandName = menuCheckCommand.CheckCommandName;
					commandParameters = menuCheckCommand.CheckCommandParameters;
				}

				return;
			}

			var menuCommand = menuItem as IMenuCommand;
			if (menuCommand != null)
			{
				commandName = menuCommand.CommandName;
				commandParameters = menuCommand.Parameters;
				return;
			}

			throw new ApplicationException();
		}

		private Image GetImage(string name, TargetMenuType menuType)
		{
			return name != null
				? _styleImageManager.TryGetImage(
					name,
					(menuType == TargetMenuType.Menu
							|| menuType == TargetMenuType.ContextMenu
							|| _useSmallImages
						? StyleImageType.Small
						: StyleImageType.Default))
				: null;
		}

		private static ToolStripItemDisplayStyle GetDisplayStyle(
			MenuItemDisplayStyle displayStyle,
			bool image,
			TargetMenuType menuType)
		{
			switch (displayStyle)
			{
				case MenuItemDisplayStyle.Default:
					return menuType == TargetMenuType.Toolbar
						? image
							? ToolStripItemDisplayStyle.Image
							: ToolStripItemDisplayStyle.Text
						: ToolStripItemDisplayStyle.ImageAndText;
				case MenuItemDisplayStyle.Text:
					return ToolStripItemDisplayStyle.Text;
				case MenuItemDisplayStyle.Image:
					return ToolStripItemDisplayStyle.Image;
				case MenuItemDisplayStyle.TextAndImage:
					return ToolStripItemDisplayStyle.ImageAndText;
			}
			throw new ApplicationException();
		}

		private enum TargetMenuType
		{
			Menu,
			ContextMenu,
			Toolbar
		}

		#endregion
	}
}