using System.Resources;

namespace Rsdn.Janus
{
	public abstract class ResourceDisplayNameAttribute : DisplayNameAttribute
	{
		protected ResourceDisplayNameAttribute(string displayNameResource)
			: base(displayNameResource)
		{}

		protected abstract ResourceManager GetResourceManager();

		/// <summary>
		/// Локализует строку
		/// </summary>
		/// <param name="value">исходная строка</param>
		/// <returns>локализованная строка</returns>
		protected override string GetLocalizedString(string value)
		{
			return GetResourceManager().GetString(value) ?? $"<{value}>";
		}
	}
}