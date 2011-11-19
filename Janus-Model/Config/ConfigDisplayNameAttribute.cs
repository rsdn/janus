using System;
using System.Resources;

namespace Rsdn.Janus
{
	[AttributeUsage(AttributeTargets.All)]
	internal class ConfigDisplayNameAttribute : ResourceDisplayNameAttribute
	{
		public ConfigDisplayNameAttribute(string displayNameResource) : base(displayNameResource)
		{}

		protected override ResourceManager GetResourceManager()
		{
			return ConfigResources.ResourceManager;
		}
	}
}