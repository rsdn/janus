using System;
using System.Reactive.Linq;

using CodeJam.Extensibility;

namespace Rsdn.Janus
{
	internal class SmilesToolbarGenerator : IDisposable
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly IMenuService _menuService;
		private readonly ICommandHandlerService _commandHandlerService;
		private readonly IStyleImageManager _styleImageManager;
		private readonly string _menuName;
		private readonly SmilesToolbar _toolbar;
		private readonly IDisposable _menuChangedSubscription;

		public SmilesToolbarGenerator(
			IServiceProvider provider, string menuName, SmilesToolbar toolbar)
		{
			if (provider == null)
				throw new ArgumentNullException(nameof(provider));
			if (menuName == null)
				throw new ArgumentNullException(nameof(menuName));
			if (toolbar == null)
				throw new ArgumentNullException(nameof(toolbar));

			_serviceProvider = provider;
			_toolbar = toolbar;
			_menuName = menuName;

			_menuService = _serviceProvider.GetRequiredService<IMenuService>();
			_commandHandlerService = _serviceProvider.GetRequiredService<ICommandHandlerService>();
			_styleImageManager = _serviceProvider.GetRequiredService<IStyleImageManager>();

			UpdateMenu();

			_menuChangedSubscription = _menuService
				.MenuChanged
				.Where(changedMenuName => _menuName == changedMenuName)
				.Subscribe(changedMenuName => UpdateMenu());
			_toolbar.ButtonClick += ToolbarButtonClick;
		}

		#region IDisposable Members

		public void Dispose()
		{
			_toolbar.ButtonClick -= ToolbarButtonClick;
			_menuChangedSubscription.Dispose();
		}

		#endregion

		private void ToolbarButtonClick(object sender, ButtonInfo buttonInfo)
		{
			var menuCommand = (IMenuCommand)buttonInfo.Tag;
			_commandHandlerService.ExecuteCommand(
				menuCommand.CommandName,
				new CommandContext(_serviceProvider, menuCommand.Parameters));
		}

		private void UpdateMenu()
		{
			_toolbar.BeginUpdate();
			_toolbar.ButtonInfos.Clear();
			var menu = _menuService.GetMenu(_menuName);
			foreach (var menuGroup in menu.Groups)
			{
				if (_toolbar.ButtonInfos.Count > 0)
					_toolbar.ButtonInfos.Add(ButtonInfo.NewLineButton);

				foreach (var menuItem in menuGroup.Items)
				{
					var menuCommand = menuItem as IMenuCommand;
					if (menuCommand != null)
					{
						_toolbar.ButtonInfos.Add(
							new ButtonInfo(
								menuCommand.Text,
								menuCommand.Description,
								menuCommand.Image != null
									? _styleImageManager.TryGetImage(
										menuCommand.Image, StyleImageType.ConstSize)
									: null,
								menuCommand));
					}
					else
						throw new NotSupportedException();
				}
			}
			_toolbar.EndUpdate();
		}
	}
}