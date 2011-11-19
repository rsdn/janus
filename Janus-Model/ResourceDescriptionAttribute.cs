using System.ComponentModel;
using System.Resources;

namespace Rsdn.Janus
{
	public abstract class ResourceDescriptionAttribute : DescriptionAttribute
	{
		private bool _localized;

		/// <summary>
		/// Инициализирует экземпляр именем ресурса.
		/// </summary>
		protected ResourceDescriptionAttribute(string descriptionResource) : base(descriptionResource)
		{}

		/// <summary>
		/// Описание
		/// </summary>
		public override string Description
		{
			get
			{
				if (!_localized)
				{
					var localizedValue = GetResourceManager().GetString(DescriptionValue);
					DescriptionValue = localizedValue ?? "<" + DescriptionValue + ">";
					_localized = true;
				}
				return base.Description;
			}
		}

		protected abstract ResourceManager GetResourceManager();
	}
}