using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Указывает отображаемое имя свойства или типа компонента.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
	[Serializable]
	public class DisplayNameAttribute : Attribute
	{
		private string _displayName;
		private bool _localized;

		/// <summary>
		/// Инициализирует экземпляр
		/// </summary>
		/// <param name="displayName">отображаемое имя</param>
		public DisplayNameAttribute(string displayName)
		{
			_displayName = displayName;
		}

		/// <summary>
		/// Отображаемое имя
		/// </summary>
		public string DisplayName
		{
			get
			{
				if (!_localized)
				{
					_displayName = GetLocalizedString(_displayName);
					_localized = true;
				}
				return _displayName;
			}
		}

		/// <summary>
		/// Локализует строку
		/// </summary>
		/// <param name="value">исходная строка</param>
		/// <returns>локализованная строка</returns>
		/// <remarks>
		/// Ничего не делает. Потомки при перекрытии должны реализовать локализацию.
		/// </remarks>
		protected virtual string GetLocalizedString(string value)
		{
			return value;
		}
	}
}