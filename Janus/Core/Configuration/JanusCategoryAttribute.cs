using System;
using System.ComponentModel;

namespace Rsdn.Janus
{
	/// <summary>
	/// Атрибут Category, выбирающий локализацию из Janus.exe.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class JanusCategoryAttribute : CategoryAttribute
	{
		/// <summary>
		/// Инициализирует экземпляр именем ресурса.
		/// </summary>
		public JanusCategoryAttribute(string category) : base (category)
		{}

		/// <summary>
		/// Локализует строку
		/// </summary>
		/// <param name="value">исходная строка</param>
		/// <returns>локализованная строка</returns>
		protected override string GetLocalizedString(string value)
		{
			var localizedValue = SR.ResourceManager.GetString(value);
			return localizedValue ?? "<" + value + ">";
		}
	}
}
