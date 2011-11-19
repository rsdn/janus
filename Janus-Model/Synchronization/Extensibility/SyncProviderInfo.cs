using System;

using JetBrains.Annotations;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	public class SyncProviderInfo : KeyedElementInfo<string>
	{
		public SyncProviderInfo(string name, [NotNull] Type type)
			: base(type, name)
		{}

		public SyncProviderInfo(string name, [NotNull] Type type, string description)
			: base(type, name, description)
		{
		}
	}
}