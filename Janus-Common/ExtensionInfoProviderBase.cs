using System;
using System.Drawing;

namespace Rsdn.Janus
{
	/// <summary>
	/// Базовая реализация <see cref="IExtensionInfoProvider"/>
	/// </summary>
	public abstract class ExtensionInfoProviderBase : IExtensionInfoProvider
	{
		private readonly string _resourceName;
		private Image _icon;

		protected ExtensionInfoProviderBase(string resourceName)
		{
			if (resourceName == null)
				throw new ArgumentNullException("resourceName");

			_resourceName = resourceName;
		}

		#region IExtensionInfoProvider Members
		string IExtensionInfoProvider.GetDisplayName()
		{
			return GetDisplayName();
		}

		public Image GetIcon()
		{
			return
				_icon ?? (_icon = Image.FromStream(GetType().Assembly.GetRequiredResourceStream(_resourceName)));
		}

		#endregion

		protected abstract string GetDisplayName();
	}
}