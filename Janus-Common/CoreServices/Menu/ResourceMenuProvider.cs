using System;
using System.ComponentModel;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	/// <summary>
	/// Провайдер меню. Загружает меню, описанное в xml, из ресурсов.
	/// </summary>
	public abstract class ResourceMenuProvider : IMenuProvider
	{
		private readonly Lazy<IMenuRoot> _menuRoot;

		#region Constructors

		protected ResourceMenuProvider(
			[NotNull] IServiceProvider serviceProvider,
			[NotNull, Localizable(false)] string menuResourceName)
			: this(serviceProvider, menuResourceName, null) { }

		protected ResourceMenuProvider(
			[NotNull] IServiceProvider serviceProvider,
			[NotNull, Localizable(false)] string menuResourceName,
			[Localizable(false)] string stringResourceName)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException("serviceProvider");
			if (menuResourceName == null)
				throw new ArgumentNullException("menuResourceName");

			_menuRoot =
				new Lazy<IMenuRoot>(
					() =>
					{
						var assembly = GetType().Assembly;
						return
							XmlMenuLoader.LoadMenu(
								serviceProvider,
								assembly.GetRequiredResourceStream(menuResourceName),
								ResourceHelper.CreateResourceStringGetter(assembly, stringResourceName));
					});
		}

		#endregion

		#region IMenuProvider Members

		public string MenuName
		{
			get { return _menuRoot.Value.Name; }
		}

		public IMenuRoot CreateMenu()
		{
			return _menuRoot.Value;
		}

		#endregion
	}
}