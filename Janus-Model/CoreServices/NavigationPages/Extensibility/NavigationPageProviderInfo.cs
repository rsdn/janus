using System;

using CodeJam.Extensibility;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public class NavigationPageProviderInfo : ElementInfo
	{
		public NavigationPageProviderInfo([NotNull] Type type, [NotNull] string pathMask)
			: base(type)
		{
			if (pathMask == null)
				throw new ArgumentNullException(nameof(pathMask));

			PathMask = pathMask;
		}

		public string PathMask { get; }
	}
}