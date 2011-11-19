using System.Resources;

namespace Rsdn.Janus
{
	internal class ConfigDescriptionAttribute : ResourceDescriptionAttribute
	{
		public ConfigDescriptionAttribute(string descriptionResource) : base(descriptionResource)
		{}

		protected override ResourceManager GetResourceManager()
		{
			return ConfigResources.ResourceManager;
		}
	}
}