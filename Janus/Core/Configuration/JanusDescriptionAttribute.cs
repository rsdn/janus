using System;
using System.ComponentModel;

namespace Rsdn.Janus
{
	/// <summary>
	/// Специальная версия атрибута LocDescription.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class JanusDescriptionAttribute : DescriptionAttribute
	{
		private bool _localized;

		/// <summary>
		/// Инициализирует экземпляр именем ресурса.
		/// </summary>
		public JanusDescriptionAttribute(string description) : base(description)
		{
		}

		/// <summary>
		/// Описание
		/// </summary>
		public override string Description
		{
			get
			{
				if (!_localized)
				{
					var localizedValue = SR.ResourceManager.GetString(DescriptionValue);
					DescriptionValue = localizedValue ?? "<" + DescriptionValue + ">";
					_localized = true;
				}
				return base.Description;
			}
		}
	}
}
