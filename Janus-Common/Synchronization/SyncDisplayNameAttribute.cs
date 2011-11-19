using System.Resources;

using Rsdn.Janus.Synchronization;

namespace Rsdn.Janus
{
	internal class SyncDisplayNameAttribute : ResourceDisplayNameAttribute
	{
		public SyncDisplayNameAttribute(string displayNameResource)
			: base(displayNameResource)
		{}

		protected override ResourceManager GetResourceManager()
		{
			return SyncResources.ResourceManager;
		}
	}
}